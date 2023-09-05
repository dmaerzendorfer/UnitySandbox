using UnityEngine;


[CreateAssetMenu(fileName = "PlayerStats", menuName="PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float rotationFactorPerFrame = 15f;
    public float runMultiplier = 3f;
    public float gravity = -0.8f;
    public float maxJumpTime = 0.5f;
    public float maxJumpHeight = 1.0f;
    public float fallMultiplier = 2f;
    public float comboJumpTimeFrame = 0.5f;
    public float movementSpeed = 5f;

    public float maxFallSpeed = -20f;
    //todo: make these jump time and height params a animation curve in the editor -> also the fall multiplier!

}