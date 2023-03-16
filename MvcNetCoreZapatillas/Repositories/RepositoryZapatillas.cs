using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreZapatillas.Data;
using MvcNetCoreZapatillas.Models;

#region SQL SERVER
//VUESTRO PROCEDIMIENTO DE PAGINACION DE IMAGENES DE ZAPATILLAS
/*
CREATE PROCEDURE SP_GRUPO_ZAPATILLAS (@IDPRODUCTO INT, @POSICION INT)
AS
	SELECT IDIMAGEN, IDPRODUCTO, IMAGEN FROM
	(SELECT CAST(ROW_NUMBER() OVER (ORDER BY IDIMAGEN) AS INT) AS POSICION, IDIMAGEN, IDPRODUCTO, IMAGEN FROM IMAGENESZAPASPRACTICA WHERE IDPRODUCTO = @IDPRODUCTO) AS QUERY
	WHERE QUERY.POSICION >= 1 AND QUERY.POSICION < (@POSICION + 1)
GO
*/
#endregion

namespace MvcNetCoreZapatillas.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public int GetNumeroRegistrosZapatillas(int idproducto)
        {
            return this.context.ImagenesZapatillas.Count(x => x.IdProducto == idproducto);
        }

        public List<Zapatilla> GetZapatillas()
        {
            return this.context.Zapatillas.ToList();
        }

        public Zapatilla FindZapatilla(int idproducto)
        {
            return this.context.Zapatillas.FirstOrDefault(x => x.IdProducto == idproducto);
        }

        public async Task<List<ImagenZapatilla>> GetImagenesAsync(int idproducto, int posicion)
        {
            string sql = "SP_GRUPO_ZAPATILLAS @IDPRODUCTO, @POSICION";
            SqlParameter pamid = new SqlParameter("@IDPRODUCTO", idproducto);
            SqlParameter pamposicion = new SqlParameter("@POSICION", posicion);
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamid, pamposicion);
            return await consulta.ToListAsync();
        }
    }
}
