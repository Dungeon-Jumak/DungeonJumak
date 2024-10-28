using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <inheritdoc cref="IGestureRecognizer"/>
    /// <summary>
    /// Performs gesture recognition on a set of patterns.
    /// Designed for recognizing both types of patterns: Uniforms and un uniforms.
    /// Use it if you have patterns of both types in your set.
    /// </summary>
    /// <typeparam name="G">Gesture pattern</typeparam>
    public sealed class GestureRecognizer<G> : GestureRecognizerBase<G> where G : IGesturePattern {
        private readonly List<G> _uniformPatterns = new();
        private readonly List<G> _unUniformPatterns = new();

        private NativeArray<float2> _unUniformResampledPatternsBuffer;
        private NativeArray<float2> _uniformResampledPatternsBuffer;

        private NativeArray<float2> _unUniformResampleBuffer;
        private NativeArray<float2> _uniformResampleBuffer;

        private NativeArray<float> _unUniformDistanceBuffer;
        private NativeArray<float> _uniformDistanceBuffer;

        public override RecognizeResult<G> Result => FindBestDistanceResult();

        /// <summary>
        /// Pass your set of patterns to the recognizer constructor.
        /// You can also choose the number of points to resample.
        /// A higher value gives more accurate results but takes longer to process.
        /// 128 is the default and gives good results, but you can choose a better value for your pattern set.
        /// </summary>
        /// <param name="patterns">Set of patterns</param>
        /// <param name="resamplePointsNumber">Number of points to resample</param>
        public GestureRecognizer(IEnumerable<G> patterns, int resamplePointsNumber = 128) : base(resamplePointsNumber) {
            foreach (var pattern in patterns) {
                if (pattern.ScalingMode == GestureScalingMode.Uniform) _uniformPatterns.Add(pattern);
                else _unUniformPatterns.Add(pattern);
            }

            _unUniformResampledPatternsBuffer = new NativeArray<float2>(_unUniformPatterns.Count * _ResamplePointsNumber, Allocator.Persistent);
            _uniformResampledPatternsBuffer = new NativeArray<float2>(_uniformPatterns.Count * _ResamplePointsNumber, Allocator.Persistent);

            _unUniformDistanceBuffer = new NativeArray<float>(_unUniformPatterns.Count, Allocator.Persistent);
            _uniformDistanceBuffer = new NativeArray<float>(_uniformPatterns.Count, Allocator.Persistent);

            _unUniformResampleBuffer = new NativeArray<float2>(_ResamplePointsNumber, Allocator.Persistent);
            _uniformResampleBuffer = new NativeArray<float2>(_ResamplePointsNumber, Allocator.Persistent);

            PreparePatterns(_unUniformPatterns, _unUniformResampledPatternsBuffer);
            PreparePatterns(_uniformPatterns, _uniformResampledPatternsBuffer);
        }

        public override RecognizeResult<G> Recognize(NativeSlice<float2> path, bool runParallel = true) {
            ScheduleRecognition(path, runParallel).Complete();
            return FindBestDistanceResult();
        }

        public override JobHandle ScheduleRecognition(NativeSlice<float2> path, bool runParallel = true) {
            var unUniformTmpPathBuffer = new NativeArray<float2>(path.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);
            var uniformTmpPathBuffer = new NativeArray<float2>(path.Length, Allocator.TempJob, NativeArrayOptions.UninitializedMemory);

            for (int o = 0; o < path.Length; o++) {
                var point = path[o];
                unUniformTmpPathBuffer[o] = point;
                uniformTmpPathBuffer[o] = point;
            }

            var unUniformPrepareJobHandle = new PreparePathJob {
                writeBuffer = _unUniformResampleBuffer,
                readBuffer = unUniformTmpPathBuffer,
                isUniformScaling = false,
            }.Schedule();

            var uniformPrepareJobHandle = new PreparePathJob {
                writeBuffer = _uniformResampleBuffer,
                readBuffer = uniformTmpPathBuffer,
                isUniformScaling = true,
            }.Schedule();

            JobHandle unUniformJob;
            JobHandle uniformJob;

            if (runParallel) {
                unUniformJob = new ParallelCompareJob {
                    patternPaths = _unUniformResampledPatternsBuffer,
                    distanceBuffer = _unUniformDistanceBuffer,
                    gesturePath = _unUniformResampleBuffer,
                }.ScheduleParallel(_unUniformPatterns.Count, 1, unUniformPrepareJobHandle);

                uniformJob = new ParallelCompareJob {
                    patternPaths = _uniformResampledPatternsBuffer,
                    distanceBuffer = _uniformDistanceBuffer,
                    gesturePath = _uniformResampleBuffer,
                }.ScheduleParallel(_uniformPatterns.Count, 1, uniformPrepareJobHandle);
            } else {
                unUniformJob = new ParallelCompareJob {
                    patternPaths = _unUniformResampledPatternsBuffer,
                    distanceBuffer = _unUniformDistanceBuffer,
                    gesturePath = _unUniformResampleBuffer,
                }.Schedule(_unUniformPatterns.Count, unUniformPrepareJobHandle);
                uniformJob = new ParallelCompareJob {
                    patternPaths = _uniformResampledPatternsBuffer,
                    distanceBuffer = _uniformDistanceBuffer,
                    gesturePath = _uniformResampleBuffer,
                }.Schedule(_uniformPatterns.Count, uniformPrepareJobHandle);
            }

            return JobHandle.CombineDependencies(unUniformJob, uniformJob);
        }

        public override void Dispose() {
            _unUniformResampledPatternsBuffer.Dispose();
            _uniformResampledPatternsBuffer.Dispose();

            _unUniformResampleBuffer.Dispose();
            _uniformResampleBuffer.Dispose();

            _unUniformDistanceBuffer.Dispose();
            _uniformDistanceBuffer.Dispose();
        }

        private RecognizeResult<G> FindBestDistanceResult() {
            var (unUniformDistance, unUniformIndex) = FindBestDistance(_unUniformDistanceBuffer);
            var (uniformDistance, uniformIndex) = FindBestDistance(_uniformDistanceBuffer);

            if (uniformDistance < unUniformDistance) {
                return new RecognizeResult<G>(uniformDistance, _uniformPatterns[uniformIndex]);
            }

            return new RecognizeResult<G>(unUniformDistance, _unUniformPatterns[unUniformIndex]);
        }
    }
}
