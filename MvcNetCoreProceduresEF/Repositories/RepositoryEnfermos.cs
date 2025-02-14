using Microsoft.Data.SqlClient;
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
        private HospitalContext context;

        public RepositoryEnfermos(HospitalContext context)
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

        //public async Task FindEnfermo(string inscripcion)
        //{
        //    string sql = "SP_FIND_ENFERMO";
        //    var consulta =
        //       await this.context.Enfermos.FromSqlRaw(sql).ToListAsync();
               
        //    consulta.ToList()
        //}

        public async Task<Enfermo> FindEnfermoAsync(string inscripcion)
        {
            //PARA LLAMAR A PROCEDIMIENTOS ALMACENADOS
            //CON PARAMETROS LA LLAMADA SE REALIZA MEDIANTE
            //EL NOMBRE DEL PROCEDIMIENTO Y CADA PARAMETRO
            //A CONTINUACION SEPARADO MEDIANTE COMAS
            // SP_PROCEDIMIENTO @PARAM1, @PARAM2
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            //DEBEMOS CREAR LOS PARAMETROS
            SqlParameter pamInscripcion =
                new SqlParameter("@INSCRIPCION", inscripcion);
            //SI LOS DATOS QUE DEVUELVE EL PROCEDIMIENTO 
            //ESTAN MAPEADOS CON UN MODEL, PODEMOS UTILIZAR 
            //EL METODO FromSqlRaw CON LINQ
            var consulta = 
               await this.context.Enfermos.FromSqlRaw(sql, pamInscripcion)
                .ToListAsync();
            Enfermo enfermo = consulta.FirstOrDefault();
            return enfermo;
        }

        public async Task DeleteEnfermoAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamInscripcion =
                new SqlParameter("@INSCRIPCION", inscripcion);
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamInscripcion);
                await com.Connection.OpenAsync();
                com.ExecuteNonQuery();
                await com.Connection.CloseAsync();
                com.Parameters.Clear();
            }
        }

        public async Task DeleteEnfermoRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter pamInscripcion =
                new SqlParameter("@INSCRIPCION", inscripcion);
            //DENTRO DEL CONTEXT TENEMOS UN METODO PARA
            //PODER LLAMAR A PROCEDIMIENTOS DE CONSULTAS DE ACCION
            await this.context.Database
                .ExecuteSqlRawAsync(sql, pamInscripcion);
        }

        public async Task InsertEnfermoAsync
            (string apellido, string direccion, 
            DateTime fechaNacimiento, string genero)
        {
            string sql = "SP_INSERT_ENFERMO @apellido, @direccion "
                + ", @fechanac, @genero";
            SqlParameter pamApellido =
                new SqlParameter("@apellido", apellido);
            SqlParameter pamDireccion =
                new SqlParameter("@direccion", direccion);
            SqlParameter pamFecha =
                new SqlParameter("@fechanac", fechaNacimiento);
            SqlParameter pamGen =
                new SqlParameter("@genero", genero);
            await this.context.Database
                .ExecuteSqlRawAsync(sql, pamApellido, pamDireccion
                , pamFecha, pamGen);
        }
    }
}
