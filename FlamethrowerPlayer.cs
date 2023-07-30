using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace BossFlamethrower
{
    public class FlamethrowerPlayer : ModPlayer
    {
        public bool boss = false;
        public bool flame = false;
        public int cooldown = 0;
        public int cooldownMax = 0;
        public int duration = 0;
        public int durationMax = 0;
    }
}
