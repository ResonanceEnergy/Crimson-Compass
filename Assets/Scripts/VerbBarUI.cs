using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum Verb
{
    MOVE,
    OBSERVE,
    ENGAGE,
    KIT,
    PROTOCOL
}

public class VerbBarUI : MonoBehaviour
{
    [Header("Verb Buttons")]
    public Button moveButton;
    public Button observeButton;
    public Button engageButton;
    public Button kitButton;
    public Button protocolButton;
    public Button pauseButton;

    [Header("Button Text")]
    public TextMeshProUGUI moveText;
    public TextMeshProUGUI observeText;
    public TextMeshProUGUI engageText;
    public TextMeshProUGUI kitText;
    public TextMeshProUGUI protocolText;

    [Header("Visual Feedback")]
    public Color selectedColor = Color.yellow;
    public Color normalColor = Color.white;

    private Verb selectedVerb = Verb.MOVE;
    private Dictionary<Verb, Button> verbButtons;
    private Dictionary<Verb, TextMeshProUGUI> verbTexts;

    void Start()
    {
        InitializeVerbButtons();
        InitializeVerbTexts();
        SelectVerb(Verb.MOVE);

        // Initially lock all except MOVE and OBSERVE
        LockVerb(Verb.ENGAGE);
        LockVerb(Verb.KIT);
        LockVerb(Verb.PROTOCOL);
    }

    void InitializeVerbButtons()
    {
        verbButtons = new Dictionary<Verb, Button>
        {
            { Verb.MOVE, moveButton },
            { Verb.OBSERVE, observeButton },
            { Verb.ENGAGE, engageButton },
            { Verb.KIT, kitButton },
            { Verb.PROTOCOL, protocolButton }
        };

        foreach (var kvp in verbButtons)
        {
            Verb verb = kvp.Key;
            Button button = kvp.Value;
            button.onClick.AddListener(() => SelectVerb(verb));
        }
    }

    void InitializeVerbTexts()
    {
        verbTexts = new Dictionary<Verb, TextMeshProUGUI>
        {
            { Verb.MOVE, moveText },
            { Verb.OBSERVE, observeText },
            { Verb.ENGAGE, engageText },
            { Verb.KIT, kitText },
            { Verb.PROTOCOL, protocolText }
        };
    }

    public void SelectVerb(Verb verb)
    {
        // Deselect previous
        if (verbButtons.ContainsKey(selectedVerb))
        {
            verbButtons[selectedVerb].image.color = normalColor;
        }

        selectedVerb = verb;

        // Select new
        if (verbButtons.ContainsKey(selectedVerb))
        {
            verbButtons[selectedVerb].image.color = selectedColor;
        }

        // Notify interaction system
        if (AdventureGameManager.Instance != null)
        {
            AdventureGameManager.Instance.OnVerbSelected(verb);
        }
    }

    public void UnlockVerb(Verb verb)
    {
        if (verbButtons.ContainsKey(verb))
        {
            verbButtons[verb].interactable = true;
            verbTexts[verb].color = normalColor;
        }
    }

    public void LockVerb(Verb verb)
    {
        if (verbButtons.ContainsKey(verb))
        {
            verbButtons[verb].interactable = false;
            verbTexts[verb].color = Color.gray;
        }
    }

    public Verb GetSelectedVerb()
    {
        return selectedVerb;
    }
}