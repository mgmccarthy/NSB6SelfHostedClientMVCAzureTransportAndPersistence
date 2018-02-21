using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NSB6SelfHostedClientMVC.Messages.Commands;
using NServiceBus;

namespace NSB6SelfHostedClientMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEndpointInstance endpoint;

        public HomeController(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint;
        }

        public async Task<ActionResult> Index()
        {
            var sendOptions = new SendOptions();
            sendOptions.DelayDeliveryWith(TimeSpan.FromSeconds(10));
            await endpoint.Send(new TestCommand(), sendOptions).ConfigureAwait(false);

            return View();
        }
    }
}