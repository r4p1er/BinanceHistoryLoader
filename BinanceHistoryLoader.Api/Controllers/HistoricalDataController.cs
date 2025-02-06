using BinanceHistoryLoader.Api.Models;
using BinanceHistoryLoader.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace BinanceHistoryLoader.Api.Controllers;

[ApiController]
[Route("api/historical-data")]
public class HistoricalDataController(IBinanceHistoricalDataService binanceHistory) : ControllerBase
{
    [HttpPost("load")]
    public IActionResult Load([FromBody] HistoricalLoadData data)
    {
        var jobId = binanceHistory.LoadData(data.Pairs, data.StartDate, data.EndDate);

        return Ok(new { jobId = jobId });
    }

    [HttpGet("status")]
    public async Task<ActionResult<HistoricalStatusView>> GetStatus([FromQuery] string jobId)
    {
        try
        {
            return HistoricalStatusView.FromJob(await binanceHistory.CheckStatus(jobId));
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}