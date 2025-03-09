using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MathApp.Models;
using MathApp.Utils;
using System.Threading.Tasks;

namespace MathApp.Controllers
{
    public class MathController : Controller
    {
        private readonly MathDbContext _context;
        
        public MathController(MathDbContext context)
        {
            _context = context;
        }

        public IActionResult Calculate()
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            List<SelectListItem> operations = new List<SelectListItem> {
            new SelectListItem { Value = "1", Text = "+" },
            new SelectListItem { Value = "2", Text = "-" },
            new SelectListItem { Value = "3", Text = "*" },
            new SelectListItem { Value = "4", Text = "/" },
            };

            ViewBag.Operations = operations;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(decimal? FirstNumber, decimal? SecondNumber,int Operation)
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            MathCalculation mathCalculation = new MathCalculation();
            mathCalculation.FirstNumber = FirstNumber;
            mathCalculation.SecondNumber = SecondNumber;
            mathCalculation.Operation = Operation;

            switch (Operation)
            {
                case 1:
                    mathCalculation.Result = FirstNumber + SecondNumber;
                    break;
                case 2:
                    mathCalculation.Result = FirstNumber - SecondNumber;
                    break;
                case 3:
                    mathCalculation.Result = FirstNumber * SecondNumber;
                    break;
                default:
                    if (SecondNumber != 0)
                        mathCalculation.Result = FirstNumber / SecondNumber;
                    break;
            }

            if (ModelState.IsValid)
            {
                _context.Add(mathCalculation);
                await _context.SaveChangesAsync();
                    
            }
            ViewBag.Result = mathCalculation.Result;

            List<SelectListItem> operations = new List<SelectListItem> {
            new SelectListItem { Value = "1", Text = "+" },
            new SelectListItem { Value = "2", Text = "-" },
            new SelectListItem { Value = "3", Text = "*" },
            new SelectListItem { Value = "4", Text = "/" },
            };

            ViewBag.Operations = operations;

            return View();
        }
        
        public async Task<IActionResult> History()
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(await _context.MathCalculations.ToListAsync());
        }

        public async Task<IActionResult> Clear()
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            _context.MathCalculations.RemoveRange(await _context.MathCalculations.ToListAsync<MathCalculation>());
            await _context.SaveChangesAsync();

            return RedirectToAction("History");
        }
    }
}