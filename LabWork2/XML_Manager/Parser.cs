// LabWork2/XML_Manager/Parser.cs

using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace LabWork2.XML_Manager
{
   
    public abstract class Parser : IParser
    {
        protected List<Person> People;

        protected Parser()
        {
            People = new List<Person>();
        }

      
        public IList<Person> Find(Filters filters)
        {
            return People.Where(filters.ValidatePerson).ToList();
        }

        public abstract bool Load(Stream inputStream, XmlReaderSettings settings);
    }
}