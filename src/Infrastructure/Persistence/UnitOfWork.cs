using Application.Abstractions;

namespace Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly BlockchainDbContext _context;

    public UnitOfWork(BlockchainDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken ct = default)
        => _context.SaveChangesAsync(ct);
}