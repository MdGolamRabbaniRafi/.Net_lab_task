//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Product_catagories.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.ProductOrders = new HashSet<ProductOrder>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public Nullable<int> C_Id { get; set; }
        public Nullable<int> Quantity { get; set; }
    
        public virtual Catagory Catagory { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
