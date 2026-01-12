using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Api;

namespace IntegrationTests;

public class BlockchainApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BlockchainApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    public async Task FetchEthThenGetHistory_ReturnsAtLeastOneItem()
    {
        const string type = "eth";

        // trigger fetch
        var fetchResponse = await _client.PostAsync($"/api/blockchain/{type}/fetch", content: null);
        fetchResponse.StatusCode.Should().Be(HttpStatusCode.Accepted);

        // read history
        var historyResponse = await _client.GetAsync($"/api/blockchain/{type}/history?pageNumber=1&pageSize=10");
        historyResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var history = await historyResponse.Content.ReadFromJsonAsync<List<BlockchainSnapshotDto>>();
        history.Should().NotBeNull();
        history!.Count.Should().BeGreaterThan(0);
    }

    // DTO matching the API returns for history items
    private sealed class BlockchainSnapshotDto
    {
        public Guid Id { get; set; }
        public int BlockchainType { get; set; }
        public string RawJson { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}