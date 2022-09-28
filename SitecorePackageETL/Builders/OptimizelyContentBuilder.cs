using ModelLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SitecorePackageETL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SitecorePackageETL.Builders
{
    /*
     * 
     *Input Start Node:
     *  InputModel.OptimizelyContentStartNodes 
     *  
     *  Iterate over each start node
     *      Locate start node in sitecore content tree
     *      Iterate over each Folder (that is not a guid)
     *          Check if item matches a specific template to import ("Article")
     *          Create output mapping in a flat structure
     *          
     *        
     *          
     * 
     *      /Feature
     *          /{...} (guid) // Skip in MVP
     *                /en
     *                     xml
     *                /jp 
     *                     xml  
     *          /News
     *                /{...} (Folder guid)
     *                     /en
     *                          xml
     *                     /jp 
     *                          xml
     *                /News
     *                      /{...}
     *                           /en
     *                               xml
     *                          /jp 
     *                               xml
     *                     /2015
     *                         /{...}
     *                               /en
     *                                   xml
     *                              /jp  
     *                                   xml
     *                         /08
     *                             /{...}
     *                                   /en
     *                                       xml
     *                                  /jp  
     *                                       xml
     *                             ... (full structure is 2015/08/27/17/57)
     *                              /Best In Class
     *                                  /{...}
     *                                        /en
     *                                            xml
     *                                       /jp  
     *                                            xml
     *                                  /_Local
     *                                       /{...} (skip)
     *                                            /en
     *                                                xml
     *                                            /jp  
     *                                                xml
     *                                       /Local Content Example
     *                                            /{...}
     *                                                /en
     *                                                    xml
     *                                                /jp  
     *                                                    xml
     * 
     * 
     * 
     */

    /// <summary>
    /// 
    /// </summary>
    internal class OptimizelyContentBuilder
    {
        private InputModel _inputModel { get; set; }
        private List<OptimizelyContentModel> _optiContent = new List<OptimizelyContentModel>();
        public OptimizelyContentBuilder(InputModel inputModel)
        {
            _inputModel = inputModel;
        }


        public void GenerateContent()
        {
            foreach (var startNode in _inputModel.OptimizelyContentStartNodes)
            {
                var dirInfo = new DirectoryInfo(_inputModel.MapContentStartNode(startNode));
                if (!dirInfo.Exists)
                {
                    throw new Exception($"Path not found: {_inputModel.MapContentStartNode(startNode)}");
                }
                WalkDirectory(dirInfo, startNode);
            }
            LogOptimizelyContent();
        }


        private void LogOptimizelyContent()
        {
            using (StreamWriter writer = System.IO.File.CreateText(_inputModel.OutputLocation + "\\Content.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(_optiContent));
            }
        }

        private bool isGuidFolder(string name)
        {
            var test = Guid.Empty;
            return Guid.TryParse(name, out test);
        }

        private void WalkDirectory(DirectoryInfo info, string path)
        {
            var templateModel = new TemplateModel();
            //Retrieve child folder with guid for a name
            var folderItem = info.GetDirectories().FirstOrDefault(x => isGuidFolder(x.Name));

            if (folderItem != null)
            {
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

                    Guid templateId;

                    if (Guid.TryParse(doc.DocumentElement.Attributes["tid"]?.Value, out templateId) && _inputModel.TemplatesToParse.Contains(templateId))
                    {

                        var templateM = new OptimizelyContentModel()
                        {
                            SourceId = doc.DocumentElement.GetAttribute("id"),
                            SourceItemPath = path,
                            SourceName = doc.DocumentElement.GetAttribute("name"),
                            SourceTemplate = doc.DocumentElement.Attributes["tid"]?.Value
                        };
                        foreach (XmlNode field in doc.DocumentElement.SelectNodes("//field"))
                        {
                            templateM.SourceFields.Add(new OptimizelyContentFieldModel()
                            {
                                SourceName = field.Attributes["key"].Value,
                                SourceFieldType = field.Attributes["type"].Value,
                                SourcefieldId = field.Attributes["tfid"].Value,
                                SourceValue = field.FirstChild.InnerText

                            });
                        }

                        _optiContent.Add(templateM);
                    }
                }
            }
            foreach (var dir in info.GetDirectories())
            {
                WalkDirectory(dir, path + '\\' + dir.Name);
            }
        }
    }
}
