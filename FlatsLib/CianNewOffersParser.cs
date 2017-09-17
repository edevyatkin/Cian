using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace FlatsLib
{
    class CianNewOffersParser : IFlatMailParser
    {
        static Func<HtmlNode, string, string> GetCleanUrl =
            (n, attr) =>
            {
                var value = n.GetAttributeValue(attr, string.Empty);
                var signIndex = value.IndexOf('?');
                if (signIndex < 0)
                    return value;
                return value.Substring(0, signIndex);
            };

        public Func<Mail, IEnumerable<Flat>> Algorithm =>
            (mail) =>
            {
                var html = mail.Body;
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                //var body = doc.DocumentNode.SelectSingleNode("//body");
                var flats = new List<Flat>();
                var photoNodes = doc.DocumentNode.SelectNodes("//td[contains(@class , 'cell offer_photo')]");
                var descriptionNodes = doc.DocumentNode.SelectNodes("//td[contains(@class , 'offer_description')]");
                var flatCount = photoNodes.Count; // or descriptionNodes.Count because it is equal
                for (int i = 0; i < flatCount; i++)
                {
                    Flat flat = new Flat();

                    var photoNode = photoNodes[i];

                    flat.Id = GetId(photoNode);
                    flat.OfferUrl = GetOfferUrl(photoNode);
                    flat.PhotoUrl = GetPhotoUrl(photoNode);
                    flat.PhotoCount = GetPhotoCount(photoNode);

                    var descriptionNode = descriptionNodes[i];

                    flat.Building = GetBuilding(descriptionNode);
                    flat.Metro = GetMetro(descriptionNode);
                    flat.Address = GetAddress(descriptionNode);
                    flat.Price = GetPrice(descriptionNode);
                    flat.EscapeDate = GetEscapeDate(descriptionNode);
                    flat.MailDate = mail.DateTime;

                    flat.Active = GetActiveStatus(flat.OfferUrl);

                    flats.Add(flat);
                }
                return flats;
            };

        private bool GetActiveStatus(string offerUrl)
        {
            //HttpClient client = new HttpClient();
            //var html = client.GetStringAsync(offerUrl).Result;
            //return html.IndexOf("Объявление снято с публикации") > 0;
            return true;
        }

        private int GetId(HtmlNode photoNode)
        {
            var aNode = photoNode.Element("a");
            var link = GetCleanUrl(aNode, "href");
            return int.Parse(Regex.Match(link, @"/(\d+)/").Groups[1].Value);
        }

        private int GetPhotoCount(HtmlNode photoNode)
        {
            var photoCountXPath = "./span[2]";
            var photosText = photoNode.SelectSingleNode(photoCountXPath)?.InnerHtml;
            if (photosText == null)
                return 0;
            return int.Parse(photosText.Substring(0, photosText.IndexOf(' ')));
        }

        private string GetEscapeDate(HtmlNode descriptionNode)
        {
            var timeXPath = "./table/tr/td[2]/span";
            return descriptionNode.SelectSingleNode(timeXPath).InnerHtml;
        }

        private int GetPrice(HtmlNode descriptionNode)
        {
            var priceXPath = "./span[2]/b";
            return int.Parse(descriptionNode
                .SelectSingleNode(priceXPath)
                .InnerHtml
                .Replace("&nbsp;", "")
                .Replace("руб.",""));
        }

        private string GetAddress(HtmlNode descriptionNode)
        {
            var addressXPath = "./span[1]";
            var addressHtml = descriptionNode
                .SelectSingleNode(addressXPath)
                .InnerHtml;
            Match m = Regex.Match(addressHtml, "(<br>)?<br>(.*)<br><br>");
            return m.Groups[2].Value;
        }

        private string GetMetro(HtmlNode descriptionNode)
        {
            var metroXPath = "./span[1]";
            var metroHtml = descriptionNode
                .SelectSingleNode(metroXPath)
                .InnerHtml;
            Match m = Regex.Match(metroHtml, "(.*)<br><br>(.*)<br><br>");
            return m.Groups[1].Value.Replace("&nbsp;", " ");

        }

        private string GetBuilding(HtmlNode descriptionNode)
        {
            var buildingXPath = "./h2";
            return descriptionNode
                        .SelectSingleNode(buildingXPath)
                        .InnerHtml
                        .Replace("&nbsp;", " ")
                        .Replace("&sup2;", "\u00b2");
        }

        private string GetPhotoUrl(HtmlNode photoNode)
        {
            var aNode = photoNode.Element("a");
            var imgNode = aNode.Element("img");
            return GetCleanUrl(imgNode, "src");
        }

        private string GetOfferUrl(HtmlNode photoNode)
        {
            var aNode = photoNode.Element("a");
            return GetCleanUrl(aNode, "href");
        }

        public Func<Mail, bool> Validator =>
            mail =>
            {
                var from = "noreply@cian.ru";
                var subject = "Свежие предложения по вашей подписке на ЦИАН";
                return mail.From.Equals(from) && mail.Subject.Equals(subject);
            };
    }
}
