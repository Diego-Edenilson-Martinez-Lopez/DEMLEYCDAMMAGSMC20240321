using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using DEMLEYCDAMMAGSMC20240321.Models;
using Rotativa.AspNetCore;

namespace DEMLEYCDAMMAGSMC20240321.Controllers
{
    public class ReportsComputersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsComputersController (ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
