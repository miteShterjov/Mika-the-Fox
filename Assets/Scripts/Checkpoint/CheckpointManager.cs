using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint currentCheckpoint = null;
    private Vector3 currentPlayerRespawnPoint;

    void Start()
    {
        // get the player starting position as the first ever checkpoint
        currentPlayerRespawnPoint = Player.Instance.transform.position;
    }
    // gahter all checkpoints in the scene and subscribe to their events
    private void OnEnable()
    {
        foreach (Checkpoint checkpoint in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
        {
            checkpoint.OnCheckpointActivated += Checkpoint_OnCheckpointActivated;
        }
    }
    // when a player passes a chackpoint it makes the current one active and resets the previous one
    private void Checkpoint_OnCheckpointActivated(object sender, System.EventArgs e)
    {
        if (currentCheckpoint != null) currentCheckpoint.ResetCheckpoint(); // Reset previous checkpoint
        currentCheckpoint = (Checkpoint)sender; // Set new checkpoint
        currentPlayerRespawnPoint = currentCheckpoint.RespawnPoint; // Update respawn point
        Player.Instance.RespawnPoint = currentPlayerRespawnPoint; // Update player respawn point
    }




}
