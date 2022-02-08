using BlazorApp.Shared.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace BlazorApp.Api.Infra
{
    public class Repository
    {
        public async Task<int> CreateTodo(TodoItem todoItem)
        {
            string sql = "INSERT INTO TodoItem (Title, IsCompleted) Values (@Title, @IsCompleted);";
            
            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnStr")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Title = todoItem.Title, IsCompleted = todoItem.IsCompleted });
                return affectedRows;
            }
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItems()
        {
            string sql = "SELECT * FROM TodoItem";

            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnStr")))
            {
                var todoItems = await connection.QueryAsync<TodoItem>(sql);
                return todoItems;
            }
        }

        public async Task DeleteTodo(int id)
        {
            string sql = "DELETE FROM TodoItem WHERE Id = @id;";

            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnStr")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task UpdateTodo(TodoItem todoItem)
        {
            string sql = "UPDATE TodoItem SET IsCompleted = @isCompleted WHERE Id = @id;";

            using (var connection = new SqlConnection(Environment.GetEnvironmentVariable("SqlConnStr")))
            {
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = todoItem.Id, IsCompleted = todoItem.IsCompleted });
            }
        }
    }
}
