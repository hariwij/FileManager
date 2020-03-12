using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManager
{
    public class FileBrowser
    {
        public string CurrentDir = @"\";
        public Dir FilesAndDirs;

        public event FileClicked FileOnClick;
        public delegate void FileClicked(string FilePath);

        public FileBrowser()
        {
            FilesAndDirs = new Dir();
            GetFilesAndDirs();
        }
        public void GetFilesAndDirs()
        {
            if (CurrentDir == @"\")
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                FilesAndDirs = null;
                var temp = new Dir { Path = CurrentDir, Dirs = new List<string>() };
                foreach (var d in allDrives)
                {
                    temp.Dirs.Add(d.Name);
                    Console.WriteLine("-----------------------------------------------------------------------");
                    Console.WriteLine("Drive {0}", d.Name);
                    Console.WriteLine("Drive type: {0}", d.DriveType);
                    if (d.IsReady == true)
                    {
                        Console.WriteLine("  Volume label: {0}", d.VolumeLabel);
                        Console.WriteLine("  File system: {0}", d.DriveFormat);
                        Console.WriteLine("  Available space to current user:{0, 15} bytes", d.AvailableFreeSpace);
                        Console.WriteLine("  Total available space:{0, 15} bytes", d.TotalFreeSpace);
                        Console.WriteLine("  Total size of drive:{0, 15} bytes ", d.TotalSize);
                    }
                    Console.WriteLine("-----------------------------------------------------------------------");
                }
                FilesAndDirs = temp;
            }
            else
            {
                var temp = new Dir { Path = CurrentDir, Files = new List<string>(), Dirs = new List<string>() };
                Console.WriteLine("Files>>>>>-------------------------------------------------------------");
                foreach (string f in Directory.GetFiles(CurrentDir))
                {
                    temp.Files.Add(Path.GetFileName(f));
                    Console.WriteLine(Path.GetFileName(f));
                }
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Dirs>>>>>--------------------------------------------------------------");
                foreach (string d in Directory.GetDirectories(CurrentDir))
                {
                    temp.Dirs.Add(Path.GetDirectoryName(d));
                    Console.WriteLine(Path.GetDirectoryName(d));
                }
                Console.WriteLine("-----------------------------------------------------------------------");
                FilesAndDirs = temp;
            }
        }

        public void NavigateTo(string path)
        {
            string tmpath = Path.Combine(CurrentDir, path);

            if (IsDir(tmpath))
            {
                CurrentDir = tmpath;
            }
            else
            {
                var handler = FileOnClick;
                handler?.Invoke(tmpath);
            }
            GetFilesAndDirs();
        }
        public void GoBack()
        {
            if (IsDir(CurrentDir))
            {
                var info = Directory.GetParent(CurrentDir);
                CurrentDir = info.FullName;
                GetFilesAndDirs();
            }
        }

        private bool IsDir(string path)
        {
            try
            {
                Directory.GetDirectories(path);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }

    public class Dir
    {
        public string Path { get; set; }
        public List<string> Dirs { get; set; }
        public List<string> Files { get; set; }
        public long Size { get; set; } = 0;
    }
}
