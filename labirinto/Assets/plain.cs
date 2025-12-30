using System;
using UnityEngine;

public class plain : MonoBehaviour
{
    public event EventHandler<EventArgs> player_uscito;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            player_uscito?.Invoke(this, EventArgs.Empty);
        }
    }
}