using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecorePackageETL.Models
{
    internal class IndexModel
    {
        internal List<IndexEntryModel> IndexEntries = new List<IndexEntryModel>();
    }

    internal class IndexEntryModel
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Language { get; set; }
        public string LatestVersion { get; set; }
    }
}
