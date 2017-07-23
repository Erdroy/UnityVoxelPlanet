
using MyHalp.MyMath;
using UnityEngine;

namespace UnityVoxelPlanet
{
    public enum BoundsOctreeNeighbor
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3,
        Front = 4,
        Back = 5
    }

    public class BoundsOctree<T, TH> where T : BoundsOctreeNode<T, TH>, IBoundsOctreeNode, new()
    {
        private T[] _childNodes;
        private int _baseSize;

        public void Init(int initialChildCount, TH owner)
        {
            _baseSize = initialChildCount;
            _childNodes = new T[initialChildCount * initialChildCount * initialChildCount];
            
            for (var y = 0; y < initialChildCount; y++)
            {
                for (var x = 0; x < initialChildCount; x++)
                {
                    for (var z = 0; z < initialChildCount; z++)
                    {
                        var idx = z * initialChildCount * initialChildCount + y * initialChildCount + x;

                        _childNodes[idx] = new T
                        {
                            Level = 0,
                            Handler = owner,
                            ParentNode = null
                        };
                        
                        var node = _childNodes[idx];
                        node.Level = 0;

                        var position = new MyVector3(x - initialChildCount / 2, y - initialChildCount / 2, z - initialChildCount / 2);
                        position *= BaseNodeSize;
                        node.Position = Int3.FromVector3(position);
                        node.Bounds = new Bounds
                        {
                            min = position,
                            max = new Vector3(position.X + BaseNodeSize.x, position.Y + BaseNodeSize.y, position.Z + BaseNodeSize.z)
                        };

                        node.OnCreate();
                    }
                }
            }

            // build neighbors
            for (var y = 0; y < initialChildCount; y++)
            {
                for (var x = 0; x < initialChildCount; x++)
                {
                    for (var z = 0; z < initialChildCount; z++)
                    {
                        var chunk = GetBaseChunk(x, y, z);
                        chunk.NeighborChunks = new T[6];

                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Top] = GetBaseChunk(x, y + 1, z);
                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Bottom] = GetBaseChunk(x, y - 1, z);
                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Left] = GetBaseChunk(x - 1, y, z);
                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Right] = GetBaseChunk(x + 1, y, z);
                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Front] = GetBaseChunk(x, y, z + 1);
                        chunk.NeighborChunks[(int)BoundsOctreeNeighbor.Back] = GetBaseChunk(x, y, z - 1);
                    }
                }
            }

            Debug.Log("Octree nodes created");
        }

        public T GetBaseChunk(int x, int y, int z)
        {
            if (x < 0 || y < 0 || z < 0 || x >= _baseSize || y >= _baseSize || z >= _baseSize)
                return null;

            var idx = z * _baseSize * _baseSize + y * _baseSize + x;

            if (idx >= _childNodes.Length)
                return null;

            return _childNodes[idx];
        }

        public void Destroy()
        {
            // TODO: kill all nodes
            Debug.Log("Octree nodes destroyed");
        }

        public T[] GetChildNodes()
        {
            return _childNodes;
        }

        public void DrawDebug()
        {
            foreach (var child in _childNodes)
            {
                child.DrawDebug();
            }
        }

        public Vector3 Position { get; set; }

        public Vector3 BaseNodeSize { get; set; }
    }
}
