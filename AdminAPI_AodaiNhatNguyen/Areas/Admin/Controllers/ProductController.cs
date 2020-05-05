using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Model.EF;
using PagedList;

namespace AdminAPI_AodaiNhatNguyen.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private AodaiDbContext db = new AodaiDbContext();

        public ActionResult Index()
        {
            // Translation. Requires using System.Threading; using System.Globalization;
            /*if (!String.IsNullOrEmpty(Request["locale"]))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(Request["locale"]);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(Request["locale"]);
            }*/

            var list = db.Products.ToList();


            switch (Request["order"])
            {
                case "Name":
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.Name).ToList()
                                                                : list.OrderBy(item => item.Name).ToList();
                    break;
                case "Quantity":
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.Quantity).ToList()
                                                                : list.OrderBy(item => item.Quantity).ToList();
                    break;
                default:
                    list = (Request["sort"] == "desc") ? list.OrderByDescending(item => item.Price).ToList()
                                                                : list.OrderBy(item => item.Price).ToList();
                    break;
            }


            if (!String.IsNullOrEmpty(Request["search"]))
            {
                string search = Request["search"];
                list = list.Where(item => item.Name.Contains(search)).ToList();
            }


            int PageNumber = String.IsNullOrEmpty(Request["pageNumber"]) ? 1 : Convert.ToInt32(Request["pageNumber"]);
            int PageSize = String.IsNullOrEmpty(Request["pageSize"]) ? 3 : Convert.ToInt32(Request["pageSize"]);

            return View(list.ToPagedList(PageNumber, PageSize));
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,MetaTile,Description,Image,MoreImages,Price,PromotionPrice,IncludeVAT,Quantity,CategoryID,Detail,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDesciptions,Status,TopHot")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,MetaTile,Description,Image,MoreImages,Price,PromotionPrice,IncludeVAT,Quantity,CategoryID,Detail,CreateDate,CreateBy,ModifiedDate,ModifiedBy,MetaKeywords,MetaDesciptions,Status,TopHot")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.ProductCategories, "ID", "Name");
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
