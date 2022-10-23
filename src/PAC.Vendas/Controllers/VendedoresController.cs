using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;

namespace PAC.Vendas.Controllers
{
    [Route("[controller]")]
    public class VendedoresController : ControllerBase
    {
        public VendedoresController() { }

        [NonAction]
        [CapSubscribe("funcionario.registrado")]
        public void CheckReceivedMessage(object mensagem)
        {
            Console.WriteLine(mensagem);
        }
    }
}
