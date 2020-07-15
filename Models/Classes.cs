using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace CRS.Models
{
    public class user{
        public string email { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string cnic { get; set; }
        public string type { get; set; }
        public string password { get; set; }

        public static List<user> GetAll(){
            return CRUD.GetRecord<user>("SELECT * FROM Users;");
        }

        public static List<user> GetWhere(string query){
            return CRUD.GetRecord<user>("SELECT * FROM Users WHERE " + query + ";");
        }
    }

    public class dept{
        public string id { get; set; }
        public string name { get; set; }
        public string AdminEmail { get; set; }
        public static List<dept> GetAll(){
            return CRUD.GetRecord<dept>("SELECT * FROM department;");
        }

        public static List<dept> GetWhere(string query){
            return CRUD.GetRecord<dept>("SELECT * FROM department WHERE " + query + ";");
        }

        public List<user> GetAdmin(){
            return CRUD.GetRecord<user>("SELECT * FROM Users WHERE email = '" + AdminEmail + "';");
        }
        public string initials(){
            char finit = name[0];
            int sec = name.IndexOf(' ');
            char sinit = name[sec+1];
            return Char.ToString(finit) + Char.ToString(sinit);
        }
    }

    class ftemp{
        public string count { get; set; }
    }

    public class feedback{
        public string id { get; set; }
        public string filer { get; set; }
        public string content { get; set; }
        public string deptId { get; set; }
        public string type { get; set; }

        public static List<feedback> GetAll(){
            return CRUD.GetRecord<feedback>("SELECT * FROM Feedback;");
        }

        public static List<feedback> GetWhere(string query){
            return CRUD.GetRecord<feedback>("SELECT * FROM Feedback WHERE " + query + ";");
        }

        public static int GetCount(string query){
            string temp =  CRUD.GetRecord<ftemp>("SELECT COUNT(*) FROM Feedback WHERE " + query + ";")[0].count;
            return Convert.ToInt32(temp);
        }
    }

    public class complaints{
        public string id { get; set; }
        public string deadline { get; set; }
        public string priority { get; set; }

        public static List<complaints> GetAll(){
            return CRUD.GetRecord<complaints>("SELECT * FROM Complaints;");
        }

        public static List<complaints> GetWhere(string query){
            return CRUD.GetRecord<complaints>("SELECT * FROM Complaints WHERE " + query + ";");
        }

        public static List<complaints> GetWhereUser(string query){
            return CRUD.GetRecord<complaints>("SELECT Complaints.id, [deadline], [priority] FROM Complaints Join Feedback ON Complaints.id=Feedback.id WHERE " + query + ";");
        }

        public List<feedback> GetWhole(){
            return CRUD.GetRecord<feedback>("SELECT * FROM Feedback WHERE id = " + id + ";");
        }

        public static int GetCount(string query){
            string temp =  CRUD.GetRecord<ftemp>("SELECT COUNT(*) FROM Complaints Join Feedback ON Complaints.id=Feedback.id WHERE " + query + ";")[0].count;
            return Convert.ToInt32(temp);
        }
    }

    public class img{
        public string id { get; set; }
        public string imgdata { get; set; }
        public string type { get; set; }
        public string feedbackId { get; set; }

        public static List<img> GetAll(){
            return CRUD.GetRecord<img>("SELECT * FROM Image;");
        }

        public static List<img> GetWhere(string query){
            return CRUD.GetRecord<img>("SELECT * FROM Image WHERE " + query + ";");
        }
    }

    public class comments{
        public string ComplaintId { get; set; }
        public string CommentorEmail { get; set; }
        public string content { get; set; }
        public string timestamp { get; set; }

        public static List<comments> GetAll(){
            return CRUD.GetRecord<comments>("SELECT * FROM Comments;");
        }

        public static List<comments> GetWhere(string query){
            return CRUD.GetRecord<comments>("SELECT * FROM Comments WHERE " + query + ";");
        }
    }
}