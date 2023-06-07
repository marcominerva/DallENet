using System.Diagnostics;
using DallENet;

namespace DallEConsole;

internal class Application
{
    private readonly IDallEClient dallEClient;

    public Application(IDallEClient dallEClient)
    {
        this.dallEClient = dallEClient;
    }

    public async Task ExecuteAsync()
    {
        string? prompt = null;

        do
        {
            try
            {
                Console.Write("Describe the image you want to create: ");
                prompt = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(prompt))
                {
                    Console.Write("I'm working... ");

                    var response = await dallEClient.GenerateImagesAsync(prompt);

                    var imageUrl = response.GetImageUrl();
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        Console.WriteLine("Opening generated image.");
                        Process.Start(new ProcessStartInfo(imageUrl) { UseShellExecute = true });

                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine();
                Console.WriteLine(ex.Message);
                Console.WriteLine();

                Console.ResetColor();
            }
        } while (!string.IsNullOrWhiteSpace(prompt));
    }
}
