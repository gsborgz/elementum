using UnityEngine;

public class PlayerListenerController : ListenerController
{
    private PlayerSpellCastingController spellCastingController;

    private void Awake()
    {
        spellCastingController = GetComponent<PlayerSpellCastingController>();
    }

    private void Start()
    {
        AddKeywords();
    }

    private void AddKeywords()
    {
        if (spellCastingController.AvailableSpells == null)
        {
            Debug.LogError("AvailableSpells is null in AddKeywords!");
            return;
        }

        foreach (var spell in spellCastingController.AvailableSpells)
        {
            if (spell != null && !string.IsNullOrEmpty(spell.Keyword))
            {
                keywords.Add(spell.Keyword, () => spell.Cast());
            }
        }
    }

}
