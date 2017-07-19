
using UnityEngine;

namespace UnityVoxelPlanet
{
    public class VoxelPlanetChunk : OctreeNode<VoxelPlanetChunk, VoxelPlanet>, IOctreeNode
    {
        public override void OnCreate()
        {

        }

        public override void OnDestroy()
        {
            Debug.Log("PlanetChunk destroyed. Level: " + Level);
        }

        public override void OnPopulated()
        {
        }

        public override void OnDepopulated()
        {
        }

        public void Update(Vector3 cameraPosition)
        {
            
        }

        public override Vector3 Position { get; set; }

        public override Bounds Bounds { get; set; }
    }
}
