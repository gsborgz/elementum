using UnityEditor;
using UnityEngine;

public class IgnisCrepitusController : SpellController
{

    public IgnisCrepitusController()
    {
        keywords = new[] { "Ignis Crepitus", "IgnisCrepitus", "Igniscrepitus", "Iguinis Crepitus", "Inis Crepitus", "Iniscrepitus" };
        manaCost = 20f;
        duration = 5f;
        range = 15f;
        statusEffect = Status.Burning;
        statusEffectDuration = 10f;
        category = Category.Fire;
    }

    protected override void ExecuteSpellEffect()
    {
        GameObject wispPrefab = Resources.Load<GameObject>("SpellEffectPrefabs/Wisp");

        if (wispPrefab != null)
        {
            Vector3 spawnPosition = User.transform.position + User.transform.forward * 2f;

            GameObject willOWispInstance = Object.Instantiate(wispPrefab, spawnPosition, User.transform.rotation);

            willOWispInstance.GetComponent<WispController>().SpellController = this;

            Object.Destroy(willOWispInstance, duration);
        }
        else
        {
            Debug.LogError("Wisp prefab not found in Resources/SpellEffectPrefabs/");
        }
    }

}
