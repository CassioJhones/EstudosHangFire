
# Estudos sobre Hangfire

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)

Funcionamento do pacote HangFire


## Uso/Exemplos

```csharp
builder.Services.AddHangfire((sp, config) =>
{//ConnectionString presente no appconfig.json
    string? connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("ConexaoPadrao");
    config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();
```


```csharp
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
```

