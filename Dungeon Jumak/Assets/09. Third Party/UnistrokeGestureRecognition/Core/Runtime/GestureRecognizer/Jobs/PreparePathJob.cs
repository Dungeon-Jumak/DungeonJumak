using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Job for performing path preparation 
    /// (resampling and normalizing). 
    /// </summary>
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    internal struct PreparePathJob : IJob {
        /// <summary>
        /// Initial path buffer.
        /// </summary>
        [DeallocateOnJobCompletion]
        [ReadOnly] public NativeArray<float2> readBuffer;

        /// <summary>
        /// Scaling strategy.
        /// </summary>
        [ReadOnly] public bool isUniformScaling;

        /// <summary>
        /// Prepared path buffer.
        /// </summary>
        public NativeArray<float2> writeBuffer;

        public void Execute() {
            GestureMath.ResampleAndNormalizePath(readBuffer, writeBuffer, isUniformScaling);
        }
    }
}