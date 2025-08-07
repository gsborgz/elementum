using UnityEngine;

public class PlayerSpellCastingController : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerListenerController listenerController;
    private SpellController[] availableSpells;

    public SpellController[] AvailableSpells
    {
        get => availableSpells;
        set => availableSpells = value;
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        InitializeSpells();
    }

    private void Start()
    {
        listenerController = GetComponent<PlayerListenerController>();

        StartCoroutine(StartListeningAfterInitialization());
    }

    private System.Collections.IEnumerator StartListeningAfterInitialization()
    {
        yield return null;

        if (availableSpells != null && availableSpells.Length > 0)
        {
            listenerController.StartListening();
        }
        else
        {
            Debug.LogError("No spells available to initialize keywords!");
        }
    }

    private void InitializeSpells()
    {
        IgnisSpectrisController ignisSpectris = new IgnisSpectrisController
        {
            User = playerController,
            SpellCastingController = this
        };

        availableSpells = new SpellController[] { ignisSpectris };
    }

}
