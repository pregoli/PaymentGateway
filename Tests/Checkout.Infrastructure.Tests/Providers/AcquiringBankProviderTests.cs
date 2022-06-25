using Checkout.Command.Application.Dtos;
using Checkout.Infrastructure.Providers;
using FluentAssertions;
using NUnit.Framework;

namespace Checkout.Infrastructure.Tests.Providers;

[TestFixture]
internal class AcquiringBankProviderTests
{
    private AcquiringBankProvider _acquiringBankProvider = null!;

    [SetUp]
    public void Setup()
    {
        _acquiringBankProvider = new AcquiringBankProvider(default!);
    }

    [TestCaseSource(nameof(InvalidTransactionAmountsLastDigits))]
    public void Given_An_Invalid_Request_Then_The_Transaction_Should_Be_Rejected(decimal amount)
    {
        //Arrange
        var request = new TransactionAuthorizationRequest(Guid.NewGuid(), Guid.NewGuid(), amount);
        
        //Act
        var result = _acquiringBankProvider.ValidateTransaction(request);

        //Assert
        result.TransactionId.Should().Be(request.TransactionId);
        result.Authorized.Should().BeFalse();
    }

    [TestCaseSource(nameof(ValidTransactionAmountsLastDigits))]
    public void Given_A_Valid_Request_Then_The_Transaction_Should_Be_Authorized(decimal amount)
    {
        //Arrange
        var request = new TransactionAuthorizationRequest(Guid.NewGuid(), Guid.NewGuid(), amount);

        //Act
        var result = _acquiringBankProvider.ValidateTransaction(request);

        //Assert
        result.TransactionId.Should().Be(request.TransactionId);
        result.Authorized.Should().BeTrue();
    }

    protected static readonly object[] InvalidTransactionAmountsLastDigits = {
                new object[] { (decimal)105 },
                new object[] { (decimal)112 },
                new object[] { (decimal)114 },
                new object[] { (decimal)151 },
                new object[] { (decimal)154 },
                new object[] { (decimal)162 },
                new object[] { (decimal)163 },
                new object[] { (decimal)19998 },
                new object[] { (decimal)1150 },
                new object[] { (decimal)16900 },
                new object[] { (decimal)15000 },
                new object[] { (decimal)15029 },
                new object[] { (decimal)16510 },
                new object[] { (decimal)16520 },
                new object[] { (decimal)16530 },
                new object[] { (decimal)16540 },
                new object[] { (decimal)133 },
                new object[] { (decimal)14001 },
                new object[] { (decimal)14008 },
                new object[] { (decimal)12011 },
                new object[] { (decimal)12013 }
            };
    
    protected static readonly object[] ValidTransactionAmountsLastDigits = {
                new object[] { (decimal)1 },
                new object[] { (decimal)100 },
                new object[] { (decimal)1000 },
                new object[] { (decimal)10000 },
                new object[] { (decimal)100000 }
            };
}
