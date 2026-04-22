using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TaskManagementAPI.Data;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly DatabaseHelper _db;

        public TestController(DatabaseHelper db)
        {
            _db = db;
        }

        [HttpGet("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                return Ok(new
                {
                    status = "success",
                    message = "Koneksi ke PostgreSQL berhasil"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "error",
                    message = ex.Message
                });
            }
        }
    }
}