
using UnityEngine;

namespace UnityVoxelPlanet
{
    public abstract class BoundsOctreeNode<T, TH> where T : BoundsOctreeNode<T, TH>, IBoundsOctreeNode
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
            if (IsPopulated)
            {
                Debug.LogWarning("Cannot populate! Node is already populated or not cleaned properly.");
                return;
            }

            // TODO: calculate positions

        }

        public void Depopulate()
        {
            if (!IsPopulated)
            {
                Debug.LogWarning("Cannot depopulate! Node is not populated.");
                return;
            }

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
        
        public abstract Vector3 Position { get; set; }

        public abstract Bounds Bounds { get; set; }
    }
}
