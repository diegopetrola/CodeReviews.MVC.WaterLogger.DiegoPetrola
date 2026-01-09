using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WaterDrinkingLogger.Models;

namespace WaterDrinkingLogger.Pages
{
    public class CreateModel(IConfiguration configuration) : PageModel
    {
        private readonly IConfiguration _configuration = configuration;

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }

            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            connection.Open();

            using var tableCommand = connection.CreateCommand();
            tableCommand.CommandText = "INSERT INTO drinking_water (date, quantity) VALUES (@date, @quantity)";
            tableCommand.Parameters.Add(new SqlParameter("@date", WaterDrinking.Date));
            tableCommand.Parameters.Add(new SqlParameter("@quantity", WaterDrinking.Quantity));
            tableCommand.ExecuteNonQuery();

            return RedirectToPage("Index");
        }

        [BindProperty]
        public WaterDrinkingModel WaterDrinking { get; set; }
    }
}
