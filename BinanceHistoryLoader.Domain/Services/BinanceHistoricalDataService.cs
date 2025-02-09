using System.Collections.Concurrent;
using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Entities;
using BinanceHistoryLoader.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace BinanceHistoryLoader.Domain.Services;

public class BinanceHistoricalDataService(
    IServiceScopeFactory scopeFactory,
    IJobsRepository jobs,
    ITradeListsRepository tradeLists,
    ILogger<BinanceHistoricalDataService> logger,
    ConcurrentDictionary<string, Task> tasksDictionary) : IBinanceHistoricalDataService
{
    public string LoadData(List<string> pairs, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var jobId = ObjectId.GenerateNewId();

        tasksDictionary[jobId.ToString()] = Task.Run(async () =>
        {
            using var scope = scopeFactory.CreateScope();
            var binanceClient = scope.ServiceProvider.GetRequiredService<IBinanceClientService>();
            var job = new Job
            {
                Id = jobId,
                Status = JobStatus.Processing,
                EndTime = null
            };
            await jobs.AddAsync(job);
            logger.LogInformation("Job {JobId} added with status Processing", jobId.ToString());

            try
            {
                logger.LogInformation("Job {JobId} processing started", jobId.ToString());
                var tasks = pairs.Select(async pair =>
                {
                    logger.LogDebug("Retrieving trades for pair {Pair} for Job {JobId}", pair, jobId.ToString());
                    var trades = await binanceClient.GetAggTradesAsync(pair, startDate, endDate, cancellationToken);
                    logger.LogDebug("Retrieved {Count} trades for pair {Pair} for Job {JobId}", trades.Count, pair,
                        jobId.ToString());
                    var tradeList = new AggregateTradeList
                    {
                        Id = ObjectId.GenerateNewId(),
                        Symbol = pair,
                        StartTime = startDate,
                        EndTime = endDate,
                        Trades = trades,
                        JobId = jobId.ToString()
                    };
                    await tradeLists.AddAsync(tradeList, cancellationToken);
                    logger.LogInformation("TradeList for pair {Pair} saved for Job {JobId}", pair,
                        jobId.ToString());
                });
                await Task.WhenAll(tasks);

                job.Status = JobStatus.Completed;
                logger.LogInformation("Job {JobId} completed successfully", jobId.ToString());
            }
            catch (OperationCanceledException ex)
            {
                job.Status = JobStatus.Failed;
                logger.LogError(ex, "Job {JobId} processing cancelled", jobId.ToString());
            }
            catch (Exception ex)
            {
                job.Status = JobStatus.Failed;
                logger.LogError(ex, "Job {JobId} failed during processing", jobId.ToString());
            }
            finally
            {
                job.EndTime = DateTime.UtcNow;
                await jobs.ReplaceByIdAsync(jobId.ToString(), job);
                logger.LogInformation("Job {JobId} status updated to {Status} with EndTime {EndTime}", jobId.ToString(),
                    job.Status.ToString(), job.EndTime);
            }
        }, cancellationToken);

        return jobId.ToString();
    }

    public async Task<Job> CheckStatusAsync(string id)
    {
        if (tasksDictionary.TryGetValue(id, out var task) && task.IsCompleted)
        {
            tasksDictionary.TryRemove(id, out _);
            if (task.IsFaulted) await task;
        }

        var job = await jobs.FindByIdAsync(id);

        if (job == null)
        {
            logger.LogWarning("Job not found for JobId: {JobId}", id);
            throw new KeyNotFoundException("Job not found");
        }

        return job;
    }
}