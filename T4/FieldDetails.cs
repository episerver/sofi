using System;

namespace T4
{
    public class FieldDetails<T>
    {
        public T Value { get; set; }
        public string FieldType { get; set; }
        public string FieldId { set; get; }
        
    }

    public class ImageField
    {
        public string Name { get; set; }
        public string Extension { get; set; }

        public string Value { get; set; }

        public string BlobId { get; set; }
    }

    public class BaseTemplate
    {
        public string SourceId { set; get; }
        public string SourceItemPath { set; get; }
        
    }
}
