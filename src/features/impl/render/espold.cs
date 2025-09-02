// using System.Collections.Generic;
// using System.Linq;
// using System.Reflection;
// using UnityEngine;
// using GameObjectExt = FluffyUnderware.DevTools.Extensions.GameObjectExt;
//
// namespace plugin.features.impl
// {
//     public class espold : Module
//     {
//         public espold() : base("Esp", "Render", "", true) {}
//
//         public override void OnRender()
//         {
//             Camera camera = GetCamera();
//             if (!camera) return;
//             
//             List<PlayerManager> players = GetPlayers();
//             players.ForEach((player) =>
//             {
//                 Vector3 position = player.player.transform.position;
//                 FieldInfo fi = player.GetType().GetField("SpawnedObject", 
//                     BindingFlags.NonPublic | BindingFlags.Instance);
//                 if (fi != null)
//                 {
//                     position = ((GameObject)fi.GetValue(player)).transform.position;
//                 }
//                 
//                 Vector2 pos = camera.WorldToScreenPoint(position);   //player.player.transform.position);
//                 pos.y = Screen.height - pos.y;
//                 
//                 string playerName = "meow";
//                 FieldInfo fi2 = player.GetType().GetField("ClientScript", 
//                     BindingFlags.NonPublic | BindingFlags.Instance);
//                 if (fi2 != null)
//                 {
//                     playerName = ((ClientInstance)fi2.GetValue(player)).PlayerName;
//                 }
//                 
//                 Render.DrawString(pos, playerName);
//             });
//         }
//
//         // private List<PlayerManager> GetPlayers() => new List<PlayerManager>(FindObjectsOfType<PlayerManager>()).Where(player => player.player != null).ToList();
//         private List<PlayerManager> GetPlayers() => new List<PlayerManager>(FindObjectsOfType<PlayerManager>()).Where(player => player.player != null).ToList();
//         private Camera GetCamera() => FirstPersonController.instance?.playerCamera;
//     }
// }