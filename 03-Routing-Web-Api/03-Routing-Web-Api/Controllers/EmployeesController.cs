using _03_Routing_Web_Api.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace _03_Routing_Web_Api.Controllers
{
    // preferible utilizar el /[action] y no usarlo en el Http
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        List<EmployeeDTO> ListEmployees;

        public EmployeesController()
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
        // incluir los parametros para reducir el url
        //[HttpGet("GetEmployeeById/{id}/{name}")]

        // para fechas
        [HttpGet("GetEmployeeById/{id}&{name}")]
        public ActionResult GetEmployeeById([Required] decimal id, [Required] string name)
        {
            var employee = ListEmployees.Find(x => x.Id == id);

            return Ok(employee);
        }

        [HttpPost("CreateEmployee")]
        public ActionResult CreateEmployee( EmployeeDTO employee) 
        {
            //var employee = new EmployeeDTO() {};
            //employee.Id = 4;
            //employee.Name = "Chalo";
            //employee.Position = "Angelito";

            ListEmployees.Add(employee);

            
            
            
            return Ok(ListEmployees);
        }
    }
}
