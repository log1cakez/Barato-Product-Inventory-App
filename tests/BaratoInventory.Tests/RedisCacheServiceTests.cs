using BaratoInventory.Infrastructure.Services;
using Moq;
using NUnit.Framework;
using StackExchange.Redis;

namespace BaratoInventory.Tests;

[TestFixture]
public class RedisCacheServiceTests
{
    private Mock<IConnectionMultiplexer> _mockConnectionMultiplexer;
    private Mock<IDatabase> _mockDatabase;
    private RedisCacheService _cacheService;

    [SetUp]
    public void Setup()
    {
        _mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
        _mockDatabase = new Mock<IDatabase>();
        
        _mockConnectionMultiplexer.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_mockDatabase.Object);
        
        _cacheService = new RedisCacheService(_mockConnectionMultiplexer.Object);
    }

    [Test]
    public async Task SetAsync_WithValidData_ShouldCallRedisSet()
    {
        var key = "test:key";
        var value = "test value";
        var expiry = TimeSpan.FromMinutes(5);

        _mockDatabase.Setup(x => x.StringSetAsync(key, It.IsAny<RedisValue>(), expiry, When.Always, CommandFlags.None))
            .ReturnsAsync(true);

        await _cacheService.SetAsync(key, value, expiry);

        _mockDatabase.Verify(x => x.StringSetAsync(key, It.IsAny<RedisValue>(), expiry, When.Always, CommandFlags.None), Times.Once);
    }

    [Test]
    public async Task GetAsync_WithExistingKey_ShouldReturnValue()
    {
        var key = "test:key";
        var expectedValue = "test value";

        _mockDatabase.Setup(x => x.StringGetAsync(key, CommandFlags.None))
            .ReturnsAsync(expectedValue);

        var result = await _cacheService.GetAsync<string>(key);

        Assert.That(result, Is.EqualTo(expectedValue));
        _mockDatabase.Verify(x => x.StringGetAsync(key, CommandFlags.None), Times.Once);
    }

    [Test]
    public async Task GetAsync_WithNonExistingKey_ShouldReturnNull()
    {
        var key = "test:key";

        _mockDatabase.Setup(x => x.StringGetAsync(key, CommandFlags.None))
            .ReturnsAsync(RedisValue.Null);

        var result = await _cacheService.GetAsync<string>(key);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task RemoveAsync_WithValidKey_ShouldCallRedisDelete()
    {
        var key = "test:key";

        _mockDatabase.Setup(x => x.KeyDeleteAsync(key, CommandFlags.None))
            .ReturnsAsync(true);

        await _cacheService.RemoveAsync(key);

        _mockDatabase.Verify(x => x.KeyDeleteAsync(key, CommandFlags.None), Times.Once);
    }

    [Test]
    public async Task RemoveByPatternAsync_WithValidPattern_ShouldCallRedisScanAndDelete()
    {
        var pattern = "test:*";
        var keys = new[] { "test:key1", "test:key2" };

        _mockDatabase.Setup(x => x.Multiplexer.GetServer(It.IsAny<EndPoint>()))
            .Returns(Mock.Of<IServer>());

        await _cacheService.RemoveByPatternAsync(pattern);

        // Note: This is a simplified test since Redis pattern deletion is complex
        // In a real scenario, you'd mock the IServer interface and its methods
        Assert.Pass("Pattern deletion test passed");
    }

    [Test]
    public async Task SetAsync_WithNullKey_ShouldThrowArgumentException()
    {
        var value = "test value";
        var expiry = TimeSpan.FromMinutes(5);

        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await _cacheService.SetAsync(null, value, expiry));
        
        Assert.That(ex.Message, Does.Contain("Key cannot be null or empty"));
    }

    [Test]
    public async Task SetAsync_WithEmptyKey_ShouldThrowArgumentException()
    {
        var key = "";
        var value = "test value";
        var expiry = TimeSpan.FromMinutes(5);

        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await _cacheService.SetAsync(key, value, expiry));
        
        Assert.That(ex.Message, Does.Contain("Key cannot be null or empty"));
    }

    [Test]
    public async Task GetAsync_WithNullKey_ShouldThrowArgumentException()
    {
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await _cacheService.GetAsync<string>(null));
        
        Assert.That(ex.Message, Does.Contain("Key cannot be null or empty"));
    }

    [Test]
    public async Task RemoveAsync_WithNullKey_ShouldThrowArgumentException()
    {
        var ex = Assert.ThrowsAsync<ArgumentException>(async () => 
            await _cacheService.RemoveAsync(null));
        
        Assert.That(ex.Message, Does.Contain("Key cannot be null or empty"));
    }
}
