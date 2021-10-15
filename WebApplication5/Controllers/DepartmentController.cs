using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration) {

            _configuration = configuration;
        
        
        }

        [HttpGet]
        public JsonResult Get() {

            string query = @"
                          SELECT DepartmentId, DepartmentName from dbo.Department";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource)) {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    dr = cmd.ExecuteReader();
                    table.Load(dr);
                    dr.Close();
                    conn.Close();
                
                }
            
            }
            return new JsonResult(table);
        }
    }
}
