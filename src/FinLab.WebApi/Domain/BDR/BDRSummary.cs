namespace FinLab.Domain.BDR;

record BDRSummary(
    DateOnly Date,
    string Code,
    int EquivalentFraction,
    string RelatedAssetCode,
    int RelatedAssetFraction,
    decimal USDInBRL,
    decimal RelatedAssetQuoteInUSD,
    decimal QuoteInBRL)
{
    public decimal ExpectedBDRQuoteInBRL => RelatedAssetQuoteInUSD * USDInBRL / EquivalentFraction;
    public decimal Spread => QuoteInBRL / ExpectedBDRQuoteInBRL - 1;
}
