using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EstudoHangFire.Controllers;
[Route("api/[controller]")]
[ApiController]
public class JobController : ControllerBase
{

    [HttpGet]
    public void ListaInteiros()
    {
        for (int i = 0; i < 1000000; i++)
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


}
