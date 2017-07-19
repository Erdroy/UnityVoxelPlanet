
using System.Threading;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelProcessor class.
    /// Handles worker threads that does all generating/meshing stuff.
    /// </summary>
    public static class VoxelProcessor
    {
        private static Thread[] _workerThreads;
        private static bool _isRunning;

        /// <summary>
        /// Initializes the VoxelProcessor.
        /// </summary>
        /// <param name="maxThreads"></param>
        public static void Start(int maxThreads = 4)
        {
            _isRunning = true;

            _workerThreads = new Thread[maxThreads];

            for (var i = 0; i < maxThreads; i++)
            {
                _workerThreads[i] = new Thread(VoxelProcessorWorker)
                {
                    IsBackground = true
                };
                _workerThreads[i].Start();
            }
        }

        /// <summary>
        /// Dispatches the outgoing events from the VoxelProcessor worker threads.
        /// </summary>
        public static void Dispatch()
        {
            
        }

        /// <summary>
        /// Shutdowns the VoxelProcessor.
        /// </summary>
        public static void Shutdown()
        {
            _isRunning = false;

            foreach (var thread in _workerThreads)
            {
                thread.Abort();
            }
        }

        // private
        private static void VoxelProcessorWorker()
        {
            while (_isRunning)
            {


                // sleep, no work currently
                Thread.Sleep(10);
            }
        }
    }
}
