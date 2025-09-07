using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Multiplayer.Entity.Client;
using UnityEngine;

namespace kys
{
    public class ESP
    {

        public void OnRender()
        {
            Render.DrawString(new Vector2(Screen.width / 2, 20), "meowin on da shit :3 😈");
            Camera camera = GetCam();//Camera.main;
            if (camera == null) return;
            
            ClientPlayer[] players = GetPlayers();

            bool logged = false;
            int spectateCount = 0;
            foreach (ClientPlayer target in players)
            {
                if (target == null || target.head == null) continue;
                if (target.specting) spectateCount++;

                Vector3 targetPos = target.head.position;
                Vector3 screenPos = camera.WorldToScreenPoint(targetPos);
                screenPos.y = Screen.height - screenPos.y;

                if (screenPos.z <= 0) continue;
                if (target.playerState.JDKPJMAILFE == ClientPlayer.DKLDHDJCKCF.Team) continue;
                //if (target.playerState.NOJCDJOPIGB <= 0) continue;
                // isDead
                if (target.JHEIDIGEOCM) continue;

                //if (!logged)
                //{
                //    Utils.Log.LogMessage($"{players[1].head.position.ToString()} | {camera.WorldToScreenPoint(players[1].head.position).ToString()}");
                //    logged = true;
                //}

                Render.DrawString(new Vector2(screenPos.x, screenPos.y), "meow");//target.nameText.text);
            }

            if (spectateCount > 0) Render.DrawString(new Vector2(Screen.width / 2, Screen.height / 2), $"WARNING: {spectateCount} players in spectate");
        }


        private Camera GetCam()
        {
            Camera cameraObj = null;
            Camera[] cameras = GameObject.FindObjectsOfType<Camera>();
            foreach (Camera cam in cameras)
            {
                if (cam == null || cam.name != "FirstPersonCamera") continue;
                cameraObj = cam;
            }
            return cameraObj;
        }

        private ClientPlayer[] GetPlayers()
        {
            return GameObject.FindObjectsOfType<ClientPlayer>();
        }
    }
}
