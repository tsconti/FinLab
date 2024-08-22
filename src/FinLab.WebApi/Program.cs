var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/bdrsummary", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new BDRSummary
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            "BITO39",
            10,
            "ITOT",
            1,
            5.78m,
            112.97m,
            65.5m
        ))
        .ToArray();
    return forecast;
})
.WithName("GetBDRSummary")
.WithOpenApi();

app.Run();

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
