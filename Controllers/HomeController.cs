using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CRS.Models;
using System.IO;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Hosting;

namespace CRS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Homes
        public IActionResult User_Homepage()
        {
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.depts = dept.GetAll();
            ViewBag.Complaint = complaints.GetWhereUser("filer = '" + ViewBag.Email + "' ORDER BY id");
            ViewBag.feedback = feedback.GetWhere("filer = '" + ViewBag.Email + "' AND [type] = 'Complaint' ORDER BY id");
            return View("User_Homepage");
        }

        public IActionResult MainAdmin_Homepage(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.depts = dept.GetAll();
            return View();
        }

        public IActionResult Admin_Homepage(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            ViewBag.Count = complaints.GetCount("deptId = " + ViewBag.dept[0].id + " AND [priority] != 'Done' AND [priority] != 'Rejected'");
            ViewBag.ComplaintsCount = feedback.GetCount("[type] = 'Complaint' AND deptId = " + ViewBag.Dept[0].id);
            ViewBag.SuggestionsCount = feedback.GetCount("[type] = 'Suggestion' AND deptId = " + ViewBag.Dept[0].id);
            return View();
        }

        public IActionResult Full_Complaint_Log(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Count = feedback.GetCount("[type] = 'Complaint'");
            ViewBag.feedback = feedback.GetWhere("[type] = 'Complaint' ORDER BY id");
            ViewBag.Complaints = complaints.GetAll();
            return View("Complaint_Log");
        }
        
        public IActionResult Complaint_Log(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Count = feedback.GetCount("[type] = 'Complaint' AND deptId = " + id);
            ViewBag.feedback = feedback.GetWhere("[type] = 'Complaint' AND deptId = " + id + " ORDER BY id");
            ViewBag.Complaints = complaints.GetWhereUser("deptId = " + id);
            ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            return View("Complaint_Log");
        }

        public IActionResult Full_Suggestion_Log(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Count = feedback.GetCount("[type] = 'Suggestion'");
            ViewBag.feedback = feedback.GetWhere("[type] = 'Suggestion' ORDER BY id");
            return View("Suggestion_Log");
        }
        
        public IActionResult Suggestion_Log(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Count = feedback.GetCount("[type] = 'Suggestion' AND deptId = " + id);
            ViewBag.feedback = feedback.GetWhere("[type] = 'Suggestion' AND deptId = " + id + " ORDER BY id");
            ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            return View("Suggestion_Log");
        }

        public IActionResult DelDept(string id){
            CRUD.Del_Dept(Convert.ToInt32(id));
            return RedirectToAction("MainAdmin_Homepage", "Home");
        }

        public IActionResult Add_Admin(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.type = "Admin";
            ViewBag.id = id;
            return View("Add_Admin");
        }

        public IActionResult Add_Admin_Proc(int Dept_Id, String Email, String Fname, String Lname, String Password, String cnic){
            string result = CRUD.Change_Admin(Dept_Id,Email, Fname, Lname, Password, cnic);
            System.Console.Write(Dept_Id);
            if (result != "Admin Changed!"){
                ViewBag.Error = result;
                return Add_Admin(Convert.ToString(Dept_Id));
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Dept = dept.GetWhere("id = " + Dept_Id);
            ViewBag.Admin = user.GetWhere("email = '" + ViewBag.Dept[0].AdminEmail + "'");
            ViewBag.ComplaintsCount = feedback.GetCount("[type] = 'Complaint' AND deptId = " + ViewBag.Dept[0].id);
            ViewBag.SuggestionsCount = feedback.GetCount("[type] = 'Suggestion' AND deptId = " + ViewBag.Dept[0].id);
            return View("Dept", (object)result);
        }

        public IActionResult Add_Dept(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.type = "Dept";
            return View("Add_Admin");
        }

        public IActionResult Add_Dept_Proc(String Dept_Name, String Email, String Fname, String Lname, String Password, String cnic){
            string result = CRUD.Add_Department(Dept_Name,Email, Fname, Lname, Password, cnic);
            if (result != "Department Created!"){
                ViewBag.Error = result;
                return Add_Dept();
            }
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.depts = dept.GetAll();
            return View("MainAdmin_Homepage", (object)result);
        }

        public IActionResult Individual_Complaint(string id, string error) {
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            if (ViewBag.Type == "Admin"){
                ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            }
            Dictionary<string, user> commentors = new Dictionary<string, user>();
            Dictionary<string, string> color = new Dictionary<string, string>();
            ViewBag.Complaint = complaints.GetWhere("id = " + id);
            ViewBag.feedback = feedback.GetWhere("id = " + id);
            ViewBag.img = img.GetWhere("feedbackid = " + id);
            ViewBag.Comments = comments.GetWhere("complaintId = " + id);
            string[] c = {"#e8b047","#aba4cc"};
            int i = 0;
            foreach (var item in ViewBag.Comments)
            {
                if (commentors.ContainsKey(item.CommentorEmail) == false){
                    commentors[item.CommentorEmail] = user.GetWhere("email = '" + item.CommentorEmail + "'")[0];
                    color[item.CommentorEmail] = c[i++];
                }
            }
            ViewBag.Error = error;
            ViewBag.color = color;
            ViewBag.commentors = commentors;
            return View("Individual_Complaint");
        }

        public IActionResult Pass(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            if (ViewBag.Type == "Admin"){
                ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            }
            return View("Change_pass");
        }

        public IActionResult Individual_Suggestion(string id) {
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.feedback = feedback.GetWhere("id = " + id);
            ViewBag.img = img.GetWhere("feedbackid = " + id);
            if (ViewBag.Type == "Admin"){
                ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            }
            return View("Individual_Suggestion");
        }

        public IActionResult Change_Pass(string pass){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            if (ViewBag.Type == "Admin"){
                ViewBag.dept = dept.GetWhere("AdminEmail = '" + ViewBag.Email + "'");
            }
            string email = HttpContext.Session.GetString("Email");
            string result = CRUD.Change_Password(email, pass);
            return View("Change_Pass", (object)result);
        }

        public IActionResult Dept(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Dept = dept.GetWhere("id = " + id);
            ViewBag.Admin = user.GetWhere("email = '" + ViewBag.Dept[0].AdminEmail + "'");
            ViewBag.ComplaintsCount = feedback.GetCount("[type] = 'Complaint' AND deptId = " + ViewBag.Dept[0].id);
            ViewBag.SuggestionsCount = feedback.GetCount("[type] = 'Suggestion' AND deptId = " + ViewBag.Dept[0].id);
            return View();
        }

        public IActionResult Disp_Users(){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "User"){
                return RedirectToAction("User_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.Users = user.GetAll();
            return View();
        }

        public IActionResult Del_User(string id){
            if (id == null){
                CRUD.Del_User(HttpContext.Session.GetString("Email"));
                return RedirectToAction("Logout", "Authentication");
            }
            else{
                CRUD.Del_User(id);
                return RedirectToAction("Disp_Users", "Home");
            }
        }

        public IActionResult Add_Sugg(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.chosen = id;
            ViewBag.depts = dept.GetAll();
            return View();
        }

        public IActionResult Add_Comp(string id){
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.chosen = id;
            ViewBag.depts = dept.GetAll();
            return View();
        }

        public IActionResult Add_Comment(string complaintid, string content){
            CRUD.Add_Comment(Convert.ToInt32(complaintid), HttpContext.Session.GetString("Email"), content);
            return RedirectToAction("Individual_Complaint", new { id = complaintid });
        }

        public IActionResult Add_CompProc(string deptname, string content, List<IFormFile> img){
            string result;
            List<byte[]> images = new List<byte[]>();
            List<string> imagesType = new List<string>();
            try
            {
                foreach (var file in img)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            byte[] fileBytes = ms.ToArray();
                            string type = file.ContentType.Split('/')[1];
                            images.Add(fileBytes);
                            imagesType.Add("." + type);
                        }
                    }
                }
                int deptid = Convert.ToInt32(dept.GetWhere("name = '" + deptname + "'")[0].id);
                result = CRUD.Add_Complaint(HttpContext.Session.GetString("Email"), content, deptid, DateTime.Today.AddDays(7).ToString(), "Low");
                if (result == "Complaint Registered!"){
                    int fid = Convert.ToInt32(CRUD.GetRecord<ftemp>("SELECT MAX(id) FROM Feedback;")[0].count);
                    for (int i = 0; i < images.Count; ++i)
                    {
                        CRUD.Add_Image(images[i], imagesType[i], fid);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Image Error: " + ex.ToString());
                result = "Technical Error Try Later!";
                throw;
            }
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.depts = dept.GetAll();
            return View("Add_Comp", (object)result);
        }

        public IActionResult Add_SuggProc(string deptname, string content, List<IFormFile> img){
            string result;
            List<byte[]> images = new List<byte[]>();
            List<string> imagesType = new List<string>();
            try
            {
                foreach (var file in img)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            byte[] fileBytes = ms.ToArray();
                            string type = file.ContentType.Split('/')[1];
                            images.Add(fileBytes);
                            imagesType.Add("." + type);
                        }
                    }
                }
                int deptid = Convert.ToInt32(dept.GetWhere("name = '" + deptname + "'")[0].id);
                result = CRUD.Add_Suggestion(HttpContext.Session.GetString("Email"), content, deptid);
                if (result == "Suggestion Added!"){
                    int fid = Convert.ToInt32(CRUD.GetRecord<ftemp>("SELECT MAX(id) FROM Feedback;")[0].count);
                    for (int i = 0; i < images.Count; ++i)
                    {
                        CRUD.Add_Image(images[i], imagesType[i], fid);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Image Error: " + ex.ToString());
                result = "Technical Error Try Later!";
                throw;
            }
            if ( HttpContext.Session.GetString("Email") == null){
                return RedirectToAction("Login", "Authentication");
            }
            if ( HttpContext.Session.GetString("Type") == "Admin"){
                return RedirectToAction("Admin_Homepage", "Home");
            }
            else if ( HttpContext.Session.GetString("Type") == "MainAdmin"){
                return RedirectToAction("MainAdmin_Homepage", "Home");
            }
            ViewBag.Fname = HttpContext.Session.GetString("Fname");
            ViewBag.Lname = HttpContext.Session.GetString("Lname");
            ViewBag.Email = HttpContext.Session.GetString("Email");
            ViewBag.Type = HttpContext.Session.GetString("Type");
            ViewBag.depts = dept.GetAll();
            return View("Add_Sugg", (object)result);
        }

        public IActionResult Res(string id){
            string result = CRUD.Complaint_Resolved(Convert.ToInt32(id));
            return RedirectToAction("Individual_Complaint", new { id = id , error = result});
        }

        public IActionResult Rej(string id){
            string result = CRUD.Complaint_Rejected(Convert.ToInt32(id));
            return RedirectToAction("Individual_Complaint", new { id = id , error = result});
        }

        public IActionResult Priority(string prior, string cid){
            string result = CRUD.Set_Priority(prior, Convert.ToInt32(cid));
            return RedirectToAction("Individual_Complaint", new { id = cid , error = result});
        }

        public IActionResult deadline(string deadline, string cid){
            DateTime now = DateTime.Now;
            if (now > Convert.ToDateTime(deadline)){
                ViewBag.Error = "Wrong";
                return RedirectToAction("Individual_Complaint", new { id = cid , error = "Deadline Should Be Ahead Of Today!"});
            }
            string result = CRUD.Set_Deadline(deadline, Convert.ToInt32(cid));
            return RedirectToAction("Individual_Complaint", new { id = cid , error = result});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}