using BinanceHistoryLoader.Domain.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BinanceHistoryLoader.Domain.Entities;

public class Job
{
    [BsonId] public ObjectId Id { get; set; }

    public JobStatus Status { get; set; }

    public DateTime? EndTime { get; set; }
}