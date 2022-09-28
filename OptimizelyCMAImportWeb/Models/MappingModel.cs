namespace OptimizelyCMAImportWeb.Models
{
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
