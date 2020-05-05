using AdminAPI_AodaiNhatNguyen.Common;
using Model.DAO;
using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Net;
using System.Data.Entity;

namespace AdminAPI_AodaiNhatNguyen.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private AodaiDbContext db = new AodaiDbContext();
        // GET: Admin/User
        public ActionResult Index()
        {           
            var list = db.Users.ToList();

            switch (Request["order"])
            {
                case "UserName":
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.UserName).ToList()
                                                                : list.OrderBy(item => item.UserName).ToList();
                    break;
                case "Name":
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.Name).ToList()
                                                                : list.OrderBy(item => item.Name).ToList();
                    break;
                default:
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.CreateDate).ToList()
                                                                : list.OrderBy(item => item.CreateDate).ToList();
                    break;
            }


            if (!String.IsNullOrEmpty(Request["search"]))
            {
                string search = Request["search"];
                list = list.Where(item => item.UserName.Contains(search)).ToList();
            }


            int PageNumber = String.IsNullOrEmpty(Request["pageNumber"]) ? 1 : Convert.ToInt32(Request["pageNumber"]);
            int PageSize = String.IsNullOrEmpty(Request["pageSize"]) ? 6 : Convert.ToInt32(Request["pageSize"]);

            return View(list.ToPagedList(PageNumber, PageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(int id)
        {
            var user = new UserDAO().ViewDetail(id);
            return View(user);
        }

        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);

        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                var DAO = new UserDAO();
                var encryptedMD5Password = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMD5Password;

                long id = DAO.Insert(user);
                if (id > 0)
                {
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm mới bản ghi thất bại");
                }

            }
            return View("Index");
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid) { 
                //{
                //    var DAO = new UserDAO();

                //    var encryptedMD5Password = Encryptor.MD5Hash(user.Password);
                //    user.Password = encryptedMD5Password;

                //    var result = DAO.Update(user);
                //    if (result)
                //    {
                //        ModelState.AddModelError("", "Cập nhật bản ghi thành công");
                //        return RedirectToAction("Index", "User");
                //    }
                //    else
                //    {
                //        ModelState.AddModelError("", "Thêm mới bản ghi thất bại");
                //    }
                var encryptedMD5Password = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMD5Password;

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                TempData["notify"] = "Edit data success";
                return RedirectToAction("Index");
            }
            return View("Index");
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            new UserDAO().Delete(id);
            return RedirectToAction("Index");
        }



    }
}