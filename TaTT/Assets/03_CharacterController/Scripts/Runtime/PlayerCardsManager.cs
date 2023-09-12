using System.Collections.Generic;
using UnityEngine;

public class PlayerCardsManager : MonoBehaviour
{
    public List<PlayerCardController> playerCards;
    
    private void Start()
    {
        PlayerConfigurationManager.Instance.onPlayerJoin.AddListener(PlayerJoin);
    }

    private void OnDestroy()
    {
        PlayerConfigurationManager.Instance.onPlayerJoin.RemoveListener(PlayerJoin);
    }

    private void PlayerJoin(int index)
    {
        Debug.Log("Player joined " + index);
        playerCards[index].PlayerIndex = index;
    }
}