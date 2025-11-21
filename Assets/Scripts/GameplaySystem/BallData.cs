using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Settings/BallData",fileName = "BallData")]
public class BallData : ScriptableObject
{
    public float length;
    public float time;
    public float dotProductIncrement;
    public float wheelieSpeedMultiplier;
    public PhysicsMaterial2D pitchMaterial;
}
