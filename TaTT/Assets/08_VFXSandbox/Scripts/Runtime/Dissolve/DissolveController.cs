using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class DissolveController : MonoBehaviour
{
    private const string DissolveAmount = "_DissolveAmount";
    private static readonly int DissolveAmountId = Shader.PropertyToID(DissolveAmount);

    private const string FlipAnimationTrigger = "Flip";
    private static readonly int FlipId = Animator.StringToHash(FlipAnimationTrigger);

    private const string ParticlesNumber = "ParticlesNumber";


    public SkinnedMeshRenderer skinnedMesh;
    public float dissolveRate = 0.0125f;
    public float refreshRate = 0.025f;
    public float delay = 0.1f;
    
    public Animator animator;

    public VisualEffect vfxDissolve;
    public VisualEffect vfxMesh;

    private Material[] _skinnedMaterials;
    private float startMeshParticleCount;

    // Start is called before the first frame update
    void Start()
    {
        if (skinnedMesh != null)
            _skinnedMaterials = skinnedMesh.materials;

        if (vfxMesh)
        {
            startMeshParticleCount = vfxMesh.GetFloat(ParticlesNumber);
        }
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

            vfxDissolve.Stop();
            vfxMesh.SetFloat(ParticlesNumber, startMeshParticleCount);
        }
    }

    private IEnumerator DissolveCo()
    {
        yield return new WaitForSeconds(delay);
        if (animator)
        {
            animator.SetTrigger(FlipId);
        }

        if (vfxDissolve != null)
        {
            vfxDissolve.Play();
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

                var count = Remap(counter, 0, 1, startMeshParticleCount, 0);
                vfxMesh.SetFloat(ParticlesNumber, count);
                yield return new WaitForSeconds(refreshRate);
            }
        }
    }

    private static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}