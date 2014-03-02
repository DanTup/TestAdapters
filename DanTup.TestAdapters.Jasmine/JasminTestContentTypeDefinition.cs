using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace DanTup.TestAdapters.Jasmine
{
    public class JasminTestContentTypeDefinition
    {
        public const string JasminTestContentType = "JasminTest";

        [Export(typeof(ContentTypeDefinition))]
        [Name(JasminTestContentType)]
        [BaseDefinition("JavaScript")]
        public ContentTypeDefinition IJasminTestContentType { get; set; }

        [Export(typeof(FileExtensionToContentTypeDefinition))]
        [ContentType(JasminTestContentType)]
        [FileExtension(".jstests")]
        public FileExtensionToContentTypeDefinition JasminTestFileExtension { get; set; }
    }
}
