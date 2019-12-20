﻿using System.IO;
using System.Text;

namespace XmlSerializerWriters
{
    public class StringWriterUtf8 : StringWriter
    {
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
