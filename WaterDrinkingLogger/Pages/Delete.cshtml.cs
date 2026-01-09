using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages;

public class DeleteModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;

    [BindProperty]
    public WaterDrinkingModel WaterDrinking { get; set; }

    public void OnGet(int id)
    {
        WaterDrinking = GetById(id);
    }

    private WaterDrinkingModel GetById(int id)
    {
        var waterDrinkingRecord = new WaterDrinkingModel();
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add(new SqlParameter("@id", id));
        tableCmd.CommandText = "SELECT * FROM drinking_water WHERE id=@id";
        var reader = tableCmd.ExecuteReader();
        while (reader.Read())
        {
            waterDrinkingRecord.Id = reader.GetInt32(0);
            waterDrinkingRecord.Quantity = reader.GetDouble(1);
            waterDrinkingRecord.Date = reader.GetDateTime(2);
        }
        return waterDrinkingRecord;
    }
}
