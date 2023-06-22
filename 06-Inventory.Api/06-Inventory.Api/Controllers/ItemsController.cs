using _06_Inventory.Api.DTO;
using _06_Inventory.Api.Infrastructure;
using _06_Inventory.Api.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Oracle.ManagedDataAccess.Client;
using System.Drawing.Drawing2D;

namespace _06_Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly NAFContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<Resources.SharedMessages> _sharedMessagesLocalizer;


        public ItemsController(NAFContext context, IMapper mapper, IStringLocalizer<Resources.SharedMessages> sharedMessagesLocalizer)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _mapper = mapper;
            _sharedMessagesLocalizer = sharedMessagesLocalizer;

        }

        [HttpGet("GetAllItems")]
        public async Task<ActionResult> GetAllItems()
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                var dato4 = _sharedMessagesLocalizer.GetString("Deduction", "Deduction").Value;
                var dato3 = _sharedMessagesLocalizer.GetString("Ingreso").Value;
                var dato2 = _sharedMessagesLocalizer.GetString("Ingreso").ToString();
                var dato1 = _sharedMessagesLocalizer.GetString("Income");


                var articulos = await (from item in _context.ARTICULOS
                                       join cate in _context.CATEGORIA on item.CATEGORIA equals cate.CODIGO
                                       select new ItemsDTO
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

        [HttpGet("GetItem/{articuloId}")]
        public async Task<ActionResult> GetItem(int articuloId)
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                //var item = await _context.ARTICULOS.FirstOrDefaultAsync(x => x.CODIGO == articuloId);
                var articulo = await (from item in _context.ARTICULOS
                                      join cate in _context.CATEGORIA
                                      on item.CATEGORIA equals cate.CODIGO
                                      where item.CODIGO == articuloId
                                      select new ItemsDTO
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
                                      }).FirstOrDefaultAsync();
                if (articulo == null)
                {
                    //return NotFound();
                    responseDTO.Type = "Danger";
                    responseDTO.Message = "No éxiste artículo por listar";

                    //var dato4 = _sharedMessagesLocalizer.GetString("Income");
                    //var dato3 = _sharedMessagesLocalizer.GetString("Ingreso").Value;
                    //var dato2 = _sharedMessagesLocalizer.GetString("Ingreso").ToString();
                    //var dato1 = _sharedMessagesLocalizer.GetString("Income", articuloId);
                    //var dato0 = _sharedMessagesLocalizer.GetString("DeleteItemExito", $"{articuloId}");

                    //responseDTO.Message = _sharedMessagesLocalizer.GetString("DeleteItemExito", articuloId);

                    return new OkObjectResult(responseDTO);
                }


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

        [HttpPut("SaveItem")]
        public async Task<ActionResult<ARTICULOS>> SaveItem(ItemsDTO ItemsDTO)
        {
            MessageResponseDTO responseDTO = new();
            try
            {
                // AsNoTracking para no esperar 
                var saveRecord = await _context.ARTICULOS.AsNoTracking().FirstOrDefaultAsync(x => x.CODIGO == ItemsDTO.Code);
                if (saveRecord == null)
                {
                    //return NotFound();
                    responseDTO.Type = "Danger";
                    responseDTO.Message = "No éxiste artículo por listar";
                    return new OkObjectResult(responseDTO);
                }

                var articulo = _mapper.Map<ARTICULOS>(ItemsDTO);

                //saveRecord.DESCRIPCION = ItemsDTO.Description;
                //saveRecord.CATEGORIA = ItemsDTO.Category;
                //saveRecord.MARCA = ItemsDTO.Brand;
                //saveRecord.PESO = ItemsDTO.Weight;
                //saveRecord.CODIGO_BARRAS = ItemsDTO.BarCode;
                //saveRecord.ULT_MODIF_TSTAMP = DateTime.Now;
                //saveRecord.ULT_MODIF_USUARIO = ItemsDTO.UpdateUser;

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

        [HttpPost("CreateItem")]
        public async Task<ActionResult<ARTICULOS>> CreateItem(ItemsDTO ItemsDTO)
        {
            MessageResponseDTO responseDTO = new();
            if (ItemsDTO == null)
                return NoContent();

            var existcategoryId = ExistItemID(ItemsDTO.Code);

            if (existcategoryId)
            {
                return BadRequest(new { message = $"Artículo {ItemsDTO.Code} ya éxiste" });
            }


            //ARTICULOS CreateItem;
            var CreateItem = _mapper.Map<ARTICULOS>(ItemsDTO);
            //CreateItem = new ARTICULOS
            //{
            //    CODIGO = ItemsDTO.Code,
            //    DESCRIPCION = ItemsDTO.Description,
            //    CATEGORIA = ItemsDTO.Category,
            //    MARCA = ItemsDTO.Brand,
            //    PESO = ItemsDTO.Weight,
            //    CODIGO_BARRAS = ItemsDTO.BarCode,
            //    ULT_MODIF_TSTAMP = DateTime.Now,
            //    ULT_MODIF_USUARIO = ItemsDTO.UpdateUser
            //};

            try
            {
                await _context.ARTICULOS.AddRangeAsync(CreateItem);
                await _context.SaveChangesAsync();

                return Ok(CreateItem);
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

        [HttpPost("DeleteItem/{ArticuloID}")]
        public async Task<ActionResult> DeleteItem(int articuloId)
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
                    response.Message = _sharedMessagesLocalizer.GetString("DeleteItemExitoso");

                    response.Message = _sharedMessagesLocalizer.GetString("DeleteItemExitoso", pResult.Value.ToString()) + $"{articuloId}";
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


        [HttpPost("ImportItems")]
        public async Task<ActionResult> ImportItems([FromBody] IEnumerable<ItemsDTO> ItemsDTOs)
        {
            if (ItemsDTOs is null)
            {
                return NotFound();
            }
            else
            {
                var addArticulos = from A in ItemsDTOs
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

        private bool ExistItemID(long articuloId)
        {
            return _context.ARTICULOS.Any(x => x.CODIGO == articuloId);
        }
    }
}
