using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Data.Common;

#region PROCEDIMIENTOS ALMACENADOS
/*
 create procedure SP_TODOS_ENFERMOS
as
	select * from ENFERMO
go
create procedure SP_FIND_ENFERMO
(@inscripcion nvarchar(50))
as
	select * from ENFERMO where INSCRIPCION=@inscripcion
go
create procedure SP_DELETE_ENFERMO
(@inscripcion nvarchar(50))
as
	delete from ENFERMO where INSCRIPCION=@inscripcion
go
 */
#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEnfermos
    {
        private EnfermosContext context;

        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context = context;
        }

        public async  Task<List<Enfermo>> GetEnfermosAsync()
        {
            //PARA CONSULTAS DE SELECCION DEBEMOS MAPEAR 
            //MANUALMENTE LOS DATOS.
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                //ABRIMOS LA CONEXION A TRAVES DEL COMMAND
                await com.Connection.OpenAsync();
                //EJECUTAMOS NUESTRO READER
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento =
                        DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }
    }
}
