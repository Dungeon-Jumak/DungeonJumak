using UnityEngine;

namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Stores recognition result data.
    /// </summary>
    public struct RecognizeResult<G> where G : IGesturePattern {
        static readonly float _sqrt2 = Mathf.Sqrt(2);

        public RecognizeResult(float distance, G pattern) {
            Score = 1 - distance / (0.5 * _sqrt2); ;
            Pattern = pattern;
        }

        /// <summary>
        /// Similarity score of pattern and gesture.
        /// A score is a value in the range of 0 to 1.
        /// Where 1 means that the recorded path is exactly the same as the pattern path.
        /// You can set the required precision value for the results.
        /// 0.7 is a good choice, but you can choose another value.
        /// </summary>
        public double Score { get; private set; }

        /// <summary>
        /// Recognized pattern.
        /// </summary>
        public G Pattern { get; private set; }
    }
}