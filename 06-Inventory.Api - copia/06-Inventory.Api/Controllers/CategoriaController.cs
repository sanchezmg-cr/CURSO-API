using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Oracle.ManagedDataAccess.Client;

using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Model.Enumerations;
using _06_Inventory.Api.Models;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly NAFContext _context;
        private readonly IStringLocalizer<Resources.SharedMessages> _sharedMessagesLocalizer;
        
        public CategoriaController(NAFContext context
                                 ,IStringLocalizer<Resources.SharedMessages> sharedMessagesLocalizer)
        {
            _context = context;
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _sharedMessagesLocalizer = sharedMessagesLocalizer;
        }


        [HttpGet("GetAllCategorias")]
        public async Task<ActionResult> GetAllCategorias()
        {
            var responsetype = new MessagesResponseTypes("Info","Johan");

            string resultado = responsetype.ToString();

            var response = new MessageResponseDTO();
            response.Message = MessagesResponseTypes.Danger.Name;
            response.Type = MessagesResponseTypes.Danger.Key;


            try
            {
                var items = await _context.CATEGORIA.OrderBy(x => x.CODIGO).ToListAsync();

                if (!items.Any())
                {
                    response.Type = "Danger";
                    response.Message = "No éxisten categorías por listar";
                    return new OkObjectResult(response);
                }

                var categorias = (from item in items
                                  select new CategoriaDTO
                                  {
                                      Code = item.CODIGO,
                                      Description = item.DESCRIPCION
                                  }).ToList();

                return Ok(categorias);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }

        [HttpGet("GetCategoria/{categoriaId}")]
        public async Task<ActionResult> GetCategoria(int categoriaId)
        {
            var response = new MessageResponseDTO();
            try
            {
                //var item = await _context.CATEGORIA.Where(x => x.CODIGO == categoriaId).FirstOrDefaultAsync();
                var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoriaId);

                if (item is null)
                {
                    //return NotFound();
                    //return NotFound(new { message = $"No Existe clase <{categoriaId}> por listar" });
                    response.Code = StatusCodes.Status204NoContent;
                    response.Type = "Info";
                    response.Message = $"No éxiste clase <{categoriaId}> por listar";

                    return new OkObjectResult(response);
                }

                CategoriaDTO record = new();

                record = new CategoriaDTO
                {
                    Code = item.CODIGO,
                    Description = item.DESCRIPCION,
                    CreateDate = item.CREACION_TSTAMP,
                    CreateUser = item.CREACION_USUARIO,
                    UpdateDate = item.ULT_MODIF_TSTAMP,
                    UpdateUser = item.ULT_MODIF_USUARIO
                };

                return Ok(record);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }

        [HttpPut("SaveCategoria")]
        public async Task<ActionResult<CATEGORIA>> SaveCategoria(CategoriaDTO categoriaDTO)
        {
            MessageResponseDTO response = new();
            try
            {
                var saveRecord = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoriaDTO.Code);

                if (saveRecord == null)
                    return NotFound(new { message = $"No Existe clase <{categoriaDTO.Code}> por listar" });

                saveRecord.DESCRIPCION = categoriaDTO.Description;
                saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
                saveRecord.ULT_MODIF_USUARIO = categoriaDTO.UpdateUser;

                _context.CATEGORIA.Update(saveRecord);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategoria), new { categoriaId = categoriaDTO.Code }, categoriaDTO);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }

        [HttpPost("CreateCategoria")]
        public async Task<ActionResult<CATEGORIA>> CreateCategoria([FromBody] CategoriaDTO categoriaDTO)
        {
            MessageResponseDTO response = new();
            try
            {
                if (categoriaDTO == null)
                    return BadRequest(new { message = "Campo Requerido" });

                var existCategoriaId = ExistCategoriaID(categoriaDTO.Code);

                if (existCategoriaId)
                {
                    return BadRequest(new { message = $"Clase {categoriaDTO.Code} ya éxiste" });
                }

                CATEGORIA createCategoria = new()
                {
                    CODIGO = categoriaDTO.Code,
                    DESCRIPCION = categoriaDTO.Description,
                    CREACION_TSTAMP = DateTime.Now,
                    CREACION_USUARIO = categoriaDTO.CreateUser
                };

                await _context.CATEGORIA.AddAsync(createCategoria);
                await _context.SaveChangesAsync();

                //return Ok(createCategoria);
                return CreatedAtAction(nameof(GetCategoria), new { categoriaId = createCategoria.CODIGO }, categoriaDTO);

            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }

        [HttpDelete("DeleteCategory/{code}")]
        public async Task<ActionResult> DeleteCategory(int categoriaId)
        {
            MessageResponseDTO response = new();
            try
            {
                var item = await _context.CATEGORIA.FirstOrDefaultAsync(c => c.CODIGO == categoriaId);

                if (item is null)
                    return NotFound();

                _context.CATEGORIA.RemoveRange(item);

                await _context.SaveChangesAsync();

                //return Ok();
                response.Code = StatusCodes.Status200OK;
                response.Type = "Success";
                response.Message = $"Las categoría {categoriaId} ha sido excluida";

                return new OkObjectResult(response);
            }

            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }


        [HttpPost("DeleteCategoria/{categoriaID}")]
        public async Task<ActionResult> DeleteCategoria(int categoriaId)
        {
            var deleteRecord = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoriaId);

            if (deleteRecord == null)
                return NotFound(new { message = $"No Existe clase <{categoriaId}> por listar" });

            var response = new MessageResponseDTO();

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                string sql = "BEGIN CAPBorrarCategoria(:pCategoria, :pResult); END;";

                OracleParameter pCategoria = new("pCategoria", categoriaId);
                OracleParameter pResult = new("pResult", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) { Size = 4000 };

                //await _context.Database.ExecuteSqlCommandAsync(sql, pCategoria, pResult);
                await _context.Database.ExecuteSqlRawAsync(sql, pCategoria, pResult);

                if (pResult.Value.ToString().Equals("null"))
                {
                    await transaction.CommitAsync();
                    response.Code = StatusCodes.Status200OK;
                    response.Type = "Success";
                    response.Message = $"Las categoría {categoriaId} ha sido excluida";
                }
                else
                {
                    response.Type = "Danger";
                    response.Message = pResult.Value.ToString();
                    await transaction.RollbackAsync();
                };

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }

        [HttpPost("ImportCategorias")]
        public async Task<ActionResult> ImportCategorias([FromBody] IEnumerable<CategoriaDTO> categoriaDTOs)
        {
            MessageResponseDTO response = new();
            if (categoriaDTOs is null)
                return NotFound();

            try
            {
                var addCategorias = from A in categoriaDTOs
                                    select new CATEGORIA
                                    {
                                        CODIGO = A.Code,
                                        DESCRIPCION = A.Description,
                                        CREACION_TSTAMP = DateTime.Now,
                                        CREACION_USUARIO = A.CreateUser
                                    };


                await _context.CATEGORIA.AddRangeAsync(addCategorias.ToList());
                await _context.SaveChangesAsync();
                
                //return Ok();
                response.Code = StatusCodes.Status200OK;
                response.Type = "Success";
                response.Message = "Las categorías han sido incluidas";

                return new OkObjectResult(response);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                response.Code = StatusCodes.Status500InternalServerError;
                response.Type = "Danger";
                response.Message = ex.Message;

                return new OkObjectResult(response);
            }
        }


        private bool ExistCategoriaID(int categoriaId)
        {
            return _context.CATEGORIA.Any(x => x.CODIGO == categoriaId);
        }
    }
}
