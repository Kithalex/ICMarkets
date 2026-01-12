using Domain.Entities;
using Domain.Enums;

namespace Application.Abstractions;

public interface IBlockchainSnapshotRepository
{
    Task AddAsync(BlockchainSnapshot snapshot, CancellationToken ct = default);

    Task<IReadOnlyList<BlockchainSnapshot>> GetHistoryAsync(
        BlockchainType type,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);
}