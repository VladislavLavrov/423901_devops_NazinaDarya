using System.ComponentModel.DataAnnotations;

namespace calculator.Models
{
    public class CalculatorModel
    {
        public double FirstNumber { get; set; }
        public double SecondNumber { get; set; }
        public double Result { get; set; }
        public string Operation { get; set; } = "+";
        public string ErrorMessage { get; set; }
    }
}