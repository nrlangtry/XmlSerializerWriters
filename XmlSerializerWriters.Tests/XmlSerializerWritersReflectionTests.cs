using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xunit;

namespace XmlSerializerWriters.Tests
{
    /// <summary>
    /// XmlSerializer has a special mode in which the serializer would use reflection instead of RefEmit to do serialization
    /// These tests use reflection
    /// </summary>
    public class XmlSerializerWritersReflectionTests
    {
        const string stringToUse = "\" & < > © ® é";

        [Fact]
        public void XmlWriter_Reflection_Test()
        {
            // Setup
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            var model = new Model() { Value = stringToUse };

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Encoding = new UTF8Encoding(false), // this instead of Encoding.UTF8 to remove the prefix "bom" from serializing to byte[]
                Indent = true,
                CheckCharacters = true,
                ConformanceLevel = ConformanceLevel.Document
            };

            settings.CloseOutput = false;

            // Execute
            var result = string.Empty;
            using (var ms = new MemoryStream())
            {
                using (var xmlWriter = XmlWriter.Create(ms, settings))
                {
                    new XmlSerializer(typeof(Model)).Serialize(xmlWriter, model);
                }

                result = Encoding.UTF8.GetString(ms.ToArray());
            }

            var exceptionThrown = false;
            try
            {
                var doc = XDocument.Parse(HttpUtility.UrlDecode(result));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Verify
            Assert.False(exceptionThrown);

            // reset
            method.Invoke(null, new object[] { 0 });
        }

        [Fact]
        public void StringWriterUtf8_Reflection_Test()
        {
            // Setup
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            var model = new Model() { Value = stringToUse };

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Encoding = new UTF8Encoding(false), // this instead of Encoding.UTF8 to remove the prefix "bom" from serializing to byte[]
                Indent = true,
                CheckCharacters = true
            };

            // Execute
            var result = string.Empty;
            using (var stringWriter = new StringWriterUtf8())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                {
                    new XmlSerializer(typeof(Model)).Serialize(xmlWriter, model);

                    result = stringWriter.ToString();
                }
            }

            var exceptionThrown = false;
            try
            {
                var doc = XDocument.Parse(HttpUtility.UrlDecode(result));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Verify
            Assert.False(exceptionThrown);

            // reset
            method.Invoke(null, new object[] { 0 });
        }

        [Fact]
        public void StreamWriter_Reflection_Test()
        {
            // Setup
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            var model = new Model() { Value = stringToUse };

            // Execute
            var result = string.Empty;
            using (var ms = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(ms, new UTF8Encoding(false)))
                {
                    new XmlSerializer(typeof(Model)).Serialize(streamWriter, model);

                    streamWriter.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Position = 0;

                    var utf8String = Encoding.UTF8.GetString(ms.ToArray());

                    result = utf8String;
                }
            }

            var exceptionThrown = false;
            try
            {
                var doc = XDocument.Parse(HttpUtility.UrlDecode(result));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Verify
            Assert.False(exceptionThrown);

            // reset
            method.Invoke(null, new object[] { 0 });
        }

        [Fact]
        public void XmlTextWriter_Reflection_Test()
        {
            // Setup
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            var model = new Model() { Value = stringToUse };

            // Execute
            var result = string.Empty;
            using (var ms = new MemoryStream())
            {
                using (var streamWriter = new XmlTextWriter(ms, new UTF8Encoding(false)))
                {
                    new XmlSerializer(typeof(Model)).Serialize(streamWriter, model);

                    streamWriter.Flush();

                    ms.Seek(0, SeekOrigin.Begin);
                    ms.Position = 0;

                    var utf8String = Encoding.UTF8.GetString(ms.ToArray());
                    result = utf8String;
                }
            }

            var exceptionThrown = false;
            try
            {
                var doc = XDocument.Parse(HttpUtility.UrlDecode(result));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Verify
            Assert.False(exceptionThrown);

            // reset
            method.Invoke(null, new object[] { 0 });
        }

        [Fact]
        public void XDocumentWriter_Reflection_Test()
        {
            // Setup
            MethodInfo method = typeof(XmlSerializer).GetMethod("set_Mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            method.Invoke(null, new object[] { 1 });

            var model = new Model() { Value = stringToUse };

            // Execute
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Model));
            XDocument doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, model);
            }

            var result = doc.ToString();

            var exceptionThrown = false;
            try
            {
                var docParsed = XDocument.Parse(HttpUtility.UrlDecode(result));
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Verify
            Assert.False(exceptionThrown);

            // reset
            method.Invoke(null, new object[] { 0 });
        }
    }
}
