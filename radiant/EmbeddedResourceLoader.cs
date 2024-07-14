using IL2CPU.API.Attribs;
using System.Collections.Generic;

namespace radiant
{
    public class EmbeddedResourceLoader
    {
        [ManifestResourceStream(ResourceName = "radiant.resource.cur.bmp")]
        static byte[] radiant_resource_cur_bmp;

        [ManifestResourceStream(ResourceName = "radiant.resource.FreeSans.ttf")]
        static byte[] radiant_resource_freesans_ttf;

        [ManifestResourceStream(ResourceName = "radiant.resource.FreeSansBoldOblique.ttf")]
        static byte[] radiant_resource_freesansboldoblique_ttf;

        static Dictionary<string, byte[]> resources = new Dictionary<string, byte[]>()
        {
            {"cur.bmp",radiant_resource_cur_bmp},
            {"FreeSans.ttf",radiant_resource_freesans_ttf},
            {"FreeSansBoldOblique.ttf",radiant_resource_freesansboldoblique_ttf}
        };

        public static byte[] LoadEmbeddedResource(string filename)
        {
            return resources[filename];
        }
    }
}
