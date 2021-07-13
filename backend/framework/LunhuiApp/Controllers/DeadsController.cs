using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LunhuiApp.Models;
using MySql.Data.MySqlClient;
using System.Runtime.InteropServices;
namespace LunhuiApp.Controllers
{
    /// <summary>
    /// deads api
    /// </summary>
    [Route("api/[controller]")]
    public class DeadsController : ApiController
    {
        /// <summary>
        /// compare two integer
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        //[DllImport(@"..\bin\Win32dll.dll",CallingConvention = CallingConvention.Cdecl)]
        //public static extern int Compare(int a, int b);
        /// <summary>
        /// api for calculate gongde
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("calculate")]
        public Object Calculate()
        {
            string conn = "Data Source= 42.192.76.159;port=3306;User ID=difu;Password=1234;DataBase=lunhui;Connect Timeout=300;";
            MySqlConnection con = new MySqlConnection(conn);
            con.Open();
            MySqlCommand cmd1 = new MySqlCommand("select * from Species", con);
            MySqlDataReader dr1 = cmd1.ExecuteReader();
            Dictionary<string, int> dic = new Dictionary<string, int>();
            while (dr1.Read())
            {
                if (dr1.HasRows)
                {
                    string id = Convert.ToString(dr1[0]);
                    int line = Convert.ToInt32(dr1[2]);
                    dic.Add(id, line);
                }
            }
            dr1.Close();
            MySqlCommand cmd = new MySqlCommand("select ObjID from Deads where status = 1",con);
            MySqlDataReader dr = cmd.ExecuteReader();
            List<int> idlist = new List<int>();
            while (dr.Read())
            {
                if (dr.HasRows)
                {
                    int id = Convert.ToInt32(dr[0]);
                    idlist.Add(id);
                }
            }
            dr.Close();
            if (idlist.Count==0)
            {
                return "no to calculate";
            }
            
            foreach(int id in idlist)
            {

                MySqlCommand find = new MySqlCommand("select sum(eventAssess) as value from Gongde where ObjID="+id, con);
                int Value = 0;
                MySqlDataReader fdr = find.ExecuteReader();
                while (fdr.Read())
                {
                    if (fdr.HasRows)
                    {
                        try
                        {
                            Value = fdr.GetInt32("value");
                        }
                        catch
                        {
                            Value = 0;
                        }
                        
                    }
                }
                
                fdr.Close();
                int target = -2;
                foreach(var item in dic)
                {
                    if(item.Value > dic[target.ToString()] && item.Value < Value)
                    {
                        target = Convert.ToInt32(item.Key);
                    }
                }
                MySqlCommand update = new MySqlCommand("update Deads set value="+Value+",status=2,tarSpeciesID="+target+" where ObjID="+ id,con);
                try
                {
                    int udr = update.ExecuteNonQuery();
                    
                }
                catch
                {
                    string error = "update failure";
                    object res = error;
                    return res;
                }

            }
            con.Close();
            string suc = "success";
            object backres = suc;
            return backres;
            
        }
        
    }
}