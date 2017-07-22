
using MyHalp.MyMath;
using UnityEngine;

namespace UnityVoxelPlanet
{
    public class BoundsOctree<T, TH> where T : BoundsOctreeNode<T, TH>, IBoundsOctreeNode, new()
    {
        private T[] _childNodes;

        public void Init(int initialChildCount, TH owner)
        {
            _childNodes = new T[initialChildCount * initialChildCount * initialChildCount];

            var idx = 0;
            for (var y = 0; y < initialChildCount; y++)
            {
                for (var x = 0; x < initialChildCount; x++)
                {
                    for (var z = 0; z < initialChildCount; z++)
                    {
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
                        node.Position = position;
                        node.Bounds = new Bounds
                        {
                            min = position,
                            max = new Vector3(position.X + BaseNodeSize.x, position.Y + BaseNodeSize.y, position.Z + BaseNodeSize.z)
                        };

                        node.OnCreate();

                        idx++;
                    }
                }
            }

            Debug.Log("Octree nodes created");
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
            Gizmos.color = new Color(1.0f, 1.0f, 0.5f, 0.6f);
            foreach (var child in _childNodes)
            {
                child.DrawDebug();
            }
        }

        public Vector3 Position { get; set; }

        public Vector3 BaseNodeSize { get; set; }
    }
}
