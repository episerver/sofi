

## Notes
Foundation project was cloned at: 
https://github.com/episerver/Foundation/tree/main

Commit: https://github.com/episerver/Foundation/commit/5ed791d16309f34390e4c37b94836b63f27edbc7

---

## Setup
Extract Habitat.1.4.0.682.zip.
This zip contains all the files from the habitat github project at https://github.com/Sitecore/Habitat/releases/tag/v1.4


Update SitecorePackageETL/input.json with the paths to the correct locations. Most important variables will be: PackageStartPath and OutputLocation.

---

## SitecorePackageETL
This project is a console app the executes a few tasks.

### 1. 
Read input from console arguments and deserialise into InputModel

### 2.
Execute IndexBuilder.

Crawl all templates in the InputModel and create corresponding TemplateModels for each.
These will be serialized to JSON and written to: TemplateIndex.csv and MappedModels.json

### 3. 
Execute CsvMappingBuilder.

This will generate the UserModelMapping.csv file. A file which can be used for developers and the business to map the fields from Sitecore to Epi.
This file isnt used as a direct input, rather it is just an aid for the mapping process.

### 4.
Execute OptimizelyContentBuilder

This will generate a single JSON file of all the content based on the templates specified in the InputModel. There is no hierarchy in the document, but there should be enough information in the content to build one if required.
AThis will be serialized and written to Content.json

### 5. 
Execute OptimizelyTypedContentBuilder

This process, is similar to 4. above, however rathen than trying to use a standard intermediate format to import. I have auto generated custom POCO classes. This builder generates a JSON object, that can be deserialised directly into a T4 generated class. This class can then be included in the Episerver project for developers to manually configure the mapping. Add an addition, this process also includes more data about Image fields that will allow us to import them directly into Optimizely

This will be serialized and written to ContentTyped.json

---

## OptimizelyCMAImportWeb
This project is a simple web project that will use OAUTH to connect to the Foundation-Net5 project in this solution.
It will use the ContentManagementAPI to import all the content from the Content.json file generated in 4. above.

Overview of process:

Step 1: Manually copy generated ContentTyped.json, Content.json, Mapping.json into wwwroot/Input


Step 2: Update blob path in ContentMigration.cs
DirectoryInfo(@"C:\Work\Epi\ContentMigration\Habitat.1.4.0.682\package\blob\master")


Step 3: Convert mappingfile.json into a c# object to definte mapping

 
Step 4: Read content.json and convert to c# object.


Step 5: Generate JSON POST object similar to below and submit to Content Management API.

```              
 "{'name': 'Alloy Track','language': {'name': 'sv'},'contentType': ['ProductPage'],'parentLink': {'id': 5},'status': 'CheckedOut','metaTitle': {'value': 'Alloy Track SV'},'uniqueSellingPoints': {'value': ['Shared timeline','Project emails','To-do lists','Workflows','Status reports']}}"
 ```

 ---

## T4
A small project that uses the MappedModel.json to generate POCO classes for each template.
The foundation project can then include this generated model for a more control over the mapping.
When opening this project, ensure you update the path in 'GeneratedModels.tt' to specify the appropriate location of hte mappedmodels.json:

```
using (StreamReader r = new StreamReader(@"C:\Work\Epi\ContentMigration\SitecorePackageETL\SitecorePackageETL\SitecorePackageETL\Output\MappedModels.json"))
{
    string json = r.ReadToEnd();
    models = JsonConvert.DeserializeObject<List<TemplateModel>>(json);
}
```

---

## Foundation
This project is used for 2 things.
1. The OptimizelyCMAImportWeb project will communicate to the Foundation project to execute the content management API.
2. A custom addon has been included in hte CMS / Admin / Content Migration tab. This is an integrated migration.

The process for the integrated migration is as follows:
1. Deserialize the typed json objects from ContentTyped.json into the appropriate models auto generated in the T4 project.
2. Use AutoMapper to allow developers to define a more complex mapping process with greate control over any transformations and field value defaults etc.
3. Import the content using the ContentRepository. This process will also create Images as well.

## Notes
Foundation project was cloned at: 
https://github.com/episerver/Foundation/tree/main

