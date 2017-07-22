
using MyHalp;
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelPlanet class.
    /// </summary>
    public class VoxelPlanet : MyComponent
    {
        /// <summary>
        /// The planet radius in units/meters.
        /// </summary>
        [Tooltip("Must be multiple of 16!")]
        public float Radius = 8192;

        /// <summary>
        /// The initial VoxelOctree size.
        /// </summary>
        public int InitialOctreeSize = 8;

        public Material DefaultMaterial;

        public bool DrawDebug = true;

        // private
        private BoundsOctree<VoxelPlanetChunk, VoxelPlanet> _octree;
        
        /// <summary>
        /// Called when this component is initialized.
        /// </summary>
        protected override void OnInit()
        {
            Position = MyTransform.position;

            var nodeSize = Radius * 2.0f / InitialOctreeSize;

            _octree = new BoundsOctree<VoxelPlanetChunk, VoxelPlanet>
            {
                Position = Position,
                BaseNodeSize = new Vector3(nodeSize, nodeSize, nodeSize)
            };

            _octree.Init(InitialOctreeSize, this);
        }

        /// <summary>
        /// Called when this component is updated.
        /// </summary>
        protected override void OnTick()
        {
            if (CameraController.Current == null)
            {
                Debug.LogWarning("There is no any camera controller.");
                return;
            }

            var chunks = _octree.GetChildNodes();

            // update all chunks
            foreach (var chunk in chunks)
            {
                chunk.Update(CameraController.Current.GetPosition());
            }
        }

        // private
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying || !DrawDebug)
                return;

            _octree.DrawDebug();
        }

        /// <summary>
        /// The planet world-space position.
        /// </summary>
        public Vector3 Position { get; set; }
    }
}
