using Cosmos.System.FileSystem;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.FileSystem.VFS;
using radiant.util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace radiant.services.filesystem
{
    public class Filesystem
    {
        public static CosmosVFS fs;

        public static void Init()
        {
            fs = new CosmosVFS();
            VFSManager.RegisterVFS(fs);
        }

        public static List<DirectoryEntry> GetListing(string dir)
        {
            var directory_list = fs.GetDirectoryListing(dir);
            return directory_list.Where(f => !f.mName.EndsWith(".private")).ToList();
        }

        public static string ReadFile(string path)
        {
            if (path.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return "";
            }
            return File.ReadAllText(FindPathRoot(path));
        }

        public static void CreateFile(string path)
        {
            if (path.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            File.Create(FindPathRoot(path));
        }

        public static string FindPathRoot(string path)
        {
            if (Path.IsPathRooted(path))
                return path;
            else
                return Path.Join(Kernel.PWD, path);
        }

        public static void WriteFile(string path, string contents)
        {
            if (path.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            File.WriteAllText(FindPathRoot(path), contents);
        }

        public static void RemoveFile(string path)
        {
            if (path.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            string rootedPath = FindPathRoot(path);
            if (path.StartsWith(@"0:\radiant"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            File.Delete(rootedPath);
        }

        public static void RemoveFolder(string path, bool recursive)
        {
            string rootedPath = FindPathRoot(path);
            if (path.StartsWith(@"0:\radiant"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            Directory.Delete(rootedPath, recursive);
        }

        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(FindPathRoot(path));
        }

        public static void CopyFile(string path1, string path2)
        {
            if (path1.EndsWith(".private") || path2.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            File.Copy(FindPathRoot(path1), FindPathRoot(path2));
        }

        public static void MoveFile(string path1, string path2)
        {
            if (path1.EndsWith(".private") || path2.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return;
            }
            ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Not Implemented");
            // File.Move(FindPathRoot(path1), FindPathRoot(path2));
        }

        public static void CreateNecessarySystemFiles()
        {
            string directoryPath = @"0:\radiant";
            string usersPath = @"0:\radiant\users";
            string configFilePath = @"0:\radiant\config.cfg";

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!Directory.Exists(usersPath))
                Directory.CreateDirectory(usersPath);

            if (!File.Exists(configFilePath))
            {
                Byte[] defconfig = new UTF8Encoding(true).GetBytes("PCName = radiant");
                using (FileStream fs = File.Create(configFilePath))
                {
                    fs.Write(defconfig, 0, defconfig.Length);
                }
            }
        }

        public static DriveInfo[] GetDisks()
        {
            try
            {
                return DriveInfo.GetDrives();
            }
            catch (Exception e)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Error Listing Disks: {e.Message}");
                return Array.Empty<DriveInfo>();
            }
        }

        public static DriveInfo GetDisk(int index)
        {
            try
            {
                return DriveInfo.GetDrives()[1];
            }
            catch (Exception e)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Error Listing Disks: {e.Message}");
                return null;
            }
        }
    }
}
