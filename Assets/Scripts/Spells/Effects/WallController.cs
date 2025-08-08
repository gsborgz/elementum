using UnityEngine;

public class WallController : MonoBehaviour
{
    private SpellController spellController;

    public SpellController SpellController
    {
        get => spellController;
        set => spellController = value;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();

            if (spellController.StatusEffect != Status.None)
            {
                player.ApplyStatus(spellController.StatusEffect, spellController.StatusEffectDuration);
            }

            if (spellController.Damage > 0)
            {
                player.TakeDamage(spellController.Damage);
            }

            Destroy(gameObject);
        }
    }
}
