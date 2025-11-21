using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName="Settings/BallDataContainer",fileName = "BallDataContainer")]
public class BallDataContainer : ScriptableObject
{
	public List<BallDataHolder> ballData;

	public BallDataHolder GetBallData(BallType ballType) {
		return ballData.Find(x => x.ballType == ballType);
	}
}
[Serializable]
public class BallDataHolder {
	public BallType ballType;
	public BallData ballData;
}
