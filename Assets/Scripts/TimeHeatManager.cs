using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    public class TimeHeatManager : MonoBehaviour
    {
        public int currentTime;
        public int currentHeat;
        public int maxTime;
        public int maxHeat = 10;

        void Start()
        {
            GameManager.Instance.eventBus.Subscribe(GameEventType.SESSION_OPEN, OnSessionOpen);
            GameManager.Instance.eventBus.Subscribe(GameEventType.HYPOTHESIS_SUBMITTED, OnHypothesisSubmitted);
        }

        void OnSessionOpen(object payload)
        {
            maxTime = GameManager.Instance.currentCase.timeBudget;
            currentTime = maxTime;
            currentHeat = 0;
            GameManager.Instance.eventBus.Publish(GameEventType.TIME_CHANGED, currentTime);
            GameManager.Instance.eventBus.Publish(GameEventType.HEAT_CHANGED, currentHeat);
        }

        void OnHypothesisSubmitted(object payload)
        {
            // Cost time for hypothesis
            currentTime -= 1;
            if (currentTime <= 0)
            {
                // Mission failed
                GameManager.Instance.eventBus.Publish(GameEventType.MISSION_COMPLETED, false);
            }
            else
            {
                GameManager.Instance.eventBus.Publish(GameEventType.TIME_CHANGED, currentTime);
            }
        }

        public void UseHint()
        {
            // Cost from hint
            var cost = GameManager.Instance.currentCase.hintCost;
            currentTime -= cost.timeHours;
            currentHeat += cost.heat;
            if (currentHeat >= maxHeat)
            {
                // Too hot, mission failed
                GameManager.Instance.eventBus.Publish(GameEventType.MISSION_COMPLETED, false);
            }
            GameManager.Instance.eventBus.Publish(GameEventType.TIME_CHANGED, currentTime);
            GameManager.Instance.eventBus.Publish(GameEventType.HEAT_CHANGED, currentHeat);
        }
    }
}