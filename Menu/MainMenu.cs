using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections.Generic;
using UnityEngine;
using Watch_Menu.mods;

namespace Watch1
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    internal class Watch : MonoBehaviour
    {
        public class ModPage
        {
            public string Name;
            public bool IsEnabled;
            public bool IsOneShot; // For mods that run once and turn off, like Disconnect.
            public Action ActionToRun;
        }

        // TO ADD A NEW MOD, JUST ADD A NEW LINE TO THIS LIST.
        private static readonly List<ModPage> modPages = new List<ModPage>
        {
            new ModPage { Name = "Disconnect",       IsEnabled = false, IsOneShot = true,  ActionToRun = () => PhotonNetwork.Disconnect() },
            new ModPage { Name = "Sticky Platforms", IsEnabled = false, IsOneShot = false, ActionToRun = StickyPlatforms.StickyPlatforms1 },
            new ModPage { Name = "NOCLIP",           IsEnabled = false, IsOneShot = false, ActionToRun = NoClip.NoClip1 },
            new ModPage { Name = "FLY",              IsEnabled = false, IsOneShot = false, ActionToRun = Fly.Fly1 },
            new ModPage { Name = "RPC Protection",   IsEnabled = false, IsOneShot = false, ActionToRun = RPCProtection.Enable },
            new ModPage { Name = "Grab Rig",         IsEnabled = false, IsOneShot = false, ActionToRun = Grab_Rig.GrabRig },
            new ModPage { Name = "Size Changer",         IsEnabled = false, IsOneShot = false, ActionToRun = Size_Changer.SizeChanger },
            // Example: new ModPage { Name = "My New Mod", IsEnabled = false, IsOneShot = false, ActionToRun = MyNewMod.Run },
        };

        private static void Prefix()
        {
            try
            {
                var huntComp = GorillaTagger.Instance.offlineVRRig.huntComputer.GetComponent<GorillaHuntComputer>();
                GorillaTagger.Instance.offlineVRRig.EnableHuntWatch(true);
                huntComp.gameObject.SetActive(true);
                huntComp.material.gameObject.SetActive(false);
                huntComp.face.gameObject.SetActive(false);
                huntComp.badge.gameObject.SetActive(false);
                huntComp.hat.gameObject.SetActive(false);
                huntComp.leftHand.gameObject.SetActive(false);
                huntComp.rightHand.gameObject.SetActive(false);
                huntComp.text.color = WatchTextColor;

                if (PagesMove)
                {
                    Vector2 axis = ControllerInputPoller.instance.rightControllerPrimary2DAxis;
                    if (axis.y > 0.85f && Time.time > Cooldown + 0.5f)
                    {
                        PageNumber--;
                        Cooldown = Time.time;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.8f);
                    }
                    else if (axis.y < -0.85f && Time.time > Cooldown + 0.5f)
                    {
                        PageNumber++;
                        Cooldown = Time.time;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(66, false, 0.8f);
                    }
                }
                int maxPage = modPages.Count;
                if (PageNumber > maxPage) PageNumber = 0;
                if (PageNumber < 0) PageNumber = maxPage;

                if (WatchCreditPage)
                {
                    PagesMove = false;
                    huntComp.text.text = "CREDITS\nBY: 2025Joe";
                }
                else
                {
                    PagesMove = true;
                    if (PageNumber == 0)
                    {
                        huntComp.text.text = MenuTitle;
                    }
                    else
                    {
                        ModPage currentMod = modPages[PageNumber - 1];
                        huntComp.text.text = $"{currentMod.Name}   ({currentMod.IsEnabled})";
                    }
                }

                if (ControllerInputPoller.instance.leftControllerSecondaryButton && Time.time > Cooldown + 0.5f)
                {
                    if (PageNumber == 0)
                    {
                        WatchCreditPage = !WatchCreditPage;
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(20, true, 0.8f);
                    }
                    else
                    {
                        ModPage currentMod = modPages[PageNumber - 1];
                        currentMod.IsEnabled = !currentMod.IsEnabled;

                        if (currentMod.IsEnabled && currentMod.IsOneShot)
                        {
                            currentMod.ActionToRun?.Invoke();
                            currentMod.IsEnabled = false;
                        }
                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(64, true, 0.8f);
                    }

                    GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                    Cooldown = Time.time;
                }

                foreach (var mod in modPages)
                {
                    if (mod.IsEnabled && !mod.IsOneShot)
                    {
                        mod.ActionToRun?.Invoke();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Watch Prefix Error: " + ex);
            }
        }

        public static float Cooldown { get; private set; }
        public static bool PagesMove = true;
        public static bool WatchCreditPage;
        public static int PageNumber;
        public static string MenuTitle = "Encryptic Watch";
        public static Color WatchTextColor = Color.aliceBlue;

        public class TimedBehaviour : MonoBehaviour
        {
            public virtual void Start()
            {
                this.startTime = Time.time;
            }

            public virtual void Update()
            {
                if (!this.complete)
                {
                    this.progress = Mathf.Clamp((Time.time - this.startTime) / this.duration, 0f, 1f);
                    if (Time.time - this.startTime > this.duration)
                    {
                        if (this.loop)
                        {
                            this.OnLoop();
                        }
                        else
                        {
                            this.complete = true;
                        }
                    }
                }
            }

            public virtual void OnLoop()
            {
                this.startTime = Time.time;
            }

            public bool complete = false;
            public bool loop = true;
            public float progress = 0f;
            protected bool paused = false;
            protected float startTime;
            protected float duration = 2f;
        }

        public class ColorChanger : TimedBehaviour
        {
            public override void Start()
            {
                base.Start();
                this.gameObjectRenderer = base.GetComponent<Renderer>();
            }

            public override void Update()
            {
                base.Update();
                if (this.colors != null)
                {
                    if (this.timeBased)
                    {
                        this.color = this.colors.Evaluate(this.progress);
                    }
                    this.gameObjectRenderer.material.SetColor("_Color", this.color);
                    this.gameObjectRenderer.material.SetColor("_EmissionColor", this.color);
                }
            }

            public Renderer gameObjectRenderer;
            public Gradient colors = null;
            public Color color;
            public bool timeBased = true;
        }
    }
}
