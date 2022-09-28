

using System.Collections.Generic;
using Newtonsoft.Json;
namespace T4
{
        public class ContentTyped{
         
    [JsonProperty("Article")]
    public List<Article> Articles {get;set;}
     
    [JsonProperty("Employee")]
    public List<Employee> Employees {get;set;}
    

        }
         
           public class Article : BaseTemplate {
                
                                public FieldDetails<string> Body {get; set;}
                                public FieldDetails<ImageField> Image {get; set;}
                                public FieldDetails<string> Summary {get; set;}
                                public FieldDetails<string> Title {get; set;}
                                public FieldDetails<string> IncludeInSearchResults {get; set;}
                                public FieldDetails<string> NavigationTitle {get; set;}
                                public FieldDetails<string> ShowChildren {get; set;}
                                public FieldDetails<string> ShowInNavigation {get; set;}
                                public FieldDetails<string> OpenGraphDescription {get; set;}
                                public FieldDetails<ImageField> OpenGraphImage {get; set;}
                                public FieldDetails<string> OpenGraphTitle {get; set;}
                                public FieldDetails<string> CssCode {get; set;}
                                public FieldDetails<string> InheritAssets {get; set;}
                                public FieldDetails<string> JavascriptCodeBottom {get; set;}
                                public FieldDetails<string> JavascriptCodeTop {get; set;}
                                public FieldDetails<string> BrowserTitle {get; set;}
                                public FieldDetails<string> CanIndex {get; set;}
                                public FieldDetails<string> CustomMetaData {get; set;}
                                public FieldDetails<string> MetaDescription {get; set;}
                                public FieldDetails<string> MetaKeywords {get; set;}
                                public FieldDetails<string> SeoFollowLinks {get; set;}
                                public FieldDetails<string> __Enableitemfallback {get; set;}
                                public FieldDetails<string> __Enforceversionpresence {get; set;}
                                public FieldDetails<string> __Source {get; set;}
                                public FieldDetails<string> __SourceItem {get; set;}
                                public FieldDetails<string> __Standardvalues {get; set;}
                                public FieldDetails<string> __Tracking {get; set;}
                                public FieldDetails<string> __ContextMenu {get; set;}
                                public FieldDetails<string> __Displayname {get; set;}
                                public FieldDetails<string> __Editor {get; set;}
                                public FieldDetails<string> __Editors {get; set;}
                                public FieldDetails<string> __Hidden {get; set;}
                                public FieldDetails<string> __Icon {get; set;}
                                public FieldDetails<string> __Originator {get; set;}
                                public FieldDetails<string> __Preview {get; set;}
                                public FieldDetails<string> __ReadOnly {get; set;}
                                public FieldDetails<string> __Ribbon {get; set;}
                                public FieldDetails<string> __Skin {get; set;}
                                public FieldDetails<string> __Sortorder {get; set;}
                                public FieldDetails<string> __Style {get; set;}
                                public FieldDetails<string> __SubitemsSorting {get; set;}
                                public FieldDetails<string> __Thumbnail {get; set;}
                                public FieldDetails<string> __Helplink {get; set;}
                                public FieldDetails<string> __Longdescription {get; set;}
                                public FieldDetails<string> __Shortdescription {get; set;}
                                public FieldDetails<string> __Boost {get; set;}
                                public FieldDetails<string> __BoostingRules {get; set;}
                                public FieldDetails<string> __Facets {get; set;}
                                public FieldDetails<string> __InsertRules {get; set;}
                                public FieldDetails<string> __Masters {get; set;}
                                public FieldDetails<string> __BucketParentReference {get; set;}
                                public FieldDetails<string> __Bucketable {get; set;}
                                public FieldDetails<string> __DefaultBucketQuery {get; set;}
                                public FieldDetails<string> __DefaultView {get; set;}
                                public FieldDetails<string> __EnabledViews {get; set;}
                                public FieldDetails<string> __IsBucket {get; set;}
                                public FieldDetails<string> __PersistentBucketFilter {get; set;}
                                public FieldDetails<string> __QuickActions {get; set;}
                                public FieldDetails<string> __ShouldNotOrganizeInBucket {get; set;}
                                public FieldDetails<string> __ContentTest {get; set;}
                                public FieldDetails<string> __Controller {get; set;}
                                public FieldDetails<string> __ControllerAction {get; set;}
                                public FieldDetails<string> __FinalRenderings {get; set;}
                                public FieldDetails<string> __PageLevelTestSetDefinition {get; set;}
                                public FieldDetails<string> __Presets {get; set;}
                                public FieldDetails<string> __Renderers {get; set;}
                                public FieldDetails<string> __Renderings {get; set;}
                                public FieldDetails<string> __Hideversion {get; set;}
                                public FieldDetails<string> __Validfrom {get; set;}
                                public FieldDetails<string> __Validto {get; set;}
                                public FieldDetails<string> __Neverpublish {get; set;}
                                public FieldDetails<string> __Publish {get; set;}
                                public FieldDetails<string> __Publishinggroups {get; set;}
                                public FieldDetails<string> __Unpublish {get; set;}
                                public FieldDetails<string> __Owner {get; set;}
                                public FieldDetails<string> __Security {get; set;}
                                public FieldDetails<string> __Created {get; set;}
                                public FieldDetails<string> __Createdby {get; set;}
                                public FieldDetails<string> __Revision {get; set;}
                                public FieldDetails<string> __Updated {get; set;}
                                public FieldDetails<string> __Updatedby {get; set;}
                                public FieldDetails<string> __Semantics {get; set;}
                                public FieldDetails<string> __Archivedate {get; set;}
                                public FieldDetails<string> __ArchiveVersiondate {get; set;}
                                public FieldDetails<string> __Reminderdate {get; set;}
                                public FieldDetails<string> __Reminderrecipients {get; set;}
                                public FieldDetails<string> __Remindertext {get; set;}
                                public FieldDetails<string> __QuickActionBarValidationRules {get; set;}
                                public FieldDetails<string> __SuppressedValidationRules {get; set;}
                                public FieldDetails<string> __ValidateButtonValidationRules {get; set;}
                                public FieldDetails<string> __ValidatorBarValidationRules {get; set;}
                                public FieldDetails<string> __WorkflowValidationRules {get; set;}
                                public FieldDetails<string> __Defaultworkflow {get; set;}
                                public FieldDetails<string> __Lock {get; set;}
                                public FieldDetails<string> __Workflow {get; set;}
                                public FieldDetails<string> __Workflowstate {get; set;}
                
           }
     
