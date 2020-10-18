using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic.Define;
using BusinessLogic.Implement;
using DataAccess.Context;
using DataAccess.Database;
using DataAccess.Repositories;
using DataAccess.Repository.Implement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ninject;
using Ninject.Activation;
using Ninject.Infrastructure.Disposal;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Swashbuckle.AspNetCore.Swagger;
using CapstoneUI.Utils;

namespace CapstoneUI
{
  public class Startup
  {
    private readonly AsyncLocal<Scope> scopeProvider = new AsyncLocal<Scope>();
    private IKernel Kernel { get; set; }

    private object Resolve(Type type) => Kernel.Get(type);
    private object RequestScope(IContext context) => scopeProvider.Value;

    private sealed class Scope : DisposableObject { }
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<CapstoneContext>(o =>
      {
        o.UseSqlServer(Configuration.GetConnectionString("CapstoneContext"));
      });

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtBearerOptions =>
                {
                  jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                  {
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "spcs",
                    ValidAudience = "spcs",
                    IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.Default.GetBytes("thisismysecretkey123123123")),
                    ClockSkew = TimeSpan.Zero
                  };
                });

      // cors for local call ---
      services.AddCors();
      // ---

      // combine angular ---
      services.AddMvc();
      // ---

      services.AddMvc().AddJsonOptions(
            options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
      );

      //--- fix bug auto lower case property
      services.AddMvc().AddJsonOptions(
            options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()
      );
      //---

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
      services.AddRequestScopingMiddleware(() => scopeProvider.Value = new Scope());
      services.AddCustomControllerActivation(Resolve);
      services.AddCustomViewComponentActivation(Resolve);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "SPCS API", Version = "v1" });

        c.AddSecurityDefinition("Bearer",
                   new ApiKeyScheme
                   {
                     In = "header",
                     Description = "Please enter into field the word 'Bearer' following by space and JWT",
                     Name = "Authorization",
                     Type = "apiKey"
                   });
        c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", Enumerable.Empty<string>() },
                });
      });

      //--Hangfire

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SPCS API V1");
      });
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      // cors for local call ---
      app.UseCors(
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()
      );
      // ---
      app.UseAuthentication();

      app.UseMvc();
      this.Kernel = this.RegisterApplicationComponents(app);

      // combine angular ---
      app.Use(async (context, next) =>
      {
        await next();
        if (context.Response.StatusCode == 404 &&
           !Path.HasExtension(context.Request.Path.Value) &&
           !context.Request.Path.Value.StartsWith("/api/") &&
           !context.Request.Path.Value.StartsWith("/swagger/"))
        {
          context.Request.Path = "/index.html";
          await next();
        }
      });

      // default route of '/api/[Controller]'
      app.UseMvcWithDefaultRoute();

      // config app to serve the index.html file from /wwwroot when access from a web browser
      app.UseDefaultFiles();
      app.UseStaticFiles();

      //---

      PathUtil.rootPath = env.WebRootPath;

      //--

      ////--config path documen Template
      //app.UseStaticFiles(new StaticFileOptions()
      //{
      //  FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
      //  RequestPath = new PathString("/Resources")
      //});
      ////--
    }

    private IKernel RegisterApplicationComponents(IApplicationBuilder app)
    {
      // IKernelConfiguration config = new KernelConfiguration();
      var kernel = new StandardKernel();

      // Register application services
      foreach (var ctrlType in app.GetControllerTypes())
      {
        kernel.Bind(ctrlType).ToSelf().InScope(RequestScope);
      }

      // This is where our bindings are configurated
      kernel.Bind<IConstantTypeService>().To<ConstantTypeService>();
      kernel.Bind<IDocumentService>().To<DocumentService>();
      kernel.Bind<IEmployeeService>().To<EmployeeService>();
      kernel.Bind<IFieldService>().To<FieldService>();
      kernel.Bind<IFieldTypeService>().To<FieldTypeService>();
      kernel.Bind<IFormulaService>().To<FormulaService>();
      kernel.Bind<IFormulaDetailService>().To<FormulaDetailService>();
      kernel.Bind<IFormulaTypeService>().To<FormulaTypeService>();
      kernel.Bind<IMonthlySalaryComponentService>().To<MonthlySalaryComponentService>();
      kernel.Bind<IPayrollService>().To<PayrollService>();
      kernel.Bind<IPayslipService>().To<PayslipService>();
      kernel.Bind<IPayslipTemplateService>().To<PayslipTemplateService>();
      kernel.Bind<IReferenceTableDetailService>().To<ReferenceTableDetailService>();
      kernel.Bind<IReferenceTableService>().To<ReferenceTableService>();
      kernel.Bind<IReferenceTableTypeService>().To<ReferenceTableTypeService>();
      kernel.Bind<IRoleService>().To<RoleService>();
      kernel.Bind<ISalaryComponentService>().To<SalaryComponentService>();
      kernel.Bind<IAccountService>().To<AccountService>();
      kernel.Bind<IDepartmentService>().To<DepartmentService>();
      kernel.Bind<IPositionService>().To<PositionService>();
      kernel.Bind<IPositionDetailService>().To<PositionDetailService>();
      kernel.Bind<IPayrollComponentService>().To<PayrollComponentService>();

      kernel.Bind<IUnitOfWork>().To<UnitOfWork>();
      kernel.Bind<IEntityContext>().To<CapstoneContext>();

      // Cross-wire required framework services
      kernel.BindToMethod(app.GetRequestService<IViewBufferScope>);

      return kernel;
    }

  }
}
