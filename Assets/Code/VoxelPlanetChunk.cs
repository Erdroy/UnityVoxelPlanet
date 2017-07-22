
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelPlanetChunk class.
    /// </summary>
    public class VoxelPlanetChunk : BoundsOctreeNode<VoxelPlanetChunk, VoxelPlanet>, IBoundsOctreeNode
    {
        /// <summary>
        /// The size of voxels array (Size x Size x Size).
        /// </summary>
        public const int Size = 16; // 16x16x16 voxels.

        /// <summary>
        /// Called when this chunk is created.
        /// </summary>
        public override void OnCreate()
        {
            Voxels = new byte[Size * Size * Size];

            // TODO: create mesh
        }

        /// <summary>
        /// Called when this chunk is destroyed.
        /// </summary>
        public override void OnDestroy()
        {
            Debug.Log("PlanetChunk destroyed. Level: " + Level);
        }

        /// <summary>
        /// Called when this chunk is populated.
        /// </summary>
        public override void OnPopulated()
        {
            // TODO: unload mesh if any
        }

        /// <summary>
        /// Called when this chunk is depopulated.
        /// </summary>
        public override void OnDepopulated()
        {
            // TODO: create mesh
        }

        /// <summary>
        /// Updates this chunk/BoundsOctreeNode.
        /// </summary>
        /// <param name="cameraPosition"></param>
        public void Update(Vector3 cameraPosition)
        {
            
        }

        /// <summary>
        /// The voxel array.
        /// </summary>
        public byte[] Voxels { get; private set; }

        public override Vector3 Position { get; set; }

        public override Bounds Bounds { get; set; }
    }
}
