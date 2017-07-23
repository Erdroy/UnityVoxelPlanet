
using UnityEngine;

namespace UnityVoxelPlanet
{
    public static class VoxelGenerator
    {
        /// <summary>
        /// Generates flat (round ofc.) planet surface.
        /// </summary>
        /// <param name="planet">The planet instance.</param>
        /// <param name="chunk">The planet chunk instance.</param>
        public static bool GenerateTempVoxels(VoxelPlanet planet, VoxelPlanetChunk chunk)
        {
            const int width = VoxelPlanetChunk.Size;

            if (chunk.Voxels == null)
                return false;

            var planetCenter = planet.Position;
            var planetRadius = planet.Radius;
            var chunkPosition = chunk.Position;

            var voxelSize = chunk.GetVoxelSize();

            var hasBlock = false;

            for (var y = 0; y < width; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    for (var z = 0; z < width; z++)
                    {
                        // calculate all needed data
                        var idx = z * width * width + y * width + x;
                        var blockCenter = chunkPosition + new Vector3(x + 0.5f, y + 0.5f, z + 0.5f) * voxelSize;
                        var distanceToCenter = Vector3.Distance(planetCenter, blockCenter);

                        // if block is placed higher than planet radius then it is 0 (air), else the voxel is 1 (block, something)
                        if (distanceToCenter <= planetRadius)
                        {
                            hasBlock = true;
                            chunk.Voxels[idx] = 1;
                        }
                        else
                        {
                            chunk.Voxels[idx] = 0;
                        }
                    }
                }
            }

            return hasBlock;
        }
    }
}
