using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterDrinkingLogger.Models;
using static WaterDrinkingLogger.Utils.ColumnNames;

namespace WaterDrinkingLogger.Pages;

public class IndexModel(IConfiguration configuration) : PageModel
{
    private readonly IConfiguration _configuration = configuration;
    public List<WaterDrinkingModel> Records { get; set; }

    public void OnGet()
    {
        Records = GetAllRecords();
    }

    public List<WaterDrinkingModel> GetAllRecords()
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText = "SELECT * FROM drinking_water";
        var tableData = new List<WaterDrinkingModel>();
        var reader = tableCmd.ExecuteReader();
        while (reader.Read())
        {
            tableData.Add(
                    new WaterDrinkingModel
                    {
                        Id = reader.GetInt32((int)WaterDrinkingColumns.Id),
                        Date = reader.GetDateTime((int)WaterDrinkingColumns.Date),
                        Quantity = reader.GetDouble((int)WaterDrinkingColumns.Quantity)
                    }
                );
        }
        return tableData;
    }
}
