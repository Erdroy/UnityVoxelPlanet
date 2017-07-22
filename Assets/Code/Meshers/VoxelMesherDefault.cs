
using System;

namespace UnityVoxelPlanet.Meshers
{
    public class VoxelMesherDefault : VoxelMesher
    {
        public override void CreateMesh(VoxelPlanetChunk chunk, VoxelPlanetChunk[] neighborChunks)
        {
            var mesh = chunk.GetMesh();

            // TODO: generate mesh
            throw new NotImplementedException();
        }
    }
}
