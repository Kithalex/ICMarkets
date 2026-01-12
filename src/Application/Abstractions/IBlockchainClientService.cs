using Domain.Entities;
using Domain.Enums;

namespace Application.Abstractions;

public interface IBlockchainClientService
{
    Task<BlockchainSnapshot> FetchSnapshotAsync(
        BlockchainType type,
        CancellationToken ct = default);
}