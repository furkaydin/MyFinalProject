using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity,TContext>:IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext,new()
    {
        public void Add(TEntity entity)
        {
            // using bağlantısı ile nesne ömrü yönetimi yapılır.(bellek yönetimini kontrol altına alır.)
            using (TContext context = new TContext())
            {
                var addedEntity = context.Entry(entity); // ürünün referansını yakalar.
                addedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Added; // Nesnenin veritabanına ekleneceğini belirtir.
                context.SaveChanges(); // kaydet
            }
        }

        public void Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var deletedEntity = context.Entry(entity); // referansı yakala
                deletedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Deleted; // durumu silme olarak set et.
                context.SaveChanges(); // kaydet
            }
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                return filter == null
                    ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public void Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                var uptadedEntity = context.Entry(entity); // referansı yakala
                uptadedEntity.State = Microsoft.EntityFrameworkCore.EntityState.Modified; // durumu güncelleme olarak set et.
                context.SaveChanges(); // kaydet
            }
        }
    }
}
