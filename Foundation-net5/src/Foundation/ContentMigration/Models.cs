using System.Collections.Generic;

namespace Foundation.ContentMigration
{
    public class OptimizelyContentModel
    {
        public string SourceItemPath { get; set; }
        public string SourceId { get; set; }
        public string SourceName { get; set; }
        public string SourceTemplate { get; set; }

        public List<OptimizelyContentFieldModel> SourceFields = new List<OptimizelyContentFieldModel>();

    }

    public class OptimizelyContentFieldModel
    {
        public string SourceValue { get; set; }
        public string SourcefieldId { get; set; }
        public string SourceFieldType { get; set; }
        public string SourceName { get; set; }
    }

    public class FieldMapping
    {
        public string SourceField { get; set; }
        public string DestinationField { get; set; }
        public string TransformFunction { get; set; }
    }

    public class TemplateMappingDefinition
    {
        public string SourceTemplateId { get; set; }
        public string DestinationContentType { get; set; }
        public int DestinationParentId { get; set; }
        public string DestinationLanguage { get; set; }
        public string ItemName { get; set; }
        public List<FieldMapping> FieldMappings { get; set; }
    }

    public class Root
    {
        public List<TemplateMappingDefinition> TemplateMappingDefinition { get; set; }
    }


}
