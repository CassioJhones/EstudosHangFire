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
}
