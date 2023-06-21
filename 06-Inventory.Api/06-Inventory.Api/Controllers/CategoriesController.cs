using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Oracle.ManagedDataAccess.Client;

using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Model.Enumerations;
using _06_Inventory.Api.Models;
using AutoMapper;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly NAFContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoriesController> _logger;

        private readonly IStringLocalizer<Resources.SharedMessages> _sharedMessagesLocalizer;
        
        public CategoriesController(NAFContext context,
            IMapper mapper,
            ILogger<CategoriesController> logger
                                 ,IStringLocalizer<Resources.SharedMessages> sharedMessagesLocalizer)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;


            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _sharedMessagesLocalizer = sharedMessagesLocalizer;
        }


        [HttpGet("GetAllCategories")]
        public async Task<ActionResult> GetAllCategories()
        {
            var responsetype = new MessagesResponseTypes("Info","Johan");

            string resultado = responsetype.ToString();

            var response = new MessageResponseDTO();
            response.Message = MessagesResponseTypes.Danger.Name;
            response.Type = MessagesResponseTypes.Danger.Key;


            try
            {
                _logger.LogError("Inicio log");

                var items = await _context.CATEGORIA.OrderBy(x => x.CODIGO).ToListAsync();

                if (!items.Any())
                {
                    response.Type = "Danger";
                    response.Message = "No éxisten categorías por listar";
                    return new OkObjectResult(response);
                }

                var categorias = (from item in items
                                  select new CategoryDTO
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

        [HttpGet("GetCategory/{categoryId}")]
        public async Task<ActionResult> GetCategory(int categoryId)
        {
            var response = new MessageResponseDTO();
            try
            {
                //var item = await _context.CATEGORIA.Where(x => x.CODIGO == categoryId).FirstOrDefaultAsync();
                var item = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoryId);

                if (item is null)
                {
                    //return NotFound();
                    //return NotFound(new { message = $"No Existe clase <{categoryId}> por listar" });
                    response.Code = StatusCodes.Status204NoContent;
                    response.Type = "Info";
                    response.Message = $"No éxiste clase <{categoryId}> por listar";

                    return new OkObjectResult(response);
                }

                CategoryDTO record = new();

                record = new CategoryDTO
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

        [HttpPut("SaveCategory")]
        public async Task<ActionResult<CATEGORIA>> SaveCategory(CategoryDTO CategoryDTO)
        {
            MessageResponseDTO response = new();
            try
            {
                var saveRecord = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == CategoryDTO.Code);

                if (saveRecord == null)
                    return NotFound(new { message = $"No Existe clase <{CategoryDTO.Code}> por listar" });

                saveRecord.DESCRIPCION = CategoryDTO.Description;
                saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
                saveRecord.ULT_MODIF_USUARIO = CategoryDTO.UpdateUser;

                _context.CATEGORIA.Update(saveRecord);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { categoryId = CategoryDTO.Code }, CategoryDTO);
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

        [HttpPost("CreateCategory")]
        public async Task<ActionResult<CATEGORIA>> CreateCategory([FromBody] CategoryDTO CategoryDTO)
        {
            MessageResponseDTO response = new();
            try
            {
                if (CategoryDTO == null)
                    return BadRequest(new { message = "Campo Requerido" });

                var existcategoryId = ExistcategoryId(CategoryDTO.Code);

                if (existcategoryId)
                {
                    return BadRequest(new { message = $"Clase {CategoryDTO.Code} ya éxiste" });
                }

                CATEGORIA CreateCategory = new()
                {
                    CODIGO = CategoryDTO.Code,
                    DESCRIPCION = CategoryDTO.Description,
                    CREACION_TSTAMP = DateTime.Now,
                    CREACION_USUARIO = CategoryDTO.CreateUser
                };

                await _context.CATEGORIA.AddAsync(CreateCategory);
                await _context.SaveChangesAsync();

                //return Ok(CreateCategory);
                return CreatedAtAction(nameof(GetCategory), new { categoryId = CreateCategory.CODIGO }, CategoryDTO);

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

        //[HttpDelete("DeleteCategory/{code}")]
        //public async Task<ActionResult> DeleteCategory(int categoryId)
        //{
        //    MessageResponseDTO response = new();
        //    try
        //    {
        //        var item = await _context.CATEGORIA.FirstOrDefaultAsync(c => c.CODIGO == categoryId);

        //        if (item is null)
        //            return NotFound();

        //        _context.CATEGORIA.RemoveRange(item);

        //        await _context.SaveChangesAsync();

        //        //return Ok();
        //        response.Code = StatusCodes.Status200OK;
        //        response.Type = "Success";
        //        response.Message = $"Las categoría {categoryId} ha sido excluida";

        //        return new OkObjectResult(response);
        //    }

        //    catch (Exception ex)
        //    {
        //        //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
        //        response.Code = StatusCodes.Status500InternalServerError;
        //        response.Type = "Danger";
        //        response.Message = ex.Message;

        //        return new OkObjectResult(response);
        //    }
        //}


        [HttpPost("DeleteCategory/{categoryId}")]
        public async Task<ActionResult> DeleteCategory(int categoryId)
        {
            var deleteRecord = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == categoryId);

            if (deleteRecord == null)
                return NotFound(new { message = $"No Existe clase <{categoryId}> por listar" });

            var response = new MessageResponseDTO();

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                string sql = "BEGIN CAPBorrarCategoria(:pCategoria, :pResult); END;";

                OracleParameter pCategoria = new("pCategoria", categoryId);
                OracleParameter pResult = new("pResult", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) { Size = 4000 };

                //await _context.Database.ExecuteSqlCommandAsync(sql, pCategoria, pResult);
                await _context.Database.ExecuteSqlRawAsync(sql, pCategoria, pResult);

                if (pResult.Value.ToString().Equals("null"))
                {
                    await transaction.CommitAsync();
                    response.Code = StatusCodes.Status200OK;
                    response.Type = "Success";
                    response.Message = $"Las categoría {categoryId} ha sido excluida";
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

        [HttpPost("ImportCategories")]
        public async Task<ActionResult> ImportCategories([FromBody] IEnumerable<CategoryDTO> CategoryDTOs)
        {
            MessageResponseDTO response = new();
            if (CategoryDTOs is null)
                return NotFound();

            try
            {
                var addCategorias = from A in CategoryDTOs
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


        private bool ExistcategoryId(int categoryId)
        {
            return _context.CATEGORIA.Any(x => x.CODIGO == categoryId);
        }
    }
}
