using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public static class RPCProtection
{
    private static bool isActive;

    public static void Enable()
    {
        if (isActive) return;
        try
        {
            if (!PhotonNetwork.InRoom || PhotonNetwork.LocalPlayer == null) return;
            var rig = GorillaTagger.Instance != null ? GorillaTagger.Instance.myVRRig : null;
            if (rig != null)
            {
                var view = rig.GetComponent<PhotonView>();
                if (view != null && view.IsMine)
                {
                    PhotonNetwork.OpCleanRpcBuffer(view);
                    PhotonNetwork.RemoveBufferedRPCs(view.ViewID);
                    PhotonNetwork.CleanRpcBufferIfMine(view);
                }
            }
            PhotonNetwork.NetworkingClient.EventReceived += OnPhotonEvent;
            isActive = true;
            Debug.Log("<color=lime>RPC Protection Activated</color>");
        }
        catch (Exception err)
        {
            Debug.LogError("[RPCProtector] Exception: " + err.Message);
        }
    }

    private static void OnPhotonEvent(ExitGames.Client.Photon.EventData eventData)
    {
        if (eventData.Sender == PhotonNetwork.LocalPlayer.ActorNumber) return;
        byte code = eventData.Code;
        if (code == 199 || code <= 5)
        {
            Debug.LogWarning($"[RPCProtector] Blocked Photon event ({code}) from sender {eventData.Sender}");
        }
    }
}
