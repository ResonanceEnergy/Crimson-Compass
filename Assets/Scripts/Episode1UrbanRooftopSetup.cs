using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass
{
    /// <summary>
    /// Scene setup for Episode 1 Urban Rooftop
    /// Creates rooftop environment with surveillance equipment and city views
    /// </summary>
    public class Episode1UrbanRooftopSetup : MonoBehaviour
    {
        void Awake()
        {
            CreateSceneObjects();
            SetupManagers();
        }

        void CreateSceneObjects()
        {
            // Create Surveillance Camera
            GameObject camera = new GameObject("SurveillanceCamera");
            camera.transform.position = new Vector3(-2f, 1f, 0f);
            camera.AddComponent<BoxCollider2D>();
            camera.AddComponent<SpriteRenderer>().color = Color.black;

            // Create Antenna Array
            GameObject antenna = new GameObject("AntennaArray");
            antenna.transform.position = new Vector3(2f, 1f, 0f);
            antenna.AddComponent<BoxCollider2D>();
            antenna.AddComponent<SpriteRenderer>().color = Color.gray;

            // Create Ventilation Shaft
            GameObject vent = new GameObject("VentilationShaft");
            vent.transform.position = new Vector3(0f, -1f, 0f);
            vent.AddComponent<BoxCollider2D>();
            vent.AddComponent<SpriteRenderer>().color = Color.gray;

            // Create City View (background element)
            GameObject cityView = new GameObject("CitySkyline");
            cityView.transform.position = new Vector3(0f, 3f, 0f);
            cityView.AddComponent<SpriteRenderer>().color = Color.blue;

            // Create Access Ladder
            GameObject ladder = new GameObject("AccessLadder");
            ladder.transform.position = new Vector3(-3f, 0f, 0f);
            ladder.AddComponent<BoxCollider2D>();
            ladder.AddComponent<SpriteRenderer>().color = new Color(0.6f, 0.4f, 0.2f);

            // Create NPCs or clues
            CreateNPC("SuspiciousFigure", new Vector3(1f, 0f, 0f), Color.red);
        }

        void CreateNPC(string name, Vector3 position, Color color)
        {
            GameObject npc = new GameObject(name);
            npc.transform.position = position;
            npc.AddComponent<BoxCollider2D>();
            npc.AddComponent<SpriteRenderer>().color = color;
            npc.AddComponent<NPC>();
            AddLabel(npc, name);
        }

        void AddLabel(GameObject obj, string text)
        {
            var textObj = new GameObject("Label");
            textObj.transform.SetParent(obj.transform);
            textObj.transform.localPosition = new Vector3(0f, 0.5f, 0f);
            var tmp = textObj.AddComponent<TMPro.TextMeshPro>();
            tmp.text = text;
            tmp.fontSize = 2;
            tmp.alignment = TMPro.TextAlignmentOptions.Center;
        }

        void SetupManagers()
        {
            // Ensure AdventureGameManager exists
            if (FindObjectOfType<AdventureGameManager>() == null)
            {
                GameObject manager = new GameObject("AdventureGameManager");
                manager.AddComponent<AdventureGameManager>();
            }

            // Setup UI if not present
            SetupUI();
        }

        void SetupUI()
        {
            // Create Verb Bar
            if (GameObject.Find("VerbBar") == null)
            {
                GameObject verbBar = new GameObject("VerbBar");
                verbBar.AddComponent<VerbBarUI>();
                // Position at bottom
                verbBar.transform.position = new Vector3(0f, -4f, 0f);
            }

            // Create Inventory UI
            if (GameObject.Find("InventoryUI") == null)
            {
                GameObject inventory = new GameObject("InventoryUI");
                inventory.AddComponent<InventoryUI>();
                inventory.transform.position = new Vector3(0f, -3f, 0f);
            }
        }
    }
}