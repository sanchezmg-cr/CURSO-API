using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Drawing.Drawing2D;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticulosController : ControllerBase
    {
        private readonly NAFContext _context;
        private readonly IMapper _mapper;

        public ArticulosController(NAFContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("GetAllArticulos")]
        public async Task<ActionResult> GetAllArticulos()
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                //var items = await _context.ARTICULOS.OrderBy(x => x.CODIGO).ToListAsync();


                //if (!items.Any())
                //{
                //    //return NotFound();
                //    //return NotFound(new { message = "No Existen Articulos por listar" });
                //    responseDTO.Type = "Danger";
                //    responseDTO.Message = "No éxisten artículos por listar";
                //    return new OkObjectResult(responseDTO);
                //}

                //var articulos = _mapper.Map<List<ArticulosDTO>>(items);

                ////// opcion 1 poco eficiente
                ////foreach (var item in articulos)
                ////{
                ////    var category = await _context.CATEGORIA.FirstOrDefaultAsync(x => x.CODIGO == item.Category);
                ////    item.CategoryDescription = category.DESCRIPCION;
                ////}


                //// forma 2 poco eficiente
                //var articulos = await (from item in _context.ARTICULOS
                //                 select new ArticulosDTO
                //                 {
                //                     Code = item.CODIGO,
                //                     Description = item.DESCRIPCION,
                //                     Category = item.CATEGORIA,
                //                     CategoryDescription = _context.CATEGORIA.FirstOrDefault(x => x.CODIGO == item.CATEGORIA).DESCRIPCION,
                //                     Brand = item.MARCA,
                //                     Weight = item.PESO,
                //                     BarCode = item.CODIGO_BARRAS,
                //                     CreateDate = item.CREACION_TSTAMP,
                //                     CreateUser = item.CREACION_USUARIO,
                //                     UpdateDate = item.ULT_MODIF_TSTAMP,
                //                     UpdateUser = item.ULT_MODIF_USUARIO
                //                 }).ToListAsync();

                var articulos = await (from item in _context.ARTICULOS
                                       join cate in _context.CATEGORIA
                                       on item.CATEGORIA equals cate.CODIGO
                                       select new ArticulosDTO
                                       {
                                           Code = item.CODIGO,
                                           Description = item.DESCRIPCION,
                                           Category = item.CATEGORIA,
                                           CategoryDescription = cate.DESCRIPCION,
                                           Brand = item.MARCA,
                                           Weight = item.PESO,
                                           BarCode = item.CODIGO_BARRAS,
                                           CreateDate = item.CREACION_TSTAMP,
                                           CreateUser = item.CREACION_USUARIO,
                                           UpdateDate = item.ULT_MODIF_TSTAMP,
                                           UpdateUser = item.ULT_MODIF_USUARIO
                                       }).ToListAsync();

                return Ok(articulos);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                responseDTO.Code = StatusCodes.Status500InternalServerError;
                responseDTO.Type = "Danger";
                responseDTO.Message = ex.Message;

                return new OkObjectResult(responseDTO);
            }

        }

        [HttpGet("GetArticulo/{articuloId}")]
        public async Task<ActionResult> GetCategoria(int articuloId)
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                //var item = await _context.ARTICULOS.FirstOrDefaultAsync(x => x.CODIGO == articuloId);
                var articulo = await (from item in _context.ARTICULOS
                                      join cate in _context.CATEGORIA
                                      on item.CATEGORIA equals cate.CODIGO
                                      where item.CODIGO == articuloId
                                      select new ArticulosDTO
                                      {
                                          Code = item.CODIGO,
                                          Description = item.DESCRIPCION,
                                          Category = item.CATEGORIA,
                                          CategoryDescription = cate.DESCRIPCION,
                                          Brand = item.MARCA,
                                          Weight = item.PESO,
                                          BarCode = item.CODIGO_BARRAS,
                                          CreateDate = item.CREACION_TSTAMP,
                                          CreateUser = item.CREACION_USUARIO,
                                          UpdateDate = item.ULT_MODIF_TSTAMP,
                                          UpdateUser = item.ULT_MODIF_USUARIO
                                      }).ToListAsync();
                if (articulo == null)
                {
                    //return NotFound();
                    responseDTO.Type = "Danger";
                    responseDTO.Message = "No éxiste artículo por listar";
                    return new OkObjectResult(responseDTO);
                }

                //var record = _mapper.Map<ArticulosDTO>(item);
                //ArticulosDTO record = new();

                //record = new ArticulosDTO
                //{
                //    Code = item.CODIGO,
                //    Description = item.DESCRIPCION,
                //    Category = item.CATEGORIA,
                //    Brand = item.MARCA,
                //    Weight = item.PESO,
                //    BarCode = item.CODIGO_BARRAS
                //};

                return Ok(articulo);
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                responseDTO.Code = StatusCodes.Status500InternalServerError;
                responseDTO.Type = "Danger";
                responseDTO.Message = ex.Message;

                return new OkObjectResult(responseDTO);
            }
        }

        [HttpPut("SaveArticulo")]
        public async Task<ActionResult<ARTICULOS>> SaveArticulo(ArticulosDTO articulosDTO)
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                // AsNoTracking para no esperar 
                var saveRecord = await _context.ARTICULOS.AsNoTracking().FirstOrDefaultAsync(x => x.CODIGO == articulosDTO.Code);
                if (saveRecord == null)
                {
                    //return NotFound();
                    responseDTO.Type = "Danger";
                    responseDTO.Message = "No éxiste artículo por listar";
                    return new OkObjectResult(responseDTO);
                }

                var articulo = _mapper.Map<ARTICULOS>(articulosDTO);

                //saveRecord.DESCRIPCION = articulosDTO.Description;
                //saveRecord.CATEGORIA = articulosDTO.Category;
                //saveRecord.MARCA = articulosDTO.Brand;
                //saveRecord.PESO = articulosDTO.Weight;
                //saveRecord.CODIGO_BARRAS = articulosDTO.BarCode;
                //saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
                //saveRecord.ULT_MODIF_USUARIO = articulosDTO.UpdateUser;

                try
                {
                    _context.ARTICULOS.Update(articulo);
                    await _context.SaveChangesAsync();

                    return Ok(articulo);
                }
                catch (Exception ex)
                {
                    //return BadRequest(ex.Message);
                    responseDTO.Code = StatusCodes.Status500InternalServerError;
                    responseDTO.Type = "Danger";
                    responseDTO.Message = ex.Message;

                    return new OkObjectResult(responseDTO);
                }
            }
            catch (Exception ex)
            {
                //return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
                responseDTO.Code = StatusCodes.Status500InternalServerError;
                responseDTO.Type = "Danger";
                responseDTO.Message = ex.Message;

                return new OkObjectResult(responseDTO);
            }
        }

        [HttpPost("CreateArticulo")]
        public async Task<ActionResult<CATEGORIA>> CreateArticulo(ArticulosDTO articulosDTO)
        {
            MessageResponseDTO responseDTO = new();
            if (articulosDTO == null)
                return NoContent();

            var existCategoriaId = ExistArticuloID(articulosDTO.Code);

            if (existCategoriaId)
            {
                return BadRequest(new { message = $"Artículo {articulosDTO.Code} ya éxiste" });
            }


            //ARTICULOS createArticulo;
            var createArticulo = _mapper.Map<ARTICULOS>(articulosDTO);
            //createArticulo = new ARTICULOS
            //{
            //    CODIGO = articulosDTO.Code,
            //    DESCRIPCION = articulosDTO.Description,
            //    CATEGORIA = articulosDTO.Category,
            //    MARCA = articulosDTO.Brand,
            //    PESO = articulosDTO.Weight,
            //    CODIGO_BARRAS = articulosDTO.BarCode,
            //    ULT_MODIF_TSTAMP = DateTime.Now,
            //    ULT_MODIF_USUARIO = articulosDTO.UpdateUser
            //};

            try
            {
                await _context.ARTICULOS.AddRangeAsync(createArticulo);
                await _context.SaveChangesAsync();

                return Ok(createArticulo);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
                responseDTO.Code = StatusCodes.Status500InternalServerError;
                responseDTO.Type = "Danger";
                responseDTO.Message = ex.Message;

                return new OkObjectResult(responseDTO);
            }
        }

        [HttpPost("DeleteArticulo/{ArticuloID}")]
        public async Task<ActionResult> DeleteArticulo(int articuloId)
        {
            var deleteRecord = await _context.ARTICULOS.FirstOrDefaultAsync(x => x.CODIGO == articuloId);

            if (deleteRecord == null)
                return NotFound(new { message = $"No Existe clase <{articuloId}> por listar" });

            var response = new MessageResponseDTO();

            try
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                string sql = "BEGIN CAPBorrarArticulos(:pArticulo, :pResult); END;";

                OracleParameter pArticulo = new("pArticulo", articuloId);
                OracleParameter pResult = new("pResult", OracleDbType.Varchar2, System.Data.ParameterDirection.InputOutput) { Size = 4000 };

                //await _context.Database.ExecuteSqlCommandAsync(sql, pArticulo, pResult);
                await _context.Database.ExecuteSqlRawAsync(sql, pArticulo, pResult);

                if (pResult.Value.ToString().Equals("null"))
                {
                    await transaction.CommitAsync();
                    response.Code = StatusCodes.Status200OK;
                    response.Type = "Success";
                    response.Message = $"El artículo {articuloId} ha sido excluido";
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


        [HttpPost("ImportArticulos")]
        public async Task<ActionResult> ImportArticulos([FromBody] IEnumerable<ArticulosDTO> articulosDTOs)
        {
            if (articulosDTOs is null)
            {
                return NotFound();
            }
            else
            {
                var addArticulos = from A in articulosDTOs
                                   select new ARTICULOS
                                   {
                                       CODIGO = A.Code,
                                       DESCRIPCION = A.Description,
                                       CATEGORIA = A.Category,
                                       MARCA = A.Brand,
                                       PESO = A.Weight,
                                       CODIGO_BARRAS = A.BarCode,
                                       CREACION_TSTAMP = DateTime.Now,
                                       CREACION_USUARIO = A.CreateUser
                                   };
                try
                {
                    await _context.ARTICULOS.AddRangeAsync(addArticulos.ToList());
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

                return Ok();
            }
        }

        private bool ExistArticuloID(long articuloId)
        {
            return _context.ARTICULOS.Any(x => x.CODIGO == articuloId);
        }
    }
}
