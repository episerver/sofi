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
using System.Xml.Linq;

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
    internal class OptimizelyTypedContentBuilder
    {
        private InputModel _inputModel { get; set; }
        private List<OptimizelyContentModel> _optiContent = new List<OptimizelyContentModel>();

        private Dictionary<string, List<JObject>> keyValuePairs = new Dictionary<string, List<JObject>>();
        public OptimizelyTypedContentBuilder(InputModel inputModel)
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
            using (StreamWriter writer = System.IO.File.CreateText(_inputModel.OutputLocation + "\\ContentTyped.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(keyValuePairs));
            }
        }

        private bool isGuidFolder(string name)
        {
            var test = Guid.Empty;
            return Guid.TryParse(name, out test);
        }

        private JObject ImageSearch(DirectoryInfo info, string id)
        {
            var folderItem = info.GetDirectories().FirstOrDefault(x => isGuidFolder(x.Name) && x.Name.Equals(id,StringComparison.OrdinalIgnoreCase));

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

                    var objc = new JObject(
                                        new JProperty("Name", doc.DocumentElement.Attributes["name"]?.Value),
                                          new JProperty("Id", doc.DocumentElement.Attributes["id"]?.Value)
                                  );


                    foreach (XmlNode field in doc.DocumentElement.SelectNodes("//field"))
                    {
                        if (field.Attributes["key"].Value.Equals("extension"))
                        {
                            objc.Add(new JProperty("Extension", field.FirstChild.InnerText));
                        }
                        if (field.Attributes["key"].Value.Equals("blob"))
                        {
                            objc.Add(new JProperty("BlobId", field.FirstChild.InnerText));
                        }
                    }

                    return objc;

                }
            }
            foreach (var dir in info.GetDirectories())
            {
                var ret = ImageSearch(dir, id);
                if (ret != null)
                {
                    return ret;
                }
            }
            return null;
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
                        var templateName = doc.DocumentElement.GetAttribute("template");
                        if (!keyValuePairs.ContainsKey(templateName))
                        {
                            keyValuePairs.Add(templateName, new List<JObject>());
                        }

                        /*
                         * {
                         *      templates:{
                         *          employee:[],
                         *          article:[]
                         *      
                         *      }
                         * 
                         * 
                         * 
                         * 
                         * }
                         */



                        JObject objectData =
                        new JObject(
                            new JProperty("SourceId", doc.DocumentElement.GetAttribute("id")),
                            new JProperty("SourceItemPath", path));

                        foreach (XmlNode field in doc.DocumentElement.SelectNodes("//field"))
                        {
                            if (field.Attributes["type"].Value.Equals("Image"))
                            {
                                var xdoc = new XmlDocument();
                                xdoc.LoadXml(field.FirstChild.InnerText);
       
                                //Find file and extract info!
                                var dirInfo = new DirectoryInfo(_inputModel.MediaLibraryStartPath);
                                var findImageObj = ImageSearch(dirInfo, xdoc.DocumentElement.GetAttribute("mediaid"));

                                objectData.Add(new JProperty(field.Attributes["key"].Value.Replace(" ", ""),
                                      new JObject(new JProperty("Value", findImageObj),
                                      new JProperty("FieldType", field.Attributes["type"].Value),
                                      new JProperty("FieldId", field.Attributes["tfid"].Value))
                                      ));
                            }
                            else
                            {
                                objectData.Add(new JProperty(field.Attributes["key"].Value.Replace(" ", ""),
                              new JObject(new JProperty("Value", field.FirstChild.InnerText),
                              new JProperty("FieldType", field.Attributes["type"].Value),
                              new JProperty("FieldId", field.Attributes["tfid"].Value))
                              ));
                            }

                        }

                        keyValuePairs[templateName].Add(objectData);


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
