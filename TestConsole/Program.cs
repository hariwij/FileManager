using System;
using FileManager;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            FileBrowser fb = new FileBrowser();
            fb.FileOnClick += Fb_FileOnClick;
            fb.CurrentDir = @"E:\Artists\Inna";
            fb.NavigateTo("Inna_-_Amazing.mp4");

            Console.WriteLine(fb.FilesAndDirs.Path);
            Console.ReadLine();
        }

        private static void Fb_FileOnClick(string FilePath)
        {
            Console.WriteLine($"Clicked >>> {FilePath}");
        }
    }
}
