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
    {//This method pulls in 3rd party librarys/services and configures them
     //this second line sets up the MVC pattern for us in conjunction with line 45
     //this first lines instatiates a burgers repository when needed, then tears down
     //when done
      services.AddTransient<IDbConnection>(x => CreateDBContext());
      services.AddTransient<BurgersRepository>();
      services.AddTransient<SmoothiesRepository>();
      services.AddTransient<FriesRepository>();

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      else
      {
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseMvc();
    }

    private IDbConnection CreateDBContext()
    {
      //insert my connection below
      var connection = new MySqlConnection(_connectionString);
      connection.Open();
      return connection;
    }
  }
}
