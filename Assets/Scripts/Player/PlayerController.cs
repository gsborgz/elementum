using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    [Header("Player Stats")]
    [SerializeField] private float health = 100f;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float mana = 100f;
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private Status status = Status.None;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource footstepSound;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource landSound;
    [SerializeField] private AudioSource crouchSound;

    public float Health => health;
    public float MaxHealth => maxHealth;
    public float Mana => mana;
    public float MaxMana => maxMana;
    public Status CurrentStatus => status;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (mana < maxMana)
        {
            RestoreMana(0.1f * Time.deltaTime);
        }
    }

    public void TakeDamage(float damage, float duration = 0)
    {
        float actualDamage = Mathf.Abs(damage);
        
        if (duration > 0)
        {
            StartCoroutine(TakeDamageOverTime(actualDamage, duration));
        }
        else
        {
            UpdateHealth(-actualDamage);
        }
    }

    public void ReduceMana(float amount)
    {
        float actualMana = Mathf.Abs(amount);

        mana -= actualMana;
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    public void Heal(float amount)
    {
        float actualHeal = Mathf.Abs(amount);

        UpdateHealth(actualHeal);
    }

    public void ApplyStatus(Status newStatus, float duration)
    {
        status = newStatus;

        switch (newStatus)
        {
            case Status.Burning:
                StartCoroutine(TakeDamageOverTime(1f, duration));
                break;
        }

        if (duration > 0)
        {
            StartCoroutine(RemoveStatusAfterDuration(duration));
        }
    }

    private void RestoreMana(float value)
    {
        float actualMana = Mathf.Abs(value);

        mana += actualMana;
        mana = Mathf.Clamp(mana, 0, maxMana);
    }

    private void UpdateHealth(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    private IEnumerator RemoveStatusAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        status = Status.None;
    }

    private IEnumerator TakeDamageOverTime(float damage, float duration)
    {
        float timeElapsed = 0f;
        float interval = 1f;

        while (timeElapsed < duration)
        {
            UpdateHealth(-damage);
            timeElapsed += interval;
            yield return new WaitForSeconds(interval);
        }
    }


}
