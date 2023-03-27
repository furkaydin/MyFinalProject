using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Utilities.IoC
{
    public interface ICoreModule
    {
        void Load(IServiceCollection serviceCollection); // genel bağımlılıkları yükleyecek
        // webApıden ziyade her yerde kullanabilmek için bunu core katmanına aldık.
    }
}
