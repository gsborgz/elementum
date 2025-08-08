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
            if (spell != null && spell.Keywords != null && spell.Keywords.Length > 0)
            {
                foreach (var keyword in spell.Keywords)
                {
                    keywords.Add(keyword, () => spell.Cast());
                }
            }
        }
    }

}
