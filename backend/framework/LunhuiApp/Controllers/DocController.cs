using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LunhuiApp.Models;
using MySql.Data.MySqlClient;

namespace LunhuiApp.Controllers
{
    [Route("api/[controller]")]
    public class DocController : ApiController
    {
        [HttpPost]
        [Route("document/getall")]
        public Object getall()
        {
            string conn = "Data Source= 42.192.76.159;port=3306;User ID=difu;Password=1234;DataBase=lunhui";
            MySqlConnection con = new MySqlConnection(conn);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT DISTINCT DATE_FORMAT(deathTime,'%Y-%m-%d') AS date FROM Deads;", con);
            MySqlDataReader dr = cmd.ExecuteReader();
            List<DateTime> datelist = new List<DateTime>();
            DateTime Today = DateTime.Now.AddDays(-1);
            while (dr.Read())
            {
                if (dr.HasRows)
                {
                    DateTime date = dr.GetDateTime("date");
                    datelist.Add(date);
                }
            }
            dr.Close();
            if(datelist.Count == 0)
            {
                return "no documents";
            }
            List<Doc> doclist = new List<Doc>();
            foreach(DateTime date in datelist)
            {
                DateTime nextday = date.AddDays(1);
                Doc doc = new Doc();
                MySqlCommand cald = new MySqlCommand("SELECT COUNT(*) AS cnt FROM Deads WHERE deathTime>=\""+date+"\" and deathTime<=\""+nextday+"\"", con);
                MySqlDataReader ddr = cald.ExecuteReader();
                int dnum = 0;
                while (ddr.Read())
                {
                    if (ddr.HasRows)
                    {
                        dnum = ddr.GetInt32("cnt");
                    }
                }
                ddr.Close();
                MySqlCommand calq = new MySqlCommand("SELECT COUNT(*) AS cnt FROM Deads WHERE status<>3 and deathTime>=\"" + date + "\" and deathTime<=\"" + nextday + "\"", con);
                MySqlDataReader qdr = calq.ExecuteReader();
                int qnum = 0;
                while (qdr.Read())
                {
                    if (qdr.HasRows)
                    {
                        qnum = qdr.GetInt32("cnt");
                    }
                }
                qdr.Close();
                MySqlCommand calb = new MySqlCommand("SELECT count(*) as cnt FROM Toutai WHERE date>=\"" + date + "\" and date<=\"" + nextday + "\"", con);
                MySqlDataReader bdr = calb.ExecuteReader();
                int bnum = 0;
                while (bdr.Read())
                {
                    if (bdr.HasRows)
                    {
                        bnum = bdr.GetInt32("cnt");
                    }
                }
                bdr.Close();
                doc.date = date.ToShortDateString().ToString();
                doc.newborn = bnum;
                doc.newdeads = dnum;
                doc.queue = qnum;
                doclist.Add(doc);
            }
            return doclist;
        }
        [HttpPost]
        [Route("document/gettoday")]
        public Doc getToday()
        {
            string conn = "Data Source= 42.192.76.159;port=3306;User ID=difu;Password=1234;DataBase=lunhui;Connect Timeout=300;";
            MySqlConnection con = new MySqlConnection(conn);
            con.Open();
            DateTime today = DateTime.Now.Date;
            Doc doc = new Doc();
            MySqlCommand cald = new MySqlCommand("SELECT COUNT(*) AS cnt FROM Deads WHERE deathTime>=\"" + today + "\"", con);
            MySqlDataReader ddr = cald.ExecuteReader();
            int dnum = 0;
            while (ddr.Read())
            {
                if (ddr.HasRows)
                {
                    dnum = ddr.GetInt32("cnt");
                }
            }
            ddr.Close();
            MySqlCommand calq = new MySqlCommand("SELECT COUNT(*) AS cnt FROM Deads WHERE status<>3", con);
            MySqlDataReader qdr = calq.ExecuteReader();
            int qnum = 0;
            while (qdr.Read())
            {
                if (qdr.HasRows)
                {
                    qnum = qdr.GetInt32("cnt");
                }
            }
            qdr.Close();
            MySqlCommand calb = new MySqlCommand("SELECT count(*) as cnt FROM Toutai WHERE date>=\"" + today + "\"", con);
            MySqlDataReader bdr = calb.ExecuteReader();
            int bnum = 0;
            while (bdr.Read())
            {
                if (bdr.HasRows)
                {
                    bnum = bdr.GetInt32("cnt");
                }
            }
            bdr.Close();
            doc.date = today.ToShortDateString().ToString();
            doc.newborn = bnum;
            doc.newdeads = dnum;
            doc.queue = qnum;
            return doc;

        }

    }
}