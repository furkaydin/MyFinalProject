using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class EFProductDal : IProductDal
    {
        public void Add(Product entity)
        {
            // using bağlantısı ile nesne ömrü yönetimi yapılır.(bellek yönetimini kontrol altına alır.)
            using (NorthwindContext context = new NorthwindContext())
            {
                var addedEntity = context.Entry(entity); // ürünün referansını yakalar.
                addedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Added; // Nesnenin veritabanına ekleneceğini belirtir.
                context.SaveChanges(); // kaydet
            }
        }

        public void Delete(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var deletedEntity = context.Entry(entity); // referansı yakala
                deletedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted; // durumu silme olarak set et.
                context.SaveChanges(); // kaydet
            }
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            using (NorthwindContext context = new NorthwindContext())
            {
                var uptadedEntity = context.Entry(entity); // referansı yakala
                uptadedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Modified; // durumu güncelleme olarak set et.
                context.SaveChanges(); // kaydet
            }
        }
    }
}
