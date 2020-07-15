using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using CRS.Models;

namespace CRS.Controllers
{
    public class Authentication : Controller {

        public ActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") != null){
                return RedirectToAction("User_Homepage", "Home");
            }
            return View("Login");
        }

        public ActionResult SignUp()
        {
            if (HttpContext.Session.GetString("Email") != null){
                return RedirectToAction("User_Homepage", "Home");
            }
            return View("SignUp");
        }

        public ActionResult SignUpProc(string Email, string Fname, string Lname, string Password, string cnic)
        {
            string result = CRUD.SignUp(Email, Fname, Lname, Password, cnic);

            if (result != "User Created!"){
                ViewBag.Error = result;
                return SignUp();
            }
            return View("Login", (object)result);
        }

        public ActionResult LoginProc(string Email, string Password)
        {
            string result = CRUD.Login(Email, Password);

            if (result == "User" || result == "Admin" || result == "MainAdmin"){
                user active = user.GetWhere("email = '" + Email + "'")[0];
                HttpContext.Session.Set("Email", System.Text.Encoding.ASCII.GetBytes(Email));
                HttpContext.Session.Set("Fname", System.Text.Encoding.ASCII.GetBytes(active.fname));
                HttpContext.Session.Set("Lname", System.Text.Encoding.ASCII.GetBytes(active.lname));
                HttpContext.Session.Set("Type", System.Text.Encoding.ASCII.GetBytes(result));
                return RedirectToAction(result + "_Homepage", "Home");
            }
            else{
                return View("Login", (object)result);
            }
        }

        public ActionResult logout(){
            HttpContext.Session.Clear();
            return View("Login");
        }
    }
}