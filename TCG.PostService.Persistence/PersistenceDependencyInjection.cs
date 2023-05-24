using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TCG.PostService.Application.IHelpers;
using TCG.PostService.Persistence.Helpers;

namespace TCG.PostService.Persistence
{
    public static class PersistenceDependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            services.AddSingleton<IPictureHelper, PictureHelper>();
            return services;
        }
    }
}
