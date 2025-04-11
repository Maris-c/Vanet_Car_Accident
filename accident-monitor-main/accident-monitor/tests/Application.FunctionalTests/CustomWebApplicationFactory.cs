using System.Data.Common;
using AccidentMonitor.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;


namespace AccidentMonitor.Application.FunctionalTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly string _connectionString;

    public CustomWebApplicationFactory(DbConnection connection, string connectionString)
    {
        _connection = connection;
        _connectionString = connectionString;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:AccidentMonitorDb", _connectionString);
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<IUser>();
            //.AddTransient(provider => Mock.Of<IUser>(s => s.Guid == GetUserId()));
        });
    }
}
