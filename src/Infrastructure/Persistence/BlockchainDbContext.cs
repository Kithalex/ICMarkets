using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class BlockchainDbContext : DbContext
{
    public BlockchainDbContext(DbContextOptions<BlockchainDbContext> options)
        : base(options) { }

    public DbSet<BlockchainSnapshot> BlockchainSnapshots => Set<BlockchainSnapshot>();
}