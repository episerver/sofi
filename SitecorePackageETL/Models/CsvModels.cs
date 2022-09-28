using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecorePackageETL.Models
{
    internal class TemplateCsvModel
    {
        public string Template { get; set; }
        public string FieldTemplate { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldGuid { get; set; }

    }

    internal class TemplateCsvModelMap : ClassMap<TemplateCsvModel>
    {
        public TemplateCsvModelMap()
        {
            Map(m => m.Template).Index(0);
            Map(m => m.FieldTemplate).Index(1);
            Map(m => m.FieldName).Index(2);
            Map(m => m.FieldType).Index(3);
            Map(m => m.FieldGuid).Index(4);

        }
    }
}
