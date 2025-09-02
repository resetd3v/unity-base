// pasted cuz how do aimbot in unity

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace plugin.features.impl.combat
{
    public class Aim : Module
    {
        public Aim() : base("Aids items", "Exploit", "", true) {}
        
        // trol
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        
        public Camera camera;
        public static int layerMask = 11;

        // erm so i can access my modules classes from unity explorer but just not erm properly (they are bugged as destroyed prob cuz they extend monobehav)
        // so static abuse here to "debug" and "config"
        public static float fov = 100f;
        public static float baseSmoothing = 1; //2
        public static float minSmoothing = 0.5f;
        public static float maxSmoothing = 5f;
        public static float maxDistance = 50f;
        
        // public static GameObject aimTarget;
        public static Vector2 aimTargetV = Vector2.zero;


        public override void OnEnable()
        {
            camera = GetCamera();
        }
        
        public override void OnUpdate()
        {
            camera = GetCamera();
            if (!camera || !Cache.selfPlayer) return;
            List<GameObject> players = GetPlayers();
            
            float curDist = 9999999f;
            // GameObject aimTarget = null;
            // Vector2 aimTargetV = Vector2.zero;
            Cache.aimTarg = null;
            aimTargetV = Vector2.zero;
            foreach (GameObject player in players)
            {
                PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();
                // dead
                if (!playerHealth || playerHealth.isKilled || playerHealth.health <= 0) continue;
                
                PlayerValues pv = player.GetComponent<PlayerValues>();
                if (!pv) continue;
                if (TeamCheck(Cache.selfPlayer.GetComponent<PlayerValues>(), pv)) continue;
                
                Vector3 head = pv.transform.position;
                FieldInfo fi2 = pv.GetType().GetField("head", 
                    BindingFlags.NonPublic | BindingFlags.Instance);
                if (fi2 != null)
                {
                    head = ((Transform) fi2.GetValue(pv)).position;
                }
                
                Vector3 screenPos = camera.WorldToScreenPoint(head);
                screenPos.y = Screen.height - screenPos.y;
                if (screenPos.z <= 0) continue;
            
                float fovDist = Math.Abs(Vector2.Distance(new Vector2(screenPos.x, Screen.height - screenPos.y), new Vector2((Screen.width / 2), (Screen.height / 2))));
                if (fovDist > fov) continue;
                if (!VisibleCheck(camera.transform.position, head)) continue;
                if (fovDist > curDist) continue;
            
                curDist = fovDist;
                //aimTarget = player; //new Vector2(screenPos.x, screenPos.y);
                Cache.aimTarg = player;
                aimTargetV = new Vector2(screenPos.x, screenPos.y);
            }
            
            //if (aimTarget != null)
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

        public override void OnRender()
        {
            if (!camera || !camera.transform) return;
            DrawFOV();
            // p1000 targethud
            if (Cache.aimTarg) DrawTarg();
        }

        private void DrawFOV()
        {
            float halfFov = fov / 2f;
            Vector3 origin = camera.transform.position;
            Vector3 forward = camera.transform.forward;

            Vector3 leftBoundary = Quaternion.Euler(0, -halfFov, 0) * forward * maxDistance;
            Vector3 rightBoundary = Quaternion.Euler(0, halfFov, 0) * forward * maxDistance;

            Render.DrawLine(new Vector2(origin.x, origin.y), new Vector2(origin.x + leftBoundary.x, origin.y + leftBoundary.y), Color.red, 2f);
            Render.DrawLine(new Vector2(origin.x, origin.y), new Vector2(origin.x + rightBoundary.x, origin.y + rightBoundary.y), Color.red, 2f);

            Render.DrawLine(new Vector2(origin.x + leftBoundary.x, origin.y + leftBoundary.y), new Vector2(origin.x + rightBoundary.x, origin.y + rightBoundary.y), Color.red, 2f);
        }

        private void DrawTarg()
        {
            PlayerValues pv = Cache.aimTarg.GetComponent<PlayerValues>();
            
            Vector3 head = pv.transform.position;
            FieldInfo fi2 = pv.GetType().GetField("head", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi2 != null)
            {
                head = ((Transform) fi2.GetValue(pv)).position;
            }
            
            Vector3 pos = camera.WorldToScreenPoint(head);
            pos.y = Screen.height - pos.y;
            if (pos.z <= 0) return;
            
            PlayerHealth playerHealth = Cache.aimTarg.gameObject.GetComponent<PlayerHealth>();
            
            Render.DrawString(new Vector2(Screen.width/2  + 25, Screen.height/2  + 10), pv.playerClient.PlayerName);
            Render.DrawString(new Vector2(Screen.width/2  + 25, Screen.height/2  + 25), Mathf.Ceil(playerHealth.health / 4f * 100f).ToString());
            Render.DrawLine(new Vector2(Screen.width/2, Screen.height/2), pos, (Input.GetKey(KeyCode.Mouse4)) ? Color.red : Color.magenta, 1f);
        }


        // fuck this shit
        // private void Rotate( targetPlayer)
        // {
        //     Vector3 targetDirection = targetPlayer.head.transform.position - camera.transform.position;
        //     if (targetDirection == Vector3.zero) return;
        //
        //     Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //     camera.transform.rotation = targetRotation;//Quaternion.Slerp(camera.transform.rotation, targetRotation, Time.deltaTime * 2);
        //
        //     //float dist = targetDirection.magnitude;
        //     //float smoothing = Mathf.Lerp(maxSmoothing, minSmoothing, dist / maxDistance);
        //     //smoothing = Mathf.Clamp(smoothing, minSmoothing, maxSmoothing);
        //     //camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, Time.deltaTime * smoothing);
        // }

        private bool TeamCheck(PlayerValues self, PlayerValues target)
        {
            int teamId = ScoreManager.Instance.GetTeamId(target.playerClient.PlayerId);
            int teamId2 = ScoreManager.Instance.GetTeamId(self.playerClient.PlayerId);
            return teamId == teamId2;
        }

        // fuck this shit
        private bool VisibleCheck(Vector3 startPos, Vector3 targetPos)
        {
            // RaycastHit hit;
            return true;
            // return Physics.Raycast(startPos, targetPos, out hit, 999f, layerMask);
            //return Physics.Raycast(startPos, targetPos, 9999f);
        }
        
        private List<GameObject> GetPlayers() => new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        private Camera GetCamera() => Settings.Instance.localPlayer?.playerCamera;
        
        // private Camera GetCamera() => FirstPersonController.instance?.playerCamera;
        // private List<PlayerValues> GetPlayers() => new List<PlayerValues>(FindObjectsOfType<PlayerValues>()).Where(player => player && player.playerClient).ToList();
    }
}
