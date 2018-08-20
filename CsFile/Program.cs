using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using Microsoft.CSharp.RuntimeBinder;
namespace CsFile
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }

    public class CsFile
    {
        public List<String> Imports { get; set; }
        public List<CsNamespace> Namespaces { get; set; }
    }
    public class CsNamespace
    {
        public List<CsClass> Classes { get; set; }
        public List<CsEnum> Enums { get; set; }
        public List<CsInterface> Interfaces { get; set; }
        public List<CsStructures> Structures { get; set; }
    }
}
