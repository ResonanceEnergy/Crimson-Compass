using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using CrimsonCompass.Core;

public class GameManagerTests
{
    [UnityTest]
    public IEnumerator GameManager_InitializesCorrectly()
    {
        // Arrange
        var gameObject = new GameObject();
        var gameManager = gameObject.AddComponent<GameManager>();

        // Act
        GameManager.Instance = gameManager;
        gameManager.eventBus = new EventBus();

        yield return null; // Wait a frame

        // Assert
        Assert.IsNotNull(GameManager.Instance);
        Assert.IsNotNull(gameManager.eventBus);
    }

    [UnityTest]
    public IEnumerator EventBus_PublishesAndSubscribes()
    {
        // Arrange
        var eventBus = new EventBus();
        bool eventReceived = false;

        // Act
        eventBus.Subscribe(GameEventType.TEST_EVENT, (payload) => { eventReceived = true; });
        eventBus.Publish(GameEventType.TEST_EVENT, null);

        yield return null;

        // Assert
        Assert.IsTrue(eventReceived);
    }
}