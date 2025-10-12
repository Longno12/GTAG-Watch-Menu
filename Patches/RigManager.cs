using Photon.Realtime;
using Photon.Pun;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using ExitGames.Client.Photon;
using System.Collections.Generic;
using System.Linq;

namespace StupidTemplate.Classes
{
    internal class RigManager : BaseUnityPlugin
    {
        public static VRRig GetVRRigFromPlayer(Player p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }
        public static VRRig GetOwnVRRig()
        {
            return GorillaTagger.Instance.offlineVRRig;
        }
        public static List<VRRig> GetAllVRRigs(bool includeSelf = true)
        {
            List<VRRig> allRigs = new List<VRRig>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (includeSelf || player != PhotonNetwork.LocalPlayer)
                {
                    allRigs.Add(GetVRRigFromPlayer(player));
                }
            }
            return allRigs;
        }
        public static bool IsPlayerInRoom(Photon.Realtime.Player player)
        {
            return PhotonNetwork.PlayerList.Contains(player);
        }
        public static List<NetPlayer> GetAllNetPlayers(bool includeSelf = true)
        {
            List<NetPlayer> allNetPlayers = new List<NetPlayer>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (includeSelf || player != PhotonNetwork.LocalPlayer)
                {
                    allNetPlayers.Add(ToNetPlayer(player));
                }
            }
            return allNetPlayers;
        }
        public static NetPlayer GetClosestNetPlayer()
        {
            NetPlayer closestPlayer = null;
            float closestDistance = float.MaxValue;

            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                float distance = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, rig.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlayer = rig.Creator;
                }
            }
            return closestPlayer;
        }
        public static VRRig GetVRRigFromPhotonView(PhotonView photonView)
        {
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (GetPhotonViewFromVRRig(rig) == photonView)
                {
                    return rig;
                }
            }
            return null;
        }
        public static void SendRPCToAllPlayers(string methodName, object[] parameters = null)
        {
            PhotonNetwork.RaiseEvent(0, parameters, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
        public static NetPlayer GetHostPlayer()
        {
            return PhotonNetwork.MasterClient;
        }
        public static string GetVRRigNetworkIdentity(VRRig rig)
        {
            PhotonView photonView = GetPhotonViewFromVRRig(rig);
            return photonView.ViewID.ToString();
        }
        public static List<PhotonView> GetAllPhotonViews()
        {
            List<PhotonView> photonViews = new List<PhotonView>();
            foreach (var player in PhotonNetwork.PlayerList)
            {
                VRRig rig = GetVRRigFromPlayer(player);
                if (rig != null)
                {
                    photonViews.Add(GetPhotonViewFromVRRig(rig));
                }
            }
            return photonViews;
        }
        public static void LogVRRigInfo(VRRig rig)
        {
            if (rig != null)
            {
                Debug.Log($"VRRig Info: ID = {rig.Creator.UserId}, Position = {rig.transform.position}, Rotation = {rig.transform.rotation}");
            }
        }
        public static VRRig GetRandomVRRig(bool includeSelf)
        {
            VRRig random = GorillaParent.instance.vrrigs[UnityEngine.Random.Range(0, GorillaParent.instance.vrrigs.Count - 1)];
            if (includeSelf)
            {
                return random;
            }
            else
            {
                if (random != GorillaTagger.Instance.offlineVRRig)
                {
                    return random;
                }
                else
                {
                    return GetRandomVRRig(includeSelf);
                }
            }
        }
        public static VRRig GetClosestVRRig()
        {
            float closestDistance = float.MaxValue;
            VRRig closestRig = null;
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == GorillaTagger.Instance.offlineVRRig) continue;
                float distance = Vector3.Distance(GorillaTagger.Instance.bodyCollider.transform.position, vrrig.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestRig = vrrig;
                }
            }
            return closestRig;
        }
        public static VRRig GetMyVRRig()
        {
            return GorillaTagger.Instance.offlineVRRig;
        }

        public static PhotonView GetPhotonViewFromVRRig(VRRig p)
        {
            return (PhotonView)Traverse.Create(p).Field("photonView").GetValue();
        }
        public static Photon.Realtime.Player GetRandomPlayer(bool includeSelf)
        {
            if (includeSelf)
            {
                return PhotonNetwork.PlayerList[UnityEngine.Random.Range(0, PhotonNetwork.PlayerList.Length - 1)];
            }
            else
            {
                return PhotonNetwork.PlayerListOthers[UnityEngine.Random.Range(0, PhotonNetwork.PlayerListOthers.Length - 1)];
            }
        }
        public static NetPlayer ToNetPlayer(Player player)
        {
            foreach (NetPlayer netPlayer in NetworkSystem.Instance.AllNetPlayers)
            {
                if (netPlayer.GetPlayerRef() == player)
                {
                    return netPlayer;
                }
            }
            return null;
        }

        public static Photon.Realtime.Player NetPlayerToPlayer(NetPlayer p)
        {
            return p.GetPlayerRef();
        }
        public static NetPlayer GetPlayerFromVRRig(VRRig p)
        {
            return p.Creator;
        }
        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (NetPlayer target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }
        public static VRRig GetVRRigFromPlayer(NetPlayer p)
        {
            return GorillaGameManager.instance.FindPlayerVRRig(p);
        }
        public static NetworkView GetNetworkViewFromVRRig(VRRig p)
        {
            return (NetworkView)Traverse.Create(p).Field("netView").GetValue();
        }
    }
}