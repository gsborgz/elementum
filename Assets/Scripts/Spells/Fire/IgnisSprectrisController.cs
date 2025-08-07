using UnityEditor;
using UnityEngine;

public class IgnisSpectrisController : SpellController
{

    public IgnisSpectrisController()
    {
        keyword = "Ignis Spectris";
        manaCost = 20f;
        duration = 5f;
        range = 15f;
        statusEffect = Status.Burning;
        statusEffectDuration = 10f;
        category = Category.Fire;
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
