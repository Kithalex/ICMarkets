using Application.Abstractions;
using Domain.Enums;
using MediatR;

namespace Application.Features.BlockchainHistory;

public record FetchBlockchainSnapshotCommand(BlockchainType Type) : IRequest;

public class FetchBlockchainSnapshotCommandHandler : IRequestHandler<FetchBlockchainSnapshotCommand>
{
    private readonly IBlockchainClientService _clientService;
    private readonly IBlockchainSnapshotRepository _repository;
    private readonly IUnitOfWork _uow;

    public FetchBlockchainSnapshotCommandHandler(
        IBlockchainClientService clientService,
        IBlockchainSnapshotRepository repository,
        IUnitOfWork uow)
    {
        _clientService = clientService;
        _repository = repository;
        _uow = uow;
    }

    public async Task<Unit> Handle(FetchBlockchainSnapshotCommand request, CancellationToken ct)
    {
        var snapshot = await _clientService.FetchSnapshotAsync(request.Type, ct);
        await _repository.AddAsync(snapshot, ct);
        await _uow.SaveChangesAsync(ct);
        return Unit.Value;
    }
}