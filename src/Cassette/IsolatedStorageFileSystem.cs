﻿using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;

namespace Cassette
{
    public class IsolatedStorageFileSystem : IFileSystem
    {
        public IsolatedStorageFileSystem(IsolatedStorageFile storage, string basePath = "/")
        {
            this.storage = storage;
            this.basePath = basePath;
        }

        readonly IsolatedStorageFile storage;
        readonly string basePath;

        public bool FileExists(string filename)
        {
            return storage.FileExists(GetAbsolutePath(filename));
        }

        public Stream OpenFile(string filename, FileMode mode, FileAccess access)
        {
            return storage.OpenFile(GetAbsolutePath(filename), mode, access);
        }

        public void DeleteAll()
        {
            foreach (var filename in storage.GetFileNames(basePath + "/*"))
            {
                storage.DeleteFile(GetAbsolutePath(filename));
            }
        }

        public DateTime GetLastWriteTimeUtc(string filename)
        {
            return storage.GetLastWriteTime(GetAbsolutePath(filename)).UtcDateTime;
        }

        public string GetAbsolutePath(string path)
        {
            return Path.Combine(basePath, path);
        }

        public IFileSystem NavigateTo(string path, bool createIfNotExists)
        {
            var fullPath = GetAbsolutePath(path);
            if (storage.DirectoryExists(fullPath) == false)
            {
                if (createIfNotExists)
                {
                    storage.CreateDirectory(fullPath);
                }
                else
                {
                    throw new DirectoryNotFoundException("Directory not found: " + fullPath);
                }
            }
            return new IsolatedStorageFileSystem(storage, fullPath);
        }

        public IEnumerable<string> GetFiles(string directory)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFiles(string directory, string searchPattern)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDirectories(string relativePath)
        {
            throw new NotImplementedException();
        }

        public FileAttributes GetAttributes(string path)
        {
            throw new NotImplementedException();
        }

        public bool DirectoryExists(string path)
        {
            return storage.DirectoryExists(GetAbsolutePath(path));
        }
    }
}
