using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using APICORE.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Cors;



namespace APICORE.Controllers
{
    [EnableCors("ReglasCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly string CadenaSQL;

        public ProductosController(IConfiguration config)
        {
            CadenaSQL = config.GetConnectionString("CadenaSQL");
        }

        //---------Listar Productos

        [HttpGet]
        [Route("Lista")]
        public IActionResult Lista()
        {
            List<Producto> lista = new List<Producto>();
            try
            {

                using (var conexion = new SqlConnection(CadenaSQL)) { 
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])

                            });
                        }
                    }
                } 
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = lista });

            } catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = lista });
            }
        }

        //--------------Obtener Producto por Id

        [HttpGet]
        [Route("Obtener/{IdProducto:int}")]
        public IActionResult Obtener(int IdProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();
            try
            {

                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_listar_productos", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Producto
                            {
                                IdProducto = Convert.ToInt32(rd["IdProducto"]),
                                CodigoBarra = rd["CodigoBarra"].ToString(),
                                Nombre = rd["Nombre"].ToString(),
                                Marca = rd["Marca"].ToString(),
                                Categoria = rd["Categoria"].ToString(),
                                Precio = Convert.ToDecimal(rd["Precio"])

                            });
                        }
                    }
                }
                producto = lista.Where(item => item.IdProducto == IdProducto).FirstOrDefault();

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", response = producto });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, response = producto });
            }
        }

        //--------------Guardar Producto

        [HttpPost]
        [Route("Guardar")]
        public IActionResult Guardar([FromBody] Producto objeto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();
            try
            {

                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_guardar_productos", conexion);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        //--------------Guardar Producto

        [HttpPut]
        [Route("Editar")]
        public IActionResult Editar([FromBody] Producto objeto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();
            try
            {

                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_editar_productos", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", objeto.IdProducto == 0 ? DBNull.Value : objeto.IdProducto);
                    cmd.Parameters.AddWithValue("CodigoBarra", objeto.CodigoBarra is null ? DBNull.Value : objeto.CodigoBarra);
                    cmd.Parameters.AddWithValue("Nombre", objeto.Nombre is null ? DBNull.Value : objeto.Nombre);
                    cmd.Parameters.AddWithValue("Marca", objeto.Marca is null ? DBNull.Value : objeto.Marca);
                    cmd.Parameters.AddWithValue("Categoria", objeto.Categoria is null ? DBNull.Value : objeto.Categoria);
                    cmd.Parameters.AddWithValue("Precio", objeto.Precio == 0 ? DBNull.Value : objeto.Precio);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "editado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        //--------------Eliminar Producto

        [HttpDelete]
        [Route("Eliminar/{IdProducto:int}")]
        public IActionResult Eliminar(int IdProducto)
        {
            List<Producto> lista = new List<Producto>();
            Producto producto = new Producto();
            try
            {

                using (var conexion = new SqlConnection(CadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("sp_eliminar_producto", conexion);
                    cmd.Parameters.AddWithValue("IdProducto", IdProducto);
                 
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.ExecuteNonQuery();
                }
                return StatusCode(StatusCodes.Status200OK, new { mensaje = "eliminado" });

            }
            catch (Exception error)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message });
            }
        }

        //----------------
    }
}
