using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass
{
    /// <summary>
    /// Sets up the initial inventory for Episode 1
    /// </summary>
    public class Episode1InventorySetup : MonoBehaviour
    {
        void Start()
        {
            SetupInitialInventory();
        }

        void SetupInitialInventory()
        {
            var gameManager = AdventureGameManager.Instance;
            if (gameManager == null) return;

            // Agency ID Badge
            var idBadge = new Item
            {
                id = "agency_id_badge",
                name = "Agency ID Badge",
                description = "Official Crimson Compass identification. Required for security clearance.",
                icon = Resources.Load<Sprite>("Icons/id_badge")
            };
            gameManager.AddToInventory(idBadge);

            // Digital Tablet
            var tablet = new Item
            {
                id = "digital_tablet",
                name = "Digital Tablet",
                description = "Multi-purpose device for data analysis, communication, and scanning.",
                icon = Resources.Load<Sprite>("Icons/tablet")
            };
            gameManager.AddToInventory(tablet);

            // Encrypted Communicator
            var communicator = new Item
            {
                id = "encrypted_communicator",
                name = "Encrypted Communicator",
                description = "Secure communication device for contacting team members and allies.",
                icon = Resources.Load<Sprite>("Icons/communicator")
            };
            gameManager.AddToInventory(communicator);

            Debug.Log("Episode 1 initial inventory setup complete");
        }
    }
}