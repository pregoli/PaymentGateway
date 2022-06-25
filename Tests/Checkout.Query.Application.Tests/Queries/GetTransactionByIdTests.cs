using Checkout.Query.Application.Interfaces;
using Checkout.Query.Application.Queries;
using Moq;
using NUnit.Framework;
using Checkout.Domain.Transaction;
using Checkout.Domain.Transaction.ValueObjects;
using FluentAssertions;

namespace Checkout.Query.Application.Tests.Queries;

[TestFixture]
internal class GetTransactionByIdTests
{
    private Mock<ITransactionsQueryRepository> _transactionsQueryRepository = null!;
    private GetTransactionByIdQueryHandler _handler = null!;

    [SetUp]
    public void Setup()
    {
        _transactionsQueryRepository = new Mock<ITransactionsQueryRepository>();
        _handler = new GetTransactionByIdQueryHandler(_transactionsQueryRepository.Object, default!);
    }

    [Test]
    public async Task Given_An_Invalid_Query_Payload_Then_A_Not_Null_TransactionResponse_Should_Be_Expected()
    {
        //Arrange
        var query = new GetTransactionById(Guid.NewGuid());
        Transaction transaction = null!;
        _transactionsQueryRepository.Setup(repo => repo.GetByTransactionIdAsync(query.Id))
            .ReturnsAsync(transaction);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Given_A_Valid_Query_Payload_Then_A_Not_Null_TransactionResponse_Should_Be_Expected()
    {
        //Arrange
        var query = new GetTransactionById(Guid.NewGuid());
        var transaction = Transaction.Create(
            Guid.NewGuid(), 100, CardDetails.Create("Paolo Regoli", "4242424242424242", "12", "2026", "100"));

        _transactionsQueryRepository.Setup(repo => repo.GetByTransactionIdAsync(query.Id))
            .ReturnsAsync(transaction);

        //Act
        var result = await _handler.Handle(query, default);

        //Assert
        result.Should().NotBeNull();
    }
}
