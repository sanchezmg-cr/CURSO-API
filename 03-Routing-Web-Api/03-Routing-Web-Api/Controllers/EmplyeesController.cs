using _03_Routing_Web_Api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _03_Routing_Web_Api.Controllers
{
    // preferible utilizar el /[action] y no usarlo en el Http
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class EmplyeesController : ControllerBase
    {
        List<EmployeeDTO> ListEmployees;

        public EmplyeesController()
        {
            ListEmployees = new List<EmployeeDTO>()
            {
                new EmployeeDTO() { Id = 1, Name = "Cristian", Position = "Programador" },
                new EmployeeDTO() { Id = 2, Name = "Miguel", Position = "Contador" },
                new EmployeeDTO() { Id = 3, Name = "Ana", Position = "Programador" },
            };
        }

        // con [action] [HttpGet]
        [HttpGet("GetAllEmployees")]
        public ActionResult GetAllEmployees()
        {
            return Ok(ListEmployees);
        }

        //[HttpGet]
        [HttpGet("GetEmployeeById/{id}/{name}")]
        public ActionResult GetEmployeeById([Required] decimal id, [Required] string name)
        {
            var employee = ListEmployees.Find(x => x.Id == id);

            return Ok(employee);
        }
    }
}
