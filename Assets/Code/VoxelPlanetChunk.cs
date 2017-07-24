
using System;
using UnityEngine;
using Object = UnityEngine.Object;

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
        private GameObject _chunkObject;
        private bool _hasBlocks;

        private int _generatedChunks;
        
        /// <summary>
        /// Called when this chunk is created.
        /// </summary>
        public override void OnCreate()
        {
            CreateMesh();
        }

        /// <summary>
        /// Called when this chunk is destroyed.
        /// </summary>
        public override void OnDestroy()
        {
            if (_chunkObject)
            {
                Object.Destroy(_chunkObject);
            }

            _voxelMesh.Dispose();
            Voxels = null;
        }

        /// <summary>
        /// Called when this chunk is populated.
        /// </summary>
        public override void OnPopulated()
        {
            // TODO: call when the populating is finished (all chunks have been generated)

            
        }

        /// <summary>
        /// Called when this chunk is depopulated.
        /// </summary>
        public override void OnDepopulated()
        {
            // all child nodes deleted
            CreateMesh();
        }
        
        /// <summary>
        /// Updates this chunk/BoundsOctreeNode.
        /// </summary>
        /// <param name="cameraPosition"></param>
        public void Update(Vector3 cameraPosition)
        {
            if (!_hasBlocks)
                return;

            if (IsPopulated && ChildNodes != null)
            {
                // forward to children
                foreach (var chunk in ChildNodes)
                {
                    chunk.Update(cameraPosition);
                }
            }

            // call OnUpdate
            OnUpdate(cameraPosition);
        }

        /// <summary>
        /// Gets the voxel at given coord.
        /// </summary>
        public byte GetVoxelUnsafe(int x, int y, int z)
        {
            if (Voxels == null)
                return 0;

            return Voxels[z * Size * Size + y * Size + x];
        }

        /// <summary>
        /// Gets the voxel at given coord, including neighbor chunks (only side by side).
        /// </summary>
        public byte GetVoxel(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= Size || y >= Size || z >= Size)
            {
                // try to get voxel from neigh

               /* // left/right
                if (x >= Size && NeighborChunks[(int)BoundsOctreeNeighbor.Right] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Right].GetVoxelUnsafe(x - Size, y, z);

                if (x < 0 && NeighborChunks[(int)BoundsOctreeNeighbor.Left] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Left].GetVoxelUnsafe(x + Size, y, z);

                // top/bottom
                if (y >= Size && NeighborChunks[(int)BoundsOctreeNeighbor.Top] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Top].GetVoxelUnsafe(x, y - Size, z);

                if (y < 0 && NeighborChunks[(int)BoundsOctreeNeighbor.Bottom] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Bottom].GetVoxelUnsafe(x, y + Size, z);

                // front/back
                if (z >= Size && NeighborChunks[(int)BoundsOctreeNeighbor.Front] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Front].GetVoxelUnsafe(x, y, z - Size);

                if (z < 0 && NeighborChunks[(int)BoundsOctreeNeighbor.Back] != null)
                    return NeighborChunks[(int)BoundsOctreeNeighbor.Back].GetVoxelUnsafe(x, y, z + Size);

                Debug.Log("invalid neighbor");
                */
                // invalid neighbor
                return 0;
            }

            return GetVoxelUnsafe(x, y, z);
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
        private void OnChunkGenerated()
        {
            _generatedChunks++;

            if (_generatedChunks + 1 >= NodeChildCount)
            {
                OnChunksCreated();
                _generatedChunks = 0;
            }
        }

        // private
        private void OnChunksCreated()
        {
            // called when all chunks done generating
            if (_chunkObject)
            {
                Object.Destroy(_chunkObject);
            }

            _voxelMesh.Dispose();
            Voxels = null;
        }

        // private
        private void CreateMesh()
        {
            _voxelMesh = new VoxelMesh();

            // calculate voxel size
            _voxelSize = Bounds.size.x / Size;

            _chunkObject = new GameObject("<chunk " + _voxelSize + ">");
            _chunkObject.transform.parent = Handler.transform;
            _chunkObject.transform.position = Position.ToVector3();

            // enqueue to generate
            VoxelProcessor.Enqueue(Generate, OnGenerated);
        }

        // private
        private void OnUpdate(Vector3 cameraPosition)
        {
            // check if we need to populate or depopulate

            // TODO: manage populate/depopulate

            // get proper distance to the camera
            var boundsPoint = Bounds.ClosestPoint(cameraPosition);
            var distance = Vector3.Distance(boundsPoint, cameraPosition);
            
            if (!IsPopulated && CanPopulate)
            {
                // try populate
                if (distance < Bounds.size.x * 2.0f)
                {
                    Populate();
                }
            }

            if (IsPopulated && CanDepopulate)
            {
                // try depopulate
                if (distance > Bounds.size.x * 2.25f + Size * 4.0f)
                {
                    Depopulate();
                }
            }
        }

        // private
        private void DestroyChildNodes()
        {
            foreach (var node in ChildNodes)
            {
                node.OnDestroy();
            }

            ChildNodes = null;
        }

        // private
        private void OnGenerated()
        {
            if (ParentNode != null)
                ParentNode.OnChunkGenerated();

            // check child nodes, if there are nodes, destroy!
            if (ChildNodes != null)
            {
                DestroyChildNodes();
            }

            if (_chunkObject == null)
            {
                Debug.Log("No chunk object");
                return;
            }
            
            if (!_hasBlocks)
                return;

            // upload mesh
            _voxelMesh.Upload();

            var mesh = _voxelMesh.GetMesh();

            var mf = _chunkObject.AddComponent<MeshFilter>();
            mf.sharedMesh = mesh;

            var mr = _chunkObject.AddComponent<MeshRenderer>();
            mr.material = Handler.DefaultMaterial;

            if (mesh == null)
            {
                Debug.Log("Mesh is null for " + _voxelSize);
            }
        }

        // private
        private void Generate()
        {
            try
            {
                Voxels = new byte[Size * Size * Size];

                // generate voxels
                _hasBlocks = VoxelGenerator.GenerateTempVoxels(Handler, this);

                // create mesh
                VoxelMesher.Current.CreateMesh(this, NeighborChunks);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        /// <summary>
        /// The voxel array.
        /// </summary>
        public byte[] Voxels { get; private set; }

        public override Int3 Position { get; set; }

        public override Bounds Bounds { get; set; }
    }
}
