using MailKit;
using System;
using System.IO;
using Client = MailKit.Net.Imap.ImapClient;
using System.Collections.Generic;
using MailKit.Search;
using MimeKit;
using System.Linq;
using System.Text;
using System.Threading;

namespace FlatsLib {
    internal class ImapClient : IDisposable {
        private readonly Client _client;
        private readonly MailCache _mailCache;

        public bool IsConnected { get => _client.IsConnected; }

        public ImapClient() {
            StreamWriter logStream = File.CreateText("imap.log");
            IProtocolLogger _logger = new ProtocolLogger(logStream.BaseStream);
            _client = new Client(_logger);
            string cachePath = Path.Combine(Path.GetTempPath(), "mails");
            _mailCache = new MailCache(cachePath);
        }

        public void Connect(string host, int port) {
            try {
                _client.Connect(host, port);
            }
            catch (Exception) {
                throw;
            }
        }

        public void Authenticate(string username, string password) {
            if (!_client.IsConnected)
                throw new Exception("Not connected to server");
            if (!_client.AuthenticationMechanisms.Contains("PLAIN")) {
                throw new Exception("The server does not support PLAIN auth type");
            }
            _client.AuthenticationMechanisms.Remove("XOAUTH2");
            try {
                _client.Authenticate(username, password);
            }
            catch (Exception) {
                throw new Exception("Authentication error");
            }
        }

        public IEnumerable<Mail> GetNotSeen(string folderName) {
            IMailFolder folder = OpenFolder(folderName);
            int count = folder.Count;
            //List<ImapMail> mails = new List<ImapMail>(count);
            var ids = folder.Search(SearchQuery.NotSeen);
            var summary = folder.Fetch(ids, MessageSummaryItems.UniqueId);
            foreach (var s in summary) {
                bool fromCache = _mailCache.TryGetMail(s.UniqueId, out MimeMessage message);
                if (!fromCache)
                {
                    message = folder.GetMessage(s.UniqueId);
                    _mailCache.AddToCache(s.UniqueId, message);
                }
                //MimeMessage message = folder.GetMessage(s.UniqueId);
                yield return new Mail {
                    From = message.From.Mailboxes.FirstOrDefault().Address,
                    Subject = message.Subject,
                    Body = message.HtmlBody,
                    DateTime = message.Date.DateTime
                };
                //mails.Add(mail);
            }
            //return mails;
        }

        private IMailFolder OpenFolder(string folderName) {
            IMailFolder folder;
            try {
                if (!string.IsNullOrEmpty(folderName)) {
                    folder = _client.GetFolder(folderName);
                }
                else {
                    folder = _client.Inbox;
                }
                folder.Open(FolderAccess.ReadOnly);
            }
            catch (Exception) {
                throw new Exception("Error in process of folder opening");
            }
            return folder;
        }

        public void Dispose() {
            _client.Dispose();
        }

        public void Disconnect() {
            _client.Disconnect(true);
        }
    }
}