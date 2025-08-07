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
            Debug.Log(spell.Keyword);
            if (spell != null && !string.IsNullOrEmpty(spell.Keyword))
            {
                keywords.Add(spell.Keyword, () => ExecuteSpell(spell));
            }
        }
    }

    private void ExecuteSpell(SpellController spell)
    {
        spell.Cast();
    }

}
