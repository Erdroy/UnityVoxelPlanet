
using UnityEngine;

namespace UnityVoxelPlanet
{
    public static class VoxelGenerator
    {
        public static void GenerateTempVoxels(VoxelPlanet planet, VoxelPlanetChunk chunk)
        {
            var width = VoxelPlanetChunk.Size;

            var planetCenter = planet.Position;
            var planetRadius = planet.Radius;
            var chunkPosition = chunk.Position;
            var chunkCenter = chunk.Bounds.center;

            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    for (var z = 0; z < width; z++)
                    {
                        var idx = z * width * width + y * width + x;
                        
                        var blockPosition = chunkPosition + new Vector3(x, y, z);
                        
                    }
                }
            }
        }
    }
}
