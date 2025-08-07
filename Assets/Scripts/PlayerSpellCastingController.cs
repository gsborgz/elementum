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

    // Método para permitir que os spells executem corrotinas
    public Coroutine StartSpellCoroutine(System.Collections.IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        // Inicializa os spells
        InitializeSpells();
    }

    private void Start()
    {
        listenerController = GetComponent<PlayerListenerController>();
        
        // Aguarda um frame para garantir que o PlayerListenerController terminou sua inicialização
        StartCoroutine(StartListeningAfterInitialization());
    }

    private System.Collections.IEnumerator StartListeningAfterInitialization()
    {
        yield return null; // Aguarda um frame
        
        // Garante que os spells estão inicializados antes de começar a escutar
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
