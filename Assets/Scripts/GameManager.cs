using UnityEngine;

namespace PlayerSpace
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public InventoryController inventory;

        private void Awake()
        {
            // Asegura que solo haya una instancia del GameManager
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            inventory.Initialize();
        }
    }
}

