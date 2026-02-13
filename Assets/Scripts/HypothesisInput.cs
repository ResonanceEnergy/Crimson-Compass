using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.Agents;
using CrimsonCompass.Core;

public class HypothesisInput : MonoBehaviour
    {
        public TMP_Dropdown whoDropdown;
        public TMP_Dropdown howDropdown;
        public TMP_Dropdown whereDropdown;
        public Button submitButton;

        void Start()
        {
            submitButton.onClick.AddListener(SubmitHypothesis);
            PopulateDropdowns();
        }

        void PopulateDropdowns()
        {
            if (GameManager.Instance == null || GameManager.Instance.currentCase == null)
            {
                UnityEngine.Debug.Log("Case not loaded yet, retrying...");
                Invoke("PopulateDropdowns", 0.1f);
                return;
            }
            var caseData = GameManager.Instance.currentCase;
            whoDropdown.ClearOptions();
            whoDropdown.options.Add(new TMP_Dropdown.OptionData("Select WHO"));
            foreach (var s in caseData.suspects) whoDropdown.options.Add(new TMP_Dropdown.OptionData(s.name));

            howDropdown.ClearOptions();
            howDropdown.options.Add(new TMP_Dropdown.OptionData("Select HOW"));
            foreach (var m in caseData.methods) howDropdown.options.Add(new TMP_Dropdown.OptionData(m.name));

            whereDropdown.ClearOptions();
            whereDropdown.options.Add(new TMP_Dropdown.OptionData("Select WHERE"));
            foreach (var l in caseData.locations) whereDropdown.options.Add(new TMP_Dropdown.OptionData(l.id));
        }

        void SubmitHypothesis()
        {
            if (whoDropdown.value == 0 || howDropdown.value == 0 || whereDropdown.value == 0) return;

            var hypothesis = new Hypothesis
            {
                whoId = GameManager.Instance.currentCase.suspects[whoDropdown.value - 1].id,
                howId = GameManager.Instance.currentCase.methods[howDropdown.value - 1].id,
                whereId = GameManager.Instance.currentCase.locations[whereDropdown.value - 1].id
            };

            UnityEngine.Debug.Log("HypothesisInput submitting: WHO=" + hypothesis.whoId + ", HOW=" + hypothesis.howId + ", WHERE=" + hypothesis.whereId);
            GameManager.Instance.eventBus.Publish(GameEventType.HYPOTHESIS_SUBMITTED, hypothesis);
        }
    }