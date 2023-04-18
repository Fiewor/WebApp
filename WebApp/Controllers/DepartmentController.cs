using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        //to read the connection strings from appsetttings file, we need to make use of dependency injection
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // now, an API method to get all data from department table
        [HttpGet]
        public JsonResult Get()
        {
            // currently writing raw queries, but other options are 'stored procedures' and 'entity framework'
            string query = @"select DepartmentId, DepartmentName from dbo.Department";
            // get data into a DataTable object
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"
                            insert into dbo.Department
                            values (@DepartmentName)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Added successfully");
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"
                            update dbo.Department
                            set DepartmentName = @DepartmentName
                            where DepartmentId=@DepartmentId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Updated successfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Department
                            where DepartmentId=@DepartmentId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Deleted succesfully");
        }
    }
}
