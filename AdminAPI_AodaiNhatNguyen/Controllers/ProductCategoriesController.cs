using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Model.EF;
using System.Web.Http.Cors;

namespace AdminAPI_AodaiNhatNguyen.Controllers
{
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ProductCategoriesController : ApiController
    {
        private AodaiDbContext db = new AodaiDbContext();

        // GET: api/ProductCategories
        public IQueryable<ProductCategory> GetProductCategories()
        {
            return db.ProductCategories;
        }

        // GET: api/ProductCategories/5
        [ResponseType(typeof(ProductCategory))]
        public IHttpActionResult GetProductCategory(long id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return Ok(productCategory);
        }

        // PUT: api/ProductCategories/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProductCategory(long id, ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productCategory.ID)
            {
                return BadRequest();
            }

            db.Entry(productCategory).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductCategories
        [ResponseType(typeof(ProductCategory))]
        public IHttpActionResult PostProductCategory(ProductCategory productCategory)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductCategories.Add(productCategory);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = productCategory.ID }, productCategory);
        }

        // DELETE: api/ProductCategories/5
        [ResponseType(typeof(ProductCategory))]
        public IHttpActionResult DeleteProductCategory(long id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return NotFound();
            }

            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();

            return Ok(productCategory);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductCategoryExists(long id)
        {
            return db.ProductCategories.Count(e => e.ID == id) > 0;
        }
    }
}