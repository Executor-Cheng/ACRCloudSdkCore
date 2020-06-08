using System;
using System.Threading.Tasks;

namespace ACRCloudSdkCore.Example
{
    public static class Program
    {
        public static async Task Main()
        {
            ACRCloudOptions options = new ACRCloudOptions("**Your host**", "**Your access key**", "**Your access secret**");
            ACRCloudRecognizer recognizer = new ACRCloudRecognizer(options);
            ACRCloudRecognizeResult result = await recognizer.RecognizeByFileAsync(@"**filePath**");
            if (result == null)
            {
                Console.WriteLine("No result.");
            }
            else
            {
                Console.WriteLine($"{string.Join(",", result.Artists)} - {result.Title} {(string.IsNullOrEmpty(result.Album) ? "" : $"{{{result.Album}}} ")}[{result.PlayOffset.GetValueOrDefault():g}/{result.Duration.GetValueOrDefault():g}]");
            }
        }
    }
}
