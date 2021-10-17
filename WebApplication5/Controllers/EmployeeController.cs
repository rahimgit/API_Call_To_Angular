using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using WebApplication5.Models;
using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace WebApplication5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        //dependency injection
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {

            _configuration = configuration;
            _env = env;

        }

        [HttpGet]
        public JsonResult Get()
        {

            string query = @"
                          SELECT EmployeeId, EmployeeName, Department,
                          convert(varchar(500),DateOfJoin,120) AS DateOfJoin,
                          PhotoFileName FROM dbo.EMployee";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    dr = cmd.ExecuteReader();
                    table.Load(dr);
                    dr.Close();
                    conn.Close();

                }

            }
            return new JsonResult(table);
        }


        [HttpPost]
        public JsonResult Post(Employee emp)
        {

            string query = @"
                          INSERT INTO dbo.EMployee (EmployeeName,Department,DateOfJoin,PhotoFileName) VALUES 
                          (  
                          '" + emp.EmployeeName + @"'
                           , '" + emp.Department + @"'
                            ,   '" + emp.DateOfJoin + @"'
                             ,  '" + emp.PhotoFileName + @"'
                             
                             )
                           ";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    dr = cmd.ExecuteReader();
                    table.Load(dr);
                    dr.Close();
                    conn.Close();

                }

            }
            return new JsonResult("Data has been Addedd Successfully");
        }



        [HttpPut]
        public JsonResult Put(Employee emp)
        {

            string query = @"
                          UPDATE dbo.EMployee SET  
                          EmployeeName='" +emp.EmployeeName + @"' 
                          ,Department='" + emp.Department + @"'
                           ,DateOfJoin='" + emp.DateOfJoin + @"'
                           , PhotoFileName='" + emp.PhotoFileName + @"'



WHERE EmployeeId=" + emp.EmployeeId + @"
                           ";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    dr = cmd.ExecuteReader();
                    table.Load(dr);
                    dr.Close();
                    conn.Close();

                }

            }
            return new JsonResult("Data has been Updated Successfully");
        }




        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {

            string query = @"
                          DELETE FROM dbo.EMployee
                          WHERE EmployeeId=" + id + @"
                           ";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    dr = cmd.ExecuteReader();
                    table.Load(dr);
                    dr.Close();
                    conn.Close();

                }

            }
            return new JsonResult("Deleted Successfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile() 
        {

            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var phisycalPath = _env.ContentRootPath + "/Photos/" + fileName;
                using (var stream = new FileStream(phisycalPath, FileMode.Create)) {
                    postedFile.CopyTo(stream);
                
                }

                return new JsonResult(fileName);
            }
            catch (Exception)
            {

                return new JsonResult("anonymus.png");
            }
        
        
        }



        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {

            string query = @"
                          SELECT DepartmentName from dbo.Department";
            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader dr;
            using (SqlConnection conn = new SqlConnection(sqlDataSource))
            {

                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

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
