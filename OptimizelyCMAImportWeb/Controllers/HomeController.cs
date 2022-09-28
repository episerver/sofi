using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OptimizelyCMAImportWeb.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;

namespace OptimizelyCMAImportWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            if (!User.Identity.IsAuthenticated)
            {

                return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
            }
            return View("index");
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

        private List<SitecorePackageETL.Models.OptimizelyContentModel> GetContent()
        {
            List<SitecorePackageETL.Models.OptimizelyContentModel> content;
            using (StreamReader r = new StreamReader(Path.Combine(Directory.GetCurrentDirectory() + "\\wwwroot\\Input\\Content.json")))
            {
                string json = r.ReadToEnd();
                content = JsonConvert.DeserializeObject<List<SitecorePackageETL.Models.OptimizelyContentModel>>(json);
            }
            return content;
        }

        public async Task<IActionResult> ExecuteImport()
        {
            //https://www.newtonsoft.com/json/help/html/CreatingLINQtoJSON.htm

            var mappingModel = GetModel();
            var content = GetContent();

            foreach(var item in content)
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
                foreach( var fieldMap in mappingDef.FieldMappings)
                {
                    var contentRef = item.SourceFields.Where(x=> x.SourceName.Equals(fieldMap.SourceField, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if(contentRef != null)
                    {
                        postData.Add(fieldMap.DestinationField, new JObject(new JProperty("value", contentRef.SourceValue)));
                    }
                }

                CreateItem(postData.ToString());
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

            return View("index");
        }

        //https://code-maze.com/file-upload-aspnetcore-mvc/
        //https://codewithmukesh.com/blog/file-upload-in-aspnet-core-mvc/

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Index(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
                    bool basePathExists = System.IO.Directory.Exists(basePath);
                    if (!basePathExists) Directory.CreateDirectory(basePath);

                    var fileName = Path.GetFileNameWithoutExtension(formFile.FileName);
                    var filePath = Path.Combine(basePath, formFile.FileName);
                    var extension = Path.GetExtension(formFile.FileName);
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }

                }
            }
            // process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(new
            {
                count = files.Count,
                size,
                filePaths
            });
        }

        public async void CreateItem(string message)
        {
            var t = await HttpContext.GetTokenAsync("access_token");

            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://localhost:44397/api/episerver/v3.0/contentmanagement"),
                    Method = HttpMethod.Post,
                };
                //var objAsJson = JsonConvert.SerializeObject(myObject);
                request.Content = new StringContent(message, Encoding.UTF8, "application/json");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", t);
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));

                try {
                    var response = await client.SendAsync(request);

                    var content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        var respJObject = JObject.Parse(content);
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                }
                

            }
        }

        public async Task<ActionResult> Test_Get_Method()
        {
            var t = await HttpContext.GetTokenAsync("access_token");


            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri("https://localhost:44397/api/episerver/v3.0/contentmanagement/145/"),
                    Method = HttpMethod.Get,
                };
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", t);
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));

                var response = await client.SendAsync(request);

                var jsonTask = await response.Content.ReadAsStringAsync();
                var x = JObject.Parse(jsonTask);
                int i = 1;


            }
            return View("index");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}