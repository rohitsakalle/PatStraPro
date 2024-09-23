using Microsoft.Extensions.DependencyInjection;
using PatStraPro.Dashboard.Service;
using PatStraPro.Db;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PatStraProClient
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your services here
            services.AddSingleton<IDashboardService, DashboardService>();
            services.AddSingleton<ICosmosDbService, CosmosDbService>();

            // Register MainWindow
            services.AddTransient<MainWindow>();
        }
    }

}
