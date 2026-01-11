using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterDrinkingLogger.Models;
using static WaterDrinkingLogger.Utils.ColumnNames;

namespace WaterDrinkingLogger.Pages;

public class UpdateModel(IConfiguration configuration) : PageModel
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
            waterDrinkingRecord.Id = reader.GetInt32((int)WaterDrinkingColumns.Id);
            waterDrinkingRecord.Quantity = reader.GetDouble((int)WaterDrinkingColumns.Quantity);
            waterDrinkingRecord.Date = reader.GetDateTime((int)WaterDrinkingColumns.Date);
        }
        return waterDrinkingRecord;
    }

    public IActionResult OnPost(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.Parameters.Add(new SqlParameter("@id", id));
        tableCmd.Parameters.Add(new SqlParameter("@date", WaterDrinking.Date));
        tableCmd.Parameters.Add(new SqlParameter("@quantity", WaterDrinking.Quantity));
        tableCmd.CommandText = """
            UPDATE drinking_water 
            SET date = @date, quantity = @quantity WHERE id = @id
            """;
        tableCmd.ExecuteNonQuery();

        return RedirectToPage("Index");
    }
}
