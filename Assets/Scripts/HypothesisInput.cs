using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrimsonCompass.Agents;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    public class HypothesisInput : MonoBehaviour
    {
        public Dropdown whoDropdown;
        public Dropdown howDropdown;
        public Dropdown whereDropdown;
        public Button submitButton;

        void Start()
        {
            submitButton.onClick.AddListener(SubmitHypothesis);
            PopulateDropdowns();
        }

        void PopulateDropdowns()
        {
            var caseData = GameManager.Instance.currentCase;
            whoDropdown.ClearOptions();
            whoDropdown.AddOptions(new List<string> { "Select WHO" });
            foreach (var s in caseData.suspects) whoDropdown.options.Add(new Dropdown.OptionData(s.name));

            howDropdown.ClearOptions();
            howDropdown.AddOptions(new List<string> { "Select HOW" });
            foreach (var m in caseData.methods) howDropdown.options.Add(new Dropdown.OptionData(m.name));

            whereDropdown.ClearOptions();
            whereDropdown.AddOptions(new List<string> { "Select WHERE" });
            foreach (var l in caseData.locations) whereDropdown.options.Add(new Dropdown.OptionData(l.id));
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

            GameManager.Instance.eventBus.Publish(GameEventType.HYPOTHESIS_SUBMITTED, hypothesis);
        }
    }
}