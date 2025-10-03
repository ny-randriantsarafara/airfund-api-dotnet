namespace api.Tests.src.Domain.Entities;

public class InvestmentTest
{
    [Fact]
    public void Investment_Properties_ShouldBeSettableAndGettable()
    {
        // Arrange & Act
        var investment = new Investment
        {
            Id = 1,
            Name = "Tech Startup Fund",
            CommittedCapital = 1000000m,
            DistributedCapital = 200000m,
            CurrentNetAssetValue = 1500000m
        };

        // Assert
        Assert.Equal(1, investment.Id);
        Assert.Equal("Tech Startup Fund", investment.Name);
        Assert.Equal(1000000m, investment.CommittedCapital);
        Assert.Equal(200000m, investment.DistributedCapital);
        Assert.Equal(1500000m, investment.CurrentNetAssetValue);
    }

    [Fact]
    public void CalculateTVPI_WithValidCommittedCapital_ShouldReturnCorrectTVPI()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 1000000m,
            DistributedCapital = 200000m,
            CurrentNetAssetValue = 1500000m
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        // TVPI = (CurrentNetAssetValue + DistributedCapital) / CommittedCapital
        // TVPI = (1500000 + 200000) / 1000000 = 1.7
        Assert.Equal(1.7m, tvpi);
    }

    [Fact]
    public void CalculateTVPI_WithZeroCommittedCapital_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 0m,
            DistributedCapital = 200000m,
            CurrentNetAssetValue = 1500000m
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() => investment.CalculateTVPI());
        Assert.Equal("Committed Capital cannot be zero when calculating TVPI.", exception.Message);
    }

    [Fact]
    public void CalculateTVPI_WithZeroDistributedCapitalAndCurrentNetAssetValue_ShouldReturnZero()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 1000000m,
            DistributedCapital = 0m,
            CurrentNetAssetValue = 0m
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        Assert.Equal(0m, tvpi);
    }

    [Fact]
    public void CalculateTVPI_WithOnlyDistributedCapital_ShouldReturnCorrectTVPI()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 1000000m,
            DistributedCapital = 500000m,
            CurrentNetAssetValue = 0m
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        // TVPI = (0 + 500000) / 1000000 = 0.5
        Assert.Equal(0.5m, tvpi);
    }

    [Fact]
    public void CalculateTVPI_WithOnlyCurrentNetAssetValue_ShouldReturnCorrectTVPI()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 1000000m,
            DistributedCapital = 0m,
            CurrentNetAssetValue = 1200000m
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        // TVPI = (1200000 + 0) / 1000000 = 1.2
        Assert.Equal(1.2m, tvpi);
    }

    [Theory]
    [InlineData(1000000, 200000, 1500000, 1.7)]
    [InlineData(500000, 100000, 600000, 1.4)]
    [InlineData(2000000, 500000, 2500000, 1.5)]
    [InlineData(1000000, 0, 800000, 0.8)]
    [InlineData(1000000, 1200000, 0, 1.2)]
    public void CalculateTVPI_WithVariousInputs_ShouldReturnExpectedResults(
        decimal committedCapital,
        decimal distributedCapital,
        decimal currentNetAssetValue,
        decimal expectedTVPI)
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = committedCapital,
            DistributedCapital = distributedCapital,
            CurrentNetAssetValue = currentNetAssetValue
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        Assert.Equal(expectedTVPI, tvpi);
    }

    [Fact]
    public void CalculateTVPI_WithVerySmallCommittedCapital_ShouldReturnHighTVPI()
    {
        // Arrange
        var investment = new Investment
        {
            CommittedCapital = 0.01m,
            DistributedCapital = 1000m,
            CurrentNetAssetValue = 2000m
        };

        // Act
        var tvpi = investment.CalculateTVPI();

        // Assert
        // TVPI = (2000 + 1000) / 0.01 = 300000
        Assert.Equal(300000m, tvpi);
    }
}