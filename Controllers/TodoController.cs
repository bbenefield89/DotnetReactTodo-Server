using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_react_todo.Models;

namespace dotnet_react_todo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.todos.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all todos.
                _context.todos.Add(new TodoItem { title = "Item1", is_complete = "false" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> Gettodos()
        {
            return Ok(await _context.todos.ToListAsync());
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.todos.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // POST /api/todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem([FromBody] TodoItem todoItem)
        {
            await _context.todos.AddAsync(todoItem);
            await _context.SaveChangesAsync();
            return Created("/api/todo", todoItem);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(int id, [FromBody] TodoItem item)
        {
            if (id != item.id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok(item);
        }

        // DELETE: api/todo/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(int id, [FromBody] TodoItem todoItem)
        {
            IActionResult response = null;
            if (id != todoItem.id)
            {
                response = BadRequest();
            }
            _context.todos.Remove(todoItem);
            await _context.SaveChangesAsync();
            response = Ok(todoItem);
            return response;
        }
    }
}