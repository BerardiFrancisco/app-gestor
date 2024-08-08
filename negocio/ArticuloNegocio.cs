using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;
using System.Xml.Linq;

namespace negocio
{
    public class ArticuloNegocio 
    {

        public List<Articulo> listar()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("Select A.Id as IdArticulo,Codigo, Nombre ,A.Descripcion, ImagenUrl,C.Descripcion as Categoria,M.Descripcion as Marca , Precio, C.Id as IdCategoria, M.Id as IdMarca  from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = M.Id");
                datos.EjecutarLectura();


                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArticulo"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    if ((datos.Lector["Precio"] != DBNull.Value))
                    {
                        aux.Precio = (decimal)datos.Lector["Precio"];
                    }
                    
                    

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
                catch (Exception ex)
                {
                    throw ex;
                }
        }

        public List<Articulo> filtrarPorMarca(string marca)
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("Select A.Id as IdArticulo, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, Precio, C.Id as IdCategoria, M.Id as IdMarca " +
                                     "from ARTICULOS A " +
                                     "join CATEGORIAS C on A.IdCategoria = C.Id " +
                                     "join MARCAS M on A.IdMarca = M.Id " +
                                     "where M.Descripcion = @marca");
                datos.SetearParametro("@marca", marca);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArticulo"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,ImagenUrl,IdCategoria,IdMarca, Precio) values (@Codigo,@Nombre,@Descripcion,@ImagenUrl,@IdCategoria,@IdMarca,@precio)");
                datos.SetearParametro("@Codigo",nuevo.Codigo);
                datos.SetearParametro("@Nombre",nuevo.Nombre);
                datos.SetearParametro("@Descripcion",nuevo.Descripcion);
                datos.SetearParametro("@ImagenUrl",nuevo.UrlImagen);
                datos.SetearParametro("@precio",nuevo.Precio);
                datos.SetearParametro("@IdCategoria",nuevo.Categoria.Id);
                datos.SetearParametro("@IdMarca",nuevo.Marca.Id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }finally
            {
                datos.CerrarConexion();
            }
        }

        public void modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE ARTICULOS set Codigo = @Codigo, Nombre = @Nombre, Descripcion = @Descripcion, IdMarca = @IdMarca, IdCategoria = @IdCategoria, ImagenUrl = @UrlImagen, Precio = @Precio where id = @id");
                datos.SetearParametro("@Codigo", articulo.Codigo);
                datos.SetearParametro("@Nombre", articulo.Nombre);
                datos.SetearParametro("@Descripcion",articulo.Descripcion);
                datos.SetearParametro("@IdMarca", articulo.Marca.Id);
                datos.SetearParametro("@IdCategoria",articulo.Categoria.Id);
                datos.SetearParametro("@UrlImagen", articulo.UrlImagen);
                datos.SetearParametro("@Precio",articulo.Precio);
                datos.SetearParametro("@id",articulo.Id);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void eliminar (int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("delete from ARTICULOS where id = @id");
                datos.SetearParametro("@id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "Select A.Id as IdArticulo,Codigo, Nombre ,A.Descripcion, ImagenUrl,C.Descripcion as Categoria,M.Descripcion as Marca , Precio, C.Id as IdCategoria, M.Id as IdMarca  from ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = M.Id and ";
                if (campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Empieza con":
                            consulta += "Nombre like '"+ filtro +"%'";
                        break;
                        case "Termina con":
                            consulta += "Nombre like '% "+filtro+"'";
                            break;
                        default:
                            consulta += "Nombre like '%"+filtro+"%'";
                            break;
                    }
                }else if (campo == "Precio"){

                    switch (criterio)
                    {
                        case "Mayor a":
                            consulta += "Precio >"+filtro+"";
                            break;
                        case "Menor a":
                            consulta += "Precio <" + filtro + "";
                            break;
                        default:
                            consulta += "C.Descripcion = 'Audio'";
                            break;
                    }
                }
                
      
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["IdArticulo"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["ImagenUrl"];
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = (int)datos.Lector["IdCategoria"];
                    aux.Categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Marca = new Marca();
                    aux.Marca.Id = (int)datos.Lector["IdMarca"];
                    aux.Marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrarPorCategoria(string categoria)
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();

            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.SetearConsulta("Select A.Id as IdArticulo, Codigo, Nombre, A.Descripcion, ImagenUrl, C.Descripcion as Categoria, M.Descripcion as Marca, Precio, C.Id as IdCategoria, M.Id as IdMarca " +
                             "from ARTICULOS A " +
                             "join CATEGORIAS C on A.IdCategoria = C.Id " +
                             "join MARCAS M on A.IdMarca = M.Id " +
                             "where C.Descripcion = @categoria");
                datos.SetearParametro("@categoria", categoria);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = datos.Lector["IdArticulo"] != DBNull.Value ? (int)datos.Lector["IdArticulo"] : 0;
                    aux.Codigo = datos.Lector["Codigo"] != DBNull.Value ? (string)datos.Lector["Codigo"] : string.Empty;
                    aux.Nombre = datos.Lector["Nombre"] != DBNull.Value ? (string)datos.Lector["Nombre"] : string.Empty;
                    aux.Descripcion = datos.Lector["Descripcion"] != DBNull.Value ? (string)datos.Lector["Descripcion"] : string.Empty;
                    aux.UrlImagen = datos.Lector["ImagenUrl"] != DBNull.Value ? (string)datos.Lector["ImagenUrl"] : string.Empty;
                    aux.Categoria = new Categoria();
                    aux.Categoria.Id = datos.Lector["IdCategoria"] != DBNull.Value ? (int)datos.Lector["IdCategoria"] : 0;
                    aux.Categoria.Descripcion = datos.Lector["Categoria"] != DBNull.Value ? (string)datos.Lector["Categoria"] : string.Empty;
                    aux.Marca = new Marca();
                    aux.Marca.Id = datos.Lector["IdMarca"] != DBNull.Value ? (int)datos.Lector["IdMarca"] : 0;
                    aux.Marca.Descripcion = datos.Lector["Marca"] != DBNull.Value ? (string)datos.Lector["Marca"] : string.Empty;
                    aux.Precio = datos.Lector["Precio"] != DBNull.Value ? (decimal)datos.Lector["Precio"] : 0;

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
    }
}
