
namespace UnityVoxelPlanet
{
    public interface IVoxelMesher
    {
        // TODO: we need neighbors info! How?
        void CreateMesh(byte[] voxels);
    }
}
