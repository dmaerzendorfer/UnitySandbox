using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> _playerConfigs;

    public List<PlayerConfiguration> PlayerConfigs
    {
        get { return _playerConfigs; }
    }

    [SerializeField] private int maxPlayers = 4;
    public string sceneName = "SamlpleScene";
    public static PlayerConfigurationManager Instance { get; private set; }

    private PlayerCardsManager _lobbyPlayerCardsPanel;

    public UnityEvent<int> onPlayerJoin;

    private void Awake()
    {
        _lobbyPlayerCardsPanel = GameObject.Find("PlayerCards").GetComponent<PlayerCardsManager>();

        if (Instance != null)
        {
            Debug.Log("trying to create another instance of playerconfigurationManager!");
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            _playerConfigs = new List<PlayerConfiguration>();
            onPlayerJoin = new UnityEvent<int>();
        }
    }

    public PlayerConfiguration GetPlayerConfig(int index)
    {
        return _playerConfigs[index];
    }

    public void SetPlayerColor(int index, Material material)
    {
        _playerConfigs[index].PlayerMaterial = material;
    }

    public void ReadyPlayer(int index)
    {
        _playerConfigs[index].IsReady ^= true;
        if (_playerConfigs.Count == maxPlayers && _playerConfigs.All(p => p.IsReady))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void HandlePlayerJoin(UnityEngine.InputSystem.PlayerInput playerInput)
    {
        Debug.Log("Player joined " + playerInput.playerIndex);
        if (!_playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            playerInput.transform.SetParent(transform);
            _playerConfigs.Add(new PlayerConfiguration(playerInput));
            onPlayerJoin.Invoke(playerInput.playerIndex);
        }
    }
}

//todo make this a scriptable object to set default material
public class PlayerConfiguration
{
    public PlayerConfiguration(UnityEngine.InputSystem.PlayerInput playerInput)
    {
        Input = playerInput;
        PlayerIndex = playerInput.playerIndex;
    }

    public UnityEngine.InputSystem.PlayerInput Input { get; set; }
    public int PlayerIndex { get; set; }
    public bool IsReady { get; set; }
    public Material PlayerMaterial { get; set; }
}