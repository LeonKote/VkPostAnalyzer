using Domain.Interfaces;
using Domain.Models;
using Domain.Models.Vk;
using Microsoft.Extensions.Logging;
using Moq;
using Services;
using Xunit;

namespace Tests.Unit
{
	public class VkPostAnalyzerServiceTests
	{
		private readonly Mock<ILetterCountRepository> letterCountRepositoryMock;
		private readonly Mock<IVkApiClient> vkApiClientMock;
		private readonly Mock<ILogger<VkPostAnalyzerService>> loggerMock;
		private readonly VkPostAnalyzerService vkPostAnalyzerService;

		public VkPostAnalyzerServiceTests()
		{
			letterCountRepositoryMock = new Mock<ILetterCountRepository>();
			vkApiClientMock = new Mock<IVkApiClient>();
			loggerMock = new Mock<ILogger<VkPostAnalyzerService>>();

			vkPostAnalyzerService = new VkPostAnalyzerService(letterCountRepositoryMock.Object, vkApiClientMock.Object, loggerMock.Object);
		}

		[Fact]
		public async Task AnalyzePosts_ShouldReturnLetterCounts()
		{
			// Arrange
			var accessToken = "test_access_token";
			var ownerId = 12345;
			var posts = new[] { new WallPost { Text = "Hello world!" } };
			var expectedCounts = new List<LetterCount>
			{
				new LetterCount('h', 1), new LetterCount('e', 1), new LetterCount('l', 3),
				new LetterCount('o', 2), new LetterCount('w', 1), new LetterCount('r', 1),
				new LetterCount('d', 1)
			};

			vkApiClientMock.Setup(x => x.GetPosts(accessToken, 5, ownerId))
				.ReturnsAsync(new WallResponseData { Items = posts });

			letterCountRepositoryMock.Setup(x => x.ClearAndAddRangeAsync(It.IsAny<IEnumerable<LetterCount>>()))
				.Returns(Task.CompletedTask);

			// Act
			var result = await vkPostAnalyzerService.AnalyzePosts(accessToken, ownerId);

			// Assert
			Assert.True(result.IsSuccess);
			Assert.Equal(expectedCounts.Count, result.Value.Count);
			Assert.All(result.Value, count =>
				Assert.Contains(expectedCounts, expected => expected.Letter == count.Letter && expected.Count == count.Count));
		}

		[Fact]
		public async Task AnalyzePosts_ShouldReturnFailure_WhenApiReturnsNull()
		{
			// Arrange
			vkApiClientMock.Setup(x => x.GetPosts(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<long>()))
				.ReturnsAsync((WallResponseData?)null);

			// Act
			var result = await vkPostAnalyzerService.AnalyzePosts("access_token", 0);

			// Assert
			Assert.False(result.IsSuccess);
			Assert.Equal("Error requesting posts.", result.Error);
		}
	}
}
