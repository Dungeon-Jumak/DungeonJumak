using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Job for comparing path with patterns.
    /// </summary>
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    internal struct ParallelCompareJob : IJobFor {
        /// <summary>
        /// Flatten buffer of patterns paths.
        /// </summary>
        [ReadOnly] public NativeArray<float2> patternPaths;

        /// <summary>
        /// Buffer of gesture path.
        /// </summary>
        [ReadOnly] public NativeArray<float2> gesturePath;

        /// <summary>
        /// Compare results buffer. 
        /// </summary>
        [WriteOnly] public NativeArray<float> distanceBuffer;

        public void Execute(int index) {
            int pointsNumber = gesturePath.Length;

            float distance = 0;
            for (int i = 0; i < pointsNumber; i++) {
                distance += math.distance(patternPaths[i + pointsNumber * index], gesturePath[i]);
            }

            distanceBuffer[index] = distance / pointsNumber;
        }
    }
}