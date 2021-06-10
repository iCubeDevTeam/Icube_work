using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Controllers
{
    public class TestController : ApiControllerBase
    {
        [Authorize(Policy ="HasTagService")]
        [HttpGet("TagService")]
        public ContentResult TagService()
        {

            return Content("Allow to Tag service");
        }

        [Authorize(Policy ="HasInterfaceService")]
        [HttpGet("InterfaceService")]
        public ContentResult InterfaceService()
        {

            return Content("Allow to Interface service");
        }

        [Authorize(Policy ="HasIntegrationService")]
        [HttpGet("IntegrationService")]
        public ContentResult IntegrationService()
        {

            return Content("Allow to Integration service");
        }
        
    }
}