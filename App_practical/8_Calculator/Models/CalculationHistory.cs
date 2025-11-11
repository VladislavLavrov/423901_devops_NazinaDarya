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
        public double FirstNumber { get; set; }  // ← ИЗМЕНИТЕ НА double

        [Required]
        public double SecondNumber { get; set; }  // ← ИЗМЕНИТЕ НА double

        [Required]
        [StringLength(10)]
        public string Operation { get; set; } = string.Empty;

        [Required]
        public double Result { get; set; }  // ← ИЗМЕНИТЕ НА double

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