           public class Employee : BaseTemplate {
                
                                public FieldDetails<string> Email {get; set;}
                                public FieldDetails<string> Mobile {get; set;}
                                public FieldDetails<string> Telephone {get; set;}
                                public FieldDetails<string> Biography {get; set;}
                                public FieldDetails<string> BlogLink {get; set;}
                                public FieldDetails<string> FacebookLink {get; set;}
                                public FieldDetails<string> LinkedInLink {get; set;}
                                public FieldDetails<string> TwitterLink {get; set;}
                                public FieldDetails<string> IncludeInSearchResults {get; set;}
                                public FieldDetails<string> NavigationTitle {get; set;}
                                public FieldDetails<string> ShowChildren {get; set;}
                                public FieldDetails<string> ShowInNavigation {get; set;}
                                public FieldDetails<string> OpenGraphDescription {get; set;}
                                public FieldDetails<ImageField> OpenGraphImage {get; set;}
                                public FieldDetails<string> OpenGraphTitle {get; set;}
                                public FieldDetails<string> CssCode {get; set;}
                                public FieldDetails<string> InheritAssets {get; set;}
                                public FieldDetails<string> JavascriptCodeBottom {get; set;}
                                public FieldDetails<string> JavascriptCodeTop {get; set;}
                                public FieldDetails<string> BrowserTitle {get; set;}
                                public FieldDetails<string> CanIndex {get; set;}
                                public FieldDetails<string> CustomMetaData {get; set;}
                                public FieldDetails<string> MetaDescription {get; set;}
                                public FieldDetails<string> MetaKeywords {get; set;}
                                public FieldDetails<string> SeoFollowLinks {get; set;}
                                public FieldDetails<string> Name {get; set;}
                                public FieldDetails<ImageField> Picture {get; set;}
                                public FieldDetails<string> Summary {get; set;}
                                public FieldDetails<string> Title {get; set;}
                                public FieldDetails<string> __Enableitemfallback {get; set;}
                                public FieldDetails<string> __Enforceversionpresence {get; set;}
                                public FieldDetails<string> __Source {get; set;}
                                public FieldDetails<string> __SourceItem {get; set;}
                                public FieldDetails<string> __Standardvalues {get; set;}
                                public FieldDetails<string> __Tracking {get; set;}
                                public FieldDetails<string> __ContextMenu {get; set;}
                                public FieldDetails<string> __Displayname {get; set;}
                                public FieldDetails<string> __Editor {get; set;}
                                public FieldDetails<string> __Editors {get; set;}
                                public FieldDetails<string> __Hidden {get; set;}
                                public FieldDetails<string> __Icon {get; set;}
                                public FieldDetails<string> __Originator {get; set;}
                                public FieldDetails<string> __Preview {get; set;}
                                public FieldDetails<string> __ReadOnly {get; set;}
                                public FieldDetails<string> __Ribbon {get; set;}
                                public FieldDetails<string> __Skin {get; set;}
                                public FieldDetails<string> __Sortorder {get; set;}
                                public FieldDetails<string> __Style {get; set;}
                                public FieldDetails<string> __SubitemsSorting {get; set;}
                                public FieldDetails<string> __Thumbnail {get; set;}
                                public FieldDetails<string> __Helplink {get; set;}
                                public FieldDetails<string> __Longdescription {get; set;}
                                public FieldDetails<string> __Shortdescription {get; set;}
                                public FieldDetails<string> __Boost {get; set;}
                                public FieldDetails<string> __BoostingRules {get; set;}
                                public FieldDetails<string> __Facets {get; set;}
                                public FieldDetails<string> __InsertRules {get; set;}
                                public FieldDetails<string> __Masters {get; set;}
                                public FieldDetails<string> __BucketParentReference {get; set;}
                                public FieldDetails<string> __Bucketable {get; set;}
                                public FieldDetails<string> __DefaultBucketQuery {get; set;}
                                public FieldDetails<string> __DefaultView {get; set;}
                                public FieldDetails<string> __EnabledViews {get; set;}
                                public FieldDetails<string> __IsBucket {get; set;}
                                public FieldDetails<string> __PersistentBucketFilter {get; set;}
                                public FieldDetails<string> __QuickActions {get; set;}
                                public FieldDetails<string> __ShouldNotOrganizeInBucket {get; set;}
                                public FieldDetails<string> __ContentTest {get; set;}
                                public FieldDetails<string> __Controller {get; set;}
                                public FieldDetails<string> __ControllerAction {get; set;}
                                public FieldDetails<string> __FinalRenderings {get; set;}
                                public FieldDetails<string> __PageLevelTestSetDefinition {get; set;}
                                public FieldDetails<string> __Presets {get; set;}
                                public FieldDetails<string> __Renderers {get; set;}
                                public FieldDetails<string> __Renderings {get; set;}
                                public FieldDetails<string> __Hideversion {get; set;}
                                public FieldDetails<string> __Validfrom {get; set;}
                                public FieldDetails<string> __Validto {get; set;}
                                public FieldDetails<string> __Neverpublish {get; set;}
                                public FieldDetails<string> __Publish {get; set;}
                                public FieldDetails<string> __Publishinggroups {get; set;}
                                public FieldDetails<string> __Unpublish {get; set;}
                                public FieldDetails<string> __Owner {get; set;}
                                public FieldDetails<string> __Security {get; set;}
                                public FieldDetails<string> __Created {get; set;}
                                public FieldDetails<string> __Createdby {get; set;}
                                public FieldDetails<string> __Revision {get; set;}
                                public FieldDetails<string> __Updated {get; set;}
                                public FieldDetails<string> __Updatedby {get; set;}
                                public FieldDetails<string> __Semantics {get; set;}
                                public FieldDetails<string> __Archivedate {get; set;}
                                public FieldDetails<string> __ArchiveVersiondate {get; set;}
                                public FieldDetails<string> __Reminderdate {get; set;}
                                public FieldDetails<string> __Reminderrecipients {get; set;}
                                public FieldDetails<string> __Remindertext {get; set;}
                                public FieldDetails<string> __QuickActionBarValidationRules {get; set;}
                                public FieldDetails<string> __SuppressedValidationRules {get; set;}
                                public FieldDetails<string> __ValidateButtonValidationRules {get; set;}
                                public FieldDetails<string> __ValidatorBarValidationRules {get; set;}
                                public FieldDetails<string> __WorkflowValidationRules {get; set;}
                                public FieldDetails<string> __Defaultworkflow {get; set;}
                                public FieldDetails<string> __Lock {get; set;}
                                public FieldDetails<string> __Workflow {get; set;}
                                public FieldDetails<string> __Workflowstate {get; set;}
                
           }
    }