using Domain.Enums;

namespace Domain.Entities;

public class BlockchainSnapshot
{
    public Guid Id { get; set; }
    public BlockchainType BlockchainType { get; set; }
    public string RawJson { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}