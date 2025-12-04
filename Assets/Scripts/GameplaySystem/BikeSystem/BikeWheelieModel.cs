using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BikeWheelieModel
{
	public float minRpm;
	public float maxRpm;
    public float rpmIncreaseRate;
    public float rpmDecayRate;
    public float maxWheelieAngle;
	public float wheelieRotationSpeed;
	public float wheelieRotationDecay;

    public WheelieSoundPitchData wheelieSoundPitchData;
}

[System.Serializable]
public class WheelieSoundPitchData {
    public float wheeliePitchIncreaseOffset;
    public float wheeliePitchIncreaseSpeed;
    public float wheeliePitchResetOffset;
    public float wheeliePitchResetSpeed;
}
