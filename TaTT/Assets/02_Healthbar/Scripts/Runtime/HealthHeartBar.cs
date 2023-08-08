using UnityEngine;

[ExecuteInEditMode]
public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;

    public void DisplayHearts(int count)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < count; i++)
        {
            var newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(transform);
        }
    }
}