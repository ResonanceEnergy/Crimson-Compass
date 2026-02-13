using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrimsonCompass.Agents;
using CrimsonCompass.Core;

public class HypothesisInput : MonoBehaviour
    {
        public Dropdown whoDropdown;
        public Dropdown howDropdown;
        public Dropdown whereDropdown;
        public Button submitButton;

        void Start()
        {
            if (submitButton != null) submitButton.onClick.AddListener(SubmitHypothesis);
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
            if (whoDropdown != null)
            {
                whoDropdown.ClearOptions();
                whoDropdown.options.Add(new Dropdown.OptionData("Select WHO"));
                if (caseData.suspects != null)
                {
                    foreach (var s in caseData.suspects) whoDropdown.options.Add(new Dropdown.OptionData(s.name));
                }
            }

            if (howDropdown != null)
            {
                howDropdown.ClearOptions();
                howDropdown.options.Add(new Dropdown.OptionData("Select HOW"));
                if (caseData.methods != null)
                {
                foreach (var m in caseData.methods) howDropdown.options.Add(new Dropdown.OptionData(m.name));
            }
            }

            if (whereDropdown != null)
            {
                whereDropdown.ClearOptions();
                whereDropdown.options.Add(new Dropdown.OptionData("Select WHERE"));
                if (caseData.locations != null)
                {
                    foreach (var l in caseData.locations) whereDropdown.options.Add(new Dropdown.OptionData(l.country));
                }
            }
        }

        void SubmitHypothesis()
        {
            if (GameManager.Instance == null || GameManager.Instance.currentCase == null) return;
            if (whoDropdown == null || howDropdown == null || whereDropdown == null) return;
            if (whoDropdown.value == 0 || howDropdown.value == 0 || whereDropdown.value == 0) return;

            var caseData = GameManager.Instance.currentCase;
            if (caseData.suspects == null || caseData.methods == null || caseData.locations == null) return;

            int whoIndex = whoDropdown.value - 1;
            int howIndex = howDropdown.value - 1;
            int whereIndex = whereDropdown.value - 1;

            if (whoIndex < 0 || whoIndex >= caseData.suspects.Length ||
                howIndex < 0 || howIndex >= caseData.methods.Length ||
                whereIndex < 0 || whereIndex >= caseData.locations.Length) return;

            var hypothesis = new Hypothesis
            {
                whoId = caseData.suspects[whoIndex].id,
                howId = caseData.methods[howIndex].id,
                whereId = caseData.locations[whereIndex].id
            };

            UnityEngine.Debug.Log("HypothesisInput submitting: WHO=" + hypothesis.whoId + ", HOW=" + hypothesis.howId + ", WHERE=" + hypothesis.whereId);
            GameManager.Instance.eventBus.Publish(GameEventType.HYPOTHESIS_SUBMITTED, hypothesis);
        }
    }