using FlatsLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Flats
{
    class Program
    {
        static Program() {
            Console.OutputEncoding = Encoding.UTF8;
        }

        private static void DisplayAllFlats(IEnumerable<Flat> flats) {
            if (flats != null)
                foreach (Flat flat in flats) {
                    DisplayFlatInfo(flat);
                }
        }

        private static void DisplayFlatInfo(Flat flat) {
            TableBuilder builder = new TableBuilder();
            builder
                .AddLine(nameof(flat.Building), flat.Building)
                .AddLine(nameof(flat.Metro), flat.Metro)
                .AddLine(nameof(flat.Address), flat.Address)
                .AddLine(nameof(flat.Price), flat.Price.ToString())
                .AddLine(nameof(flat.OfferUrl), flat.OfferUrl)
                .AddLine(nameof(flat.PhotoUrl), flat.PhotoUrl)
                .AddLine(nameof(flat.EscapeDate), flat.EscapeDate);
            Console.WriteLine(builder.Build());
        }

        private static ImapConfig ParseJsonConfig(string fileName) {
            try {
                using (StreamReader reader = File.OpenText(fileName))
                using (JsonReader jsonReader = new JsonTextReader(reader)) {
                    return JsonSerializer
                        .Create()
                        .Deserialize<ImapConfig>(jsonReader);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Source + ":" + e.Message);
            }
            return new ImapConfig();
        }

        static void Main(string[] args) {
            ImapConfig config = ParseJsonConfig("imapConfig.json");
            IFlatsSource source = new ImapFlatsSource(config);
            IEnumerable<Flat> flats = source.GetAll();
            DisplayAllFlats(flats);
            Console.WriteLine("ЗАВЕРШЕНО");
            Console.ReadLine();
        }
    }
}