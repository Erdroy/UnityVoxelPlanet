
namespace UnityVoxelPlanet
{
    public interface IVoxelMesher
    {
        // TODO: maybe something like voxel storage would be better, because we don't have any neighbors info at the moment
        void CreateMesh(byte[] voxels);
    }
}
