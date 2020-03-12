using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileManager
{
    public class FileBrowser
    {
        public bool ShowHiddenFiles = false;
        public bool ShowSystemFiles = true;
        public string CurrentDir = @"\";

        public Dir FilesAndDirs;

        public event FileClicked FileOnClick;
        public delegate void FileClicked(string FileName);
        public event DirChanged OnDirChanged;
        public delegate void DirChanged(string Path);

        public FileBrowser()
        {
            CurrentDir = @"\";
            FilesAndDirs = new Dir();
            GetFilesAndDirs();
        }
        public FileBrowser(string Dir)
        {
            CurrentDir = Dir;
            FilesAndDirs = new Dir();
            GetFilesAndDirs();
        }
        public void NavigateTo(string path)
        {
            string tmpath = Path.Combine(CurrentDir, path);
            if (IsDir(tmpath))
            {
                if (CurrentDir == @"\") CurrentDir = path;
                else CurrentDir = tmpath;
                GetFilesAndDirs();
            }
            else
            {
                FileOnClick?.Invoke(tmpath);
            }
        }
        public void GoBack()
        {
            if (IsDir(CurrentDir))
            {
                var info = Directory.GetParent(CurrentDir);
                if (info == null) CurrentDir = @"\";
                else CurrentDir = info.FullName;
                GetFilesAndDirs();
            }
        }

        public void Search(string txt)
        {
            var temp = new Dir { Path = CurrentDir, Files = new List<string>(), Dirs = new List<string>() };
            try
            {
                foreach (string d in Directory.GetDirectories(CurrentDir))
                {
                    string Dname = Path.GetFileName(d);
                    if (Dname.Contains(txt, StringComparison.CurrentCultureIgnoreCase)) temp.Dirs.Add(Dname);
                }
                foreach (string f in Directory.GetFiles(CurrentDir))
                {
                    string Fname = Path.GetFileName(f);
                    if (Fname.Contains(txt, StringComparison.CurrentCultureIgnoreCase)) temp.Files.Add(Fname);
                }
                FilesAndDirs = temp;
                OnDirChanged?.Invoke(CurrentDir);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Refresh()
        {
            GetFilesAndDirs();
        }
        public (bool, string) CreateFolder(string FolderName)
        {
            var tmpdir = Path.Combine(CurrentDir, FolderName);
            if (CurrentDir == @"\") return (false, "Can't Create Folder!");
            if (FilePathHasInvalidChars(tmpdir)) return (false, "Invalied Folder Name!");
            if (!FilesAndDirs.Dirs.Contains(FolderName))
            {
                Directory.CreateDirectory(tmpdir);
                GetFilesAndDirs();
                return (true, "Folder Created!");
            }
            return (false, "Folder Name Already Exits!");
        }
        public (bool, string) CreateFile(string FileName)
        {
            var tmpdir = Path.Combine(CurrentDir, FileName);
            if (CurrentDir == @"\") return (false, "Can't Create File!");
            if (FilePathHasInvalidChars(tmpdir)) return (false, "Invalied File Name!");
            if (!FilesAndDirs.Files.Contains(FileName))
            {
                var fs = File.Create(tmpdir);
                fs.Close();
                GetFilesAndDirs();
                return (true, "File Created!");
            }
            return (false, "File Name Already Exits!");
        }
        private void GetFilesAndDirs()
        {
            if (CurrentDir == @"\")
            {
                DriveInfo[] allDrives = DriveInfo.GetDrives();
                FilesAndDirs = null;
                var temp = new Dir { Path = CurrentDir, Dirs = new List<string>(), Files = new List<string>() };
                Console.WriteLine("Drives>>>>>------------------------------------------------------------");
                foreach (var d in allDrives)
                {
                    temp.Dirs.Add(d.Name);
                    Console.WriteLine("Drive {0}", d.Name);
                }
                Console.WriteLine("-----------------------------------------------------------------------");
                FilesAndDirs = temp;
            }
            else
            {
                var temp = new Dir { Path = CurrentDir, Files = new List<string>(), Dirs = new List<string>() };
                Console.WriteLine("Files>>>>>-------------------------------------------------------------");
                foreach (string f in Directory.GetFiles(CurrentDir))
                {
                    bool a = true;
                    if (!ShowHiddenFiles && File.GetAttributes(f).HasFlag(FileAttributes.Hidden)) a = false;
                    if (!ShowSystemFiles && File.GetAttributes(f).HasFlag(FileAttributes.System)) a = false;
                    if (a) temp.Files.Add(Path.GetFileName(f));
                    Console.WriteLine(Path.GetFileName(f));
                }
                Console.WriteLine("-----------------------------------------------------------------------");
                Console.WriteLine("Dirs>>>>>--------------------------------------------------------------");
                foreach (string d in Directory.GetDirectories(CurrentDir))
                {
                    bool a = true;
                    if (!ShowHiddenFiles && File.GetAttributes(d).HasFlag(FileAttributes.Hidden)) a = false;
                    if (!ShowSystemFiles && File.GetAttributes(d).HasFlag(FileAttributes.System)) a = false;
                    if (a) temp.Dirs.Add(Path.GetFileName(d));
                    Console.WriteLine(Path.GetFileName(d));
                }
                Console.WriteLine("-----------------------------------------------------------------------");
                FilesAndDirs = temp;
            }
            OnDirChanged?.Invoke(CurrentDir);
        }
        public static bool IsDir(string path)
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
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error Access Denied!");
                return true;
            }
        }
        public static bool FilePathHasInvalidChars(string path)
        {
            return (!string.IsNullOrEmpty(path) && path.IndexOfAny(Path.GetInvalidPathChars()) >= 0);
        }

        public static string GetDriveLetter(string path)
        {
            return Path.GetPathRoot(path);
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
