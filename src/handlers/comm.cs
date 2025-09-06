#region

using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using plugin.features.impl;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace plugin.handlers;

//[System.Serializable]
public class APacket
{
    /*public APacket(int opcode, string dataStr)//: base(op, data)
    {
        op = opcode;
        data = dataStr;
    }*/

    public int op { get; set; }
    public string data { get; set; }

    public static APacket Construct(string data)
    {
        JObject packetJson = JObject.Parse(data);
        if (packetJson["op"] == null || packetJson["data"] == null) return null;
        
        return new APacket
        {
            op = packetJson["op"].ToObject<int>(),
            data = packetJson["data"].ToObject<string>()
        };
    }
    
    public static APacket FromJson(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString)) return null;

        try
        {
            Utils.Log.LogDebug($"{jsonString}");
            //return JsonUtility.FromJson<APacket>(jsonString);
            return JsonConvert.DeserializeObject<APacket>(jsonString);
        }
        catch (JsonException e)
        {
            Utils.Log.LogError($"{jsonString}: {e}");
            return null;
        }
        catch (Exception e)
        {
            Utils.Log.LogError(e);
            return null;
        }
    }
}

public class Communication // : MonoBehaviour
{
    public ClientWebSocket ws;

    public async void Start()
    {
        ws = new ClientWebSocket();
        await ConnectAsync(Config.wsUrl);
    }

    public async void Init(string wsUrl)
    {
        ws = new ClientWebSocket();
        await ConnectAsync(wsUrl);
    }

    private async Task ConnectAsync(string uri)
    {
        try
        {
            await ws.ConnectAsync(new Uri(uri), CancellationToken.None);
            Utils.Log.LogInfo("[comm] connected to ws");
            await Recieve();

            //var bytes = Encoding.UTF8.GetBytes("test :3");
            //await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch (Exception e)
        {
            Utils.Log.LogError($"[comm] ws con error: {e.Message}");
        }
    }

    private async Task Recieve()
    {
        var buf = new byte[1024];

        while (ws.State == WebSocketState.Open)
        {
            var result = await ws.ReceiveAsync(new ArraySegment<byte>(buf), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text || result.MessageType == WebSocketMessageType.Binary)
            {
                string message = Encoding.UTF8.GetString(buf, 0, result.Count);
                Utils.Log.LogDebug($"[comm] msg recieved -> {message}");
                //APacket packet = APacket.FromJson("{\"op\":\"0\",\"data\":\"MEOW\"}");//"{\"op\":0,\"data\":\"MEOW\"}");//message);
                //APacket packet = JsonConvert.DeserializeObject<APacket>(message);
                APacket packet = APacket.Construct(message);
                if (packet == null) continue;
                Utils.Log.LogInfo($"[comm] packet received ({packet.op}): Data: {packet.data}");
                
                Perform(packet);
            }
        }
    }

    public async void Send(int op, string data)
    {
        APacket packet = new APacket
        {
            op = op,
            data = data
        };
        
        string json = JsonConvert.SerializeObject(packet); //JsonUtility.ToJson(packet);
        var bytes = Encoding.UTF8.GetBytes(json);
        
        await ws.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        Utils.Log.LogInfo($"[comm] sent packet ({op}): Data: {data}");
    }

    private void Perform(APacket packet)
    {
        switch (packet.op)
        {
            case 0:
                Utils.Log.LogInfo("[comm] performing init resp: " + packet.data);
                break;
            case 1:
                break;
            case 2:
                Utils.Log.LogInfo("[comm] performing script exec: " + packet.data);
                //Luaexec.LoadScript((string)packet.data);
                break;
            
            default:
                Utils.Log.LogWarning("[comm] unknown op");
                break;
        }
    }

    private void OnDestroy()
    {
       Close();
    }
    
    public async void Close()
    {
        if (ws != null)
        {
            await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            Utils.Log.LogInfo("[comm] ws connection destroyed");
        }
    }
}
