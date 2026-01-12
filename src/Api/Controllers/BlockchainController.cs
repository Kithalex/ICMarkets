using Application.Features.BlockchainHistory;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BlockchainController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlockchainController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // fetches and stores a new snapshot for the blockchain
    [HttpPost("{type}/fetch")]
    public async Task<IActionResult> Fetch(BlockchainType type, CancellationToken ct)
    {
        await _mediator.Send(new FetchBlockchainSnapshotCommand(type), ct);
        return Accepted();
    }

    // gets stored snapshots history for the blockchain
    [HttpGet("{type}/history")]
    public async Task<IActionResult> GetHistory(
        BlockchainType type,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 50,
        CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetBlockchainHistoryQuery(type, pageNumber, pageSize), ct);
        return Ok(result);
    }

    // fetches and stores a new snapshot for all supported blockchains
    [HttpPost("fetch-all")]
    public async Task<IActionResult> FetchAll(CancellationToken ct)
    {
        var types = new[]
        {
            BlockchainType.Eth,
            BlockchainType.Dash,
            BlockchainType.BtcMain,
            BlockchainType.BtcTest3,
            BlockchainType.Ltc
        };

        foreach (var type in types)
        {
            await _mediator.Send(new FetchBlockchainSnapshotCommand(type), ct);
        }

        return Accepted();
    }

}