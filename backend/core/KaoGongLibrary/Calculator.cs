using DataAccessLibrary.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CommonLibrary1;

namespace KaoGongLibrary
{
    public class Calculator
    {
        private static CoreDbContext _coreDbContext;

        static Calculator()
        {
            var contextOptions = new DbContextOptionsBuilder<CoreDbContext>().UseMySQL(Constant.connectionString).Options;
            _coreDbContext = new CoreDbContext(contextOptions);
        }

        public static void accumulate()
        {
            var query = _coreDbContext.Deads.Where(d => d.status.Equals(Constant.DeadStatus.CALCULATED));
            if (query==null)
            {
                return;
            }
            var deads=query.ToList();

            foreach (Deads d in deads)
            {
                var gongde = _coreDbContext.Gongde.Where(g => g.ObjID.Equals(d.ObjID)).ToList();
                d.value = gongde.Sum(g => g.eventAssess);
                Console.WriteLine(d.value);
                _coreDbContext.Update(d);
            }
            _coreDbContext.SaveChanges();
        }

        public static void distribute(DateTime d)
        {
            accumulate();
            var species = _coreDbContext.Species.ToList();
            species.Sort((x, y) => -x.line.CompareTo(y.line));
            var deads = _coreDbContext.Deads.ToList();
            deads.Sort((x, y) => -x.value.CompareTo(y.value));
            int idx = 0;
            foreach(var s in species)
            {
                var line = s.line;
                for (; idx < deads.Count && deads[idx].value>=line; idx++)
                {
                    deads[idx].tarSpeciesID = s.SpeciesID;
                    _coreDbContext.Update(deads[idx]);
                }
            }
            _coreDbContext.SaveChanges();
        }

        /*
        public static void Main()
        {
            Calculator.distribute(DateTime.Now);
        }
        */
    }
}
