using Application.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class BlockchainSnapshotRepository : IBlockchainSnapshotRepository
{
    private readonly BlockchainDbContext _context;

    public BlockchainSnapshotRepository(BlockchainDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(BlockchainSnapshot snapshot, CancellationToken ct = default)
    {
        await _context.BlockchainSnapshots.AddAsync(snapshot, ct);
    }

    public async Task<IReadOnlyList<BlockchainSnapshot>> GetHistoryAsync(
        BlockchainType type,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        return await _context.BlockchainSnapshots
            .Where(x => x.BlockchainType == type)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
}