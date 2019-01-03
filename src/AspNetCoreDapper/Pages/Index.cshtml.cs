using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreDapper.Models;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreDapper.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                if (connection.QueryFirstOrDefault<int?>(@"SELECT Id 
                                                               FROM Contacts 
                                                               WHERE Name = @Name",
                                                         new {Name = "Charlie Plumber"}) == null)
                {
                    connection.Execute(@"INSERT INTO Contacts (Name, Address, City, Subregion, Email) 
                                             VALUES (@Name, @Address, @City, @Subregion, @Email)",
                                       new Contact
                                       {
                                           Name = "Charlie Plumber",
                                           Address = "123 Main St",
                                           City = "Nashville",
                                           Subregion = "TN",
                                           Email = "cplumber@fake.com"
                                       });
                }

                var charile = connection.QueryFirstOrDefault<Contact>(@"SELECT Id, Name, Address, City, Subregion, Email 
                                                                            FROM Contacts 
                                                                            WHERE Name = @Name",
                                                                      new {Name = "Charlie Plumber"});
            }
        }
    }
}
