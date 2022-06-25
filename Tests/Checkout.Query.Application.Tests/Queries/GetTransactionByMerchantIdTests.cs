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
        _transactionsQueryRepository.Setup(repository => repository.GetByMerchantIdAsync(query.MerchantId))!
            .ReturnsAsync(new List<Transaction>());

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
            Transaction.Create(Guid.NewGuid(), 100, CardDetails.Create("Paolo Regoli", "4242424242424242", "12", "2026", "100"))
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