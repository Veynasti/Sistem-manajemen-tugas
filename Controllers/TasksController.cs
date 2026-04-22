using Microsoft.AspNetCore.Mvc;
using Npgsql;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly DatabaseHelper _db;

        public TasksController(DatabaseHelper db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAllTasks()
        {
            var tasks = new List<TaskModel>();

            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                string query = "SELECT * FROM tasks ORDER BY id";

                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tasks.Add(new TaskModel
                    {
                        Id = reader.GetInt32(0),
                        UserId = reader.GetInt32(1),
                        CategoryId = reader.GetInt32(2),
                        Title = reader.GetString(3),
                        Description = reader.IsDBNull(4) ? "" : reader.GetString(4),
                        Status = reader.GetString(5),
                        CreatedAt = reader.GetDateTime(6),
                        UpdatedAt = reader.GetDateTime(7)
                    });
                }

                return Ok(new
                {
                    status = "success",
                    data = tasks
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
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskModel task)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                string query = @"
                    INSERT INTO tasks (user_id, category_id, title, description, status, created_at, updated_at)
                    VALUES (@user_id, @category_id, @title, @description, @status, NOW(), NOW())
                    RETURNING id;
                ";

                using var cmd = new NpgsqlCommand(query, conn);

                // PARAMETERIZED QUERY BUAT SQL
                cmd.Parameters.AddWithValue("@user_id", task.UserId);
                cmd.Parameters.AddWithValue("@category_id", task.CategoryId);
                cmd.Parameters.AddWithValue("@title", task.Title);
                cmd.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", task.Status ?? "todo");

                var newId = (int)cmd.ExecuteScalar();

                return StatusCode(201, new
                {
                    status = "success",
                    message = "Task berhasil ditambahkan",
                    data = newId
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

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskModel task)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                //  ngecek task ada apa engga
                string checkQuery = "SELECT COUNT(*) FROM tasks WHERE id = @id";
                using var checkCmd = new NpgsqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@id", id);

                var count = (long)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Task tidak ditemukan"
                    });
                }

                //  ngevalidasi status
                if (task.Status != "todo" && task.Status != "in_progress" && task.Status != "done")
                {
                    return BadRequest(new
                    {
                        status = "error",
                        message = "Status harus: todo, in_progress, atau done"
                    });
                }

                //  Update data
                string query = @"
                    UPDATE tasks
                    SET user_id = @user_id,
                        category_id = @category_id,
                        title = @title,
                        description = @description,
                        status = @status,
                        updated_at = NOW()
                    WHERE id = @id
                ";

                using var cmd = new NpgsqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@user_id", task.UserId);
                cmd.Parameters.AddWithValue("@category_id", task.CategoryId);
                cmd.Parameters.AddWithValue("@title", task.Title);
                cmd.Parameters.AddWithValue("@description", (object?)task.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", task.Status);

                cmd.ExecuteNonQuery();

                return Ok(new
                {
                    status = "success",
                    message = "Task berhasil diupdate"
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

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            try
            {
                using var conn = _db.GetConnection();
                conn.Open();

                //  ngecek tasks
                string checkQuery = "SELECT COUNT(*) FROM tasks WHERE id = @id";
                using var checkCmd = new NpgsqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@id", id);

                var count = (long)checkCmd.ExecuteScalar();

                if (count == 0)
                {
                    return NotFound(new
                    {
                        status = "error",
                        message = "Task tidak ditemukan"
                    });
                }

                //  ngehapus data
                string deleteQuery = "DELETE FROM tasks WHERE id = @id";
                using var cmd = new NpgsqlCommand(deleteQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();

                return Ok(new
                {
                    status = "success",
                    message = "Task berhasil dihapus"
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