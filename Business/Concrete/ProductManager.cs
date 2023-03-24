using Business.Abstract;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Business;
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
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal,ILogger logger,ICategoryService categoryService) // productDal bağımlı ama interface olarak bağımlı olduğu için ben dataaccess tarafında istediğim gibi at koşturabilirim. Veri tabanı değişikliği rahatca yapabilirim.
        { // Bir manager classında birden fazla DAL classı tanımlanamaz. Onun yerine service tanımlanabilir.
            _productDal = productDal;
            _categoryService = categoryService;
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
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product),CheckIfProductName(product),CheckIfCategoryCount());
            if(result!=null)
            {
                return result;
            }

                _productDal.Add(product);
                return new SuccessResult(Messages.ProductAdded);
          
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
        private IResult CheckIfCategoryCount()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Messages.CategoryCount);
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
