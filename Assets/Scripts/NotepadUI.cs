using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CrimsonCompass.UI;

namespace CrimsonCompass
{
    public class NotepadUI : MonoBehaviour
    {
        public Text whoText;
        public Text howText;
        public Text whereText;

        private NotepadModel model = new NotepadModel();

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