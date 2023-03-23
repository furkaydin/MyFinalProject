using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception // Aspect --> Metodun başında sonunda hata verdiğinde çalışacak yapı.
    {
        private Type _validatorType;
        public ValidationAspect(Type validatorType)
        {
            // defensive coding
            if (!typeof(IValidator).IsAssignableFrom(validatorType)) // validator type ile parametre verilen eş değilse sorun ver.
            {
                throw new System.Exception("Bu bir doğrulama sınıfı değil");
            }

            _validatorType = validatorType;
        }
        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType); // instance oluştur. Örn : ProductValidotor newler.
            var entityType = _validatorType.BaseType.GetGenericArguments()[0]; // ProductValidatorun instance aldığı classın generic tipi. Yani Abstract Validator tipi <>.
            var entities = invocation.Arguments.Where(t => t.GetType() == entityType); // ValidationAspectin kullanıldığı metodun parametresi. Eğer eşit ise.
            foreach (var entity in entities)
            {
                ValidationTool.Validate(validator, entity);
            }
        }
    }
}
