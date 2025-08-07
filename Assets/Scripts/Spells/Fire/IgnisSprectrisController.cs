using UnityEditor;
using UnityEngine;

public class IgnisSpectrisController : SpellController
{

    public IgnisSpectrisController()
    {
        keyword = "Ignis Spectris";
        manaCost = 20f;
        castTime = 1.5f;
        duration = 5f;
        cooldown = 10f;
        range = 15f;
    }

    protected override void ExecuteSpellEffect()
    {
        // Carrega a prefab WillOWisp dos recursos
        GameObject willOWispPrefab = Resources.Load<GameObject>("SpellEffectPrefabs/WillOWisp");
        
        if (willOWispPrefab != null)
        {
            Vector3 spawnPosition = User.transform.position + User.transform.forward * 2f;

            GameObject willOWispInstance = Object.Instantiate(willOWispPrefab, spawnPosition, User.transform.rotation);

            Object.Destroy(willOWispInstance, duration);
        }
        else
        {
            Debug.LogError("WillOWisp prefab not found in Resources/SpellEffectPrefabs/");
        }
    }

}
