
using UnityVoxelPlanet.Meshers;

namespace UnityVoxelPlanet
{
    public abstract class VoxelMesher
    {
        public abstract void CreateMesh(VoxelPlanetChunk chunk, VoxelPlanetChunk[] neighborChunks);

        private static VoxelMesher _current;
        public static VoxelMesher Current
        {
            get { return _current ?? (_current = new VoxelMesherDefault()); }
        }
    }
}
