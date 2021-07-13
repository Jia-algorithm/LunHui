using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLibrary.Models;

namespace LunhuiApp.Controllers
{
    [Route("api/[controller]")]
    [Microsoft.AspNetCore.Mvc.ApiController]
    public class SpeciesController : Controller
    {
        private readonly CoreDbContext _coreDbContext;

        public SpeciesController(CoreDbContext coreDbContext)
        {
            _coreDbContext = coreDbContext;
        }

        // GET api/values
        [HttpGet]
        public List<Species> Get()
        {
            return _coreDbContext.Species.ToList();
        }

        // GET api/Species/getSpeciesById
        [HttpGet("{id}")]
        public IActionResult GetSpeciesById(long id)
        {
            try
            {
                var species = _coreDbContext.Species.Find(id);
                if (species==null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(species);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new BadRequestObjectResult(e);
            }
        }

        // POST api/values/add
        [HttpPost, Route("addSpecies")]
        public IActionResult AddSpecies(Species species)
        {
            try
            {
                //检查该物种是否已经在数据库中
                Dictionary<string, object> dic = new Dictionary<string, object>();
                var checkp = _coreDbContext.Species.SingleOrDefault(s => s.name.Equals(species.name));
                if (checkp != null)
                {
                    dic.Add("message", "Species " + species.name + " has been in the database");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }
                // 添加数据到数据库
                _coreDbContext.Species.Add(species);
                _coreDbContext.SaveChanges();
                var news = _coreDbContext.Species.SingleOrDefault(s => s.name.Equals(species.name));
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
        [HttpPost, Route("EasyDeleteSpecies")]
        public IActionResult EasyDeleteById(long id)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();

                // 检查该物种是否在数据库中
                Species species = _coreDbContext.Species.Find(id);
                if (species == null)
                {
                    dic.Add("message", "数据库中没有该物种");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }

                // 检查有无外键引用约束
                if (_coreDbContext.Deads.Any(d => d.SpeciesID == id) || _coreDbContext.Zhuansheng.Any(z => z.SpeciesID == id))
                {
                    dic.Add("message", "该物种在表中已有引用，请将其引用全部清除，或者选用复杂删除（这可能会删除大量的其他表的数据，也相当耗时），或者放弃删除。");
                    dic.Add("status", "warning");
                    return new BadRequestObjectResult(dic);
                }

                // 删除数据库信息
                _coreDbContext.Species.Remove(species);
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
        [HttpPost, Route("EditSpecies")]
        public ActionResult EditSpecies(Species species)
        {
            try
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                // 检查是否存在于数据库中
                Species s = _coreDbContext.Species.Find(species.SpeciesID);
                if (s == null)
                {
                    dic.Add("message", "物种 " + species.SpeciesID+" "+species.name + " 不在数据库中");
                    dic.Add("status", "error");
                    return new BadRequestObjectResult(dic);
                }
                s.Clone(species);
                //修改数据库信息
                _coreDbContext.Update(s);
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
    }
}
