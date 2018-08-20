using System.Collections.Generic;

namespace CsFile
{
    public class CsClass
    {
        public List<CsFeild> Fields { get; set; }
        public List<CsProperty> Properties { get; set; }
        public List<CsMethod> Methods { get; set; }
    }
}