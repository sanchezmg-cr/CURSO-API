using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace _02_htpp_Verbs_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HttpVerbsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get() {
            //
            return Ok("Metodo Get");
        }

        
        [HttpPost]
        public ActionResult Post()
        {
            return Ok("Metodo Post");
        }

        [HttpPut]
        public ActionResult Put()
        {
            return Ok("Metodo Put");
        }

        [HttpDelete]
        public ActionResult Delete()
        {
            return Ok("Metodo Delete");
        }

        // parcialmente un registro
        [HttpPatch]
        public ActionResult Patch()
        {
            return Ok("Metodo Patch");
        }
    }


}
