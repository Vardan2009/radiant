using IL2CPU.API.Attribs;
using System.Collections.Generic;

namespace radiant
{
    public class EmbeddedResourceLoader
    {
        [ManifestResourceStream(ResourceName = "radiant.resource.cur.bmp")]
        static byte[] radiant_resource_cur_bmp;

        [ManifestResourceStream(ResourceName = "radiant.resource.arial.ttf")]
        static byte[] radiant_resource_arial_ttf;

        [ManifestResourceStream(ResourceName = "radiant.resource.arialblackitalic.ttf")]
        static byte[] radiant_resource_arialblackitalic_ttf;

        static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>()
        {
            {"cur.bmp",radiant_resource_cur_bmp},
            {"arial.ttf",radiant_resource_arial_ttf},
            {"arialblackitalic.ttf",radiant_resource_arialblackitalic_ttf}
        };

        public static byte[] LoadEmbeddedResource(string filename)
        {
            return resources[filename];
        }
    }
}
