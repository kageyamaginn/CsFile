using System;

namespace CsFile
{
    public class CsFeild
    {
        public Type FieldType { get; set; }
        public String Name { get; set; }
        public AccessScope Access { get; set; }
    }
    public enum AccessScope { _private ,_internal, _public , _protected}
}