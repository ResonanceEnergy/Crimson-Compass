using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass
{
    /// <summary>
    /// Simple scene setup for testing Episode 1 Agency Briefing Room
    /// Creates basic GameObjects and assigns components
    /// </summary>
    public class Episode1SceneTestSetup : MonoBehaviour
    {
        void Awake()
        {
            CreateSceneObjects();
            SetupManagers();
        }

        void CreateSceneObjects()
        {
            // Create Desk Terminal
            GameObject deskTerminal = new GameObject("DeskTerminal");
            deskTerminal.transform.position = new Vector3(-2f, 0f, 0f);
            deskTerminal.AddComponent<BoxCollider2D>();
            deskTerminal.AddComponent<SpriteRenderer>().color = Color.blue;

            // Create Filing Cabinet
            GameObject filingCabinet = new GameObject("FilingCabinet");
            filingCabinet.transform.position = new Vector3(2f, 0f, 0f);
            filingCabinet.AddComponent<BoxCollider2D>();
            filingCabinet.AddComponent<SpriteRenderer>().color = Color.gray;

            // Create Coffee Machine
            GameObject coffeeMachine = new GameObject("CoffeeMachine");
            coffeeMachine.transform.position = new Vector3(0f, -2f, 0f);
            coffeeMachine.AddComponent<BoxCollider2D>();
            coffeeMachine.AddComponent<SpriteRenderer>().color = Color.red;

            // Create Door
            GameObject door = new GameObject("DoorToHallway");
            door.transform.position = new Vector3(0f, 2f, 0f);
            door.AddComponent<BoxCollider2D>();
            door.AddComponent<SpriteRenderer>().color = Color.green;

            // Create NPCs
            CreateNPC("Helix", new Vector3(-3f, 1f, 0f), Color.cyan);
            CreateNPC("Optimus", new Vector3(3f, 1f, 0f), Color.yellow);
            CreateNPC("ZTech", new Vector3(-3f, -1f, 0f), Color.magenta);
            CreateNPC("Gasket", new Vector3(3f, -1f, 0f), Color.white);

            // Create Player Character (placeholder)
            GameObject player = new GameObject("Player");
            player.transform.position = new Vector3(0f, -3f, 0f);
            player.AddComponent<SpriteRenderer>().color = Color.black;
            player.tag = "Player";
        }

        void CreateNPC(string name, Vector3 position, Color color)
        {
            GameObject npc = new GameObject(name);
            npc.transform.position = position;
            npc.AddComponent<BoxCollider2D>();
            npc.AddComponent<SpriteRenderer>().color = color;
            npc.AddComponent<NPC>().npcName = name;
        }

        void SetupManagers()
        {
            // Ensure AdventureGameManager exists
            if (AdventureGameManager.Instance == null)
            {
                GameObject manager = new GameObject("AdventureGameManager");
                manager.AddComponent<AdventureGameManager>();
            }

            // Add scene setup components
            gameObject.AddComponent<AgencyBriefingRoomSetup>();
            gameObject.AddComponent<Episode1InventorySetup>();
        }
    }
}