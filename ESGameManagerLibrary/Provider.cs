using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESGameManagerLibrary
{
    public class Provider
    {
        [XmlElement("System")]
        public required string System { get; set; }

        [XmlElement("software")]
        public required string Software { get; set; }

        [XmlElement("database")]
        public required string Database { get; set; }

        [XmlElement("web")]
        public required string Web { get; set; }
    }
}
