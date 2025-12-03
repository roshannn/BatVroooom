using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Settings/BallData", fileName = "BallData")]
public class BallData : ScriptableObject {
    public float length;
    public float time;
    public PhysicsData ballPhysics;
    public PitchPhysicsData pitchPhysics;
    public float dotProductIncrement;
    public float wheelieSpeedMultiplier;

    [System.Serializable]
    public class PhysicsData {
        public float friction;
        public float bounciness;
    }

    [System.Serializable]
    public class PitchPhysicsData : PhysicsData {
        public PhysicsMaterialCombine2D bouncinessCombine;
        public PhysicsMaterialCombine2D frictionCombine;
    }
}