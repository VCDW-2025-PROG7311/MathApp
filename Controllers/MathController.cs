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

            decimal? Result = 0;
            MathCalculation mathCalculation;

            try
            {
                mathCalculation = MathCalculation.Create(FirstNumber, SecondNumber, Operation, Result, token);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
                throw;
            }
            

            switch (Operation)
            {
                case 1:
                    mathCalculation.Result = CalculationsUtil.Add(FirstNumber, SecondNumber);
                    break;
                case 2:
                    mathCalculation.Result = CalculationsUtil.Subtract(FirstNumber, SecondNumber);
                    break;
                case 3:
                    mathCalculation.Result = CalculationsUtil.Multiply(FirstNumber, SecondNumber);
                    break;
                default:
                    mathCalculation.Result = CalculationsUtil.Divide(FirstNumber, SecondNumber);
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

            // return RedirectToAction("Calculate");
            
        }
        
        public async Task<IActionResult> History()
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View(await _context.MathCalculations.Where(m => m.FirebaseUuid.Equals(token)).ToListAsync());
        }

        public async Task<IActionResult> Clear()
        {
            var token = HttpContext.Session.GetString("currentUser");
            if (token == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var recordsToDelete = _context.MathCalculations.Where(m => m.FirebaseUuid.Equals(token));
            _context.MathCalculations.RemoveRange(recordsToDelete);

            await _context.SaveChangesAsync();
            return RedirectToAction("History");
        }
    }
}