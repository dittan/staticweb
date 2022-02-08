using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using BlazorApp.Api.Infra;
using BlazorApp.Shared.Models;

namespace BlazorApp.Api.Functions
{
    public class TodoFunctions
    {
        private readonly Repository _repository;

        public TodoFunctions(Repository repository)
        {
            _repository = repository;
        }

        [FunctionName("CreateTodo")]
        public async Task<IActionResult> CreateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "v1/todo/create")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CreateTodo function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var todoItem = JsonConvert.DeserializeObject<TodoItem>(requestBody);

                await _repository.CreateTodo(todoItem);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
            }


            return new OkResult();
        }

        [FunctionName("GetTodos")]
        public async Task<IActionResult> GetTodos(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "v1/todo/get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetTodo function processed a request.");

            var todoItems = await _repository.GetTodoItems();

            return new OkObjectResult(todoItems);
        }

        [FunctionName("DeleteTodo")]
        public async Task<IActionResult> DeleteTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "v1/todo/delete/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            log.LogInformation("DeleteTodo function processed a request.");

            try
            {
                await _repository.DeleteTodo(id);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
            }

            return new OkResult();
        }

        [FunctionName("UpdateTodo")]
        public async Task<IActionResult> UpdateTodo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "v1/todo/update")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("DeleteTodo function processed a request.");

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var todoItem = JsonConvert.DeserializeObject<TodoItem>(requestBody);

                await _repository.UpdateTodo(todoItem);
            }
            catch (Exception ex)
            {
                log.LogError($"{ex.Message}");
            }

            return new OkResult();
        }
    }
}
