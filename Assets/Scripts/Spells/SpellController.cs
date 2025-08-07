using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public enum Status {
    None,
    Burning,
    Frozen,
    Poisoned,
    Stunned
}

public class SpellController
{

    protected string keyword;
    protected float manaCost;
    protected float castTime;
    protected float duration;
    protected float cooldown;
    protected AudioSource spellSound;
    protected float range;
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

            SpellCastingController.StartSpellCoroutine(FinishCasting());
        }
        else
        {
            Debug.LogWarning("Not enough mana to cast the spell.");
        }
    }

    private IEnumerator FinishCasting()
    {
        yield return new WaitForSeconds(castTime);

        User.IsCastingSpell = false;

        ExecuteSpellEffect();
    }

    protected virtual void ExecuteSpellEffect() {}

}
