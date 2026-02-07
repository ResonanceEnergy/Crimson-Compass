using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;
using UnityEngine.SceneManagement;

public class GameTestRunner : MonoBehaviour
{
    void Start()
    {
        // Wait a frame for everything to initialize
        StartCoroutine(RunTests());
    }

    IEnumerator RunTests()
    {
        yield return null;

        Debug.Log("=== CRIMSON COMPASS SYSTEM TEST ===");

        // Test 1: Check if GameManager exists
        if (GameManager.Instance == null)
        {
            Debug.LogError("FAIL: GameManager.Instance is null");
            yield break;
        }
        Debug.Log("PASS: GameManager initialized");

        // Test 2: Check if all managers are present
        if (GameManager.Instance.seasonManager == null)
        {
            Debug.LogError("FAIL: SeasonManager not found");
            yield break;
        }
        Debug.Log("PASS: SeasonManager found");

        if (GameManager.Instance.agentManager == null)
        {
            Debug.LogError("FAIL: AgentManager not found");
            yield break;
        }
        Debug.Log("PASS: AgentManager found");

        if (GameManager.Instance.saveManager == null)
        {
            Debug.LogError("FAIL: SaveManager not found");
            yield break;
        }
        Debug.Log("PASS: SaveManager found");

        // Test 3: Check if EventBus works
        bool eventReceived = false;
        GameManager.Instance.eventBus.Subscribe(GameEventType.TEST_EVENT, (payload) => { eventReceived = true; });
        GameManager.Instance.eventBus.Publish(GameEventType.TEST_EVENT, null);

        yield return null;

        if (!eventReceived)
        {
            Debug.LogError("FAIL: EventBus not working");
            yield break;
        }
        Debug.Log("PASS: EventBus working");

        // Test 4: Try to start a new game
        GameManager.Instance.StartNewGame();
        yield return new WaitForSeconds(0.5f);

        Debug.Log("PASS: New game started");

        // Test 5: Check if episode loaded
        if (string.IsNullOrEmpty(GameManager.Instance.seasonManager?.CurrentEpisodeId))
        {
            Debug.LogError("FAIL: Episode not loaded");
            yield break;
        }
        Debug.Log("PASS: Episode loaded successfully");

        Debug.Log("=== ALL TESTS PASSED - CRIMSON COMPASS IS PLAYABLE! ===");
        Debug.Log("Current episode: " + GameManager.Instance.seasonManager.CurrentEpisodeId);
        Debug.Log("Current scene: " + GameManager.Instance.seasonManager.CurrentSceneId);
    }
}