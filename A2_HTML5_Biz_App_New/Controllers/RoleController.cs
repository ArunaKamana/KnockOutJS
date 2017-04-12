using A2_HTML5_Biz_App_New.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace A2_HTML5_Biz_App_New.Controllers
{
   
        [Authorize(Roles = "Administrator")]
        public class RoleController : Controller
        {
            PropertyStoreEntities ctx;

            public RoleController()
            {
                ctx = new PropertyStoreEntities();
            }

            // GET: RoleController
            public ActionResult Index()
            {
                var appRoles = ctx.OwnerInfoes.ToList();
                return View(appRoles);
            }

            public ActionResult Create()
            {
                return View();
            }

            // Action method to create Roles
            [HttpPost]
            public ActionResult Create(FormCollection collection)
            {
                try
                {
                    ctx.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                    {
                        Name = collection["RoleName"]
                    });
                    ctx.SaveChanges();
                    ViewBag.ResultMessage = "Role created successfully !";
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }



            public ActionResult AssignUserToRole()
            {
                var list = ctx.Roles.OrderBy(role => role.Name).ToList().Select(role => new SelectListItem { Value = role.Name.ToString(), Text = role.Name }).ToList();
                ViewBag.Roles = list;
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult UserAddToRole(string UserName, string RoleName)
            {
                ApplicationUser user = ctx.Users.Where(usr => usr.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                // Display Roles in DropDown

                var list = ctx.Roles.OrderBy(role => role.Name).ToList().Select(role => new SelectListItem { Value = role.Name.ToString(), Text = role.Name }).ToList();
                ViewBag.Roles = list;

                if (user != null)
                {
                    var account = new AccountController();
                    account.UserManager.AddToRoleAsync(user.Id, RoleName);

                    ViewBag.ResultMessage = "Role created successfully !";

                    return View("AssignUserToRole");
                }
                else
                {
                    ViewBag.ErrorMessage = "Sorry user is not available";
                    return View("AssignUserToRole");
                }

            }
        }
    }