Commit: https://github.com/episerver/Foundation/commit/5ed791d16309f34390e4c37b94836b63f27edbc7

---

## Setup
Extract Habitat.1.4.0.682.zip.
This zip contains all the files from hte habitat github project at https://github.com/Sitecore/Habitat/releases/tag/v1.4


Update SitecorePackageETL/input.json with the paths to the correct locations. Most important variables will be: PackageStartPath and OutputLocation.

---

## SitecorePackageETL
This project is a console app the executes a few tasks.

### 1. 
Read input from console arguments and deserialise into InputModel

### 2.
Execute IndexBuilder.

Crawl all templates in the InputModel and create corresponding TemplateModels for each.
These will be serialized to JSON and written to: TemplateIndex.csv and MappedModels.json

### 3. 
Execute CsvMappingBuilder.

This will generate the UserModelMapping.csv file. A file which can be used for developers and the business to map the fields from Sitecore to Epi.
This file isnt used as a direct input, rather it is just an aid for the mapping process.

### 4.
Execute OptimizelyContentBuilder

This will generate a single JSON file of all the content based on the templates specified in the InputModel. There is no hierarchy in the document, but there should be enough information in the content to build one if required.
AThis will be serialized and written to Content.json

### 5. 
Execute OptimizelyTypedContentBuilder

This process, is similar to 4. above, however rathen than trying to use a standard intermediate format to import. I have auto generated custom POCO classes. This builder generates a JSON object, that can be deserialised directly into a T4 generated class. This class can then be included in the Episerver project for developers to manually configure the mapping. Add an addition, this process also includes more data about Image fields that will allow us to import them directly into Optimizely

This will be serialized and written to ContentTyped.json

---

## OptimizelyCMAImportWeb
This project is a simple web project that will use OAUTH to connect to the Foundation-Net5 project in this solution.
It will use the ContentManagementAPI to import all the content from the Content.json file generated in 4. above.

Overview of process:

Step 1: Manually copy generated ContentTyped.json, Content.json, Mapping.json into wwwroot/Input


Step 2: Update blob path in ContentMigration.cs
DirectoryInfo(@"C:\Work\Epi\ContentMigration\Habitat.1.4.0.682\package\blob\master")


Step 3: Convert mappingfile.json into a c# object to definte mapping

 
Step 4: Read content.json and convert to c# object.


Step 5: Generate JSON POST object similar to below and submit to Content Management API.

```              
 "{'name': 'Alloy Track','language': {'name': 'sv'},'contentType': ['ProductPage'],'parentLink': {'id': 5},'status': 'CheckedOut','metaTitle': {'value': 'Alloy Track SV'},'uniqueSellingPoints': {'value': ['Shared timeline','Project emails','To-do lists','Workflows','Status reports']}}"
 ```

 ---

## T4
A small project that uses the MappedModel.json to generate POCO classes for each template.
The foundation project can then include this generated model for a more control over the mapping.
When opening this project, ensure you update the path in 'GeneratedModels.tt' to specify the appropriate location of hte mappedmodels.json:

```
using (StreamReader r = new StreamReader(@"C:\Work\Epi\ContentMigration\SitecorePackageETL\SitecorePackageETL\SitecorePackageETL\Output\MappedModels.json"))
{
    string json = r.ReadToEnd();
    models = JsonConvert.DeserializeObject<List<TemplateModel>>(json);
}
```

---

## Foundation
This project is used for 2 things.
1. The OptimizelyCMAImportWeb project will communicate to the Foundation project to execute the content management API.
2. A custom addon has been included in hte CMS / Admin / Content Migration tab. This is an integrated migration.

The process for the integrated migration is as follows:
1. Deserialize the typed json objects from ContentTyped.json into the appropriate models auto generated in the T4 project.
2. Use AutoMapper to allow developers to define a more complex mapping process with greate control over any transformations and field value defaults etc.
3. Import the content using the ContentRepository. This process will also create Images as well.

## Sample Code Support
Please note that this is reference code only and is not supported by Optimizely. Optimizely Customers and Partners can use this to kickstart their own codebase. Please clone this repository using your authenticated github account. We would love to hear ways in which you have evolved this codebase. Please reach out to Brian Lockwood to showcase your expertise. brian.lockwood@optimizely.com 
