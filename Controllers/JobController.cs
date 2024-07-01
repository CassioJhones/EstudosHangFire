using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace EstudoHangFire.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{


    [HttpPost]
    [Route("CriarBackgroundJob")]
    public ActionResult CriarBackgroundJob()
    {
        //Enfileiramento de serviços, métodos
        BackgroundJob.Enqueue(() => Console.WriteLine("Serviço em segundo plano"));
        BackgroundJob.Enqueue(() => ListaInteiros());
        return Ok();
    }
    public void ListaInteiros()
    {
        for (int i = 0; i < 100000; i++)
            Console.WriteLine($"Contagem: {i}");
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
        //Necessario usar [-CronExpression-]

        RecurringJob.AddOrUpdate("Serviço Recorrente 01", () => Console.WriteLine("Serviço Recorrente Realizado"), Cron.Minuto);
        return Ok();
    }

    [HttpPost]
    [Route("ApagarArquivosTemporarios")]
    public ActionResult ApagarArquivosTemporarios()
    {
        //-Exemplo Simples
        RecurringJob.AddOrUpdate("Arquivos Temporarios", () => Console.WriteLine("Arquivos Temporarios Deletados"), Cron.Semanal);
        return Ok();
    }

    [HttpPost]
    [Route("EnviarEmail")]
    public ActionResult EnviarEmail()
    {
        BackgroundJob.Enqueue(() => EnviarEmailDeBoasVindas("cassiojhones@exemplo.com"));
        return Ok();
    }

   //-Exemplo Simples de email
    public void EnviarEmailDeBoasVindas(string email) =>
            Console.WriteLine($"Enviando e-mail de boas-vindas para {email}");
}
