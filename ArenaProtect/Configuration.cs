using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using UnityEngine;

namespace ArenaProtect
{
    public class Configuration : IRocketPluginConfiguration
    {
        public bool Vanish;
        public bool GodMode;
        public int Seconds;
        public Color color;

        public void LoadDefaults()
        {
            Vanish = false;
            GodMode = true;
            Seconds = 10;
            color = Color.yellow;
        }
    }
}
