using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BinanceHistoryLoader.Domain.Services;

public class JobsRepository(IMongoCollection<Job> jobs) : IJobsRepository
{
    public async Task AddAsync(Job job, CancellationToken cancellationToken = default)
    {
        await jobs.InsertOneAsync(job, null, cancellationToken);
    }

    public async Task ReplaceByIdAsync(string id, Job job, CancellationToken cancellationToken = default)
    {
        var filter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

        await jobs.ReplaceOneAsync(filter, job, new ReplaceOptions(), cancellationToken);
    }

    public async Task<Job?> FindByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

        return await jobs.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
}