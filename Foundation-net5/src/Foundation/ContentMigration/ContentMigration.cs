using EPiServer.Shell.Navigation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.ContentMigration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using AutoMapper;
    using EPiServer;
    using EPiServer.Core;
    using EPiServer.DataAccess;
    using EPiServer.Framework.Blobs;
    using EPiServer.ServiceLocation;
    using EPiServer.Shell.Navigation;
    using Foundation.Features.Blog.BlogItemPage;
    using Foundation.Features.Blog.BlogListPage;
    using Foundation.Features.Media;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using T4;

    [MenuProvider]
    public class ContentMigrationMenuProvider : IMenuProvider
    {
        public IEnumerable<MenuItem> GetMenuItems()
        {
            var urlMenuItem1 = new UrlMenuItem("Content Migration", "/global/cms/admin/csp", "/ContentMigrationPage");
            urlMenuItem1.IsAvailable = context => true;
            urlMenuItem1.SortIndex = 500;

            return new List<MenuItem>(1)
            {
                urlMenuItem1
            };
        }
    }




    [Authorize(Roles = "CmsAdmin,WebAdmins,Administrators")]
    [Route("[controller]")]
    public class ContentMigrationPageController : Controller
    {
        private readonly IContentRepository _contentRepository;
        private readonly IBlobFactory _blobFactory;
        private readonly ContentAssetHelper _contentAssetHelper;
        private readonly IMapper _mapper;
        public ContentMigrationPageController(IContentRepository contentRepository, IBlobFactory blobFactory, ContentAssetHelper contentAssetHelper, IMapper mapper)
        {
            _contentRepository = contentRepository;
            _blobFactory = blobFactory;
            _contentAssetHelper = contentAssetHelper;
            _mapper = mapper;
        }

        [Route("")]
        [HttpGet]
        public IActionResult Index()
        {
            return View("contentmigration/index.cshtml");
        }

        [Route("createpage")]
        [HttpGet]
        public IActionResult CreatePage()
        {
             var parent = _contentRepository.Get<BlogListPage>(new EPiServer.Core.ContentReference(145));
             BlogItemPage myPage = _contentRepository.GetDefault<BlogItemPage>(parent.ContentLink);

             myPage.Name = "Herm";
             myPage.MetaTitle = "sdfsdf";
             myPage.PageDescription = "sdfsdfds";
             _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish);
     

            return Index();


        }

        private byte[] FindBlob(string id)
        {
            var folderItem = new DirectoryInfo(@"C:\Work\Epi\ContentMigration\Habitat.1.4.0.682\package\blob\master");// ..GetDirectories().FirstOrDefault(x => isGuidFolder(x.Name) && x.Name.Equals(id, StringComparison.OrdinalIgnoreCase));

            if (folderItem != null)
            {
                var file = folderItem.GetFiles().FirstOrDefault(x => x.Name.Equals(id.Replace("{","").Replace("}", ""),StringComparison.OrdinalIgnoreCase));
                if (file != null)
                {
                    return System.IO.File.ReadAllBytes(file.FullName);
                }
            }
            return null;
        }


        private ContentReference CreateImage(ContentReference reference, FieldDetails<ImageField> image)
        {
            Byte[] data = FindBlob(image.Value.BlobId);

            var imageFile = _contentRepository.GetDefault<ImageMediaData>(_contentAssetHelper.GetOrCreateAssetFolder(reference).ContentLink);

            imageFile.Name = image.Value.Name;

            var blob = _blobFactory.CreateBlob(imageFile.BinaryDataContainer, "." + image.Value.Extension);
            using (var s = blob.OpenWrite())
            {
                var w = new StreamWriter(s);
                w.BaseStream.Write(data, 0, data.Length);
                w.Flush();
            }
            imageFile.BinaryData = blob;
            return _contentRepository.Save(imageFile, SaveAction.Publish);


        }

        [Route("ExecuteImport")]
        [HttpGet]
        public IActionResult TypedImport() { 
            ContentTyped mapped;
            using (StreamReader r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory() + @"\wwwroot\Input\ContentTyped.json")))
            {
                string json = r.ReadToEnd();
                mapped = JsonConvert.DeserializeObject<ContentTyped>(json);
            }

            foreach (var article in mapped.Articles)
            {
                var parent = _contentRepository.Get<BlogListPage>(new EPiServer.Core.ContentReference(145));
                BlogItemPage myPage = _contentRepository.GetDefault<BlogItemPage>(parent.ContentLink);
                var employeeModel = _mapper.Map<Article, BlogItemPage>(article, myPage);
                _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish);

                var imageRef = CreateImage(myPage.ContentLink, article.Image);
                myPage.PageImage = imageRef;
                _contentRepository.Save(myPage, EPiServer.DataAccess.SaveAction.Publish);
                int i = 0;
            }

            return Index();
        }

        private void Import()
        {
            //https://www.newtonsoft.com/json/help/html/CreatingLINQtoJSON.htm

            var mappingModel = GetModel();
            var content = GetContent();

            foreach (var item in content)
            {
                var mappingDef = mappingModel.TemplateMappingDefinition.FirstOrDefault(x => x.SourceTemplateId.Equals(item.SourceTemplate, StringComparison.InvariantCultureIgnoreCase));

                if (mappingDef == null)
                {
                    continue;
                }

                JObject postData =
                        new JObject(
                            new JProperty("name", item.SourceName),
                            new JProperty("language", new JObject(new JProperty("name", mappingDef.DestinationLanguage))),
                            new JProperty("contentType", new JArray(new JValue(mappingDef.DestinationContentType))),
                            new JProperty("parentLink", new JObject(new JProperty("id", mappingDef.DestinationParentId))),
                            new JProperty("status", "CheckedOut"));
                foreach (var fieldMap in mappingDef.FieldMappings)
                {
                    var contentRef = item.SourceFields.Where(x => x.SourceName.Equals(fieldMap.SourceField, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (contentRef != null)
                    {
                        postData.Add(fieldMap.DestinationField, new JObject(new JProperty("value", contentRef.SourceValue)));
                    }
                }

                //CreateItem(postData.ToString());
            }



            /*
             * Step 1: Convert mappingfile.json into a c# object to aid mapping
             * 
             *  Step 2: Read content.json and convert to c# object.
             *  
             *  For each entry in content.json
             *     find referenced model in mappingfile
             *          create json entry below
             *          set defaults: name, language, contentType, parentLink, status (all from top level of mapping)
             *          
             *          for each field in mapping
             *              add to main object
             *              
             *              use newtonsoft jobject to build and serialize.
             *              
             *              
             * "{'name': 'Alloy Track','language': {'name': 'sv'},'contentType': ['ProductPage'],'parentLink': {'id': 5},'status': 'CheckedOut','metaTitle': {'value': 'Alloy Track SV'},'uniqueSellingPoints': {'value': ['Shared timeline','Project emails','To-do lists','Workflows','Status reports']}}"
             * 
             */


            /*
             * 
             * More fancy approach..
             * 
             * https://docs.microsoft.com/en-us/visualstudio/modeling/design-time-code-generation-by-using-t4-text-templates?view=vs-2022
             * 
             * SitecorePackageETL generates c# classes using T4 templates, those templates are seializable
             * all content from the package gets converted into a class object and serialized into json.
             * 
             * Import process then runs within the opti website codebase
             * For each object in serialized json, deserialize into generated c# class
             * Use automapper to transform the data as required.
             * Use native opti code to create items.
             * 
             * https://world.optimizely.com/documentation/developer-guides/CMS/Content/Creating-a-page-programmatically/
             * 
             * https://stackoverflow.com/questions/27993939/episerver-add-block-to-a-content-area-programmatically
             * 
             */
        }




        private Root GetModel()
        {
            Root mappingModel;
            using (StreamReader r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Input\\Mapping.json")))
            {
                string json = r.ReadToEnd();
                mappingModel = JsonConvert.DeserializeObject<Root>(json);
            }
            return mappingModel;
        }

        private List<OptimizelyContentModel> GetContent()
        {
            List<OptimizelyContentModel> content;
            using (StreamReader r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Input\\Content.json")))
            {
                string json = r.ReadToEnd();
                content = JsonConvert.DeserializeObject<List<OptimizelyContentModel>>(json);
            }
            return content;
        }
    }
}
