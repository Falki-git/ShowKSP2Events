using BepInEx;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;

namespace ShowKSP2Events;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
public class ShowKSP2Events : BaseSpaceWarpPlugin
{
    // These are useful in case some other mod wants to add a dependency to this one
    public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
    public const string ModName = MyPluginInfo.PLUGIN_NAME;
    public const string ModVer = MyPluginInfo.PLUGIN_VERSION;
    
    private bool _isWindowOpen;

    private const string ToolbarFlightButtonID = "BTN-ShowKSP2EventsFlight";
    private const string ToolbarOABButtonID = "BTN-ShowKSP2EventsOAB";

    public override void OnInitialized()
    {
        base.OnInitialized();

        Appbar.RegisterAppButton(
            "ShowKSP2Events",
            ToolbarFlightButtonID,
            AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
            isOpen =>
            {
                _isWindowOpen = isOpen;
                GameObject.Find(ToolbarFlightButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
            }
        );

        Appbar.RegisterOABAppButton(
            "ShowKSP2Events",
            ToolbarOABButtonID,
            AssetManager.GetAsset<Texture2D>($"{SpaceWarpMetadata.ModID}/images/icon.png"),
            isOpen =>
            {
                _isWindowOpen = isOpen;
                GameObject.Find(ToolbarOABButtonID)?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isOpen);
            }
        );
        
        Styles.Initialize();
        Textures.Initialize(this);
        MessageListener.Instance.InitializeSubscriptions();
        Settings.Load();
    }

    private void OnGUI()
    {
        GUI.skin = Skins.ConsoleSkin;

        if (_isWindowOpen)
            UI.Instance.DrawMessageListener();
    }

    private void Update()
    {
        try
        {
            MessageListener.Instance.CheckStickies();
            MessageListener.Instance.CheckStales();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Collection was modified"))
        {
            // Sometimes update will fail because an event is in the process of modifying the MessageListener.Messages list
            // or we're shuffling the list of messages
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}