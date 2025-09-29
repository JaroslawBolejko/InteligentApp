using InteligentApp.Components;
using InteligentApp.Services;
using System.Net.Http.Headers;

namespace InteligentApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var openAiApiKey = builder.Configuration["OpenAI:ApiKey"];
            var openAiApiEndpoint = builder.Configuration["OpenAI:ApiEndpoint"];

            var azureAiApiKey = builder.Configuration["AzureAI:ApiKey"];
            var azureAiApiEndpoint = builder.Configuration["AzureAI:ApiEndpoint"];

            builder.Services.AddHttpClient("OpenAI", client =>
            {
                client.BaseAddress = new Uri(openAiApiEndpoint);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAiApiKey);
            });

            builder.Services.AddHttpClient("AzureAI", client =>
            {
                client.BaseAddress = new Uri(azureAiApiEndpoint);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureAiApiKey);
            });

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
            builder.Services.AddSingleton<IMovieService, MovieService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
