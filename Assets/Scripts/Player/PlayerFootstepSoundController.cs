using System.Collections.Generic;
using UnityEngine;

public enum SurfaceType
{
    None,
    Grass,
    Dirt,
    Wood,
    Stone
}

public class PlayerFootstepSoundController : MonoBehaviour
{
    [Header("Footstep Sounds")]
    [SerializeField] private List<AudioClip> grassSounds;
    [SerializeField] private List<AudioClip> dirtSounds;
    [SerializeField] private List<AudioClip> woodSounds;
    [SerializeField] private List<AudioClip> stoneSounds;

    [Header("Action Sounds")]
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource crouchSound;

    private PlayerMovementController movementController;
    private AudioSource audioSource;

    void Start()
    {
        movementController = GetComponent<PlayerMovementController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        PlaySounds();
    }

    private SurfaceType SurfaceSelect()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Ray ray = new(rayOrigin, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                return GetMainTexture(hit.point, terrain);
            }
        }

        return SurfaceType.None;
    }

    private SurfaceType GetMainTexture(Vector3 worldPos, Terrain terrain)
    {
        TerrainData terrainData = terrain.terrainData;
        Vector3 terrainPos = terrain.transform.position;

        int mapX = (int)((worldPos.x - terrainPos.x) / terrainData.size.x * terrainData.alphamapWidth);
        int mapZ = (int)((worldPos.z - terrainPos.z) / terrainData.size.z * terrainData.alphamapHeight);

        float[,,] splatmapData = terrainData.GetAlphamaps(mapX, mapZ, 1, 1);

        int maxIndex = 0;
        float maxMix = 0;

        for (int i = 0; i < splatmapData.GetLength(2); i++)
        {
            if (splatmapData[0, 0, i] > maxMix)
            {
                maxIndex = i;
                maxMix = splatmapData[0, 0, i];
            }
        }

        switch (maxIndex)
        {
            case 0:
                return SurfaceType.Grass;
            case 1:
                return SurfaceType.Dirt;
            default:
                return SurfaceType.None;
        }
    }

    private void PlaySounds()
    {
        PlayerState currentState = movementController.CurrentState;

        if (ShouldPlayFootsteps(currentState))
        {
            AudioClip clip = null;
            SurfaceType currentSurface = SurfaceSelect();

            switch (currentSurface)
            {
                case SurfaceType.Grass:
                    clip = grassSounds[Random.Range(0, grassSounds.Count)];
                    break;
                case SurfaceType.Dirt:
                    clip = dirtSounds[Random.Range(0, dirtSounds.Count)];
                    break;
                case SurfaceType.Wood:
                    clip = woodSounds[Random.Range(0, woodSounds.Count)];
                    break;
                case SurfaceType.Stone:
                    clip = stoneSounds[Random.Range(0, stoneSounds.Count)];
                    break;
                default:
                    break;
            }

            if (currentSurface != SurfaceType.None && clip != null)
            {
                bool isCrouching = movementController.IsCrouching;
                bool isRunning = currentState == PlayerState.Running || currentState == PlayerState.CrouchingRun;

                float minVolume = isCrouching && !isRunning ? 0.02f : 0.05f;
                float maxVolume = isCrouching && !isRunning ? 0.05f : 0.1f;

                float basePitch = isRunning ? 1.2f : 0.9f;
                float pitchVariation = 0.1f;

                audioSource.clip = clip;
                audioSource.volume = Random.Range(minVolume, maxVolume);
                audioSource.pitch = Random.Range(basePitch - pitchVariation, basePitch + pitchVariation);
                audioSource.loop = false;
                audioSource.spatialBlend = 1f;
                audioSource.minDistance = 1f;
                audioSource.maxDistance = 10f;
                audioSource.Play();
            }
        }

        if (Input.GetButtonDown("Crouch") && crouchSound != null)
        {
            crouchSound.Stop();
            crouchSound.Play();
        }

        if (currentState == PlayerState.Jumping && jumpSound != null && !jumpSound.isPlaying)
        {
            jumpSound.Play();
        }
    }

    private bool ShouldPlayFootsteps(PlayerState state)
    {
        bool isMoving = state == PlayerState.Walking ||
            state == PlayerState.Running ||
            state == PlayerState.CrouchingWalk ||
            state == PlayerState.CrouchingRun;

        return isMoving && audioSource != null && !audioSource.isPlaying;
    }

}
