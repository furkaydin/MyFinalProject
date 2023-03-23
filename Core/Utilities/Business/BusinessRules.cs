using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.Business
{
    public class BusinessRules
    {
        public static IResult Run(params IResult[] logics)  // Run içerisine birden fazla IResult verilebiliyor. params sayesinde
        {
            foreach (var logic in logics)
            {
                if(!logic.Success)
                {
                    return logic;
                }
            }
            return null;
        }
    }
}
