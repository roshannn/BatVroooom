using UnityEngine;

public struct RevButtonPressed {}
public struct RevButtonReleased {}

public struct StartBowling {}

public struct SetBatRotation { public float value; }

public struct LockBatRotation { public bool isLocked; }
public struct LockWheelieRecharge { public bool isLocked; }

public struct WheelieTriggered { public bool isWheelie; }

public struct WheelieRPMChange {
    public float revOffset;
    public float revChangeSpeed;
}



public struct GetBouncePos {
    public float value01;
    public CircleCollider2D ballCollider;
}