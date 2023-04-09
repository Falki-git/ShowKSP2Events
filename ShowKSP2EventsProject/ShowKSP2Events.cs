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

    private MessageListener _messageListener = new ();
    private UI _ui = new();

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

        _messageListener.InitializeSubscriptions();
        Styles.InitializeStyles();
    }

    private void OnGUI()
    {
        GUI.skin = Skins.ConsoleSkin;

        if (_isWindowOpen)
            _ui.DrawMessageListener(_messageListener);
    }

    private void Update()
    {
        try
        {
            _messageListener.CheckStickies();
            _messageListener.CheckStales();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Collection was modified"))
        {
            // Sometimes update will fail because an event is in the process of modifying the MessageListener.Messages list
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
        }
    }
}