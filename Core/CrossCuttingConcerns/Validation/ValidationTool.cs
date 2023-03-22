using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.CrossCuttingConcerns.Validation
{
    public static class ValidationTool  // static bir sınıfın metodlarıda static olmalıdır.
    {
    public static void Validate(IValidator validator,object entity)
        {
            var context = new ValidationContext<object>(entity); // product validet edileceği seçiliyor.
            var result = validator.Validate(context); // oluşturduğum nesneyi validate ederken contexti gönderiyorum içine.
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors);  // Validation hatalarını döndürdü.
            }
        }
    }
}
