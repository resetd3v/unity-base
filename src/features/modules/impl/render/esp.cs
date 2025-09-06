using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameObjectExt = FluffyUnderware.DevTools.Extensions.GameObjectExt;

// <rotate=45><size="50"><color=#FF69B4><alpha=#55>meow
namespace plugin.features.impl.render;
public class esp : Module
{
    public esp() : base("Esp", "Render", "", true) {}

    public static Camera fpsCam;
    
    // private PlayerValues self;
    private GameObject ui;
    private GameObject minimal;

    public override void OnStart()
    {
        // GameObject minimal = GameObject.Find("---MINIMAL UI---");
        if (!ui) ui = GameObject.FindGameObjectWithTag("PauseManager");
        if (!minimal) minimal = ui.transform.Find("---MINIMAL UI---")?.gameObject;
    }

    // public override void OnSceneChange(Scene scene, LoadSceneMode mode) => fpsCam = GetCamera();

    public override void OnRender()
    {
        if (!minimal.activeSelf)
        {
            Utils.Log.LogMessage("reinit minimal");
            minimal.SetActive(true);
        }
        //
        // List<PlayerValues> player2s = GetPlayers();
        // List<ItemBehaviour> item2s = GetItems();
        //
        // return;
        
        fpsCam = GetCamera();
        Camera camera = fpsCam; //GetCamera();
        if (!camera || !ClientInstance.Instance || !Cache.selfPlayer) return;
        
        
        #region players
        // List<PlayerValues> players = GetPlayers();
        List<GameObject> players = GetPlayers();
        players.ForEach((playerGo) =>
        {
            PlayerValues player = playerGo.GetComponent<PlayerValues>();
            // self check
            // if (player.playerClient == ClientInstance.Instance) return;
            if (!player || playerGo == Cache.selfPlayer) return;
        
            PlayerHealth playerHealth = playerGo.GetComponent<PlayerHealth>();
            float health = Mathf.Ceil(playerHealth.health / 4f * 100f);
            
            // dead
            if (playerHealth.isKilled || playerHealth.health <= 0) return;
        
            // local only
            // long ping = playerHealth.PingDisplay.ping;
            // int count Mathf.Round(playerHealth.count);
            
            Vector3 head = player.transform.position;
            FieldInfo fi2 = player.GetType().GetField("head", 
                 BindingFlags.NonPublic | BindingFlags.Instance);
             if (fi2 != null)
             {
                 head = ((Transform) fi2.GetValue(player)).position;
             }
             
            Vector3 headPos = camera.WorldToScreenPoint(head);   //player.player.transform.position);
            headPos.y = Screen.height - headPos.y;
            // draw line on or off screen
            // dont needd to draw if aim is drawing it
            // Vector3 offscreen = (headPos.z > 0) ? headPos : camera.WorldToScreenPoint(-head);
            if (headPos.z <= 0) return;
            if (playerGo != Cache.aimTarg) Render.DrawLine(new Vector2(Screen.width/2, Screen.height/2), headPos, Color.green, 1f);
        
            // playerHealth.syncVar___health.GetValue(true)
            // idk how else better to center :/
            string txt = $"{player.playerClient.PlayerName} | {health}"; //\nd: {Math.Round(Vector3.Distance(Cache.selfPlayer.transform.position, head))}";
            string txt2 = Math.Round(Vector3.Distance(Cache.selfPlayer.transform.position, head)).ToString();
            
            // midpoint
            Vector3 pos = camera.WorldToScreenPoint(Vector3.Lerp(head, player.transform.position, 0.5f));
            pos.y = Screen.height - pos.y;
            Render.DrawString(pos, txt);
            pos.y += 15;
            Render.DrawString(pos, txt2);
        });
        #endregion
        
        // #region items
        // List<ItemBehaviour> items = GetItems();
        // items.ForEach(item =>
        // {
        //     string txt = $"{item.weaponName}\n{Math.Round(Vector3.Distance(Cache.selfPlayer.transform.position, item.transform.position))}";
        //     
        //     Vector3 pos3 = item.transform.position;
        //     
        //     Vector3 pos = camera.WorldToScreenPoint(pos3);
        //     pos.y = Screen.height - pos.y;
        //     if (pos.z <= 0) return;
        //     
        //     Render.DrawString(pos, txt);
        // });
        // #endregion
        //

        #region spawners
        // List<ItemSpawner> spawners = GetSpawners();
        // spawners.ForEach(spawner =>
        // {
        //     string txt = "spawner"; //$"{spawner.itemToSpawn.name}\n{Math.Round(Vector3.Distance(Cache.selfPlayer.transform.position, spawner.transform.position))}";
        //     
        //     Vector3 pos3 = spawner.transform.position;
        //     
        //     Vector3 pos = camera.WorldToScreenPoint(pos3);
        //     pos.y = Screen.height - pos.y;
        //     if (pos.z <= 0) return;
        //     
        //     Render.DrawString(pos, txt);
        // });
        #endregion
    }

    // private List<PlayerManager> GetPlayers() => new List<PlayerManager>(FindObjectsOfType<PlayerManager>()).Where(player => player.player != null).ToList();
    private List<GameObject> GetPlayers() => new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));                           //new List<PlayerValues>(FindObjectsOfType<PlayerValues>()).Where(player => player && player.playerClient).ToList();
    private List<ItemSpawner> GetSpawners() => new List<ItemSpawner>(FindObjectsOfType<ItemSpawner>()).Where(spawner => spawner.itemToSpawn).ToList();
    private List<ItemBehaviour> GetItems() => new List<ItemBehaviour>(FindObjectsOfType<ItemBehaviour>()).Where(item => item.canTake).ToList();

    private Camera GetCamera() => Settings.Instance.localPlayer?.playerCamera;

    // private Camera GetCamera() =>
    //     (new List<FirstPersonController>(FindObjectsOfType<FirstPersonController>())
    //         .Where(player => player && player.IsClient && player.IsOwner).ToArray())
    //     .playerCamera;  //FirstPersonController.instance?.playerCamera;
    // private Camera GetCamera()
    // {
    //     FirstPersonController[] fpsCams = new List<FirstPersonController>(FindObjectsOfType<FirstPersonController>())
    //         .Where(player => player && player.IsClient && player.IsOwner).ToArray();
    //     return (fpsCams.Length != 0) ? fpsCams.First().playerCamera : null;
    // }
}