using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetupMenuController : MonoBehaviour
{
    private int _playerIndex;

    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject _readyPanel;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private Button _readyButton;
    
    private float _ignoreInputTime = 1.5f;
    private bool inputEnabled;

    public void SetPlayerIndex(int pi)
    {
        _playerIndex = pi;
        _titleText.SetText("Player " + (pi + 1));
        _ignoreInputTime = Time.time + _ignoreInputTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void SetColor(Material color)
    {
        if (!inputEnabled) return;

        PlayerConfigurationManager.Instance.SetPlayerColor(_playerIndex, color);
        _readyPanel.SetActive(true);
        _readyButton.Select();
        _menuPanel.SetActive(false);
    }

    public void ReadyPlayer()
    {
        if (!inputEnabled) return;
        PlayerConfigurationManager.Instance.ReadyPlayer(_playerIndex);    
        _readyButton.gameObject.SetActive(false);
    }
}