using Application.Abstractions;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.BlockchainHistory;

public record GetBlockchainHistoryQuery(
    BlockchainType Type,
    int PageNumber = 1,
    int PageSize = 50) : IRequest<IReadOnlyList<BlockchainSnapshot>>;

public class GetBlockchainHistoryQueryHandler
    : IRequestHandler<GetBlockchainHistoryQuery, IReadOnlyList<BlockchainSnapshot>>
{
    private readonly IBlockchainSnapshotRepository _repository;

    public GetBlockchainHistoryQueryHandler(IBlockchainSnapshotRepository repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<BlockchainSnapshot>> Handle(
        GetBlockchainHistoryQuery request,
        CancellationToken ct)
    {
        return _repository.GetHistoryAsync(request.Type, request.PageNumber, request.PageSize, ct);
    }
}