using BinanceHistoryLoader.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BinanceHistoryLoader.Domain.Entities;

public class AggregateTradeList
{
    [BsonId] public ObjectId Id { get; set; }

    public string Symbol { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public List<AggregateTrade> Trades { get; set; } = null!;

    public string JobId { get; set; } = null!;
}