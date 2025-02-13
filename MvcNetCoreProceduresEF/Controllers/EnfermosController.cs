using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {
        private RepositoryEnfermos repo;

        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos =
                await this.repo.GetEnfermosAsync();
            return View(enfermos);
        }
    }
}
