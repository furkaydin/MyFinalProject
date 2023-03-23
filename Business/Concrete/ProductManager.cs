using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ILogger _logger;

        public ProductManager(IProductDal productDal,ILogger logger) // productDal bağımlı ama interface olarak bağımlı olduğu için ben dataaccess tarafında istediğim gibi at koşturabilirim. Veri tabanı değişikliği rahatca yapabilirim.
        {
            _productDal = productDal;
            _logger = logger;
        }
        public IDataResult<List<Product>> GetAll()
        {
            if(DateTime.Now.Hour==15)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(),Messages.Listed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetails()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails(),Messages.DetailsListed);
        }

       
        [ValidationAspect(typeof(ProductValidator))] // attiributeler type ofla atanır.
        public IResult Add(Product product)
        {
            if (CheckIfProductCountOfCategoryCorrect(product).Success)
            {
                if(CheckIfProductName(product).Success)
                {
                _productDal.Add(product);
                return new SuccessResult(Messages.ProductAdded);
                }
            }
            return new ErrorResult();
        }

        private IResult CheckIfProductCountOfCategoryCorrect(Product product)
        {
            var result = _productDal.GetAll(p => p.CategoryId == product.CategoryId).Count;
            if (result >= 10)
            {
                return new ErrorResult(Messages.ProductCount);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductName(Product product)
        {
            var result = _productDal.GetAll(p => p.ProductName == product.ProductName).Any();  // Any=> Linqtir. Bool döndürür. İlgili sorgu içerisinde eleman varsa true döndürür.
            if(result)
            {
                return new ErrorResult(Messages.SameProductName);
            }
            return new SuccessResult();
        }

        [ValidationAspect(typeof(ProductValidator))] // attiributeler type ofla atanır.
        public IResult Update(Product product)
        {
            _productDal.Update(product);
            return new SuccessResult("Güncellendi.");
        }

        public IResult Delete(Product product)
        {
            _productDal.Delete(product);
            return new SuccessResult("Veri Silindi.");

        }
    }
}
