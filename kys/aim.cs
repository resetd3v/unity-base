// pasted cuz how do aimbot in unity

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Multiplayer.Entity.Client;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

namespace kys
{
    public class Aim
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public Camera camera;
        public static int layerMask = 11;

        public static float fov = 100f;
        public static float baseSmoothing = 3; //2
        public static float minSmoothing = 0.5f;
        public static float maxSmoothing = 5f;
        public static float maxDistance = 50f;


        public Aim()
        {
            camera = GetCam();
        }
        
        public void OnUpdate()
        {
            camera = GetCam();
            if (camera == null) return;
            ClientPlayer[] players = GetPlayers();

            float curDist = 9999999f;
            //ClientPlayer aimTarget = null;
            Vector2 aimTargetV = Vector2.zero;
            foreach (ClientPlayer player in players)
            {
                if (player == null) continue;
                // isDead
                if (player.JHEIDIGEOCM) continue;
                Vector3 targetPos = player.head.transform.position;
                Vector3 screenPos = camera.WorldToScreenPoint(targetPos);
                screenPos.y = Screen.height - screenPos.y;
                if (screenPos.z <= 0) continue;

                float fovDist = Math.Abs(Vector2.Distance(new Vector2(screenPos.x, Screen.height - screenPos.y), new Vector2((Screen.width / 2), (Screen.height / 2))));
                if (fovDist > fov) continue;
                //if (player.isDead) continue
                if (!VisibleCheck(camera.transform.position, targetPos)) continue;
                if (fovDist > curDist) continue;

                curDist = fovDist;
                //aimTarget = player; //new Vector2(screenPos.x, screenPos.y);
                aimTargetV = new Vector2(screenPos.x, screenPos.y);
            }

            //if (aimTarget != null)//Vector2.zero)
            if (aimTargetV != Vector2.zero)
            {
                double distX = aimTargetV.x - Screen.width / 2.0f;
                double distY = aimTargetV.y - Screen.height / 2.0f;

                distX /= baseSmoothing;
                distY /= baseSmoothing;

                if (Input.GetKey(KeyCode.Mouse4)) mouse_event(0x0001, (int)distX, (int)distY, 0, 0);
                ///if (Input.GetKey(KeyCode.Mouse4)) Rotate(aimTarget);
            }
            
        }

        private void Rotate(ClientPlayer targetPlayer)
        {
            Vector3 targetDirection = targetPlayer.head.transform.position - camera.transform.position;
            if (targetDirection == Vector3.zero) return;

            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            camera.transform.rotation = targetRotation;//Quaternion.Slerp(camera.transform.rotation, targetRotation, Time.deltaTime * 2);

            //float dist = targetDirection.magnitude;
            //float smoothing = Mathf.Lerp(maxSmoothing, minSmoothing, dist / maxDistance);
            //smoothing = Mathf.Clamp(smoothing, minSmoothing, maxSmoothing);
            //camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, Time.deltaTime * smoothing);
        }

        private bool VisibleCheck(Vector3 startPos, Vector3 targetPos)
        {
            RaycastHit hit;
            return Physics.Raycast(startPos, targetPos, out hit, 999f, layerMask);
            //return Physics.Raycast(startPos, targetPos, 9999f);
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
