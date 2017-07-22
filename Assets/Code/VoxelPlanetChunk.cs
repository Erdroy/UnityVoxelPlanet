
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

        // private
        private VoxelMesh _voxelMesh;

        /// <summary>
        /// Called when this chunk is created.
        /// </summary>
        public override void OnCreate()
        {
            Voxels = new byte[Size * Size * Size];
            
            if(_voxelMesh == null)
                _voxelMesh = new VoxelMesh();

            VoxelProcessor.Enqueue(Generate, UploadMesh);
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
            // all child nodes deleted

            // TODO: create mesh
        }

        /// <summary>
        /// Updates this chunk/BoundsOctreeNode.
        /// </summary>
        /// <param name="cameraPosition"></param>
        public void Update(Vector3 cameraPosition)
        {
            if (IsPopulated)
            {
                // forward to children
                foreach (var chunk in ChildNodes)
                {
                    chunk.Update(cameraPosition);
                }
            }

            // call OnUpdate
            OnUpdate(Vector3.Distance(cameraPosition, Position));
        }

        public override Color GetDebugColor()
        {
            if (Voxels != null && Voxels.Length > 0)
            {
                return Color.green;
            }
            return Color.cyan;
        }

        // private
        private void OnUpdate(float distance)
        {
            // check if we need to populate or depopulate

            // TODO: manage populate/depopulate
        }

        // private
        private void UploadMesh()
        {
            // upload mesh
            _voxelMesh.Upload();
        }

        // private
        private void Generate()
        {
            // TODO: generate voxels

            // TODO: create mesh
        }

        /// <summary>
        /// The voxel array.
        /// </summary>
        public byte[] Voxels { get; private set; }

        public override Vector3 Position { get; set; }

        public override Bounds Bounds { get; set; }
    }
}
