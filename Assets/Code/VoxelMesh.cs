
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityVoxelPlanet
{
    public class VoxelMesh : IDisposable
    {
        private readonly Mesh _mesh;

        public List<Vector3> Positions = new List<Vector3>();
        public List<int> Triangles = new List<int>();

        public VoxelMesh()
        {
            _mesh = new Mesh();
        }

        public void Upload()
        {
            if (Positions.Count == 0 || Triangles.Count == 0)
                return;

            _mesh.SetVertices(Positions);
            _mesh.SetTriangles(Triangles, 0);

            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();

            _mesh.UploadMeshData(true);
        }
        
        public void Dispose()
        {
            Object.Destroy(_mesh);
            Positions.Clear();
            Triangles.Clear();
        }
    }
}
