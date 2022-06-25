namespace Checkout.Command.Application.Dtos;

public record CardDetailsDto
{
    public string HolderName { get; set; }

    public string Number { get; set; }

    public string ExpirationMonth { get; set; }

    public string ExpirationYear { get; set; }

    public string Cvv { get; set; }
}