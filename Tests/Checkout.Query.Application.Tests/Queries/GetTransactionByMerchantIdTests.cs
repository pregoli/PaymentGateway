using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using Checkout.Query.Application.Interfaces;
using Checkout.Query.Application.Queries;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace Checkout.Query.Application.Tests.Queries;

[TestFixture]
internal class GetTransactionByMerchantIdTests
{
    private Mock<ITransactionsQueryRepository> _transactionsQueryRepository = null!;
    private GetTransactionsByMerchantIdQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _transactionsQueryRepository = new Mock<ITransactionsQueryRepository>();
        _handler = new GetTransactionsByMerchantIdQueryHandler(_transactionsQueryRepository.Object, default!);
    }

    [Test]
    public async Task Given_An_Invalid_Query_Payload_Then_An_Empty_Collection_Of_TransactionResponse_Should_Be_Expected()
    {
        //Arrange
        var query = new GetTransactionsByMerchantId(Guid.NewGuid());
        List<Transaction> transactions = null!;
        _transactionsQueryRepository.Setup(repository => repository.GetByMerchantIdAsync(query.MerchantId))!
            .ReturnsAsync(transactions);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(0);
    }

    [Test]
    public async Task Given_A_Valid_Query_Payload_Then_A_Not_Empty_Collection_Of_TransactionResponse_Should_Be_Expected()
    {
        //Arrange
        var query = new GetTransactionsByMerchantId(Guid.NewGuid());
        var transactions = new List<Transaction?>
        {
            Transaction.Create(Guid.NewGuid(), 100, new CardDetails
            {
                CardHolderName = "Paolo Regoli",
                CardNumber = "4242424242424242",
                Cvv = "100",
                ExpirationMonth = "12",
                ExpirationYear = "2026"
            })
        };

        _transactionsQueryRepository.Setup(repo => repo.GetByMerchantIdAsync(query.MerchantId))
            .ReturnsAsync(transactions!);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(1);
    }
}