using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EstudoHangFire.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{
    [HttpGet]
    public void ListaInteiros()
    {
        for (int i = 0; i < 100000; i++)
        {
            Console.WriteLine(i);
        }
    }

    [HttpPost]
    [Route("CriarBackgroundJob")]
    public ActionResult CriarBackgroundJob()
    {
        //Enfileiramento de serviços, métodos
        BackgroundJob.Enqueue(() => Console.WriteLine("Serviço em segundo plano"));
        BackgroundJob.Enqueue(() => ListaInteiros());
        return Ok();
    }

    [HttpPost]
    [Route("CriarScheduleJob")]
    public ActionResult CriarScheduleJob()
    {
        //Agendamento de serviços, métodos
        DateTime agendamento = DateTime.UtcNow.AddSeconds(5);
        DateTimeOffset dateTimeOffset = new(agendamento);

        BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada"), dateTimeOffset);

        return Ok();
    }

    [HttpPost]
    [Route("CriarContinuationJob")]
    public ActionResult CriarContinuationJob()
    {
        //Serviços executados em sequencia
        DateTime agendamento = DateTime.UtcNow.AddSeconds(3);
        DateTimeOffset dateTimeOffset = new(agendamento);

        string JobId_01 = BackgroundJob.Schedule(() => Console.WriteLine("Tarefa Agendada"), dateTimeOffset);
        string JobId_02 = BackgroundJob.ContinueJobWith(JobId_01, () => Console.WriteLine("Segundo Job"));
        string JobId_03 = BackgroundJob.ContinueJobWith(JobId_02, () => Console.WriteLine("Terceiro Job"));
        string JobId_04 = BackgroundJob.ContinueJobWith(JobId_03, () => Console.WriteLine("Quarto Job"));

        return Ok();
    }

    [HttpPost]
    [Route("CriarRecurringJob")]
    public ActionResult CriarRecurringJob()
    {
        //Serviços recorrentes por tempo determinado exemplo: a cada 5s, a cada 1 semana, a cada 1 dia;
        //Necessario usar CronExpression
        string cadaUmMinuto = "* * * * *";
        string cadaUmaHora = "0 0 * * * ? *";
        string cadaUmDia = "0 0 0 * * ? *";
        string cadaUmaSemana = "0 0 0 ? * MON *";

        RecurringJob.AddOrUpdate("Serviço Recorrente 01",
            ()=> Console.WriteLine("Serviço Recorrente Realizado"), cadaUmMinuto);

        return Ok();
    }
}
