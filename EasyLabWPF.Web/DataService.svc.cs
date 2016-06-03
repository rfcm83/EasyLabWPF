using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using EasyLabWPF.Infrastructure;

namespace EasyLabWPF.Web
{
    public class DataService : IDataService
    {

        private NorthwindEntities CreateContext()
        {
            var context = new NorthwindEntities();
            context.Configuration.ProxyCreationEnabled = false;
            return context;
        }

        public List<Product> GetProducts()
        {
            try
            {
                using(var context = CreateContext())
                {
                    return context.Products.Include("Category").ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Product GetProductById(int id)
        {
            try
            {
                using(var context = CreateContext())
                {
                    return context.Products.FirstOrDefault(x => x.ProductID == id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public Product AddProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            try
            {
                using(var context = CreateContext())
                {
                    var newProduct = context.Products.Add(product);
                    context.SaveChanges();
                    return newProduct;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public void SaveProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            try
            {
                using(var context = CreateContext())
                {
                    context.Products.Attach(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

        public void DeleteProduct(Product product)
        {
            if (product == null) throw new ArgumentNullException("product");
            try
            {
                using(var context = CreateContext())
                {
                    context.Products.Attach(product);
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}