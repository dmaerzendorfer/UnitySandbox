using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCardController : MonoBehaviour
{
    public List<Material> availablePlayerColors;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private Image colorImage;
    [SerializeField] private GameObject waitingUi;
    [SerializeField] private GameObject joinedUi;

    private int _currentColorIndex = 0;

    private PlayerControls _controls;

    //to make sure players that join with a "A" button press dont ready up immediately
    private float _ignoreInputTime = 0.5f;

    public int CurrentColorIndex
    {
        get { return _currentColorIndex; }
        set
        {
            var n = availablePlayerColors.Count;
            //make sure the index wraps around the colors list in both directions
            _currentColorIndex = ((value % n) + n) % n;
            colorImage.color = availablePlayerColors[_currentColorIndex].color;
        }
    }

    private int _playerIndex;

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set
        {
            _playerIndex = value;
            IsWaiting = false;

            titleText.text = "Player " + (_playerIndex + 1);

            _ignoreInputTime = Time.time + _ignoreInputTime;

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
        colorImage.color = availablePlayerColors[_currentColorIndex].color;
    }

    private void OnDestroy()
    {
        if (PlayerConfigurationManager.Instance.PlayerConfigs.Count >= _playerIndex + 1)
            PlayerConfigurationManager.Instance.PlayerConfigs[_playerIndex].Input.onActionTriggered -=
                OnActionTriggered;
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (Time.time < _ignoreInputTime)
        {
            //dont allow interaction yet
            return;
        }

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
        else if (obj.action.name == _controls.CharacterControls.Interact.name &&
                 obj.action.phase == InputActionPhase.Started)
        {
            //on interaction button press cycle color one further
            CurrentColorIndex++;
        }
        else if (obj.action.name == _controls.CharacterControls.Jump.name &&
                 obj.action.phase == InputActionPhase.Started)
        {
            //jump button -> use this for ready
            //if pressed ready up
            PlayerConfigurationManager.Instance.ReadyPlayer(_playerIndex);
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