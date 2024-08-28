using FinLab.Domain.BDR;

using YahooFinanceApi;

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

app.MapGet("/asset/{symbol}", async (HttpContext context) =>
{
    var symbol = context.Request.RouteValues["symbol"].ToString();

    var securities = await Yahoo
        .Symbols(symbol) //.Symbols("AAPL", "GOOG")
        .Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh)
        .QueryAsync();

    var aapl = securities[symbol];
    var price = aapl.RegularMarketPrice; // aapl[Field.RegularMarketPrice];

    return new Asset(symbol, price);
});

app.Run();


record Asset(
    string Symbol,
    double Price
){ }
