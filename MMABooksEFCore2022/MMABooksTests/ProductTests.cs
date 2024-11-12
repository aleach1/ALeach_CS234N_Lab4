using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.MODELS;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        
        MMABooksContext dbContext;
        Product? p;
        List<Product>? products;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice.Equals(56.50)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }
        
        [Test]
        public void DeleteTest()
        {
            
            p = dbContext.Products.Find("A4CS");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.States.Find("A4CS"));
        }

        [Test]
        public void CreateTest()
        {
            p = new Product();
            p.ProductCode = "G7BN";
            p.Description = "New thing for sale";
            p.UnitPrice = 59;
            p.OnHandQuantity = 900;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find("G7BN"));
        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("A4CS");
            p.Description = "New description";
            dbContext.Products.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Products.Find("A4CS");
            Assert.AreEqual("New description", p.Description);
        }

        public void PrintAll(List<Product> customers)
        {
            foreach (Product p in products)
            {
                Console.WriteLine(p);
            }
        }
    }
}