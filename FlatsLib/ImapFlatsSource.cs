using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FlatsLib
{
    public class ImapFlatsSource : IFlatsSource {
        private readonly ImapConfig _config;

        public ImapFlatsSource(string host, int port, string username, string password, string folder = default(string)) {
            _config = new ImapConfig();
            _config.Host = host;
            _config.Port = port;
            _config.Username = username;
            _config.Password = password;
            _config.Folder = folder;
        }

        public ImapFlatsSource(ImapConfig config) {
            _config = config;
        }
        public IEnumerable<Flat> GetAll() {
            var flats = new List<Flat>();
            using (var client = new ImapClient()) {
                try {
                    client.Connect(_config.Host, _config.Port);
                    client.Authenticate(_config.Username, _config.Password);
                    var mails = client.GetNotSeen(_config.Folder);
                    foreach (var item in mails) {
                        Debug.WriteLine(item.From + " " + item.Subject + " " + item.DateTime);
                        var itemFlats = MailParser.ParseFlats(item);
                        flats.AddRange(itemFlats);
                    }
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                finally {
                    if (client.IsConnected)
                        client.Disconnect();
                }
                return flats;
            }
        }
    }
}
