using BingNew.BusinessLogicLayer.Interfaces.IService;
using BingNew.BusinessLogicLayer.Services;
using BingNew.BusinessLogicLayer.Services.Common;
using BingNew.DI;
using BingNew.ORM.DbContext;

namespace BingNew.PresentationLayer
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var container = new DIContainer();
            container.Register<DbBingNewsContext, DbBingNewsContext>();
            container.Register<IApiDataSource, ApiDataSource>();
            container.Register<IApiDataSource, RssDataSource>();
            container.Register<IBingNewsService, BingNewsService>();
            


            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddSingleton(container);
            builder.Services.AddSingleton(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            ////builder.Services.AddScoped<IRssDataSource, RssDataSource>();
            ////builder.Services.AddScoped<IApiDataSource, ApiDataSource>();
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