using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Shared models that are used between the ETL project and the T4 Generator Library
/// </summary>
namespace ModelLibrary.Models
{
    public class TemplateModel
    {
        public string TemplateName { get; set; }
        public Guid Guid { get; set; }

        public string Path { get; set; }

        public List<TemplateFieldModel> Fields = new List<TemplateFieldModel>();

        public List<Guid> BaseTemplates = new List<Guid>();

    }

    public class TemplateFieldModel
    {
        public string Name { get; set; }
        public string Key { get; set; }
        public string Fieldtype { get; set; }
        public Guid Guid { get; set; }

        public List<string> Templates = new List<string>();

        public string SubTemplateName { get; set; }
        public Guid SubTemplateGuid { get; set; }
    }

}
