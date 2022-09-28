using CsvHelper;
using ModelLibrary.Models;
using SitecorePackageETL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecorePackageETL.Builders
{
    internal class CsvMappingBuilder
    {
        public void GenerateCsv(InputModel inputModel,List<TemplateModel> enrichedTemplates)
        {
            var csvTemplates = new Dictionary<string, List<TemplateCsvModel>>();
            foreach (var template in enrichedTemplates)
            {
                var fields = template.Fields.Select(field => new TemplateCsvModel()
                {
                    FieldGuid = field.Guid.ToString(),
                    FieldName = field.Name,
                    FieldType = field.Fieldtype,
                    FieldTemplate = field.SubTemplateName,
                }).ToList();
                csvTemplates.Add(template.TemplateName, fields);
            }

            using (var writer = new StreamWriter(inputModel.OutputLocation + "\\UserModelMapping.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {

                csv.WriteField("Sitecore");
                csv.NextRecord();
                csv.NextRecord();

                csv.WriteField("");
                csv.WriteField("Field Template");
                csv.WriteField("Field Name");
                csv.WriteField("Field Type");
                csv.WriteField("Field Guid");
                csv.NextRecord();
                foreach (var record in csvTemplates)
                {
                    csv.WriteField(record.Key);
                    csv.NextRecord();
                    foreach (var fields in record.Value)
                    {
                        csv.WriteRecord(fields);
                        csv.NextRecord();
                    }
                    csv.NextRecord();
                    csv.NextRecord();
                };
            }
        }
    }
}
