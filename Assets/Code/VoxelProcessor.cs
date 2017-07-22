
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace UnityVoxelPlanet
{
    /// <summary>
    /// VoxelProcessor class.
    /// Handles worker threads that does all generating/meshing stuff.
    /// </summary>
    public static class VoxelProcessor
    {
        private struct QueueEntry
        {
            public Action Processor { get; set; }
            public Action Done { get; set; }
        }

        private static Thread[] _workerThreads;
        private static volatile bool _isRunning;

        private static readonly Queue<QueueEntry> ChunkProcessQueue = new Queue<QueueEntry>();
        private static readonly Queue<Action> OnDone = new Queue<Action>();

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
            lock (OnDone)
            {
                while (OnDone.Count > 0)
                {
                    var action = OnDone.Dequeue();
                    action();
                }
            }
        }

        /// <summary>
        /// Enqueue the chunk processor for processing.
        /// </summary>
        /// <param name="chunkProcessor">The processor method.</param>
        /// <param name="onDone">The method which will be called when the chunk ends processing.</param>
        public static void Enqueue(Action chunkProcessor, Action onDone)
        {
            lock (ChunkProcessQueue)
            {
                ChunkProcessQueue.Enqueue(new QueueEntry
                {
                    Done = onDone,
                    Processor = chunkProcessor
                });
            }
        }

        /// <summary>
        /// Shutdowns the VoxelProcessor.
        /// </summary>
        public static void Shutdown()
        {
            _isRunning = false;

            /*foreach (var thread in _workerThreads)
            {
                thread.Interrupt();
            }*/
            _workerThreads = null;

            Debug.Log("Shutdown all worker threads.");
        }

        // private
        private static void VoxelProcessorWorker()
        {
            while (_isRunning)
            {
                QueueEntry entry;
                lock (ChunkProcessQueue)
                {
                    if (ChunkProcessQueue.Count == 0)
                    {
                        // sleep, no work currently
                        Thread.Sleep(10);
                        continue;
                    }

                    entry = ChunkProcessQueue.Dequeue();
                }

                entry.Processor();

                lock (OnDone) OnDone.Enqueue(entry.Done);
            }
        }
    }
}
