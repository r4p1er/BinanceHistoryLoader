using BinanceHistoryLoader.Domain.Abstractions;
using BinanceHistoryLoader.Domain.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BinanceHistoryLoader.Domain.Services;

public class JobsRepository(IMongoCollection<Job> jobs) : IJobsRepository
{
    public async Task AddAsync(Job job)
    {
        await jobs.InsertOneAsync(job);
    }

    public async Task ReplaceByIdAsync(string id, Job job)
    {
        var filter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

        await jobs.ReplaceOneAsync(filter, job);
    }

    public async Task<Job?> FindByIdAsync(string id)
    {
        var filter = new BsonDocument { { "_id", ObjectId.Parse(id) } };

        return await jobs.Find(filter).FirstOrDefaultAsync();
    }
}