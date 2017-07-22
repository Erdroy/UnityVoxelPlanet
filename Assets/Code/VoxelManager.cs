
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelManager class.
    /// </summary>
    public class VoxelManager : MonoBehaviour
    {
        /// <summary>
        /// Current set of VoxelPlanets.
        /// </summary>
        public VoxelPlanet[] VoxelPlanets;

        // private
        private void Start()
        {
            Instance = this;
            VoxelProcessor.Start();
        }

        // private
        private void Update()
        {
            VoxelProcessor.Dispatch();
        }

        // private
        private void OnDestroy()
        {
            VoxelProcessor.Shutdown();
        }

        /// <summary>
        /// VoxelManager current instance.
        /// </summary>
        public static VoxelManager Instance { get; private set; }
    }
}