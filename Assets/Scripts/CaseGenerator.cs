using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrimsonCompass
{
    public class CaseGenerator : MonoBehaviour
    {
        public TextAsset citiesJson;
        public List<string> suspectTemplates = new List<string> { "The Thief", "The Spy", "The Insider" };
        public List<string> methodTemplates = new List<string> { "Stealth", "Hack", "Deception" };

        private CitiesData citiesData;

        void Start()
        {
            citiesData = JsonUtility.FromJson<CitiesData>(citiesJson.text);
        }

        public CaseData GenerateCase()
        {
            var caseData = new CaseData
            {
                caseId = "GEN-" + Random.Range(1000, 9999),
                title = "Generated Case",
                tier = "procedural",
                hook = "A mystery unfolds...",
                timeBudget = 60,
                hintsPerMission = 3,
                hintCost = new HintCost { timeHours = 2, heat = 1 },
                gadgetsOffered = new string[] { "TOOL1", "TOOL2", "TOOL3" },
                gadgetsSelectable = 3,
                suspects = GenerateSuspects(),
                methods = GenerateMethods(),
                locations = SelectLocations(),
                truth = GenerateTruth()
            };
            return caseData;
        }

        Suspect[] GenerateSuspects()
        {
            var suspects = new List<Suspect>();
            for (int i = 0; i < 3; i++)
            {
                suspects.Add(new Suspect
                {
                    id = "S" + (i + 1),
                    name = suspectTemplates[Random.Range(0, suspectTemplates.Count)] + " " + (i + 1),
                    traits = new string[] { "trait" + i }
                });
            }
            return suspects.ToArray();
        }

        Method[] GenerateMethods()
        {
            var methods = new List<Method>();
            for (int i = 0; i < 3; i++)
            {
                methods.Add(new Method
                {
                    id = "M" + (i + 1),
                    name = methodTemplates[Random.Range(0, methodTemplates.Count)] + " Method",
                    signatures = new string[] { "sig" + i }
                });
            }
            return methods.ToArray();
        }

        Location[] SelectLocations()
        {
            var selected = new List<Location>();
            var shuffled = new List<City>(citiesData.cities);
            shuffled.Shuffle();
            for (int i = 0; i < 2; i++)
            {
                selected.Add(new Location { id = shuffled[i].id, country = shuffled[i].country });
            }
            return selected.ToArray();
        }

        Truth GenerateTruth()
        {
            return new Truth
            {
                whoId = "S" + Random.Range(1, 4),
                howId = "M" + Random.Range(1, 4),
                whereId = citiesData.cities[Random.Range(0, citiesData.cities.Length)].id
            };
        }
    }

    [System.Serializable]
    public class CitiesData { public City[] cities; }
    [System.Serializable]
    public class City { public string id; public string country; public string[] facts; public string[] langs; }
}

public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}