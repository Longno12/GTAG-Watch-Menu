using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static QRCoder.PayloadGenerator;

namespace Watch_Menu.mods
{
    internal class Grab_Rig
    {
        public static bool Exceptions = false;
        public static bool stuff = false;
        public static float laggyDelay;
        public static void LaggyRig()
        {
            Exceptions = true;
            if (Time.time > laggyDelay)
            {
                GorillaTagger.Instance.offlineVRRig.enabled = true;
                stuff = true;
                laggyDelay = Time.time + 0.5f;
            }
            else
            {
                if (stuff)
                {
                    stuff = false;
                }
                else
                {
                    GorillaTagger.Instance.offlineVRRig.enabled = false;
                }
            }
        }
    }
}
