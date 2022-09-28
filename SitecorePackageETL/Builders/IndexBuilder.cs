using ModelLibrary.Models;
using Newtonsoft.Json;
using SitecorePackageETL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SitecorePackageETL.Builders
{
    public class IndexBuilder
    {
        private InputModel _inputModel;
        private IndexModel model = new IndexModel();
        private List<TemplateModel> templateModels = new List<TemplateModel>();

        public IndexBuilder(InputModel inputModel)
        {
            _inputModel = inputModel;
        }


        /// <summary>
        /// Crawl all templates in the InputModel and create corresponding TemplateModels for each.
        /// </summary>
        /// <returns>A list of models referenced in the InputModel. Each model will be flattened to included fields from all inherited templates.</returns>
        /// <exception cref="Exception"></exception>
        public List<TemplateModel> EnrichTemplates()
        {
            var dirInfo = new DirectoryInfo(_inputModel.TemplateCrawlStartPathJoined);
            if (!dirInfo.Exists)
            {
                throw new Exception($"Path not found: {_inputModel.TemplateCrawlStartPathJoined}");
            }
            WalkDirectory(dirInfo);
            LogAllTemplates();
            var enrichedTemplates = EnrichTemplateModel();
            LogEnrichedTemplates(enrichedTemplates);

            return enrichedTemplates;
        }

        private void LogAllTemplates()
        {
            using (StreamWriter writer = System.IO.File.CreateText(_inputModel.OutputLocation + "\\TemplateIndex.csv"))
            {
                writer.WriteLine("Name,Guid,Language,LatestVersion,Path");
                foreach (var entry in model.IndexEntries.OrderBy(x=> x.Name))
                {
                    writer.WriteLine($"\"{entry.Name}\",{entry.Guid},{entry.Language},{entry.LatestVersion},\"{entry.Path}\"");
                }
            }
        }

        private void LogEnrichedTemplates(List<TemplateModel> mappedTemplates)
        {
            using (StreamWriter writer = System.IO.File.CreateText(_inputModel.OutputLocation + "\\MappedModels.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(mappedTemplates));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>>A list of models referenced in the InputModel. Each model will be flattened to included fields from all inherited templates.</returns>
        private List<TemplateModel> EnrichTemplateModel()
        {
            var retModel = new List<TemplateModel>();
            foreach (var model in templateModels.Where(x => _inputModel.TemplatesToParse.Contains(x.Guid)))
            {
                if (model.BaseTemplates?.Any() ?? false)
                {
                    var baseTemplatesMapped = new List<Guid>();
                    foreach (var template in model.BaseTemplates)
                    {
                        if (baseTemplatesMapped.Contains(template))
                        {
                            continue;
                            //already added
                        }
                        var match = templateModels.FirstOrDefault(x => x.Guid == template);
                        if (match != null)//If a base template is not included in the Sitecore Content Package, this field may be null.
                            WalkTemplates(model, templateModels.First(x => x.Guid == template), baseTemplatesMapped);
                    }
                }
                //Group fields together with template.
                model.Fields = model.Fields.OrderBy(x => x.SubTemplateName).ToList();
                retModel.Add(model);
            }
            return retModel;
        }

        /// <summary>
        /// Recursive function to walk a templates base templates and included all referenced fields.
        /// </summary>
        /// <param name="parentTemplate"></param>
        /// <param name="baseTemplate"></param>
        private void WalkTemplates(TemplateModel parentTemplate, TemplateModel baseTemplate, List<Guid> baseTemplatesMapped)
        {
            baseTemplatesMapped.Add(baseTemplate.Guid);
            parentTemplate.Fields.AddRange(baseTemplate.Fields);
            if (baseTemplate.BaseTemplates != null && baseTemplate.BaseTemplates.Any())
            {
                foreach (var template in baseTemplate.BaseTemplates)
                {
                    if (baseTemplatesMapped.Contains(template))
                    {
                        continue;
                        //already added
                    }
                    var match = templateModels.FirstOrDefault(x => x.Guid == template);
                    if (match != null)//If a base template is not included in the Sitecore Content Package, this field may be null.
                        WalkTemplates(parentTemplate, templateModels.First(x => x.Guid == template), baseTemplatesMapped);
                }
            }
        }

        private bool isGuidFolder(string name)
        {
            var test = Guid.Empty;
            return Guid.TryParse(name, out test);
        }

        /// <summary>
        /// A recusive function to walk the Content package starting from the InputModel TemplateCrawlStartPath. Looks for items with the attribute template="template".
        /// A TemplateModel will be created for each template and subsequent fields will be included.
        /// </summary>
        /// <param name="info"></param>
        /// <exception cref="Exception"></exception>
        private void WalkDirectory(DirectoryInfo info)
        {
            var templateModel = new TemplateModel();
            //Retrieve child folder with guid for a name
            var folderItem = info.GetDirectories().FirstOrDefault(x => isGuidFolder(x.Name));

            if (folderItem == null)
            {
                foreach (var dir in info.GetDirectories())
                {
                    WalkDirectory(dir);
                }
                return;
            }
            /*
             * Expected Folder Structure:
             * 
             *  /Folder A
             *      /{...} (Folder A guid) // This may or may not exist for some folders in the export.
             *            /en
             *                 xml
             *            /jp 
             *                 xml  
             *      /_Template
             *            /{...} (Template guid)
             *                 /en
             *                      xml
             *                 /jp 
             *                      xml
             *            /Field Section
             *                  /{...}
             *                       /en
             *                           xml
             *                      /jp 
             *                           xml
             *                 /Field Name
             *                     /{...}
             *                           /en
             *                               xml
             *                          /jp 
             *                               xml
             * 
             * 
             */

            foreach (var languagedir in folderItem.GetDirectories().Where(x => x.Name == _inputModel.InstanceLanguage))
            {
                var latest = languagedir.GetDirectories().OrderByDescending(X => X.Name).First();
                var latestXml = latest.GetFiles().First().FullName;


                var doc = new XmlDocument();
                doc.Load(latestXml);
                if (doc.DocumentElement == null)
                {
                    throw new Exception($"Expected document to contain a root element: {latestXml}");
                }

                if (doc.DocumentElement.Attributes["template"]?.Value == "template")
                {

                    model.IndexEntries.Add(new IndexEntryModel()
                    {
                        Guid = Guid.Parse(doc.DocumentElement.GetAttribute("id")),
                        Name = doc.DocumentElement.GetAttribute("name"),
                        Path = latestXml,
                        Language = languagedir.Name,
                        LatestVersion = latest.Name
                    });

                    var templateM = new TemplateModel()
                    {
                        TemplateName = doc.DocumentElement.GetAttribute("name"),
                        Guid = Guid.Parse(doc.DocumentElement.GetAttribute("id")),
                        Path = latestXml
                    };
                    XmlNode baseFields = doc.DocumentElement.SelectSingleNode("//field[@key='__base template']");
                    if (baseFields?.InnerText != null)
                    {
                        templateM.BaseTemplates.AddRange(baseFields.InnerText.Split("|").ToList().Select(x => Guid.Parse(x)));
                    }

                    //Iterate other directories in folder to find sections
                    foreach (var templateSection in info.GetDirectories().Where(x => !isGuidFolder(x.Name)))
                    {
                        //  Iterate section to find fields
                        foreach (var field in templateSection.GetDirectories().Where(x => !isGuidFolder(x.Name)))
                        {
                            //.First assumes the field only has 1 sub folder being the guid
                            foreach (var fieldLanguage in field.GetDirectories().First().GetDirectories().Where(x => x.Name == _inputModel.InstanceLanguage))
                            {
                                var fieldlanguageVersion = fieldLanguage.GetDirectories().OrderByDescending(X => X.Name).First();
                                var fieldLanguageVersionXml = fieldlanguageVersion.GetFiles().First().FullName;//assume only 1 file xml

                                var fieldDoc = new XmlDocument();
                                fieldDoc.Load(fieldLanguageVersionXml);
                                if (fieldDoc.DocumentElement == null)
                                {
                                    throw new Exception($"Expected document to contain a root element: {fieldLanguageVersionXml}");
                                }


                                templateM.Fields.Add(new TemplateFieldModel()
                                {
                                    SubTemplateName = doc.DocumentElement.GetAttribute("name"),
                                    SubTemplateGuid = Guid.Parse(doc.DocumentElement.GetAttribute("id")),
                                    Name = field.Name,
                                    Key = fieldDoc.DocumentElement.Attributes["key"].Value,
                                    Guid = Guid.Parse(fieldDoc.DocumentElement.Attributes["id"].Value),
                                    Fieldtype = fieldDoc.DocumentElement.SelectSingleNode("//field[@key='type']").InnerText
                                });
                            }
                        }
                    }
                    templateModels.Add(templateM);
                }
                else
                {

                    foreach (var dir in info.GetDirectories())
                    {
                        WalkDirectory(dir);
                    }
                }
            }
        }
    }
}
