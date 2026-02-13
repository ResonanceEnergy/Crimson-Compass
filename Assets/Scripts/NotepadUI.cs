using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CrimsonCompass.UI;
using CrimsonCompass.Core;
using CrimsonCompass.Agents;

public class NotepadUI : MonoBehaviour
    {
public TextMeshProUGUI whoText;
    public TextMeshProUGUI howText;
    public TextMeshProUGUI whereText;

        private NotepadModel model = new NotepadModel();

        void Start()
        {
            GameManager.Instance.eventBus.Subscribe(GameEventType.DISPROOF_RETURNED, OnDisproofReturned);
            GameManager.Instance.eventBus.Subscribe(GameEventType.SESSION_OPEN, OnSessionOpen);
            UnityEngine.Debug.Log("NotepadUI subscribed to DISPROOF_RETURNED and SESSION_OPEN");
        }

        void OnSessionOpen(object payload)
        {
            if (GameManager.Instance.currentCase != null)
            {
                var caseData = GameManager.Instance.currentCase;
                whoText.text = "WHO:\n" + string.Join("\n", System.Array.ConvertAll(caseData.suspects, s => s.name));
                howText.text = "HOW:\n" + string.Join("\n", System.Array.ConvertAll(caseData.methods, m => m.name));
                whereText.text = "WHERE:\n" + string.Join("\n", System.Array.ConvertAll(caseData.locations, l => l.id));
                UnityEngine.Debug.Log("NotepadUI populated with case data");
                UpdateDisplay(); // Update with any existing disproofs
            }
        }

        void OnDisproofReturned(object payload)
        {
            var disproof = (Disproof)payload;
            UnityEngine.Debug.Log("NotepadUI received disproof: " + disproof.axis + " " + disproof.disprovedId);
            MarkDisproved(disproof.axis.ToString(), disproof.disprovedId);
        }

        public void MarkDisproved(string axis, string id)
        {
            model.MarkDisproved(axis, id);
            UpdateDisplay();
        }

        void UpdateDisplay()
        {
            if (GameManager.Instance.currentCase != null)
            {
                var caseData = GameManager.Instance.currentCase;
                
                // Show WHO with strikethrough for disproved
                var whoLines = new List<string>();
                foreach (var suspect in caseData.suspects)
                {
                    var line = suspect.name;
                    if (model.DisprovedWho.Contains(suspect.id))
                        line = "<s>" + line + "</s>";
                    whoLines.Add(line);
                }
                whoText.text = "WHO:\n" + string.Join("\n", whoLines);
                
                // Show HOW with strikethrough for disproved
                var howLines = new List<string>();
                foreach (var method in caseData.methods)
                {
                    var line = method.name;
                    if (model.DisprovedHow.Contains(method.id))
                        line = "<s>" + line + "</s>";
                    howLines.Add(line);
                }
                howText.text = "HOW:\n" + string.Join("\n", howLines);
                
                // Show WHERE with strikethrough for disproved
                var whereLines = new List<string>();
                foreach (var location in caseData.locations)
                {
                    var line = location.id;
                    if (model.DisprovedWhere.Contains(location.id))
                        line = "<s>" + line + "</s>";
                    whereLines.Add(line);
                }
                whereText.text = "WHERE:\n" + string.Join("\n", whereLines);
            }
        }
    }