using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// <inheritdoc cref="GestureRecognizerBase"/>
    /// Performs gesture recognition on a set of patterns.
    /// Designed for recognizing pattern with same type: only uniforms or only un uniforms.
    /// Use it if you have patterns of one type in your set.
    /// It is slightly performant than <see cref="GestureRecognizer{G}"/>.
    /// </summary>
    /// <typeparam name="G">Gesture pattern</typeparam>
    public sealed class SpecificGestureRecognizer<G> : GestureRecognizerBase<G> where G : IGesturePattern {
        private readonly List<G> _patterns;

        private readonly bool _isUniform;

        private NativeArray<float2> _resampledPatternsBuffer;
        private NativeArray<float2> _resampleBuffer;
        private NativeArray<float> _distanceBuffer;

        public override RecognizeResult<G> Result => FindBestDistanceResult();

        /// <summary>
        /// Pass your set of patterns to the recognizer constructor.
        /// You can also choose the number of points to resample.
        /// A higher value gives more accurate results but takes longer to process.
        /// 128 is the default and gives good results, but you can choose a better value for your pattern set.
        /// </summary>
        /// <param name="patterns">Set of patterns</param>
        /// <param name="resamplePointsNumber">Number of points to resample</param>
        public SpecificGestureRecognizer(IEnumerable<G> patterns, int resamplePointsNumber = 128, GestureScalingMode scalingMode = GestureScalingMode.Uniform) 
        : base(resamplePointsNumber) {
            _patterns = patterns.ToList();

            _isUniform = scalingMode == GestureScalingMode.Uniform;

            int patternsNumber = _patterns.Count;
            _resampledPatternsBuffer = new NativeArray<float2>(_ResamplePointsNumber * patternsNumber, Allocator.Persistent);

            _resampleBuffer = new NativeArray<float2>(_ResamplePointsNumber, Allocator.Persistent);
            _distanceBuffer = new NativeArray<float>(patternsNumber, Allocator.Persistent);

            PreparePatterns(_patterns, _resampledPatternsBuffer);
        }

        public override RecognizeResult<G> Recognize(NativeSlice<float2> path, bool runParallel = true) {
            ScheduleRecognition(path, runParallel).Complete();
            return FindBestDistanceResult();
        }

        public override JobHandle ScheduleRecognition(NativeSlice<float2> path, bool runParallel = true) {
            var tmpPathBuffer = new NativeArray<float2>(path.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            for (int o = 0; o < path.Length; o++) {
                tmpPathBuffer[o] = path[o];
            }

            var prepareJobHandle = new PreparePathJob {
                isUniformScaling = _isUniform,
                writeBuffer = _resampleBuffer,
                readBuffer = tmpPathBuffer,
            }.Schedule();

            var job = new ParallelCompareJob {
                patternPaths = _resampledPatternsBuffer,
                distanceBuffer = _distanceBuffer,
                gesturePath = _resampleBuffer
            };

            if (runParallel)
                return job.ScheduleParallel(_patterns.Count, 1, prepareJobHandle);
            return job.Schedule(_patterns.Count, prepareJobHandle);
        }

        public override void Dispose() {
            _resampledPatternsBuffer.Dispose();
            _resampleBuffer.Dispose();
            _distanceBuffer.Dispose();
        }

        private RecognizeResult<G> FindBestDistanceResult() {
            var (distance, index) = FindBestDistance(_distanceBuffer);
            return new RecognizeResult<G>(distance, _patterns[index]);
        }
    }
}