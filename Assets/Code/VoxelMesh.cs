
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelMesh class.
    /// </summary>
    public class VoxelMesh : IDisposable
    {
        // private
        private Mesh _mesh;

        /// <summary>
        /// List of mesh positions.
        /// </summary>
        public List<Vector3> Positions = new List<Vector3>();

        /// <summary>
        /// List of mesh triangles.
        /// </summary>
        public List<int> Triangles = new List<int>();

        /// <summary>
        /// Default VoxelMesh constructor.
        /// </summary>
        public VoxelMesh()
        {
            _mesh = new Mesh();
        }

        /// <summary>
        /// Uploads mesh data to the GPU,
        /// need to be called from main thread!
        /// </summary>
        public void Upload()
        {
            if (_mesh == null)
            {
                Debug.LogWarning("Cannot Upload mesh on disposed VoxelMesh.");
                return;
            }

            if (Positions.Count == 0 || Triangles.Count == 0)
                return;

            _mesh.SetVertices(Positions);
            _mesh.SetTriangles(Triangles, 0, true);
            
            _mesh.RecalculateNormals();

            _mesh.UploadMeshData(true);

            Positions = new List<Vector3>();
            Triangles = new List<int>();
        }

        /// <summary>
        /// Gets the Unity Mesh.
        /// </summary>
        public Mesh GetMesh()
        {
            return _mesh;
        }
        
        /// <summary>
        /// Disposes the VoxelMesh.
        /// </summary>
        public void Dispose()
        {
            Object.Destroy(_mesh);
            Positions.Clear();
            Triangles.Clear();

            _mesh = null;
        }
    }
}
