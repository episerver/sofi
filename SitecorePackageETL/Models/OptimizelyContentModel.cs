using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecorePackageETL.Models
{
    public class OptimizelyContentModel
    {
        public string SourceItemPath { get; set; }
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceTemplate { get;set; }

        public List<OptimizelyContentFieldModel> SourceFields = new List<OptimizelyContentFieldModel>();

    }

    public class OptimizelyContentFieldModel
    {
        public string SourceValue { get; set; }
        public string SourcefieldId { get; set; }
        public string SourceFieldType { get; set; }
        public string SourceName { get; set; }    
    }


}
