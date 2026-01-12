using System.Net.Http.Json;
using Application.Abstractions;
using Domain.Entities;
using Domain.Enums;
using System.Text.Json;

namespace Infrastructure.Http;

public class BlockchainClientService : IBlockchainClientService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BlockchainClientService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<BlockchainSnapshot> FetchSnapshotAsync(BlockchainType type, CancellationToken ct = default)
    {
        var client = _httpClientFactory.CreateClient("blockcypher");

        var url = type switch
        {
            BlockchainType.Eth => "https://api.blockcypher.com/v1/eth/main",
            BlockchainType.Dash => "https://api.blockcypher.com/v1/dash/main",
            BlockchainType.BtcMain => "https://api.blockcypher.com/v1/btc/main",
            BlockchainType.BtcTest3 => "https://api.blockcypher.com/v1/btc/test3",
            BlockchainType.Ltc => "https://api.blockcypher.com/v1/ltc/main",
            _ => throw new ArgumentOutOfRangeException(nameof(type))
        };

        using var response = await client.GetAsync(url, ct);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(ct);

        return new BlockchainSnapshot
        {
            Id = Guid.NewGuid(),
            BlockchainType = type,
            RawJson = json,
            CreatedAt = DateTime.UtcNow
        };
    }
}