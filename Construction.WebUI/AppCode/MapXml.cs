using Construction.Dal.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace Construction.WebUI.AppCode
{
    public class MapXml
    {
        public string ProjectName = ConfigurationManager.AppSettings["ProjectName"];
        public class SitemapNode
        {
            public SitemapFrequency? Frequency { get; set; }
            public DateTime? LastModified { get; set; }
            public double? Priority { get; set; }
            public string Url { get; set; }
            public string ImageUrl { get; set; }
            public string Title { get; set; }
        }
        public enum SitemapFrequency
        {
            Never,
            Yearly,
            Monthly,
            Weekly,
            Daily,
            Hourly,
            Always
        }

        public IReadOnlyCollection<SitemapNode> GetSitemapNodes(string Domain)
        {
            List<SitemapNode> nodes = new List<SitemapNode>();
            using (var db=new ConstructionContext())
            {
                var result = db.UrlRecords.Where(p => p.SecurityObject.IsFront && p.SecurityObject.IsActive )
                    .Select(p => new
                    {
                        p.SeoUrl,
                        p.Keywords,
                        p.Description,
                        p.Title
                    }).ToList();
                foreach (var item in result)
                {
                    nodes.Add(
                       new SitemapNode()
                       {
                           Url = Domain+item.SeoUrl,
                           Frequency = SitemapFrequency.Weekly,
                           Priority = 0.8,
                           LastModified=DateTime.Now,
                           Title=item.Title
                       });
                }
                var lang = db.Languages.Where(p=>p.IsActive).ToList();
                foreach (var item in lang)
                {
                    nodes.Add(
                       new SitemapNode()
                       {
                           Url = Domain + item.UrlText,
                           Frequency = SitemapFrequency.Weekly,
                           Priority = 0.8,
                           LastModified = DateTime.Now,
                           Title = item.UrlString
                       });
                    nodes.Add(
                 new SitemapNode()
                 {
                     Url = Domain + item.ContactUrl,
                     Frequency = SitemapFrequency.Weekly,
                     Priority = 0.8,
                     LastModified = DateTime.Now,
                     Title = item.ContactString
                 });
                }
            }
            return nodes;
        }
        public string GetSitemapDocument(string Domain)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xmlns + "urlset");
            var sitemapNodes = GetSitemapNodes(Domain);
            foreach (SitemapNode sitemapNode in sitemapNodes)
            {
                XElement urlElement = new XElement(
                    xmlns + "url",
                    new XElement(xmlns + "loc", Uri.EscapeUriString(sitemapNode.Url)),
                    sitemapNode.LastModified == null ? null : new XElement(
                        xmlns + "lastmod",
                        sitemapNode.LastModified.Value.ToLocalTime().ToString("yyyy-MM-ddTHH:mm:sszzz")),
                    sitemapNode.Frequency == null ? null : new XElement(
                        xmlns + "changefreq",
                        sitemapNode.Frequency.Value.ToString().ToLowerInvariant()),
                    sitemapNode.Priority == null ? null : new XElement(
                        xmlns + "priority",
                        sitemapNode.Priority.Value.ToString("F1", CultureInfo.InvariantCulture)));
                root.Add(urlElement);
            }

            XDocument document = new XDocument(root);
            return document.ToString();
        }

    }
}