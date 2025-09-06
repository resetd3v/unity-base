using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace plugin.features.impl.player;
public class Respawn : Module
{
    public Respawn() : base("Respawn", "Player", "", KeyCode.Y) {}

    // yayyyyy more static abuse
    private static ClientInstance client;
    public override void OnEnable()
    {
        client = ClientInstance.Instance;
        if (!client)
        {
            Toggle();
            return;
        }
        
        client.PlayerSpawner?.TryRespawn();
        // StartCoroutine(WaitExec());
        // client.PlayerSpawner?.SetPlayerMove(true);
        Toggle();
    }

    private IEnumerator WaitExec()
    {
        yield return new WaitForSeconds(1f);
        if (client) client.PlayerSpawner?.SetPlayerMove(true);
    }
}