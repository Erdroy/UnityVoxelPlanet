
using System;
using MyHalp.MyMath;
using UnityEngine;

namespace UnityVoxelPlanet
{
    public abstract class BoundsOctreeNode<T, TH> where T : BoundsOctreeNode<T, TH>, IBoundsOctreeNode, new()
    {
        /// <summary>
        /// Node children count.
        /// </summary>
        public const int NodeChildCount = 8;

        public abstract void OnCreate();

        public abstract void OnDestroy();

        public abstract void OnPopulated();

        public abstract void OnDepopulated();

        public abstract Color GetDebugColor();

        public void Populate()
        {
            if (Bounds.size.x * 0.5f <= 16.0f)
                return;

            if (IsPopulated)
            {
                Debug.LogWarning("Cannot populate! Node is already populated or not cleaned properly.");
                return;
            }

            ChildNodes = new T[NodeChildCount];

            var size = Bounds.size * 0.5f;

            for (var y = 0; y < 2; y++)
            {
                for (var x = 0; x < 2; x++)
                {
                    for (var z = 0; z < 2; z++)
                    {
                        var idx = z * 2 * 2 + y * 2 + x;

                        ChildNodes[idx] = new T
                        {
                            Handler = Handler,
                            ParentNode = null
                        };

                        var node = ChildNodes[idx];
                        node.Level = Level + 1;

                        var position = new MyVector3(x, y, z);

                        position *= size;
                        position += (MyVector3)Position;

                        node.Position = position;
                        node.Bounds = new Bounds
                        {
                            min = position,
                            max = new Vector3(position.X + size.x, position.Y + size.y, position.Z + size.z)
                        };
                        
                        node.OnCreate();
                    }
                }
            }

            OnPopulated();
        }

        public void Depopulate()
        {
            if (!IsPopulated)
            {
                Debug.LogWarning("Cannot depopulate! Node is not populated.");
                return;
            }

            throw new NotImplementedException();
        }

        public void DrawDebug()
        {
            if (IsPopulated)
            {
                foreach (var child in ChildNodes)
                {
                    child.DrawDebug();
                }
                return;
            }

            Gizmos.color = GetDebugColor();

            // draw bounds
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }

        public static implicit operator bool(BoundsOctreeNode<T, TH> obj)
        {
            return obj != null;
        }

        public bool IsPopulated
        {
            get { return ChildNodes != null && ChildNodes.Length > 0; }
        }

        public TH Handler { get; set; }

        public int Level { get; set; }

        public T ParentNode { get; set; }

        public T[] ChildNodes { get; set; }
        
        public T[] NeighborChunks { get; set; }

        public abstract Vector3 Position { get; set; }

        public abstract Bounds Bounds { get; set; }
    }
}
