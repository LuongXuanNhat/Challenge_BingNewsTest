using BingNew.BusinessLogicLayer.Interfaces;
using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services;
using BingNew.DI;
using BingNew.Mapping;
using BingNew.Mapping.Interface;
using BingNew.ORM.DbContext;

namespace BingNew.PresentationLayer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var container = new DIContainer();
            container.Register<DbBingNewsContext, DbBingNewsContext>();
            container.Register<IJsonDataSource, JsonDataSource>();
            container.Register<IXmlDataSource, XmlDataSource>();
            container.Register<IBingNewsService, BingNewsService>();
            container.Register<IMappingService, MappingService>();


            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddSingleton(container);
            builder.Services.AddSingleton(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ////builder.Services.AddScoped<IXmlDataSource, XmlDataSource>();
            ////builder.Services.AddScoped<IJsonDataSource, JsonDataSource>();
            ////builder.Services.AddDbContext<DbBingNewsContext>(options =>
            ////{
            ////    options.UseSqlServer("your_connection_string_here");
            ////});
            ////  builder.Services.AddScoped<IBingNewsService, BingNewsService>();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //// Lấy DIContainer từ ServiceProvider và sử dụng nó để giải quyết phụ thuộc
            ////var serviceProvider = builder.Services.BuildServiceProvider();
            ////var resolvedContainer = serviceProvider.GetRequiredService<DIContainer>();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}