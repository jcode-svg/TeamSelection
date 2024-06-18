// /////////////////////////////////////////////////////////////////////////////
// TESTING AREA
// THIS IS AN AREA WHERE YOU CAN TEST YOUR WORK AND WRITE YOUR TESTS
// /////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.Common;
using System.Linq;
using WebApi.Helpers;

namespace WebApiTest.Base
{
    internal class TestApplicationProvider
    {
    }


    internal class TestApplication : WebApplicationFactory<Program>
    {
        private readonly DbConnection _connection;

        public TestApplication(DbConnection connection)
        {
            _connection = connection;
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(DbContextOptions<DataContext>));

                services.Remove(descriptor);

                // database for testing.
                services.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlite(_connection);
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var dataContext = scopedServices.GetRequiredService<DataContext>();
                    dataContext.Database.EnsureCreated();
                }
            });

            return base.CreateHost(builder);
        }
    }
}
