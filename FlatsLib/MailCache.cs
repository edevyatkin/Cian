using System;
using MailKit;
using MimeKit;
using System.IO;
using System.Diagnostics;

namespace FlatsLib
{
    internal class MailCache
    {
        private string _path;

        public MailCache(string path)
        {
            _path = path;
            Directory.CreateDirectory(path);
        }

        internal bool TryGetMail(UniqueId uniqueId, out MimeMessage message)
        {
            message = null;
            try
            {
                string mailFile = GetFilePath(uniqueId);
                if (!File.Exists(mailFile))
                    return false;
                message = MimeMessage.Load(mailFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Source + ": " + ex.Message);
                throw;
            }

            return true;
        }

        internal void AddToCache(UniqueId uniqueId, MimeMessage message)
        {
            try
            {
                string mailFile = GetFilePath(uniqueId);
                if (File.Exists(mailFile))
                    throw new Exception("File already exists");
                message.WriteTo(mailFile);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Source + ": " + ex.Message);
                throw;
            }
            
        }

        private string GetFilePath(UniqueId uniqueId) => Path.Combine(_path, uniqueId.Id + ".eml");

    }
}