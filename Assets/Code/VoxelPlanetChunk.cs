
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
        private float _voxelSize;

        /// <summary>
        /// Called when this chunk is created.
        /// </summary>
        public override void OnCreate()
        {
            if(_voxelMesh == null)
                _voxelMesh = new VoxelMesh();

            VoxelProcessor.Enqueue(Generate, UploadMesh);

            // calculate voxel size
            _voxelSize = Bounds.size.x / Size;
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
        /// Gets octree node debug color.
        /// </summary>
        /// <returns>The color.</returns>
        public override Color GetDebugColor()
        {
            if (Voxels != null && Voxels.Length > 0)
            {
                return Color.green;
            }
            return Color.cyan;
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

        /// <summary>
        /// Gets voxel block size.
        /// </summary>
        /// <returns>The voxel block size.</returns>
        public float GetVoxelSize()
        {
            return _voxelSize;
        }

        /// <summary>
        /// Gets reference to VoxelMesh instance of this chunk.
        /// </summary>
        /// <returns>The VoxelMesh reference.</returns>
        public VoxelMesh GetMesh()
        {
            return _voxelMesh;
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
            Voxels = new byte[Size * Size * Size];

            // generate voxels
            VoxelGenerator.GenerateTempVoxels(Handler, this);

            // create mesh
            VoxelMesher.Current.CreateMesh(this, NeighborChunks);
        }

        /// <summary>
        /// The voxel array.
        /// </summary>
        public byte[] Voxels { get; private set; }

        public override Vector3 Position { get; set; }

        public override Bounds Bounds { get; set; }
    }
}
