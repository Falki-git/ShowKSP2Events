using BepInEx.Logging;
using SpaceWarp.API.Assets;
using UnityEngine;

namespace ShowKSP2Events
{
    public static class Textures
    {
        private static ShowKSP2Events _plugin;
        private static readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("ShowKSP2Events.Textures");

        public static Texture2D PermaStickyActive;
        public static Texture2D PermaStickyInactive;
        public static Texture2D Export;
        public static Texture2D Settings;
        public static Texture2D Cross;
        public static Texture2D Plus;

        public static void Initialize(ShowKSP2Events plugin)
        {
            // Icons from https://icons8.com
            // Cross/Plus icons from K2D2 mod

            _plugin = plugin;

            PermaStickyActive = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/permasticky_active.png");
            PermaStickyInactive = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/permasticky_inactive.png");
            Export = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/export-30.png");
            Settings = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/settings-20.png");
            Cross = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/cross.png");
            Plus = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/plus.png");
        }

        private static Texture2D LoadTexture(string path)
        {
            try
            {
                return AssetManager.GetAsset<Texture2D>(path);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading texture with path: {path}. Full error:\n{ex}");
                return new Texture2D(20, 20);
            }
        }
    }
}