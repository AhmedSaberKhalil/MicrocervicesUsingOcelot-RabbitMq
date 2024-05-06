
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Threading.RateLimiting;

namespace OcelotApiGateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .SetIsOriginAllowed((hosts) => true));
            });
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

            builder.Services.AddOcelot(builder.Configuration);
            // Add service to configure Rate Limiting
            //builder.Services.AddRateLimiter(options => {
            //    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(content =>
            //    RateLimitPartition.GetFixedWindowLimiter(
            //        partitionKey: content.Request.Headers.Host.ToString(),
            //        factory: Partition => new FixedWindowRateLimiterOptions
            //        {
            //            AutoReplenishment = true,
            //            PermitLimit = 5,
            //            QueueLimit = 0,
            //            Window = TimeSpan.FromSeconds(10)
            //        }
            // ));
            //    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            //});

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            //    app.UseSwagger();
            //    app.UseSwaggerUI();
            //}

            app.UseCors("CORSPolicy");
         //   app.UseRateLimiter();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            await app.UseOcelot();
            app.Run();
        }
    }
}
