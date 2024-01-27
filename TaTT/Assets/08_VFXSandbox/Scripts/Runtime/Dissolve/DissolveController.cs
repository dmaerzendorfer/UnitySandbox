using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    private const string DissolveAmount = "_DissolveAmount";
    private static readonly int DissolveAmountId = Shader.PropertyToID(DissolveAmount);

    private const string FlipAnimationTrigger = "Flip";
    private static readonly int FlipId = Animator.StringToHash(FlipAnimationTrigger);


    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;

    public Animator animator;

    public VisualEffect vfx;

    private Material[] _skinnedMaterials;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
            _skinnedMaterials = skinnedMesh.materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DissolveCo());
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //reset
            StopAllCoroutines();
            for (int i = 0; i < _skinnedMaterials.Length; i++)
            {
                _skinnedMaterials[i].SetFloat(DissolveAmountId, 0);
            }
            vfx.Stop();
        }
    }

    private IEnumerator DissolveCo()
    {
        if (animator)
        {
            animator.SetTrigger(FlipId);
        }

        if (vfx != null)
        {
            vfx.Play();
        }

        float counter = 0;
        if (_skinnedMaterials.Length > 0)
        {
            while (_skinnedMaterials[0].GetFloat(DissolveAmountId) < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < _skinnedMaterials.Length; i++)
                {
                    _skinnedMaterials[i].SetFloat(DissolveAmountId, counter);
                }

                yield return new WaitForSeconds(refreshRate);
            }
        }
    }
}