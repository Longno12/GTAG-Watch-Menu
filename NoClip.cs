using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Watch_Menu.mods
{
    internal class NoClip
    {
        public static bool noc = false;
        public static void NoClip1()
        {
            if (ControllerInputPoller.instance.rightControllerIndexFloat == 1f)
            {
                if (noc == false)
                {
                    noc = true;
                    foreach (MeshCollider p in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    {
                        p.enabled = false;
                    }
                }
            }
            else
            {
                if (noc == true)
                {
                    noc = false;
                    foreach (MeshCollider p in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    {
                        p.enabled = true;
                    }
                }
            }
        }
    }
}
