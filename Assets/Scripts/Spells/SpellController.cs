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
    Poison,
    Other
}

public class SpellController
{

    protected string[] keywords;
    protected float damage = 1f;
    protected float manaCost = 10f;
    protected float duration = 5f;
    protected float range = 10f;
    protected Status statusEffect = Status.None;
    protected float statusEffectDuration = 0f;
    protected Category category = Category.Other;
    protected AudioSource spellSound;
    protected PlayerController user;
    protected PlayerSpellCastingController spellCastingController;

    public string[] Keywords => keywords;

    public float Damage => damage;

    public Status StatusEffect => statusEffect;

    public float StatusEffectDuration => statusEffectDuration;

    public PlayerController User
    {
        get => user;
        set => user = value;
    }

    public PlayerSpellCastingController SpellCastingController
    {
        get => spellCastingController;
        set => spellCastingController = value;
    }

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
