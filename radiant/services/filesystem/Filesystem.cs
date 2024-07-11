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
    public static class Filesystem
    {
        private static CosmosVFS fs;

        public static void Init()
        {
            fs = new CosmosVFS();
            VFSManager.RegisterVFS(fs);
        }

        private static bool IsPathValid(string path, bool allowRadiant = false)
        {
            if (path.EndsWith(".private"))
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                return false;
            }

            string pathRoot = FindPathRoot(path);
            string userPathRoot = @$"0:\radiant\users\{Kernel.user.Name}\";

            if (!allowRadiant && pathRoot.StartsWith(@"0:\radiant"))
            {
                if (!pathRoot.StartsWith(userPathRoot) || pathRoot.Equals(userPathRoot, StringComparison.OrdinalIgnoreCase))
                {
                    ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Access denied");
                    return false;
                }
            }

            return true;
        }


        public static List<DirectoryEntry> GetListing(string dir)
        {
            return fs.GetDirectoryListing(dir)
                     .Where(f => !f.mName.EndsWith(".private"))
                     .ToList();
        }

        public static string ReadFile(string path)
        {
            if (!IsPathValid(path)) return string.Empty;
            return File.ReadAllText(FindPathRoot(path));
        }

        public static void CreateFile(string path)
        {
            if (!IsPathValid(path)) return;
            using (File.Create(FindPathRoot(path))) { }
        }

        public static string FindPathRoot(string path)
        {
            return Path.IsPathRooted(path) ? path : Path.Combine(Kernel.PWD, path);
        }

        public static void WriteFile(string path, string contents)
        {
            if (!IsPathValid(path)) return;
            File.WriteAllText(FindPathRoot(path), contents);
        }

        public static void RemoveFile(string path)
        {
            if (!IsPathValid(path)) return;
            File.Delete(FindPathRoot(path));
        }

        public static void MountDisk(int idx)
        {
            fs.GetDisks()[idx].Mount();
        }

        public static void RemoveFolder(string path, bool recursive)
        {
            if (!IsPathValid(path)) return;
            Directory.Delete(FindPathRoot(path), recursive);
        }

        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(FindPathRoot(path));
        }

        public static void CopyFile(string path1, string path2)
        {
            if (!IsPathValid(path1) || !IsPathValid(path2)) return;
            File.Copy(FindPathRoot(path1), FindPathRoot(path2));
        }

        public static void MoveFile(string path1, string path2)
        {
            if (!IsPathValid(path1) || !IsPathValid(path2)) return;
            ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, "Not Implemented");
            // File.Move(FindPathRoot(path1), FindPathRoot(path2));
        }

        public static void CreateNecessarySystemFiles()
        {
            string directoryPath = @"0:\radiant";
            string usersPath = Path.Combine(directoryPath, "users");
            string configFilePath = Path.Combine(directoryPath, "config.cfg");

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!Directory.Exists(usersPath))
                Directory.CreateDirectory(usersPath);

            if (!File.Exists(configFilePath))
            {
                string defConfig = "PCName = radiant";
                File.WriteAllText(configFilePath, defConfig, Encoding.UTF8);
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
                return DriveInfo.GetDrives()[index];
            }
            catch (Exception e)
            {
                ConsoleUtil.Message(ConsoleUtil.MessageType.ERR, $"Error Getting Disk: {e.Message}");
                return null;
            }
        }
    }
}
