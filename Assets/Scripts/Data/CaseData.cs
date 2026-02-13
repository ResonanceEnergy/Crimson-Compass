using System;
using UnityEngine;

namespace CrimsonCompass
{
    [System.Serializable]
    public class CaseData
    {
        public string caseId;
        public string title;
        public string tier;
        public string hook;
        public int timeBudget;
        public int hintsPerMission;
        public HintCost hintCost;
        public string[] gadgetsOffered;
        public int gadgetsSelectable;
        public Suspect[] suspects;
        public Method[] methods;
        public Location[] locations;
        public Truth truth;

        // Missing properties
        public bool gasket;
        public bool catastrophicChoice;
    }

    [System.Serializable]
    public class Suspect
    {
        public string id;
        public string name;
        public string[] traits;
    }

    [System.Serializable]
    public class Method
    {
        public string id;
        public string name;
        public string[] signatures;
    }

    [System.Serializable]
    public class Location
    {
        public string id;
        public string country;
    }

    [System.Serializable]
    public class Truth
    {
        public string whoId;
        public string howId;
        public string whereId;
    }

    [System.Serializable]
    public class HintCost
    {
        public int timeHours;
        public int heat;
    }

    [System.Serializable]
    public class CitiesData
    {
        public City[] cities;
    }

    [System.Serializable]
    public class City
    {
        public string id;
        public string country;
        public string[] facts;
        public string[] langs;
    }
}