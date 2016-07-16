using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibSunLegacy.FileFormats.Vxl
{
    /*
     * Start empty.
     * 
     * For each voxel added:
     *      For each face of the voxel:
     *          If the face opposes a face in any surface, remove the opposed face.
     *          If the face neighbours any face of exactly one surface, add it to the surface.
     *          If the face neighbours faces of multiple surfaces, merge all neighbouring surfaces and add it to the result.
     *          If none of these apply, create a new surface.
     * 
     */

    public struct VxlFace
    {
        
    }

    public sealed class LimbGraph
    {
        
    }
}
