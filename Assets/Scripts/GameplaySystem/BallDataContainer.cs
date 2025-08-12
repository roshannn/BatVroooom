using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Settings/BallData",fileName = "BallData")]
public class BallDataContainer : ScriptableObject
{
	public List<BallData> ballData;

	public BallData GetBallData(BallType ballType) {
		return ballData.Find(x => x.ballType == ballType);
	}
}
[Serializable]
public class BallData {
	public BallType ballType;
	public float length;
	public float speed;
	public float time;
}
