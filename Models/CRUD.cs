using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace CRS.Models
{
    public class CRUD
    {
        public static string connectionString = "data source=localhost\\SQLEXPRESS; Initial Catalog=CRS;Integrated Security=true";

        public static string SignUp(string Email, string Fname, string Lname, string Password, string cnic)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;

            try
            {
                cmd = new SqlCommand("user_signup", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 40).Value = Email;
                cmd.Parameters.Add("@fname", SqlDbType.NVarChar, 20).Value = Fname;
                cmd.Parameters.Add("@lname", SqlDbType.NVarChar, 20).Value = Lname;

                if (cnic != null)
                    cmd.Parameters.Add("@cnic", SqlDbType.NVarChar, 15).Value = cnic;

                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 20).Value = Password;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public static string Login(string Email, string Password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("user_login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 40).Value = Email;
                cmd.Parameters.Add("@pass", SqlDbType.NVarChar, 20).Value = Password;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Del_User(string username){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Del_User", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 40).Value = username;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Add_Suggestion(string filer, string content, int deptid){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Add_Suggestion", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@filer", SqlDbType.NVarChar, 40).Value = filer;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar, -1).Value = content;
                cmd.Parameters.Add("@deptId", SqlDbType.Int).Value = deptid;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Add_Complaint(string filer, string content, int deptid, string deadline, string priority){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Add_Complaint", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@filer", SqlDbType.NVarChar, 40).Value = filer;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar, -1).Value = content;
                cmd.Parameters.Add("@deptId", SqlDbType.Int).Value = deptid;
                cmd.Parameters.Add("@deadline", SqlDbType.Date).Value = deadline;
                cmd.Parameters.Add("@priority", SqlDbType.NVarChar, 10).Value = priority;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Add_Comment(int complaintId, string commentorEmail, string content){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Add_Comment", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@complaintId", SqlDbType.Int).Value = complaintId;
                cmd.Parameters.Add("@commentorEmail", SqlDbType.NVarChar, 40).Value = commentorEmail;
                cmd.Parameters.Add("@content", SqlDbType.NVarChar, -1).Value = content;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Add_Image(byte[] imageData, string type, int feedbackId){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Add_Image", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = imageData;
                cmd.Parameters.Add("@type", SqlDbType.NVarChar, 5).Value = type;
                cmd.Parameters.Add("@feedbackId", SqlDbType.Int).Value = feedbackId;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Set_Deadline(string deadline, int comid){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Set_Deadline", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@comid", SqlDbType.Int).Value = comid;
                cmd.Parameters.Add("@deadline", SqlDbType.Date).Value = deadline;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Set_Priority(string priority, int comid){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Set_Priority", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@comid", SqlDbType.Int).Value = comid;
                cmd.Parameters.Add("@priority", SqlDbType.NVarChar, 10).Value = priority;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Complaint_Resolved(int cid){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Complaint_Resolved", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = cid;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Complaint_Rejected(int cid){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;
            try
            {
                cmd = new SqlCommand("Complaint_Rejected", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@cid", SqlDbType.Int).Value = cid;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Add_Department(string Dept_Name, string Admin_Email, string Admin_Fname, string Admin_Lname, string Admin_Password, string Admin_Cnic)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;

            try
            {
                cmd = new SqlCommand("Add_Department", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Dept_Name", SqlDbType.NVarChar, 60).Value = Dept_Name;
                cmd.Parameters.Add("@Admin_Email", SqlDbType.NVarChar, 40).Value = Admin_Email;
                cmd.Parameters.Add("@Admin_Fname", SqlDbType.NVarChar, 20).Value = Admin_Fname;
                cmd.Parameters.Add("@Admin_Lname", SqlDbType.NVarChar, 20).Value = Admin_Lname;

                if (Admin_Cnic != null)
                    cmd.Parameters.Add("@Admin_Cnic", SqlDbType.NVarChar, 15).Value = Admin_Cnic;

                cmd.Parameters.Add("@Admin_Password", SqlDbType.NVarChar, 20).Value = Admin_Password;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Del_Dept(int Dept_Id)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;

            try
            {
                cmd = new SqlCommand("Del_Dept", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Dept_Id", SqlDbType.Int).Value = Dept_Id;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Change_Admin(int Dept_Id, string Admin_Email, string Admin_Fname, string Admin_Lname, string Admin_Password, string Admin_Cnic)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;

            try
            {
                cmd = new SqlCommand("Change_Admin", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@Dept_Id", SqlDbType.Int).Value = Dept_Id;
                cmd.Parameters.Add("@Admin_Email", SqlDbType.NVarChar, 40).Value = Admin_Email;
                cmd.Parameters.Add("@Admin_Fname", SqlDbType.NVarChar, 20).Value = Admin_Fname;
                cmd.Parameters.Add("@Admin_Lname", SqlDbType.NVarChar, 20).Value = Admin_Lname;

                if (Admin_Cnic != null)
                    cmd.Parameters.Add("@Admin_Cnic", SqlDbType.NVarChar, 15).Value = Admin_Cnic;

                cmd.Parameters.Add("@Admin_Password", SqlDbType.NVarChar, 20).Value = Admin_Password;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static string Change_Password(string username, string password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            string result;

            try
            {
                cmd = new SqlCommand("Change_Password", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 40).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 20).Value = password;
                cmd.Parameters.Add("@OutputMsg", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                result = Convert.ToString(cmd.Parameters["@OutputMsg"].Value);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = "Technical Error! Please try later!!"; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static List<T> GetRecord<T>(string query) where T : new(){
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            SqlDataReader dataReader;
            List<T> result = new List<T>();
            try
            {
                cmd = new SqlCommand(query, con);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read()){
                    T instance = new T();
                    int i = 0;
                    foreach(var prop in typeof(T).GetProperties()) {
                        if (prop.CanWrite){
                            if (prop.Name == "imgdata"){
                                prop.SetValue(instance, System.Convert.ToBase64String((byte[])dataReader.GetValue(i)));
                            }
                            else{
                                prop.SetValue(instance, dataReader.GetValue(i).ToString());
                            }
                            i++;
                        }
                    }
                    result.Add(instance);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = null; 
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}