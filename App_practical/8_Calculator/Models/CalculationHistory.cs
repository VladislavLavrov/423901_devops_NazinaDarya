using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace calculator.Models
{
    public class CalculationHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public decimal FirstNumber { get; set; }

        [Required]
        public decimal SecondNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Operation { get; set; } = string.Empty;

        [Required]
        public decimal Result { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Для отображения операции в читаемом виде
        public string OperationDisplay => Operation switch
        {
            "+" => "Сложение",
            "-" => "Вычитание",
            "*" => "Умножение",
            "/" => "Деление",
            _ => Operation
        };
    }
}