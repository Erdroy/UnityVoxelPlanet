﻿
using System.Linq;
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

        public void Populate()
        {
            if (!CanPopulate)
                return;

            if (IsPopulated)
            {
                Debug.LogWarning("Cannot populate! Node is already populated or not cleaned properly.");
                return;
            }

            IsPopulated = true;

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
                            ParentNode = (T) this
                        };

                        var node = ChildNodes[idx];
                        node.Level = Level + 1;

                        var position = new MyVector3(x, y, z);

                        position *= size;
                        position += Position.ToVector3();

                        node.Position = Int3.FromVector3(position);
                        node.Bounds = new Bounds
                        {
                            min = position,
                            max = new Vector3(position.X + size.x, position.Y + size.y, position.Z + size.z)
                        };
                        
                        node.OnCreate();
                    }
                }
            }

            // TODO: update neighs

            OnPopulated();
        }

        public void Depopulate()
        {
            if (!CanDepopulate)
                return;

            if (!IsPopulated)
            {
                Debug.LogWarning("Cannot depopulate! Node is not populated.");
                return;
            }
            
            IsPopulated = false;
            
            OnDepopulated();
        }
        
        public void DrawDebug()
        {
            if (IsPopulated && ChildNodes != null)
            {
                foreach (var child in ChildNodes)
                {
                    child.DrawDebug();
                }
                return;
            }
            
            // draw bounds
            Gizmos.DrawWireCube(Bounds.center, Bounds.size);
        }

        public static implicit operator bool(BoundsOctreeNode<T, TH> obj)
        {
            return obj != null;
        }
        
        public bool CanPopulate
        {
            get { return Bounds.size.x * 0.5f > 16.0f; }
        }

        public bool CanDepopulate
        {
            get
            {
                if (ChildNodes == null)
                    return false;

                return IsPopulated && ChildNodes.All(node => !node.IsPopulated);
            }
        }

        public bool IsPopulated { get; set; }

        public TH Handler { get; set; }

        public int Level { get; set; }

        public T ParentNode { get; set; }

        public T[] ChildNodes { get; set; }
        
        public T[] NeighborChunks { get; set; }

        public abstract Int3 Position { get; set; }

        public abstract Bounds Bounds { get; set; }
    }
}

