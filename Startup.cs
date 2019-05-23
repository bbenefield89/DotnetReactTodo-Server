﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.EntityFrameworkCore;
using dotnet_react_todo.Models;

namespace dotnet_react_todo
{
    public class Startup
    {

        readonly string DotnetReactTodoCorsPolicy = "dotnet_react_todo_cors_policy";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // sets cors polocy
            services.AddCors(options =>
            {
                options.AddPolicy(DotnetReactTodoCorsPolicy, builder =>
                {
                    builder.WithOrigins("http://localhost:3000");
                });
            });
            
            // sets DB connection to PSQL
            services.AddDbContext<TodoContext>(options => {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // tells dotnet to use our cors policy
            app.UseCors(DotnetReactTodoCorsPolicy);

            app.UseMvc();
        }
    }
}
