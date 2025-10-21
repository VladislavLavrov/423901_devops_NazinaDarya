using calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace calculator.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new CalculatorModel());
        }

        [HttpPost]
        public IActionResult Calculate(CalculatorModel model)
        {
            // Убираем проверку ModelState.IsValid
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
                    model.ErrorMessage = ""; // Очищаем сообщение об ошибке
                }
            }
            catch (Exception ex)
            {
                model.Result = 0;
                model.ErrorMessage = "Произошла ошибка при вычислении: " + ex.Message;
            }

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
