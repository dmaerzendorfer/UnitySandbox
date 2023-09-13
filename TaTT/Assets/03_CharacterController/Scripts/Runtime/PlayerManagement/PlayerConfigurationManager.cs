using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerConfigurationManager : MonoBehaviour
{
    [SerializeField] private List<PlayerConfiguration> _playerConfigs;
    public PlayerStats playerStats;

    public List<PlayerConfiguration> PlayerConfigs
    {
        get { return _playerConfigs; }
    }

    [SerializeField] private int maxPlayers = 4;
    public string sceneName = "SampleScene";
    public static PlayerConfigurationManager Instance { get; private set; }

    public UnityEvent<int> onPlayerJoin;

    private void Awake()
    {
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

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        if (!_playerConfigs.Any(p => p.PlayerIndex == playerInput.playerIndex))
        {
            //init new player configs, set their stats etc.
            playerInput.transform.SetParent(transform);
            var playerConfig = new PlayerConfiguration(playerInput);
            playerConfig.Stats = Instantiate(playerStats);
            _playerConfigs.Add(playerConfig);
            onPlayerJoin.Invoke(playerInput.playerIndex);
        }
    }
}