using DG.Tweening;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMoveY(20, 1.5f).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo).SetDelay(0.5f);
    }

}
