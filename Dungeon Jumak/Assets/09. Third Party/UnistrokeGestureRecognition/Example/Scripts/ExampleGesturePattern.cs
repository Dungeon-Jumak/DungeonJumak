using System;
using UnistrokeGestureRecognition;
using UnityEngine;

namespace UnistrokeGestureRecognition.Example {
    // You can add "CreateAssetMenu" attribute 
    public class ExampleGesturePattern : GesturePatternBase {
        // This new class will work with еру editor and inspector 
        // Add your data here
        
        [SerializeField]
        private string _name;

        public string Name => _name;
    }
}