using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CheckpointManager : MonoBehaviour
{
    private Checkpoint currentCheckpoint = null;
    private Vector3 currentPlayerRespawnPoint;

    void Start()
    {
        currentPlayerRespawnPoint = Player.Instance.transform.position;
    }

    private void OnEnable()
    {
        foreach (Checkpoint checkpoint in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
        {
            checkpoint.OnCheckpointActivated += Checkpoint_OnCheckpointActivated;
        }
    }

    private void Checkpoint_OnCheckpointActivated(object sender, System.EventArgs e)
    {
        if (currentCheckpoint != null) currentCheckpoint.ResetCheckpoint(); // Reset previous checkpoint
        currentCheckpoint = (Checkpoint)sender; // Set new checkpoint
        currentPlayerRespawnPoint = currentCheckpoint.RespawnPoint; // Update respawn point
        Player.Instance.RespawnPoint = currentPlayerRespawnPoint; // Update player respawn point
    }




}
