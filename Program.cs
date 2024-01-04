using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<RocketDb>(opt => opt.UseInMemoryDatabase("Rockets"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

app.MapGet("/", () => "Rockets API");

RouteGroupBuilder rocketItems = app.MapGroup("/rockets");
rocketItems.MapGet("/", GetAllRockets);
rocketItems.MapGet("/launched", GetLaunchedRockets);
rocketItems.MapGet("/{id}", GetRocket);
rocketItems.MapPost("/", CreateRocket);
rocketItems.MapPut("/{id}", UpdateRocket);
rocketItems.MapDelete("/{id}", DeleteRocket);

static async Task<IResult> GetAllRockets(RocketDb db)
{
    return TypedResults.Ok(await db.Rockets.Select(x => new RocketDTO(x)).ToArrayAsync());
}

static async Task<IResult> GetLaunchedRockets(RocketDb db)
{
    return TypedResults.Ok(await db.Rockets.Where(t => t.IsLaunched).Select(x => new RocketDTO(x)).ToListAsync());
}

static async Task<IResult> GetRocket(int id, RocketDb db)
{
    return await db.Rockets.FindAsync(id)
        is Rocket rocket
            ? TypedResults.Ok(new RocketDTO(rocket))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateRocket(RocketDTO rocketDTO, RocketDb db)
{
    var rocket = new Rocket
    {
        IsLaunched = rocketDTO.IsLaunched,
        Name = rocketDTO.Name
    };
    db.Rockets.Add(rocket);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/rockets/{rocket.Id}", rocketDTO);
}

static async Task<IResult> UpdateRocket(int id, RocketDTO rocketDTO, RocketDb db)
{
    var rocket = await db.Rockets.FindAsync(id);
    if (rocket is null) return TypedResults.NotFound();

    rocket.Name = rocketDTO.Name;
    rocket.IsLaunched = rocketDTO.IsLaunched;
    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteRocket(int id, RocketDb db)
{
    if (await db.Rockets.FindAsync(id) is Rocket rocket)
    {
        db.Rockets.Remove(rocket);
        return TypedResults.NoContent();
    }
    return TypedResults.NotFound();
}


app.Run();
