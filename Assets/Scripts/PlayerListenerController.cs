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
        // Adiciona as palavras-chave após a inicialização completa
        AddKeywords();

        if (spellCastingController.AvailableSpells != null)
        {
            Debug.LogError("No available spells found in PlayerListenerController!");
        }
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
                keywords.Add(spell.Keyword, () => ExecuteSpell(spell));
            }
        }
    }

    private void ExecuteSpell(SpellController spell)
    {
        spellCastingController.CurrentSpell = spell;
        spell.Cast();
    }

}
