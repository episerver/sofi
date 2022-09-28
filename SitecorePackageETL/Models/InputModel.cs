using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecorePackageETL.Models
{
    public class InputModel
    {
        public List<Guid> TemplatesToParse { get; set; }
        public string InstanceLanguage { get; set; }

        public string PackageStartPath { get; set; }

        public string OutputLocation { get; set; }

        public string TemplateCrawlStartPath { get; set; }

        public List<string> OptimizelyContentStartNodes { get; set; }

        [JsonIgnore]
        public string TemplateCrawlStartPathJoined
        {
            get
            {
                return PackageStartPath.TrimEnd('\\') + "\\" + TemplateCrawlStartPath.TrimStart('\\');
            }
        }

        [JsonIgnore]
        public string MediaLibraryStartPath
        {
            get
            {
                return PackageStartPath.TrimEnd('\\') + "\\sitecore\\media library";
            }
        }

        public string MapContentStartNode(string startNode)
        {
            return PackageStartPath.TrimEnd('\\') + "\\" + startNode.TrimStart('\\');
        }
    }
}
