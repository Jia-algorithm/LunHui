using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using LunhuiApp.Models;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
//using ATLProject4Lib;
//using Encrypt;
namespace LunhuiApp.Controllers
{
    /// <summary>
    /// controller for login and register
    /// </summary>
    [Route("api/[controller]")]
    public class AdminController : ApiController
    {
        /// <summary>
        /// an input format
        /// </summary>
        public class Param1
        {
            /// <summary>
            /// user password
            /// </summary>
            public string password { get; set; }
            /// <summary>
            /// user name
            /// </summary>
            public string name { get; set; }
        }
        /// <summary>
        /// an input format
        /// </summary>
        public class Param2
        {
            /// <summary>
            /// user password
            /// </summary>
            public string password { get; set; }
            /// <summary>
            /// username
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// code to verify
            /// </summary>
            public string vericode { get; set; }
        }
        /// POST: login
        [HttpPost]
        [Route("login")]
        public string Login([FromBody] Param1 user)
        {

            string conn = "Data Source= 42.192.76.159;port=3306;User ID=difu;Password=1234;DataBase=lunhui";
            MySqlConnection con = new MySqlConnection(conn);
            con.Open();
            //Encryption desEncryption = new Encryption();
            //int newname = desEncryption.Encryptid(user.name);
            int newname = 0;
            for (int i = 0; i < user.name.Length; i++) //遍历字符串
            {
                char ch = user.name[i];
                newname = newname + ch;
            }
            MySqlCommand cmd = new MySqlCommand("select * from Admin where AdminName=\"" + newname + "\"", con);
            MySqlDataReader dr = cmd.ExecuteReader();
            string temp = "";
            while (dr.Read())
            {
                if (dr.HasRows)
                {
                    temp = dr.GetString("password");
                }
            }
            if (temp.Equals(""))
                return "no username";
            dr.Close();
            con.Close();
            //MD5Encryption Encryption = new MD5Encryption();

            if (temp.Equals(user.password))
            {
                return "success";
            }
            else
                return "password error";
        }
        /// POST: register
        [HttpPost]
        [Route("register")]
        public string Register([FromBody] Param2 reader)
        {
            string conn = "Data Source= 42.192.76.159;port=3306;User ID=difu;Password=1234;DataBase=lunhui;";
            MySqlConnection con = new MySqlConnection(conn);
            con.Open();
            //Encryption desEncryption = new Encryption();
            //int newname = desEncryption.Encryptid(reader.name);
            int newname = 0;
            for (int i = 0; i < reader.name.Length; i++) //遍历字符串
            {
                char ch = reader.name[i];
                newname = newname + ch;
            }
            MySqlCommand findexist = new MySqlCommand("select * from Admin where AdminName=\"" + newname + "\"", con);
            MySqlDataReader rd = findexist.ExecuteReader();
            while (rd.Read())
            {
                if (rd.HasRows)
                {
                    return "userexist";
                }
            }
            rd.Close();
            MySqlCommand cmd = new MySqlCommand("select max(ID) as maxid from Admin", con);
            MySqlDataReader rr = cmd.ExecuteReader();
            int temp = 0;
            while (rr.Read())
            {
                if (rr.HasRows)
                {
                    temp = rr.GetInt32("maxid");
                }
            }
            temp += 1;
            //MD5Encryption Encryption = new MD5Encryption();
            //string pass = Encryption.Encryption(reader.password);
            rr.Close();
            if(reader.vericode == "DFLH2021")
            {
                try
                {
                    cmd = new MySqlCommand("insert into Admin(ID,AdminName,password) values(" + temp + "," + newname + "," +"\""+ reader.password + "\")", con);
                    int val = cmd.ExecuteNonQuery();
                }
                catch (SystemException e)
                {
                    return "fail";
                }
                return "success";
            }
            else
            {
                return "wrong code";
            }
            con.Close();
        }
    }
}