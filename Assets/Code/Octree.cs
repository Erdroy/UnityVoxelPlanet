
using UnityEngine;

namespace UnityVoxelPlanet
{
    public class Octree<T, TH> where T : OctreeNode<T, TH>, IOctreeNode, new()
    {
        private T[] _childNodes;

        public void Init(int initialChildCount, TH owner)
        {
            _childNodes = new T[initialChildCount * initialChildCount * initialChildCount];

            var idx = 0;

            for (var y = 0; y < _childNodes.Length; y++)
            {
                for (var x = 0; x < _childNodes.Length; x++)
                {
                    for (var z = 0; z < _childNodes.Length; z++)
                    {
                        _childNodes[idx] = new T
                        {
                            Level = 0,
                            Handler = owner,
                            ParentNode = null
                        };
                        
                        var node = _childNodes[x];
                        node.Level = 0;
                        node.OnCreate();

                        idx++;
                    }
                }
            }

            Debug.Log("Chunk count: " + _childNodes.Length);
        }

        public void Destroy()
        {
            // TODO: kill all nodes
        }

        public T[] GetChildNodes()
        {
            return _childNodes;
        }

        public void DrawDebug()
        {
            Gizmos.color = new Color(1.0f, 1.0f, 0.5f, 0.1f);
            foreach (var child in _childNodes)
            {
                child.DrawDebug();
            }
        }
    }
}
