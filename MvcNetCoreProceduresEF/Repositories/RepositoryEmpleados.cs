using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;

#region VISTAS Y PROCEDIMIENTOS ALMACENADOS
/*
 create view V_EMPLEADOS_DEPARTAMENTOS
as
	select CAST(
	ISNULL(ROW_NUMBER() over (order by APELLIDO), 0) as int)
	as ID
	, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO
	, DEPT.DNOMBRE AS DEPARTAMENTO
	, DEPT.LOC AS LOCALIDAD
	from EMP
	inner join DEPT
	on EMP.DEPT_NO = DEPT.DEPT_NO
go
 */
#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<VistaEmpleado>>
            GetVistaEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistasEmpleados
                           select datos;
            return await consulta.ToListAsync();
        }
    }
}
