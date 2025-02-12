using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossFlamethrower
{
    //set the config label
    [Label("$Mods.BossFlamethrower.Config.Label")]
    public class Config : ModConfig
    {
        //make the config only run on the non client side
        public override ConfigScope Mode => ConfigScope.ServerSide;

        //get a public instance of the config
        public static Config Instance;

        //set the general headers
        [Header("$Mods.BossFlamethrower.Config.Header.GeneralOptions")]

        //set the length of the cooldown
        [Label("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Tooltip")]
        [DefaultValue(30)]
        public int FlamethrowerCooldown;

        //set the length of the duration
        [Label("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Tooltip")]
        [DefaultValue(10)]
        public int FlamethrowerDuration;

        //should the unit be in minutes?
        [Label("$Mods.BossFlamethrower.Config.UseMinutes.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseMinutes.Tooltip")]
        [DefaultValue(false)]
        public bool UseMinutes;

        //should cursed flames debuff be applied in PHM
        [Label("$Mods.BossFlamethrower.Config.CursedPHM.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.CursedPHM.Tooltip")]
        [DefaultValue(true)]
        public bool CursedPHM;

        //should curse flames be used at all
        [Label("$Mods.BossFlamethrower.Config.UseCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseCursed.Tooltip")]
        [DefaultValue(true)]
        public bool UseCursed;

        //how long should the flamethrower be
        [Label("$Mods.BossFlamethrower.Config.FlameDis.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlameDis.Tooltip")]
        [DefaultValue(6f)]
        [Range(1f, 25f)]
        public float FlameDis;

        //should moon lord use the flamethrower
        [Label("$Mods.BossFlamethrower.Config.MoonCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.MoonCursed.Tooltip")]
        [DefaultValue(false)]
        public bool MoonCursed;

        //how much extra damage should the flamethrower do
        [Label("$Mods.BossFlamethrower.Config.FlameDamMulti.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlameDamMulti.Tooltip")]
        [DefaultValue(1f)]
        [Range(0.1f, 2f)]
        public float FlameDamMulti;
    }

    //set the config label
    [Label("$Mods.BossFlamethrower.BossGUI.Label")]
    public class GUIConfig : ModConfig
    {
        //make the config only run on the client side
        public override ConfigScope Mode => ConfigScope.ClientSide;

        //get a public instance of the config
        public static GUIConfig Instance;

        [Header("$Mods.BossFlamethrower.BossGUI.Header.GeneralOptions")]

        //should the bar be displayed when a boss is alive
        [Label("$Mods.BossFlamethrower.BossGUI.DisplayBar.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.DisplayBar.Tooltip")]
        [DefaultValue(true)]
        public bool DisplayBar;

        //set the x pos of the bar
        [Label("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarX.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarX.Tooltip")]
        [DefaultValue(50)]
        [Slider()]
        [Range(0, 100)]
        public int FlamethrowerBarX;

        //set the y pos of the bar
        [Label("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarY.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarY.Tooltip")]
        [DefaultValue(3)]
        [Slider()]
        [Range(0, 100)]
        public int FlamethrowerBarY;
    }
}
