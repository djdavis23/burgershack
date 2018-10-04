using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using burgershack.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace burgershack
{
  public class Startup
  {
    //specify connection string to our database
    private readonly string _connectionString = "";
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      //grabs the connection string from our appsettings.json file
      _connectionString = configuration.GetSection("DB").GetValue<string>("mySQLConnectionString");
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      //This method pulls in 3rd party librarys/services and configures them

      //ADD USER AUTH THROUGH JSON WEB TOKEN
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(options =>
      {
        options.LoginPath = "/Account/Login";
        options.Events.OnRedirectToLogin = (context =>
        {
          context.Response.StatusCode = 401;
          return Task.CompletedTask;
        });
      });

      //next section added to connect front end
      //cors allows front end to talk to back end
      services.AddCors(options =>
        {
          options.AddPolicy("CorsDevPolicy", builder =>
          {
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
          });
        });

      //Sets up our MVC pattern for us (ICW App.UseMvc)
      services.AddMvc();

      //A transient service is instantiated when needed, then torn down
      //We need add transients for our DB connection and all repsitories      
      services.AddTransient<IDbConnection>(x => CreateDBContext());
      services.AddTransient<BurgersRepository>();
      services.AddTransient<SmoothiesRepository>();
      services.AddTransient<FriesRepository>();
      services.AddTransient<UserRepository>();

      //next line was default when creating new web api, but caused errors
      // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        //added to connect front end
        app.UseCors("CorsDevPolicy");
      }
      else
      {
        app.UseHsts();
      }
      app.UseAuthentication();
      //The next line was causing some configuration issues
      //app.UseHttpsRedirection();

      //following two statements added to connect front end 
      //this tells app to use C# default file structure for navigation 
      app.UseDefaultFiles();
      //inside default route (www) is my front end
      app.UseStaticFiles();
      app.UseMvc();
    }

    //opens a connection to the DB
    private IDbConnection CreateDBContext()
    {
      //insert my connection below
      var connection = new MySqlConnection(_connectionString);
      connection.Open();
      return connection;
    }
  }
}
