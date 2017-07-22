
using System.Collections.Generic;
using UnityEngine;

namespace UnityVoxelPlanet.Meshers
{
    public class VoxelMesherDefault : VoxelMesher
    {
        public override void CreateMesh(VoxelPlanetChunk chunk, VoxelPlanetChunk[] neighborChunks)
        {
            var mesh = chunk.GetMesh();
            var voxelSize = chunk.GetVoxelSize();

            // generate mesh
            for (var y = 0; y < VoxelPlanetChunk.Size; y++)
            {
                for (var x = 0; x < VoxelPlanetChunk.Size; x++)
                {
                    for (var z = 0; z < VoxelPlanetChunk.Size; z++)
                    {
                        var voxel = chunk.GetVoxelUnsafe(x, y, z);

                        if (voxel == 0)
                            continue;

                        // build mesh
                        if (IsTransparent(chunk, x - 1, y, z))
                            BuildFace(new Vector3(x, y, z), Vector3.up, Vector3.forward, false, voxelSize, mesh.Positions, mesh.Triangles);

                        if (IsTransparent(chunk, x + 1, y, z))
                            BuildFace(new Vector3(x + 1, y, z), Vector3.up, Vector3.forward, true, voxelSize, mesh.Positions, mesh.Triangles);

                        if (IsTransparent(chunk, x, y - 1, z))
                            BuildFace(new Vector3(x, y, z), Vector3.forward, Vector3.right, false, voxelSize, mesh.Positions, mesh.Triangles);

                        if (IsTransparent(chunk, x, y + 1, z))
                            BuildFace(new Vector3(x, y + 1, z), Vector3.forward, Vector3.right, true, voxelSize, mesh.Positions, mesh.Triangles);

                        if (IsTransparent(chunk, x, y, z - 1))
                            BuildFace(new Vector3(x, y, z), Vector3.up, Vector3.right, true, voxelSize, mesh.Positions, mesh.Triangles);

                        if (IsTransparent(chunk, x, y, z + 1))
                            BuildFace(new Vector3(x, y, z + 1), Vector3.up, Vector3.right, false, voxelSize, mesh.Positions, mesh.Triangles);
                    }
                }
            }
        }

        // private
        private static bool IsTransparent(VoxelPlanetChunk chunk, int x, int y, int z)
        {
            return chunk.GetVoxel(x, y, z) == 0;
        }

        // private
        private static void BuildFace(Vector3 corner, Vector3 up, Vector3 right, bool reversed, float scale, List<Vector3> vertices, List<int> indices)
        {
            var idx = vertices.Count;

            vertices.Add(corner * scale);
            vertices.Add((corner + up) * scale);
            vertices.Add((corner + up + right) * scale);
            vertices.Add((corner + right) * scale);

            if (reversed)
            {
                indices.Add(idx + 0);
                indices.Add(idx + 1);
                indices.Add(idx + 2);
                indices.Add(idx + 2);
                indices.Add(idx + 3);
                indices.Add(idx + 0);
            }
            else
            {
                indices.Add(idx + 1);
                indices.Add(idx + 0);
                indices.Add(idx + 2);
                indices.Add(idx + 3);
                indices.Add(idx + 2);
                indices.Add(idx + 0);
            }
        }
    }
}
