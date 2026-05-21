using Microsoft.EntityFrameworkCore;
using PayPalsBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB connectie
var connectionString = "Server=localhost;Database=paypals_db;User=root;Password=1234;";

builder.Services.AddDbContext<PaypalsDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// =======================
// PRODUCTEN
// =======================

app.MapGet("/products", async (PaypalsDbContext db) =>
{
    return await db.Products.OrderBy(p => p.Name).ToListAsync();
});

app.MapPost("/products", async (Product product, PaypalsDbContext db) =>
{
    db.Products.Add(product);
    await db.SaveChangesAsync();
    return Results.Created($"/products/{product.ProductId}", product);
});

app.MapDelete("/products/{id}", async (int id, PaypalsDbContext db) =>
{
    var product = await db.Products.FindAsync(id);
    if (product == null) return Results.NotFound();

    db.Products.Remove(product);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// =======================
// PERSONEN
// =======================

app.MapGet("/persons", async (PaypalsDbContext db) =>
{
    return await db.People.OrderBy(p => p.Name).ToListAsync();
});

app.MapPost("/persons", async (Person person, PaypalsDbContext db) =>
{
    db.People.Add(person);
    await db.SaveChangesAsync();
    return Results.Created($"/persons/{person.PersonId}", person);
});

app.MapDelete("/persons/{id}", async (int id, PaypalsDbContext db) =>
{
    var person = await db.People.FindAsync(id);
    if (person == null) return Results.NotFound();

    db.People.Remove(person);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// =======================
// SHOPPING LISTS
// =======================

app.MapGet("/shoppinglists", async (PaypalsDbContext db) =>
{
    return await db.ShoppingLists.OrderBy(s => s.Title).ToListAsync();
});

app.MapPost("/shoppinglists", async (ShoppingList shoppingList, PaypalsDbContext db) =>
{
    db.ShoppingLists.Add(shoppingList);
    await db.SaveChangesAsync();
    return Results.Created($"/shoppinglists/{shoppingList.ShoppingListId}", shoppingList);
});

app.MapDelete("/shoppinglists/{id}", async (int id, PaypalsDbContext db) =>
{
    var list = await db.ShoppingLists.FindAsync(id);
    if (list == null) return Results.NotFound();

    db.ShoppingLists.Remove(list);
    await db.SaveChangesAsync();
    return Results.NoContent();
});


// =======================
// PERSONEN IN LIJST
// =======================

app.MapGet("/shoppinglists/{shoppingListId}/persons", async (int shoppingListId, PaypalsDbContext db) =>
{
    return await db.ShoppingListPeople
        .Where(slp => slp.ShoppingListId == shoppingListId)
        .Include(slp => slp.Person)
        .Select(slp => new
        {
            slp.PersonId,
            name = slp.Person.Name,
            slp.PaysForAlcohol
        })
        .ToListAsync();
});

app.MapPost("/shoppinglists/{shoppingListId}/persons/{personId}", async (int shoppingListId, int personId, PaypalsDbContext db) =>
{
    var exists = await db.ShoppingListPeople
        .AnyAsync(slp => slp.ShoppingListId == shoppingListId && slp.PersonId == personId);

    if (exists) return Results.Conflict();

    var slp = new ShoppingListPerson
    {
        ShoppingListId = shoppingListId,
        PersonId = personId,
        PaysForAlcohol = true
    };

    db.ShoppingListPeople.Add(slp);
    await db.SaveChangesAsync();

    return Results.Created("", slp);
});

app.MapPut("/shoppinglists/{shoppingListId}/persons/{personId}/toggle-alcohol", async (int shoppingListId, int personId, PaypalsDbContext db) =>
{
    var slp = await db.ShoppingListPeople
        .FirstOrDefaultAsync(x => x.ShoppingListId == shoppingListId && x.PersonId == personId);

    if (slp == null) return Results.NotFound();

    slp.PaysForAlcohol = !(slp.PaysForAlcohol ?? false);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/shoppinglists/{shoppingListId}/persons/{personId}", async (int shoppingListId, int personId, PaypalsDbContext db) =>
{
    var slp = await db.ShoppingListPeople
        .FirstOrDefaultAsync(x => x.ShoppingListId == shoppingListId && x.PersonId == personId);

    if (slp == null) return Results.NotFound();

    db.ShoppingListPeople.Remove(slp);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


// =======================
// PRODUCTEN IN LIJST
// =======================

app.MapGet("/shoppinglists/{shoppingListId}/products", async (int shoppingListId, PaypalsDbContext db) =>
{
    return await db.ShoppingListProducts
        .Where(slp => slp.ShoppingListId == shoppingListId)
        .Include(slp => slp.Product)
        .Select(slp => new
        {
            slp.ProductId,
            name = slp.Product.Name,
            price = slp.Product.Price,
            slp.Product.IsAlcoholic,
            slp.Quantity
        })
        .ToListAsync();
});

app.MapPost("/shoppinglists/{shoppingListId}/products/{productId}/{quantity}", async (int shoppingListId, int productId, int quantity, PaypalsDbContext db) =>
{
    var exists = await db.ShoppingListProducts
        .AnyAsync(x => x.ShoppingListId == shoppingListId && x.ProductId == productId);

    if (exists) return Results.Conflict();

    var item = new ShoppingListProduct
    {
        ShoppingListId = shoppingListId,
        ProductId = productId,
        Quantity = quantity
    };

    db.ShoppingListProducts.Add(item);
    await db.SaveChangesAsync();

    return Results.Created("", item);
});

app.MapPut("/shoppinglists/{shoppingListId}/products/{productId}/quantity/{quantity}", async (int shoppingListId, int productId, int quantity, PaypalsDbContext db) =>
{
    var item = await db.ShoppingListProducts
        .FirstOrDefaultAsync(x => x.ShoppingListId == shoppingListId && x.ProductId == productId);

    if (item == null) return Results.NotFound();

    item.Quantity = quantity;
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/shoppinglists/{shoppingListId}/products/{productId}", async (int shoppingListId, int productId, PaypalsDbContext db) =>
{
    var item = await db.ShoppingListProducts
        .FirstOrDefaultAsync(x => x.ShoppingListId == shoppingListId && x.ProductId == productId);

    if (item == null) return Results.NotFound();

    db.ShoppingListProducts.Remove(item);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


// =======================
// BEREKENING
// =======================

app.MapGet("/shoppinglists/{shoppingListId}/calculation", async (int shoppingListId, PaypalsDbContext db) =>
{
    var shoppingList = await db.ShoppingLists
        .Include(sl => sl.Payer)
        .Include(sl => sl.ShoppingListPeople)
            .ThenInclude(slp => slp.Person)
        .Include(sl => sl.ShoppingListProducts)
            .ThenInclude(slp => slp.Product)
        .FirstOrDefaultAsync(sl => sl.ShoppingListId == shoppingListId);

    if (shoppingList == null)
        return Results.NotFound();

    var persons = shoppingList.ShoppingListPeople.ToList();
    var products = shoppingList.ShoppingListProducts.ToList();

    var amounts = persons.ToDictionary(
        p => p.PersonId,
        p => 0m
    );

    foreach (var item in products)
    {
        var total = item.Product.Price * item.Quantity;

        var eligiblePersons = item.Product.IsAlcoholic
            ? persons.Where(p => p.PaysForAlcohol == true).ToList()
            : persons;

        if (!eligiblePersons.Any())
            continue;

        var share = total / eligiblePersons.Count;

        foreach (var person in eligiblePersons)
        {
            amounts[person.PersonId] += share;
        }
    }

    var personsResult = persons.Select(p => new
    {
        personId = p.PersonId,
        name = p.Person.Name,

        // Als deze persoon de betaler is, moet die niets terugbetalen
        amountToPay = p.PersonId == shoppingList.PayerId
            ? 0m
            : Math.Round(amounts[p.PersonId], 2)
    }).ToList();

    var repayments = new List<object>();

    if (shoppingList.PayerId != null)
    {
        foreach (var person in persons)
        {
            if (person.PersonId == shoppingList.PayerId)
                continue;

            repayments.Add(new
            {
                fromPersonId = person.PersonId,
                fromName = person.Person.Name,
                toPersonId = shoppingList.PayerId,
                toName = shoppingList.Payer?.Name ?? "Onbekend",
                amount = Math.Round(amounts[person.PersonId], 2)
            });
        }
    }

    return Results.Ok(new
    {
        shoppingListId = shoppingList.ShoppingListId,
        title = shoppingList.Title,
        payerId = shoppingList.PayerId,
        payerName = shoppingList.Payer?.Name,
        totalAmount = Math.Round(products.Sum(p => p.Product.Price * p.Quantity), 2),
        persons = personsResult,
        repayments = repayments
    });
});

app.MapPut("/shoppinglists/{shoppingListId}/payer/{personId}", async (int shoppingListId, int personId, PaypalsDbContext db) =>
{
    var shoppingList = await db.ShoppingLists.FindAsync(shoppingListId);

    if (shoppingList == null)
        return Results.NotFound("Boodschappenlijst niet gevonden.");

    var personInList = await db.ShoppingListPeople
        .AnyAsync(slp => slp.ShoppingListId == shoppingListId && slp.PersonId == personId);

    if (!personInList)
        return Results.BadRequest("Deze persoon zit niet in deze boodschappenlijst.");

    shoppingList.PayerId = personId;

    await db.SaveChangesAsync();

    return Results.Ok(new
    {
        shoppingList.ShoppingListId,
        shoppingList.PayerId
    });
});

app.Run();