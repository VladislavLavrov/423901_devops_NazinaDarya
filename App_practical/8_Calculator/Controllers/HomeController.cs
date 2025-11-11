using calculator.Models;
using calculator.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new CalculatorModel();
            // Загружаем историю вычислений
            model.History = await _context.CalculationHistory
                .OrderByDescending(h => h.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Calculate(CalculatorModel model)
        {
            try
            {
                model.Result = model.Operation switch
                {
                    "+" => model.FirstNumber + model.SecondNumber,
                    "-" => model.FirstNumber - model.SecondNumber,
                    "*" => model.FirstNumber * model.SecondNumber,
                    "/" => model.SecondNumber != 0 ? model.FirstNumber / model.SecondNumber : 0,
                    _ => 0
                };

                if (model.Operation == "/" && model.SecondNumber == 0)
                {
                    model.ErrorMessage = "Деление на ноль невозможно!";
                    model.Result = 0;
                }
                else
                {
                    model.ErrorMessage = "";

                    // Сохраняем в базу данных
                    var history = new CalculationHistory
                    {
                        FirstNumber = model.FirstNumber,
                        SecondNumber = model.SecondNumber,
                        Operation = model.Operation,
                        Result = model.Result
                    };

                    _context.CalculationHistory.Add(history);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                model.Result = 0;
                model.ErrorMessage = "Произошла ошибка при вычислении: " + ex.Message;
            }

            // Обновляем историю
            model.History = await _context.CalculationHistory
                .OrderByDescending(h => h.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View("Index", model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
