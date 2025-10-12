using System;
using HarmonyLib;
using Photon.Pun;
using UnityEngine;
using Watch_Menu.mods;

namespace Watch1
{
    [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer))]
    [HarmonyPatch("LateUpdate", MethodType.Normal)]
    internal class Watch : MonoBehaviour
    {

        private static void Prefix()
        {
            try
            {
                var huntCompObj = GorillaTagger.Instance.offlineVRRig.huntComputer;
                var huntComp = huntCompObj.GetComponent<GorillaHuntComputer>();
                GorillaTagger.Instance.offlineVRRig.EnableHuntWatch(true);
                huntCompObj.SetActive(true);
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

                if (PageNumber > MaxPage) PageNumber = 0;
                if (PageNumber < 0) PageNumber = MaxPage;

                string[] pageTitles =
                {
            MenuTitle,
            "Disconnect",
            "Sticky Platforms",
            "NOCLIP",
            "FLY",
            "RPC Protection",
            "Grab Rig"
        };

                if (WatchCreditPage)
                {
                    PagesMove = false;
                    huntComp.text.text = "CREDITS\nBY: 2025Joe";
                }
                else
                {
                    PagesMove = true;
                    huntComp.text.text = pageTitles[PageNumber] + (PageNumber == 0 ? "" : $"   ({(PageNumber == 1 ? WatchMod1 : PageNumber == 2 ? WatchMod2 : PageNumber == 3 ? WatchMod3 : WatchMod4)})");
                    if (PageNumber > 0 && ControllerInputPoller.instance.leftControllerSecondaryButton && Time.time > Cooldown + 0.5f)
                    {
                        switch (PageNumber)
                        {
                            case 1: WatchMod1 = !WatchMod1; break;
                            case 2: WatchMod2 = !WatchMod2; break;
                            case 3: WatchMod3 = !WatchMod3; break;
                            case 4: WatchMod4 = !WatchMod4; break;
                            case 5: WatchMod5 = !WatchMod5; break;
                            case 6: WatchMod6 = !WatchMod6; break;
                        }

                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(64, true, 0.8f);
                        GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                        Cooldown = Time.time;
                    }
                    else if (PageNumber == 0 && ControllerInputPoller.instance.leftControllerSecondaryButton && Time.time > Cooldown + 0.5f)
                    {
                        WatchCreditPage = !WatchCreditPage;

                        GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(20, true, 0.8f);
                        GorillaTagger.Instance.StartVibration(true, GorillaTagger.Instance.tagHapticStrength / 2f, GorillaTagger.Instance.tagHapticDuration / 2f);
                        Cooldown = Time.time;
                    }
                }
                if (WatchMod1)
                {
                    PhotonNetwork.Disconnect();
                    WatchMod1 = false;
                }
                if (WatchMod2)
                {
                    StickyPlatforms.StickyPlatforms1();
                }
                if (WatchMod3)
                {
                    NoClip.NoClip1();
                }
                if (WatchMod4)
                {
                    Fly.Fly1();
                }
                if (WatchMod5)
                {
                    RPCProtection.Enable();
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError("Watch Prefix Error: " + ex);
            }
        }

        public static float Cooldown { get; private set; }
        public static bool PagesMove = true;
        public static bool WatchCreditPage;
        public static bool WatchMod1;
        public static bool WatchMod2;
        public static bool WatchMod3;
        public static bool WatchMod4;
        public static bool WatchMod5;
        public static bool WatchMod6;

        public static int PageNumber;
        public static int MaxPage = 5;
        public static string MenuTitle = "Encryptic Watch";
        public static Color WatchTextColor = Color.aliceBlue;

        public enum PhotonEventCodes
        {
            left_jump_photoncode = 69,
            right_jump_photoncode,
            left_jump_deletion,
            right_jump_deletion
        }

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

        public class ColorChanger : Watch.TimedBehaviour
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
