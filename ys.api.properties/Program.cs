namespace ys.api.properties
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();

                    // Configure Kestrel to use PORT environment variable (Railway) or default to 5055
                    var port = Environment.GetEnvironmentVariable("PORT") ?? "5055";
                    webBuilder.UseUrls($"http://0.0.0.0:{port}");
                });
    }
}
