using UnityEditor;
using UnityEngine;

public class FlammeumMurumController : SpellController
{

    public FlammeumMurumController()
    {
        keywords = new[] { "Flammeum Murum", "FlammeumMurum", "Flammeumurum", "Flameum Murum", "FlameumMurum", "Flameumurum" };
        manaCost = 20f;
        duration = 5f;
        range = 15f;
        statusEffect = Status.Burning;
        statusEffectDuration = 10f;
        category = Category.Fire;
    }

    protected override void ExecuteSpellEffect()
    {
        GameObject wallPrefab = Resources.Load<GameObject>("SpellEffectPrefabs/Wall");

        if (wallPrefab != null)
        {
            Vector3 spawnPosition = User.transform.position + User.transform.forward * 5f;

            GameObject wallInstance = Object.Instantiate(wallPrefab, spawnPosition, User.transform.rotation);

            wallInstance.GetComponent<WallController>().SpellController = this;

            Object.Destroy(wallInstance, duration);
        }
        else
        {
            Debug.LogError("Wall prefab not found in Resources/SpellEffectPrefabs/");
        }
    }

}
