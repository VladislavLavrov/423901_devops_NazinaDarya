using calculator.Data;
using calculator.Models;
using calculator.Services;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace calculator.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;
        private readonly KafkaProducerService<Null, string> _producer;

        public CalculatorController(CalculatorContext context, KafkaProducerService<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        /// <summary>
        /// Отображение страницы Index.
        /// </summary>
        public IActionResult Index()
        {
            var data = _context.DataInputVariants.OrderByDescending(x => x.ID_DataInputVariant).ToList();
            return View(data);
        }

        /// <summary>
        /// Обработка запроса на вычисление.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(double num1, double num2, Operation operation)
        {
            // Подготовка объекта для расчета
            var dataInputVariant = new DataInputVariant
            {
                Operand_1 = num1,
                Operand_2 = num2,
                Type_operation = operation,
            };

            // Отправка данных в Kafka
            await SendDataToKafka(dataInputVariant);

            // Перенаправление на страницу Index
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Callback([FromBody] DataInputVariant inputData)
        {
            // Сохранение данных и результата в базе данных
            SaveDataAndResult(inputData);
            return Ok();
        }

        /// <summary>
        /// Сохранение данных и результата в базе данных.
        /// </summary>
        private DataInputVariant SaveDataAndResult(DataInputVariant inputData)
        {
            _context.DataInputVariants.Add(inputData);
            _context.SaveChanges();
            return inputData;
        }

        /// <summary>
        /// Отправка данных в Kafka.
        /// </summary>
        private async Task SendDataToKafka(DataInputVariant dataInputVariant)
        {
            var json = JsonSerializer.Serialize(dataInputVariant);
            await _producer.ProduceAsync("nazina", new Message<Null, string> { Value = json });
        }
    }
}
