using System;

public static class RPCProtection
{
    private static bool enabled = false;

    public static void Enable()
    {
        if (enabled) return;

        try
        {
            if (!Photon.Pun.PhotonNetwork.InRoom || Photon.Pun.PhotonNetwork.LocalPlayer == null)
                return;
            var rig = GorillaTagger.Instance?.myVRRig;
            if (rig != null)
            {
                var pv = rig.GetComponent<Photon.Pun.PhotonView>();
                if (pv != null && pv.IsMine)
                {
                    Photon.Pun.PhotonNetwork.OpCleanRpcBuffer(pv);
                    Photon.Pun.PhotonNetwork.RemoveBufferedRPCs(pv.ViewID);
                    Photon.Pun.PhotonNetwork.CleanRpcBufferIfMine(pv);
                }
            }
            Photon.Pun.PhotonNetwork.NetworkingClient.EventReceived += (ev) =>
            {
                if (ev.Sender == Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber) return;
                if (ev.Code == 199 || ev.Code <= 5)
                {
                    UnityEngine.Debug.LogWarning($"[SimpleRPCProtection] Blocked event {ev.Code} from {ev.Sender}");
                }
            };

            enabled = true;
            UnityEngine.Debug.Log("<color=limeRPC Protection Enabled</color>");
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogError("[SimpleRPCProtection] Error: " + ex);
        }
    }
}
