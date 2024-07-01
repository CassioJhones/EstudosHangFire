using Hangfire;
using HangfireBasicAuthenticationFilter;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHangfire((sp, config) =>
{//ConnectionString presente no appconfig.json
    string? connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("ConexaoPadrao");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

//https://localhost:7279/HangDashboard/ Link do DashBoard do HangFire
app.UseHangfireDashboard("/HangDashboard",new DashboardOptions
{
    DashboardTitle = "EstudosHangFire",
    DarkModeEnabled = true,
    StatsPollingInterval = 500,
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {//usuario e senha para acessar o Dashboard
            User="admin",
            Pass="admin"
        }
    }
});
app.MapControllers();

app.Run();
