using Microsoft.EntityFrameworkCore;
using ReviewApplication.Interface;
using ReviewApplication.MappingProfile;
using ReviewApplication.Services;
using ReviewInfrastructure.DBContext;
using ReviewInfrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program)); // hoặc assembly chứa profile


builder.Services.AddDbContext<ReviewDBContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReviewDBConnection")));

builder.Services.AddAutoMapper(typeof(ReviewMappingProfile));


builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<ReviewRepository>();

var app = builder.Build();

// Tự động áp dụng migrations VÀ XỬ LÝ LỖI - Cách 2
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider; // <-- chỉ tồn tại trong scope này
    try
    {
        // Lấy DbContext đã đăng ký
        var context = services.GetRequiredService<ReviewDBContext>();

        // Tự động áp dụng migration
        context.Database.Migrate();
        // -------------------------------
        Console.WriteLine("Database migration applied successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred during database migration.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
