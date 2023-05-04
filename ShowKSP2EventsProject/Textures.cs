using BepInEx.Logging;
using SpaceWarp.API.Assets;
using UnityEngine;

namespace ShowKSP2Events
{
    internal static class Textures
    {
        private static ShowKSP2Events _plugin;
        private static readonly ManualLogSource _logger = BepInEx.Logging.Logger.CreateLogSource("ShowKSP2Events.Textures");

        internal static Texture2D PermaStickyActive;
        internal static Texture2D PermaStickyInactive;
        internal static Texture2D Save;
        internal static Texture2D SaveSuccess;
        internal static Texture2D Settings;
        internal static Texture2D Close;

        internal static void Initialize(ShowKSP2Events plugin)
        {
            // Icons from https://icons8.com

            _plugin = plugin;

            PermaStickyActive = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/permasticky_active.png");
            PermaStickyInactive = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/permasticky_inactive.png");
            Save = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/save-30.png");
            SaveSuccess = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/save-success-30.png");
            Settings = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/settings-20.png");
            Close = LoadTexture($"{_plugin.SpaceWarpMetadata.ModID}/images/close-15.png");
        }

        private static Texture2D LoadTexture(string path)
        {
            try
            {
                return AssetManager.GetAsset<Texture2D>(path);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading texture with path: {path}. Full error: \n{ex}");
                return new Texture2D(20, 20);
            }
        }
    }
}