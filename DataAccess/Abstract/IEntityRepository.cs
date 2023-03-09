using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Abstract
{
    // IEntity bir interface olduğu için newlenemez. Burada new(), ekleyerek sadece IEntityden implement edilmiş somut bir yapıyı kullanabilir.
    public interface IEntityRepository<T> where T:class, IEntity,new()  // hem referans tip olmasını istedik hemde IEntity implemente eden bir nesne olabilir. 
    {
        List<T> GetAll(Expression<Func<T,bool>> filter=null);
        T Get(Expression<Func<T,bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
