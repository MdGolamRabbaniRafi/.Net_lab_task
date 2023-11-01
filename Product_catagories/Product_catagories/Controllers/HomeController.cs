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
            var db = new Product_catagoriesEntities6();
            ViewBag.Catagory = db.Catagories.ToList();

            var products = db.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult EditCatagory(int Id)
        {
            var db = new Product_catagoriesEntities6();
            var data = db.Catagories.Find(Id);
            //var data = (from d in db.Departments
            //            where d.Id == id
            //            select d).SingleOrDefault();
            return View(data);
        }
        [HttpPost]
        public ActionResult EditCatagory(Catagory d)
        {
            var db = new Product_catagoriesEntities6();
            var ex = db.Catagories.Find(d.Id);
            ex.Name = d.Name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult EditProduct(int Id)
        {
            var db = new Product_catagoriesEntities6();
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
            var db = new Product_catagoriesEntities6();
            var ex = db.Products.Find(d.Id);
            ex.Name = d.Name;
            ex.Price=d.Price;
            ex.C_Id= d.C_Id;
            ex.Quantity=d.Quantity;
            db.SaveChanges();
            return RedirectToAction("Product","Home");
        }

        [HttpPost]
        public ActionResult addProduct(Product p)
        {
            var db = new Product_catagoriesEntities6();
            db.Products.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");

        }
        [HttpGet]
        public ActionResult Product()
        {
            var db = new Product_catagoriesEntities6();
            var data = db.Products.ToList();
            ViewBag.Catagory = db.Catagories.ToList();
            return View(data);
        }
        [HttpGet]
        public ActionResult Catagory()
        {
            var db = new Product_catagoriesEntities6();
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
            var db = new Product_catagoriesEntities6();
            db.Catagories.Add(p);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult deleteCatagory(int Id)
        {
            var db = new Product_catagoriesEntities6();
            var data = db.Catagories.Find(Id);
            db.Catagories.Remove(data);
            db.SaveChanges();
            return RedirectToAction("Product", "Home");

        }

        [HttpGet]
        public ActionResult deleteProduct(int Id)
        {
            var db = new Product_catagoriesEntities6();
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
        public ActionResult Login(FormCollection p)
        {
            /* var db = new Product_catagoriesEntities3();
             db.Catagories.Add(p);
             db.SaveChanges();*/
            var db = new Product_catagoriesEntities6();

            var user =db.User_type.ToList();
            //  Session["Name"] = p.Name;
            var name = p["Name"];
            var password = p["Password"];
            foreach (var item in user)
            {
                if(item.Username.Equals(name) && item.Password.Equals(password))
                {
                    Session["Name"] = name;
                    if (item.UserType.Equals("Customer"))
                    {
                        return RedirectToAction("ProductBuy", "Home");

                    }
                    else if (item.UserType.Equals("Admin"))
                    {
                        return RedirectToAction("ProductBuyAdmin", "Home");

                    }

                }

            }
            return View();


        }

        [Logged]
        [HttpGet]
        public ActionResult ProductBuyAdmin()
        {
            var db = new Product_catagoriesEntities6();
            ViewBag.Product = db.Products.ToList();
            ViewBag.Order = db.Orders.ToList();
            ViewBag.Catagory = db.Catagories.ToList();
            var productOrder = db.ProductOrders.ToList();

            return View(productOrder);
        }
        [Logged]
        [HttpGet]
        public ActionResult ProductBuy()
        {
            var db = new Product_catagoriesEntities6();
            var products = db.Products.ToList();
            ViewBag.Catagory = db.Catagories.ToList();

            return View(products);
        }
        [Logged]

        [HttpPost]
        public ActionResult ProductBuy(FormCollection Form)
        {
            var db = new Product_catagoriesEntities6();
            ViewBag.Catagory = db.Catagories.ToList();
            ViewBag.Product = db.Products.ToList();
            Session["SelectedProductIds"] = "";
            string[] selectedProductIds = Form.GetValues("selectedProducts");

            if (selectedProductIds != null)
            {
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
                var db = new Product_catagoriesEntities6();
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


            var db = new Product_catagoriesEntities6();
            ViewBag.Catagory = db.Catagories.ToList();
            ViewBag.Product = db.Products.ToList();

            var orders = db.Orders.ToList();
            Order o = new Order();
            o.Customer_Name = Session["Name"].ToString();
            DateTime currentDateTime = DateTime.Now;
            string formattedDate = currentDateTime.ToString("yyyy-MM-dd");
            string formattedTime = currentDateTime.ToString("HH:mm:ss");
            o.Order_Date = formattedDate;
            o.Order_Time = formattedTime;


            db.Orders.Add(o);
            db.SaveChanges();
            ProductOrder po=new ProductOrder();
            string[] selectedProductIds = Session["SelectedProductIds"] as string[];
            foreach (var item in selectedProductIds)
            {
                po.O_Id = o.Id;

                po.P_Id = int.Parse(item);
                po.Status = "Ordered";

                db.ProductOrders.Add(po);
                db.SaveChanges();

            }
            return View(orders);
        }

        [Logged]
        [HttpGet]
        public ActionResult OrderView(int id)
        {
            var name = Session["Name"].ToString();
            var db = new Product_catagoriesEntities6();
            ViewBag.Catagory = db.Catagories.ToList();
            var products = (from productOrder in db.ProductOrders
                            join product in db.Products on productOrder.P_Id equals product.Id
                            where productOrder.O_Id == id
                            select product).ToList();
            ViewBag.orders = db.Orders.ToList();
            ViewBag.productorders = (from productOrder in db.ProductOrders
                                 where productOrder.O_Id == id
                                 select productOrder).ToList();




            return View(products);
        }
        [Logged]
        [HttpGet]
        public ActionResult CancelOrder(int id)
        {
  
           var db = new Product_catagoriesEntities6();

           var productOrder = db.ProductOrders.Find(id);

               productOrder.Status = "Cancelled";
               db.SaveChanges();
     


            return RedirectToAction("OrderView", new { id = productOrder.O_Id });
        }





        [Logged]
        [HttpGet]
        public ActionResult AcceptOrder(int Id)
        {
            var db = new Product_catagoriesEntities6();

            var data = db.ProductOrders.Find(Id);
            var product= db.Products.ToList();

            if (data != null)
            {
                data.Status = "Accetpted";
                db.SaveChanges();
                foreach (var item in product) {
                 if(data.P_Id==item.Id)
                    {
                        item.Quantity--;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("ProductBuyAdmin");
        }

        [Logged]
        [HttpGet]
        public ActionResult RejectOrder(int Id)
        {
            var db = new Product_catagoriesEntities6();

            var data = db.ProductOrders.Find(Id);
            var product = db.Products.ToList();

            if (data != null)
            {
                data.Status = "Rejected";
                db.SaveChanges();
            }

            return RedirectToAction("ProductBuyAdmin");
        }

        [Logged]
        [HttpGet]
        public ActionResult OrderList()
        {
            var db = new Product_catagoriesEntities6();

            var name = Session["Name"].ToString();

            var orderInfo = (from order in db.Orders
                            where order.Customer_Name == name
                            select order).ToList();
            return View(orderInfo);
        }


    }
}