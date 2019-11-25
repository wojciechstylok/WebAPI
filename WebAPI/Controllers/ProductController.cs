using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductsContext _myDbContext = new ProductsContext();
        // GET: api/<controller>
        [HttpGet]
        public string Get()
        {
            string returnedValue = "GET: \n";
            List<ProductModel> productList = new List<ProductModel>();
            productList.AddRange(_myDbContext.Product.ToList<ProductModel>());

            foreach(ProductModel pa in productList)
            {
                returnedValue += $"product: {pa.Id}, {pa.Name}, {pa.Price}\n";
            }

            return returnedValue;
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(Guid id)
        {
            ProductModel result = _myDbContext.Product.Find(id);

            return $"GET product by ID: {result.Id}, {result.Name}, {result.Price}";
        }

        // POST api/<controller>
        [HttpPost]
        public IActionResult Post([FromBody]ProductCreateInputModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductModel prod = new ProductModel()
            {
                Id = Guid.NewGuid(),
                Name = product.Name,
                Price = product.Price
            };
            _myDbContext.Product.Add(prod);
            _myDbContext.SaveChanges();

            return CreatedAtAction("Get", new { id = prod.Id, name = prod.Name, price = prod.Price });
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public string Put(Guid id, [FromBody]ProductUpdateInputModel product)
        {
            ProductModel result = _myDbContext.Product.FirstOrDefault(p => p.Id.Equals(id));
            if (result != null)
            {
                result.Name = product.Name;
                result.Price = product.Price;

                _myDbContext.Product.Update(result);
                _myDbContext.SaveChanges();

                return $"Produkt {result.Id} został zaktualizowany\nAktualna nazwa: {result.Name}, Aktualna cena: {result.Price}";
            }
            return "ERROR";
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public string Delete(Guid id)
        {
            ProductModel model = _myDbContext.Product.Find(id);
            using (var context = new ProductsContext())
            {
                context.Product.Remove(model);
                context.SaveChanges();
            }

            return $"Produkt {id} został usunięty";
        }
    }
}
