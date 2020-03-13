using System;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = MakeRelative(@"E:\Artists\Inna\Inna_-_Amazing.mp4", @"C:\");
            Console.WriteLine(result);
            Console.ReadLine();
        }
        public static string MakeRelative(string filePath, string referencePath)
        {
            var fileUri = new Uri(filePath);
            var referenceUri = new Uri(referencePath);
            return referenceUri.MakeRelativeUri(fileUri).ToString();
        }

        private static void Fb_FileOnClick(string FilePath)
        {
            Console.WriteLine($"Clicked >>> {FilePath}");
        }
    }
}
