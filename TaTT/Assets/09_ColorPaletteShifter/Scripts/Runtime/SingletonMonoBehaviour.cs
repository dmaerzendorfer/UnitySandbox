using UnityEngine;

namespace _09_ColourPaletteShifter.Scripts.Runtime
{
    [ExecuteInEditMode]
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this.GetComponent<T>();
            }
        }
    }
}