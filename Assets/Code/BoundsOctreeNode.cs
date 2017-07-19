
using UnityEngine;

namespace UnityVoxelPlanet
{
    public abstract class BoundsOctreeNode<T, TH> where T : BoundsOctreeNode<T, TH>, IBoundsOctreeNode
    {
        public abstract void OnCreate();

        public abstract void OnDestroy();

        public abstract void OnPopulated();

        public abstract void OnDepopulated();

        public void Populate()
        {
            // TODO: calculate positions
        }

        public void Depopulate()
        {
            
        }

        public void DrawDebug()
        {
            if (ChildNodes != null)
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

        public TH Handler { get; set; }

        public int Level { get; set; }

        public T ParentNode { get; set; }

        public T[] ChildNodes { get; set; }
        
        public abstract Vector3 Position { get; set; }

        public abstract Bounds Bounds { get; set; }
    }
}
