namespace Checkout.Query.Application.Extensions;

public static class CardsExtensions
{
    public static string Mask(this string source, char maskCharacter)
    {
        if (12 > source.Length)
            throw new ArgumentException("Mask length is greater than string length");

        string mask = new(maskCharacter, 12);
        string unMaskStart = source.Substring(0, 0);
        string unMaskEnd = source.Substring(0 + 12, source.Length - 12);

        return unMaskStart + mask + unMaskEnd;
    }
}
