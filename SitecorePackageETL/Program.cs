// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json;
using SitecorePackageETL;
using SitecorePackageETL.Builders;
using SitecorePackageETL.Models;


InputModel inputModel;
using (StreamReader r = new StreamReader(args[0]))
{
    string json = r.ReadToEnd();
    inputModel = JsonConvert.DeserializeObject<InputModel>(json);
}

IndexBuilder indexBuilder = new IndexBuilder(inputModel);
var enrichedTemplates = indexBuilder.EnrichTemplates();

CsvMappingBuilder mappingBuilder = new CsvMappingBuilder();
mappingBuilder.GenerateCsv(inputModel, enrichedTemplates);

OptimizelyContentBuilder optimizelyContentBuilder = new OptimizelyContentBuilder(inputModel);
optimizelyContentBuilder.GenerateContent();


OptimizelyTypedContentBuilder builder = new OptimizelyTypedContentBuilder(inputModel);
builder.GenerateContent();