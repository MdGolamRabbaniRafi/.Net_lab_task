using Product_catagories.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Product_catagories.Models;
using Product_catagories.Auth;

namespace Product_catagories.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult addProduct()
        {
            var db = new Product_catagoriesEntities5();
            ViewBag.Catagory = db.Catagories.ToList();

            var products = db.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult EditCatagory(int Id)
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Catagories.Find(Id);
            //var data = (from d in db.Departments
            //            where d.Id == id
            //            select d).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult EditCatagory(Catagory d)
        {
            var db = new Product_catagoriesEntities5();
            var ex = db.Catagories.Find(d.Id);
            ex.Name = d.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult EditProduct(int Id)
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Products.Find(Id);
            ViewBag.Catagory = db.Catagories.ToList();
            //var data = (from d in db.Departments
            //            where d.Id == id
            //            select d).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult EditProduct(Product d)
        {
            var db = new Product_catagoriesEntities5();
            var ex = db.Products.Find(d.Id);
            ex.Name = d.Name;
            ex.Price=d.Price;
            ex.Catagory= d.Catagory;
            db.SaveChanges();
            return RedirectToAction("Product","Home");
        }




        [HttpPost]
        public ActionResult addProduct(Product p)
        {
            var db = new Product_catagoriesEntities5();
            db.Products.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Product()
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Products.ToList();
            ViewBag.Catagory = db.Catagories.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Catagory()
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Catagories.ToList();
            return View(data);
        }


        [HttpGet]
        public ActionResult addCatagory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addCatagory(Catagory p)
        {
            var db = new Product_catagoriesEntities5();
            db.Catagories.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult deleteCatagory(int Id)
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Catagories.Find(Id);
            db.Catagories.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Product", "Home");

        }

        [HttpGet]
        public ActionResult deleteProduct(int Id)
        {
            var db = new Product_catagoriesEntities5();
            var data = db.Products.Find(Id);
            db.Products.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Product","Home");

        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Login p)
        {
            /* var db = new Product_catagoriesEntities3();
             db.Catagories.Add(p);
             db.SaveChanges();*/
            Session["Name"] = p.Name;

            return RedirectToAction("ProductBuy","Home");

        }
        [Logged]
        [HttpGet]
        public ActionResult ProductBuy()
        {
            var db = new Product_catagoriesEntities5();
            var products = db.Products.ToList();
            ViewBag.Catagory = db.Catagories.ToList();

            return View(products);
        }
        [Logged]

        [HttpPost]
        public ActionResult ProductBuy(FormCollection Form)
        {
            var db = new Product_catagoriesEntities5();
            ViewBag.Catagory = db.Catagories.ToList();
            ViewBag.Product = db.Products.ToList();
            Session["SelectedProductIds"] = "";

            string[] selectedProductIds = Form.GetValues("selectedProducts");

            if (selectedProductIds != null)
            {
                // Store the selected product IDs in the session
                Session["SelectedProductIds"] = selectedProductIds;
            }

            return RedirectToAction("Cart");
        }


        [Logged]
        [HttpGet]

        public ActionResult Cart()
        {
            string[] selectedProductIds = Session["SelectedProductIds"] as string[];

            if (selectedProductIds != null)
            {
                var db = new Product_catagoriesEntities5();
                var selectedProductIdsInt = selectedProductIds.Select(id => int.Parse(id)).ToArray();
                var selectedProducts = db.Products.Where(p => selectedProductIdsInt.Contains(p.Id)).ToList();
                ViewBag.SelectedProducts = selectedProducts;
                ViewBag.Catagory = db.Catagories.ToList();

            }

            return View();
        }


        [Logged]
        [HttpPost]
        public ActionResult Cart(FormCollection Form)
        {


            var db = new Product_catagoriesEntities5();
            ViewBag.Catagory = db.Catagories.ToList();
            ViewBag.Product = db.Products.ToList();

            var orders = db.Orders.ToList();
            Order o = new Order();
            o.Customer_Name = Session["Name"].ToString();
            db.Orders.Add(o);
            db.SaveChanges();
            ProductOrder po=new ProductOrder();
            string[] selectedProductIds = Session["SelectedProductIds"] as string[];
            foreach (var item in selectedProductIds)
            {
                po.O_Id = o.Id;

                po.P_Id = int.Parse(item);
                db.ProductOrders.Add(po);
                db.SaveChanges();

            }
            return View(orders);
        }

    }
}