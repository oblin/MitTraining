using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JagiCore.Admin;
using JagiCore.Admin.Data;
using JagiCore.Interfaces;
using JagiCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MitTraining.Services;

namespace MitTraining
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AdminContext>(config => config.UseInMemoryDatabase("default"));

            // Basic Object for JagiCore service
            services.AddScoped<IUserResolverService, FakeUserResolverService>();
            services.AddMemoryCache();
            services.AddScoped<CacheService>(); // Initial caches
            services.AddSingleton<CodeService>();

            services.AddCors();
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CacheService cache)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Seed Data
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    using (var context = services.GetRequiredService<AdminContext>())
                    {
                        SeedAdminData(context);
                    }
                }
            }

            // 載入代碼檔案
            cache.CreateCodeCache();

            app.UseCors(options => options.AllowAnyOrigin());
            app.UseMvc();
        }

        private void SeedAdminData(AdminContext context)
        {
            var codefile1 = new CodeFile { Id = 1, ItemType = "Sex", TypeName = "性別" };
            var codedetail11 = new CodeDetail { CodeFileId = 1, ItemCode = "1", Description = "男性" };
            var codedetail12 = new CodeDetail { CodeFileId = 1, ItemCode = "2", Description = "女性" };
            var codedetail13 = new CodeDetail { CodeFileId = 1, ItemCode = "3", Description = "不詳" };

            var codefile2 = new CodeFile { Id = 2, ItemType = "Code2", TypeName = "this is code 2" };
            var codedetail21 = new CodeDetail { CodeFileId = 2, ItemCode = "t1", Description = "Code 2 Detail 1" };
            var codedetail22 = new CodeDetail { CodeFileId = 2, ItemCode = "t2", Description = "Code 2 Detail 2" };

            context.CodeFiles.Add(codefile1);
            context.CodeFiles.Add(codefile2);

            context.CodeDetails.Add(codedetail11);
            context.CodeDetails.Add(codedetail12);
            context.CodeDetails.Add(codedetail13);
            context.CodeDetails.Add(codedetail21);
            context.CodeDetails.Add(codedetail22);

            context.SaveChanges();
        }
    }
}
