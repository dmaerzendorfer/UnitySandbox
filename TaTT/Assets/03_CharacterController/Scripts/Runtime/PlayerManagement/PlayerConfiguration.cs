using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        Input = playerInput;
        PlayerIndex = playerInput.playerIndex;
    }

    public PlayerStats Stats { get; set; }
    public PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}