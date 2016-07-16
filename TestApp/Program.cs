using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using TibSunLegacy.FileFormats.Vxl;

namespace TestApp
{
    public static class Program
    {
        public static void Main(string[] AArguments)
        {
            VxlModel vmModel = new VxlModel();

            using (VxlReader vrReader = new VxlReader(File.OpenRead(@".\1tnk.vxl"), vmModel))
                vrReader.ReadToEnd();

            using (VxlWriter vWriter = new VxlWriter(File.OpenWrite(@".\1tnk_copy.vxl"), vmModel))
                vWriter.WriteToEnd();
        }
    }
}
