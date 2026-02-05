using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrimsonCompass.UI;
using CrimsonCompass.Core;

namespace CrimsonCompass
{
    public class NotepadUI : MonoBehaviour
    {
        public Text whoText;
        public Text howText;
        public Text whereText;

        private NotepadModel model = new NotepadModel();

        void Start()
        {
            GameManager.Instance.eventBus.Subscribe(GameEventType.DISPROOF_RETURNED, OnDisproofReturned);
        }

        void OnDisproofReturned(object payload)
        {
            var disproof = (DisproofEngine.Disproof)payload;
            MarkDisproved(disproof.axis.ToString(), disproof.disprovedId);
        }

        public void MarkDisproved(string axis, string id)
        {
            model.MarkDisproved(axis, id);
            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            whoText.text = "WHO:\n" + string.Join("\n", model.DisprovedWho);
            howText.text = "HOW:\n" + string.Join("\n", model.DisprovedHow);
            whereText.text = "WHERE:\n" + string.Join("\n", model.DisprovedWhere);
        }
    }
}