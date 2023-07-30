using System;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace BossFlamethrower
{
    [Label("$Mods.BossFlamethrower.Config.Label")]
    public class Config : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static Config Instance;

        [Header("$Mods.BossFlamethrower.Config.Header.GeneralOptions")]

        [Label("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerCooldown.Tooltip")]
        [DefaultValue(30)]
        public int FlamethrowerCooldown;

        [Label("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlamethrowerDuration.Tooltip")]
        [DefaultValue(10)]
        public int FlamethrowerDuration;

        [Label("$Mods.BossFlamethrower.Config.UseMinutes.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseMinutes.Tooltip")]
        [DefaultValue(false)]
        public bool UseMinutes;

        [Label("$Mods.BossFlamethrower.Config.CursedPHM.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.CursedPHM.Tooltip")]
        [DefaultValue(true)]
        public bool CursedPHM;

        [Label("$Mods.BossFlamethrower.Config.UseCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.UseCursed.Tooltip")]
        [DefaultValue(true)]
        public bool UseCursed;

        [Label("$Mods.BossFlamethrower.Config.FlameDis.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlameDis.Tooltip")]
        [DefaultValue(6f)]
        [Range(1f, 25f)]
        public float FlameDis;

        [Label("$Mods.BossFlamethrower.Config.MoonCursed.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.MoonCursed.Tooltip")]
        [DefaultValue(false)]
        public bool MoonCursed;

        [Label("$Mods.BossFlamethrower.Config.FlameDamMulti.Label")]
        [Tooltip("$Mods.BossFlamethrower.Config.FlameDamMulti.Tooltip")]
        [DefaultValue(1f)]
        [Range(0.1f, 2f)]
        public float FlameDamMulti;
    }

    [Label("$Mods.BossFlamethrower.BossGUI.Label")]
    public class GUIConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
        public static GUIConfig Instance;

        [Header("$Mods.BossFlamethrower.BossGUI.Header.GeneralOptions")]

        [Label("$Mods.BossFlamethrower.BossGUI.DisplayBar.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.DisplayBar.Tooltip")]
        [DefaultValue(true)]
        public bool DisplayBar;

        [Label("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarX.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarX.Tooltip")]
        [DefaultValue(50)]
        [Slider()]
        [Range(0, 100)]
        public int FlamethrowerBarX;

        [Label("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarY.Label")]
        [Tooltip("$Mods.BossFlamethrower.BossGUI.FlamethrowerBarY.Tooltip")]
        [DefaultValue(3)]
        [Slider()]
        [Range(0, 100)]
        public int FlamethrowerBarY;
    }
}
