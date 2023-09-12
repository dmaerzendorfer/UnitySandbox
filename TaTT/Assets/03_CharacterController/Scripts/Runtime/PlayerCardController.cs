using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCardController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private Image colorImage;
    [SerializeField] private GameObject waitingUi;
    [SerializeField] private GameObject joinedUi;
    public List<Material> availablePlayerColors;
    private int _currentColorIndex = 0;

    private PlayerControls _controls;

    public int CurrentColorIndex
    {
        get { return _currentColorIndex; }
        set
        {
            var n = availablePlayerColors.Count;
            //make sure the index wraps around the colors list in both directions
            _currentColorIndex = ((value % n) + n) % n;
            colorImage.material = availablePlayerColors[_currentColorIndex];
        }
    }

    private int _playerIndex;

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set
        {
            Debug.Log("Set playerIndex");
            _playerIndex = value;
            IsWaiting = false;

            titleText.text = "Player " + (_playerIndex + 1);

            //subscribe to new playercontrols etc
            var playerConfigManager =
                PlayerConfigurationManager.Instance;
            playerConfigManager.PlayerConfigs[_playerIndex].PlayerMaterial =
                availablePlayerColors[_currentColorIndex];
            playerConfigManager.PlayerConfigs[_playerIndex].Input.onActionTriggered += OnActionTriggered;
        }
    }

    private bool _isWaiting = true;

    public bool IsWaiting
    {
        get { return _isWaiting; }
        set
        {
            _isWaiting = value;
            if (_isWaiting)
            {
                waitingUi.SetActive(true);
                joinedUi.SetActive(false);
            }
            else
            {
                waitingUi.SetActive(false);
                joinedUi.SetActive(true);
            }
        }
    }

    private void Start()
    {
        _controls = new PlayerControls();
        colorImage.material = availablePlayerColors[_currentColorIndex];
    }

    private void OnDestroy()
    {
        if (PlayerConfigurationManager.Instance.PlayerConfigs.Count >= _playerIndex + 1)
            PlayerConfigurationManager.Instance.PlayerConfigs[_playerIndex].Input.onActionTriggered -=
                OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action.name == _controls.CharacterControls.Move.name && obj.action.phase == InputActionPhase.Started)
        {
            //move action -> use this for changing color
            //if move right increase index otherwise decrease
            var val = obj.ReadValue<Vector2>();
            if (val.x > 0)
            {
                CurrentColorIndex++;
            }
            else if (val.x < 0)
            {
                CurrentColorIndex--;
            }
        }
        else if (obj.action.name == _controls.CharacterControls.Jump.name &&
                 obj.action.phase == InputActionPhase.Started)
        {
            //jump button -> use this for ready
            //if pressed ready up
            PlayerConfigurationManager.Instance.PlayerConfigs[_playerIndex].IsReady ^= true;
            if (PlayerConfigurationManager.Instance.PlayerConfigs[_playerIndex].IsReady)
            {
                readyText.text = "Ready";
            }
            else
            {
                readyText.text = "Press A to ready up";
            }
        }
    }
}