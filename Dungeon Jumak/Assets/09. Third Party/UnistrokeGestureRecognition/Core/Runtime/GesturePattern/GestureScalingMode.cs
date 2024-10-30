namespace UnistrokeGestureRecognition {
    /// <summary>
    /// Define path scaling strategy.
    /// </summary>
    public enum GestureScalingMode {
        /// <summary>
        /// Scale path inside square without shape deformation.
        /// 1D gestures can be scaled with this mode.
        /// </summary>
        Uniform,

        /// <summary>
        /// Scale path inside gesture rectangle. 
        /// Deformation of the shape is possible.
        /// 1D gestures (horizontal or vertical) can't be scaled with this mode.
        /// Good for shape invariant patterns. 
        /// </summary>
        UnUniform
    }
}