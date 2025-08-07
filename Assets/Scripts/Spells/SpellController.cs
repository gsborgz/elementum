using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public enum Status
{
    None,
    Burning,
    Frozen,
    Poisoned,
    Stunned
}

public enum Category
{
    Fire,
    Ice,
    Lightning,
    Poison
}

public class SpellController
{

    protected string keyword;
    protected float manaCost;
    protected float duration;
    protected float range;
    protected Status statusEffect;
    protected float statusEffectDuration;
    protected Category category;
    protected AudioSource spellSound;
    protected PlayerController _user;
    protected PlayerSpellCastingController _spellCastingController;

    public PlayerController User
    {
        get => _user;
        set => _user = value;
    }

    public PlayerSpellCastingController SpellCastingController
    {
        get => _spellCastingController;
        set => _spellCastingController = value;
    }

    public string Keyword => keyword;

    public void Cast()
    {
        if (User.Mana >= manaCost)
        {
            User.ReduceMana(manaCost);
            User.IsCastingSpell = true;

            ExecuteSpellEffect();
        }
        else
        {
            Debug.LogWarning("Not enough mana to cast the spell.");
        }
    }

    protected virtual void ExecuteSpellEffect() { }

}
