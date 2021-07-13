using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace LunhuiApp.Controllers
{
    [Route("api/[controller]",Name ="待办事项")]
    [ApiController]
    public class TodoController : Controller
    {
        private readonly DataAccessLibrary.Models.CoreDbContext _coreDbContext;

        public TodoController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        // GET api/values
        [HttpGet(Name ="获取所有待办事项")]
        public List<Todos> Get()
        {
            return _coreDbContext.Set<Todos>().ToList();
        }


        // GET api/values/5
        [HttpGet("{id}",Name ="通过id获取待办事项")]
        public IActionResult GetById(long id)
        {
            var todo = _coreDbContext.Todos.Where(t => t.id.Equals(id)).Select(t => new { t.id, t.content,t.date,t.isCompleted }).ToList();
            if (todo.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todo);
        }

        
        // GET api/values/5
        [HttpGet("GetByTime",Name ="获取指定时间内的待办事项")]
        public IActionResult GetByTime(DateTime beginTime,DateTime endTime)
        {
            var todos = _coreDbContext.Todos.Where(t => t.date>=beginTime && t.date<=endTime).Select(t => new { t.id, t.content, t.date, t.isCompleted }).ToList();
            if (todos.Count == 0)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(todos);
        }
        

        // POST api/values/add
        [HttpPost, Route("addTodo",Name ="增添待办事项")]
        public IActionResult AddTodo(Todos todo)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                Todos newt = new Todos
                {
                    content = todo.content,
                    date = todo.date
                };

                // 添加数据到数据库
                _coreDbContext.Todos.Add(newt);
                _coreDbContext.SaveChanges();

                dic.Add("message", "添加成功");
                dic.Add("status", "success");
                return new OkObjectResult(dic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        // POST api/values/Delete
        [HttpPost,Route("deleteTodo", Name = "删除待办事项")]
        public IActionResult DeleteById(long id)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                // 删除数据库信息
                Todos todo = _coreDbContext.Todos.Find(id);
                if (todo == null)
                {
                    
                    dic.Add("message", "该待办不存在，请刷新试试看");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }
                _coreDbContext.Todos.Remove(todo);
                _coreDbContext.SaveChanges();
                dic.Add("message", "删除成功");
                dic.Add("status", "success");

                return new OkObjectResult(dic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        // POST: api/values//Edit
        [HttpPost, Route("EditTodo", Name = "修改待办事项，包括时间、内容、是否完成")]
        public IActionResult EditTodo(Todos todo)
        {
            try
            {

                Dictionary<string, object> dic = new Dictionary<string, object>();
                //修改数据库信息
                Todos t = _coreDbContext.Todos.Find(todo.id);
                if (t == null)
                {
                    
                    dic.Add("message", "该待办不存在，请刷新试试看");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }
                t.Clone(todo);
                _coreDbContext.Update(t);
                _coreDbContext.SaveChanges();
                dic.Add("message", "修改成功");
                dic.Add("status", "success");

                return new OkObjectResult(dic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        // POST: api/values//Edit
        [HttpPost, Route("CompleteTodo", Name = "完成待办事项")]
        public IActionResult CompleteTodo(long id,bool isCompleted=true)
        {
            try
            {
                //修改数据库信息
                Todos todo = _coreDbContext.Todos.Find(id);
                if (todo == null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("message", "该待办不存在，请刷新试试看");
                    return new BadRequestObjectResult(dic);
                }
                todo.isCompleted = isCompleted;

                _coreDbContext.Update(todo);
                _coreDbContext.SaveChanges();

                return new OkObjectResult(todo);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }
    }
}
