var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();

var products = new List<Product>
{
    new(1, "Laptop", "Electronics", 999.99m, 50),
    new(2, "Headphones", "Electronics", 149.99m, 200),
    new(3, "Running Shoes", "Footwear", 89.99m, 150),
    new(4, "Backpack", "Accessories", 49.99m, 300),
    new(5, "Water Bottle", "Accessories", 19.99m, 500)
};

app.MapGet("/api/products", () => Results.Ok(products))
   .WithName("GetProducts")
   .WithOpenApi();

app.MapGet("/api/products/{id}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
.WithOpenApi();

app.MapGet("/api/products/health", () => Results.Ok(new { service = "be-ecom-products", status = "healthy" }));

app.Run();

record Product(int Id, string Name, string Category, decimal Price, int Stock);
