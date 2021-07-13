using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using KaoGongLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860



namespace LunhuiApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeadsController : Controller
    {
        private readonly CoreDbContext _coreDbContext;

        public DeadsController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            var query = from d in _coreDbContext.Deads
                        join s in _coreDbContext.Species
                        on d.SpeciesID equals s.SpeciesID
                        select new DeadInfo
                        {
                            ID = d.ObjID,
                            Name = d.name,
                            species = s.name,
                            speciesID = d.SpeciesID,
                            birth = d.birthTime,
                            death=d.deathTime,
                            reason=d.reason,
                            value=d.value,
                            tarSpeciesID=d.tarSpeciesID,
                            status=d.status
                        };
            return new OkObjectResult(query);
        }

        // POST api/Deads/getDeadsById
        [HttpGet("{id}")]
        public IActionResult GetDeadsById(long id)
        {
            try
            {
                var Deads = _coreDbContext.Deads.Find(id);
                if (Deads == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(Deads);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        [HttpPost,Route("status")]
        public IActionResult GetDeadByStatus(int status)
        {
            try
            {
                var deads = from d in _coreDbContext.Deads
                            join s in _coreDbContext.Species
                            on d.SpeciesID equals s.SpeciesID
                            where d.status.Equals(status)
                            select new DeadInfo
                            {
                                ID = d.ObjID,
                                Name = d.name,
                                species = s.name,
                                speciesID = d.SpeciesID,
                                birth = d.birthTime,
                                death = d.deathTime,
                                reason = d.reason,
                                value = d.value,
                                tarSpeciesID=d.tarSpeciesID,
                                status = d.status
                            };

                return new OkObjectResult(deads);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        // POST api/values/add
        [HttpPost, Route("addDeads")]
        public IActionResult AddDeads(Deads deads)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                //检查该死者是否已经在数据库中
                var checkd = _coreDbContext.Deads.SingleOrDefault(d => d.name.Equals(deads.name) && d.birthTime.Equals(deads.birthTime) && d.deathTime.Equals(deads.deathTime));
                if (checkd != null)
                {

                    dic.Add("message", "Deads " + deads.name + " has been in the database");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }

                //检查该物种是否已经在数据库中
                var checks = _coreDbContext.Species.SingleOrDefault(s => s.SpeciesID.Equals(deads.SpeciesID));
                if (checks == null)
                {
                    dic.Add("message", "Species " + deads.SpeciesID + " not in the database, please add species first!");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }

                // 添加数据到数据库
                _coreDbContext.Deads.Add(deads);
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
        [HttpPost, Route("EasyDeleteDeads")]
        public IActionResult EasyDeleteById(long id)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                // 检查该物种是否在数据库中
                Deads Deads = _coreDbContext.Deads.Find(id);
                if (Deads == null)
                {
                    dic.Add("message", "数据库中没有该死者");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }

                // 检查有无外键引用约束
                if (_coreDbContext.Deads.Any(d => d.SpeciesID == id) || _coreDbContext.Zhuansheng.Any(z => z.SpeciesID == id))
                {
                    dic.Add("message", "该死者在表中已有引用，请将其引用全部清除，或者选用复杂删除（这可能会删除大量的其他表的数据，也相当耗时），或者放弃删除。");
                    dic.Add("status", "warning");
                    return new BadRequestObjectResult(dic);
                }

                // 删除数据库信息
                _coreDbContext.Deads.Remove(Deads);
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
        [HttpPost, Route("EditDeads")]
        public IActionResult EditDeads(Deads deads)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                // 检查死者是否在数据库中
                Deads d = _coreDbContext.Deads.Find(deads.ObjID);
                if (d == null)
                {
                    dic.Add("message", "Deads " + deads.ObjID + " " + deads.name + " not in the database");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }
                d.Clone(deads);
                //修改数据库信息
                _coreDbContext.Update(d);
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

        
        // POST: api/values/ViewDetails
        [HttpPost, Route("ViewGongde")]
        public IActionResult viewGongdes(long objID)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                // 检查死者是否在数据库中
                Deads checkp = _coreDbContext.Deads.Find(objID);
                if (checkp == null)
                {
                    dic.Add("message", "Deads " + objID + " not in the database");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }

                // 获取死者功德
                var records = _coreDbContext.Gongde.Where(g => g.ObjID.Equals(objID)).Select(g => new { g.eventID, g.eventTime, g.eventContent, g.eventAssess });
                if (records==null)
                {
                    return new NotFoundResult();
                }
                
                dic.Add("message", "查询成功");
                dic.Add("status", "success");
                dic.Add("gongdes", records);
                
                return new OkObjectResult(dic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        /*
        [HttpPost,Route("Calculate")]
        public IActionResult calculateGonde(DateTime d)
        {
            Calculator.distribute(d);
            return new OkResult();
        }
        */

        private class DeadInfo
        {
            public long ID { get; set; }
            public string Name { get; set; }
            public string species { get; set; }
            public long speciesID { get; set; }
            public DateTime birth { get; set; }
            public DateTime death { get; set; }
            public string reason { get; set; }
            public int value { get; set; }
            public long tarSpeciesID { get; set; }
            public int status { get; set; }
        }
    }
}
