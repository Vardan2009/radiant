using IL2CPU.API.Attribs;
using System.Collections.Generic;

namespace radiant
{
    public class EmbeddedResourceLoader
    {
        [ManifestResourceStream(ResourceName = "radiant.resource.cur.bmp")]
        static byte[] radiant_resource_cur_bmp;

        static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>()
        {
            {"cur.bmp",radiant_resource_cur_bmp}
        };

        public static byte[] LoadEmbeddedResource(string filename)
        {
            return resources[filename];
        }
    }
}
