using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToutaiMultiThread;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LunhuiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToutaiContrller : Controller
    {
        private readonly CoreDbContext _coreDbContext;

        public ToutaiContrller(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        // GET api/values
        [HttpGet("Zhuansheng")]
        public List<Zhuansheng> GetZhuansheng()
        {
            return _coreDbContext.Set<Zhuansheng>().ToList();
        }

        // POST api/Zhuanshengs/getDeadsById
        [HttpPost,Route("GetZhuanshengById")]
        public IActionResult GetZhuanshengById(long id)
        {
            try
            {
                var Zhuanshengs = _coreDbContext.Zhuansheng.Find(id);
                if (Zhuanshengs == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(Zhuanshengs);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost,Route("GetZhuanshengByStatus")]
        public IActionResult GetZhuanshengByStatus(int status)
        {
            var query = from z in _coreDbContext.Zhuansheng
                        where z.status == status
                        select z;
            return new OkObjectResult(query);
        }

        /*
        // POST api/values/add
        [HttpPost, Route("addZhuanshengs")]
        public IActionResult AddZhuansheng()
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }

        // POST api/values/Delete
        [HttpPost, Route("EasyDeleteZhuansheng")]
        public IActionResult EasyDeleteZhuanshengById(long id)
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }

        // POST: api/values//Edit
        [HttpPost, Route("EditZhuansheng")]
        public IActionResult EditZhuanshengs(Zhuansheng zhuansheng)
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }
        */

        // GET api/values
        [HttpGet("Toutai")]
        public List<Toutai> GetToutai()
        {
            return _coreDbContext.Set<Toutai>().ToList();
        }

        // POST api/Zhuanshengs/getDeadsById
        [HttpPost, Route("GetToutaiById")]
        public IActionResult GetToutaiById(long id)
        {
            try
            {
                var toutai = _coreDbContext.Zhuansheng.Find(id);
                if (toutai == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(toutai);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost, Route("GetToutaiByStatus")]
        public IActionResult GetToutaiByStatus(int status)
        {
            var query = from t in _coreDbContext.Toutai
                        where t.status == status
                        select t;
            return new OkObjectResult(query);
        }

        /*
        // POST api/values/add
        [HttpPost, Route("addToutai")]
        public IActionResult AddToutai()
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }

        // POST api/values/Delete
        [HttpPost, Route("EasyDeleteToutai")]
        public IActionResult EasyDeleteToutaiById(long id)
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }

        // POST: api/values//Edit
        [HttpPost, Route("EditToutai")]
        public IActionResult EditToutai(Toutai toutai)
        {
            // 未完成，需要另建模块。
            return new EmptyResult();
        }

        [HttpGet,Route("addWell")]
        public IActionResult addWell()
        {
            return new EmptyResult();
        }

        [HttpGet,Route("dropWell")]
        public IActionResult dropWell()
        {
            return new EmptyResult();
        }
        */

        [HttpPost,Route("BeginToutai")]
        public IActionResult BeginToutai(int wellNum=5)
        {
            ToutaiPool.Execute(wellNum);
            return new EmptyResult();
        }

        [HttpGet,Route("getCurrentCompleted")]
        public IActionResult getCompleted()
        {
            var dict = ToutaiPool.GetCompleted();
            return new OkObjectResult(dict);
        }

        [HttpGet,Route("getWaiting")]
        public IActionResult getWating()
        {
            int num = ToutaiPool.getWaiting();
            return new OkObjectResult(num);
        }
    }
}
