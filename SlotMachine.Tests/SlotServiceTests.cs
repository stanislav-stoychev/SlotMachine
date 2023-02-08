using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal.Execution;
using SlotMachine.Models;
using SlotMachine.Services;
using System.Runtime.Serialization;
using System;

namespace SlotMachine.Tests;

public class SlotServiceTests
{
    private Mock<ICombosGenerator> _combosGeneratorMock;
    private Mock<ILogger<SlotService>> _loggerMock;
    private MockRepository _mockRepository;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new MockRepository(MockBehavior.Strict);
        _combosGeneratorMock = _mockRepository.Create<ICombosGenerator>();
        _loggerMock = _mockRepository.Create<ILogger<SlotService>>();
    }

    [TestCaseSource(nameof(TestData))]
    public void Spin_behaves_as_expected(List<List<Symbol>> lines, decimal bet, decimal profit)
    {
        // Arrange
        _combosGeneratorMock.Setup(c => c.GetLines())
            .Returns(lines);

        _loggerMock.Setup(l => l.Log(
            It.IsAny<LogLevel>() 
            , It.IsAny<EventId>()
            , It.IsAny<It.IsAnyType>()
            , It.IsAny<Exception>()
            , It.IsAny<Func<It.IsAnyType, Exception, string>>())
        );

        // Act
        var result = GetSut()
            .Spin(bet);

        // Assert
        Assert.That(result, Is.EqualTo(profit));
    }

    private static object[] TestData()
    {
        var banana = new Symbol()
        {
            Name = "Banana",
            Alias = "B",
            Odds = 0.35M,
            Coefficient = 0.6M,
            IsWildcard = false
        };

        var apple = new Symbol()
        {
            Name = "Apple",
            Alias = "A",
            Odds = 0.45M,
            Coefficient = 0.4M,
            IsWildcard = false
        };

        var wildcard = new Symbol()
        {
            Name = "Wildcard",
            Alias = "*",
            Odds = 0.05M,
            Coefficient = 0.0M,
            IsWildcard = true
        };

        return new object[]
        {
            new object[]
            {
                new List<List<Symbol>>
                {
                    new List<Symbol>()
                    {
                        banana, apple, apple
                    },
                    new List<Symbol>()
                    {
                        apple, apple, apple
                    },
                    new List<Symbol>()
                    {
                        apple, wildcard, banana
                    },
                    new List<Symbol>()
                    {
                        wildcard, apple, apple
                    }
                }
                , 10M
                , 20M
            }
        };
    }

    private SlotService GetSut()
        => new(_combosGeneratorMock.Object, _loggerMock.Object);
}
