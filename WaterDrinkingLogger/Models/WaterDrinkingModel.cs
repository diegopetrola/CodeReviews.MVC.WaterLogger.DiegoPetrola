using System.ComponentModel.DataAnnotations;

namespace WaterDrinkingLogger.Models;

public class WaterDrinkingModel
{
    public int Id { get; set; }
    [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}")]
    public DateTime Date { get; set; }
    [Range(0, Double.MaxValue, ErrorMessage = "Value must be positive!")]
    public double Quantity { get; set; }
}
