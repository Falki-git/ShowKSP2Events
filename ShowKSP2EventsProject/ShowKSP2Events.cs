using BepInEx;
using HarmonyLib;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.Game;
using SpaceWarp.API.Game.Extensions;
using SpaceWarp.API.UI;
using SpaceWarp.API.UI.Appbar;
using UnityEngine;
using KSP.Game;
using KSP.Messages;
using System.Reflection;

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
    private Rect _windowRect = new Rect(650, 140, 400, 100);

    private const string ToolbarFlightButtonID = "BTN-ShowKSP2EventsFlight";
    private const string ToolbarOABButtonID = "BTN-ShowKSP2EventsOAB";

    public static ShowKSP2Events Instance { get; set; }

    private GameInstance _gameInstance;

    /// <summary>
    /// Runs when the mod is first initialized.
    /// </summary>
    public override void OnInitialized()
    {
        base.OnInitialized();

        Instance = this;

        // Register Flight AppBar button
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

        // Register OAB AppBar Button
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

        // Register all Harmony patches in the project
        Harmony.CreateAndPatchAll(typeof(ShowKSP2Events).Assembly);

        // Try to get the currently active vessel, set its throttle to 100% and toggle on the landing gear
        try
        {
            var currentVessel = Vehicle.ActiveVesselVehicle;
            if (currentVessel != null)
            {
                currentVessel.SetMainThrottle(1.0f);
                currentVessel.SetGearState(true);
            }
        }
        catch (Exception e) {}
        
        // Fetch a configuration value or create a default one if it does not exist
        var defaultValue = "my_value";
        var configValue = Config.Bind<string>("Settings section", "Option 1", defaultValue, "Option description");
        
        // Log the config value into <KSP2 Root>/BepInEx/LogOutput.log
        Logger.LogInfo($"Option 1: {configValue.Value}");

        SubscribeToEvents();
    }
    
    /// <summary>
    /// Draws a simple UI window when <code>this._isWindowOpen</code> is set to <code>true</code>.
    /// </summary>
    private void OnGUI()
    {
        _OnGUI++;

        // Set the UI
        GUI.skin = Skins.ConsoleSkin;

        if (_isWindowOpen)
        {
            _windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                _windowRect,
                FillWindow,
                "ShowKSP2Events"            
            );
        }

        //GameStateConfiguration gameStateConfiguration = GameManager.Instance.Game.GlobalGameState.GetGameState();
        //if (gameStateConfiguration.IsObjectAssembly)
        //{
        //    _windowRect = GUILayout.Window(
        //        GUIUtility.GetControlID(FocusType.Passive),
        //        FillWindow,
        //        DrawTestWindowMethod,
        //        "ShowKSP2Events",
        //        GUILayout.Height(400),
        //        GUILayout.Width(100)
        //        );
        //}
    }

    private void Update()
    {
        _Update++;

        //var assembly = Assembly.Load("Assembly-CSharp");

        //var messageTypes = assembly.GetTypes()
        //    .Where(t => typeof(MessageCenterMessage).IsAssignableFrom(t))
        //    .ToList();

        //foreach (var messageType in messageTypes)
        //{
        //    Console.WriteLine(messageType.Name);
        //}
    }

    private void SubscribeToEvents()
    {
        _gameInstance = GameManager.Instance?.Game;
        _gameInstance.Messages.Subscribe<GameStateChangedMessage>(new Action<MessageCenterMessage>(this.GameStateChangedMessage));
        _gameInstance.Messages.Subscribe<GameStateEnteredMessage>(new Action<MessageCenterMessage>(this.GameStateEnteredMessage));
        _gameInstance.Messages.Subscribe<GameStateLeftMessage>(new Action<MessageCenterMessage>(this.GameStateLeftMessage));
        _gameInstance.Messages.Subscribe<KSCLoadedMessage>(new Action<MessageCenterMessage>(this.KSCLoadedMessage));
        _gameInstance.Messages.Subscribe<MapInitializedMessage>(new Action<MessageCenterMessage>(this.MapInitializedMessage));
        _gameInstance.Messages.Subscribe<MapModeChangedMessage>(new Action<MessageCenterMessage>(this.MapModeChangedMessage));
        _gameInstance.Messages.Subscribe<MapShutdownMessage>(new Action<MessageCenterMessage>(this.MapShutdownMessage));
        _gameInstance.Messages.Subscribe<MapViewEnteredMessage>(new Action<MessageCenterMessage>(this.MapViewEnteredMessage));
        _gameInstance.Messages.Subscribe<MapViewLeftMessage>(new Action<MessageCenterMessage>(this.MapViewLeftMessage));
        _gameInstance.Messages.Subscribe<OABLoadedMessage>(new Action<MessageCenterMessage>(this.OABLoadedMessage));
        _gameInstance.Messages.Subscribe<OABLoadFinalizedMessage>(new Action<MessageCenterMessage>(this.OABLoadFinalizedMessage));
        _gameInstance.Messages.Subscribe<OABMainAssemblyChanged>(new Action<MessageCenterMessage>(this.OABMainAssemblyChanged));
        _gameInstance.Messages.Subscribe<PartSeparatedMessage>(new Action<MessageCenterMessage>(this.PartSeparatedMessage));
        _gameInstance.Messages.Subscribe<PartPlacedOnMainAssemblyMessage>(new Action<MessageCenterMessage>(this.PartPlacedOnMainAssemblyMessage));
        _gameInstance.Messages.Subscribe<PartPlacedMessage>(new Action<MessageCenterMessage>(this.PartPlacedMessage));
        _gameInstance.Messages.Subscribe<PartRemovedFromMainAssemblyMessage>(new Action<MessageCenterMessage>(this.PartRemovedFromMainAssemblyMessage));
        _gameInstance.Messages.Subscribe<PartStageChangedMessage>(new Action<MessageCenterMessage>(this.PartStageChangedMessage));
        _gameInstance.Messages.Subscribe<StageAddedMessage>(new Action<MessageCenterMessage>(this.StageAddedMessage));
        _gameInstance.Messages.Subscribe<StateChangedMessage>(new Action<MessageCenterMessage>(this.StateChangedMessage));

        _gameInstance.Messages.Subscribe<TrackingStationLoadedMessage>(new Action<MessageCenterMessage>(this.TrackingStationLoadedMessage));
        _gameInstance.Messages.Subscribe<TrackingStationUnloadedMessage>(new Action<MessageCenterMessage>(this.TrackingStationUnloadedMessage));

        _gameInstance.Messages.Subscribe<VesselDeltaVCalculationMessage>(new Action<MessageCenterMessage>(this.VesselDeltaVCalculationMessage));

        _gameInstance.Messages.Subscribe<VesselLoadedMessage>(new Action<MessageCenterMessage>(this.VesselLoadedMessage));

        _gameInstance.Messages.Subscribe<CloseEngineersReportWindowMessage>(new Action<MessageCenterMessage>(this.CloseEngineersReportWindowMessage));
        _gameInstance.Messages.Subscribe<StagingMessageBase>(new Action<MessageCenterMessage>(this.StagingMessageBase));
    }

    /// <summary>
    /// Defines the content of the UI window drawn in the <code>OnGui</code> method.
    /// </summary>
    /// <param name="windowID"></param>
    private void FillWindow(int windowID)
    {
        GUILayout.BeginVertical();
        GUILayout.Label($"_OnGUI: {_OnGUI}");
        GUILayout.Label($"_Update: {_Update}");
        GUILayout.Label($"_VesselDeltaVCalculationMessage: {_VesselDeltaVCalculationMessage}");
        GUILayout.Label($"_VesselDeltaVCalculationMessageOabOnly: {_VesselDeltaVCalculationMessageOabOnly}");
        GUILayout.Label($"_GameStateChangedMessage: {_GameStateChangedMessage}");
        GUILayout.Label($"_GameStateEnteredMessage: {_GameStateEnteredMessage}");
        GUILayout.Label($"_GameStateLeftMessage: {_GameStateLeftMessage}");
        GUILayout.Label($"_KSCLoadedMessage: {_KSCLoadedMessage}");
        GUILayout.Label($"_MapInitializedMessage: {_MapInitializedMessage}");
        GUILayout.Label($"_MapModeChangedMessage: {_MapModeChangedMessage}");
        GUILayout.Label($"_MapShutdownMessage: {_MapShutdownMessage}");
        GUILayout.Label($"_MapViewEnteredMessage: {_MapViewEnteredMessage}");
        GUILayout.Label($"_MapViewLeftMessage: {_MapViewLeftMessage}");
        GUILayout.Label($"_OABLoadedMessage: {_OABLoadedMessage}");
        GUILayout.Label($"_OABLoadFinalizedMessage: {_OABLoadFinalizedMessage}");
        GUILayout.Label($"_OABMainAssemblyChanged: {_OABMainAssemblyChanged}");
        GUILayout.Label($"_PartSeparatedMessage: {_PartSeparatedMessage}");
        GUILayout.Label($"_PartPlacedOnMainAssemblyMessage: {_PartPlacedOnMainAssemblyMessage}");
        GUILayout.Label($"_PartPlacedMessage: {_PartPlacedMessage}");
        GUILayout.Label($"_PartRemovedFromMainAssemblyMessage: {_PartRemovedFromMainAssemblyMessage}");
        GUILayout.Label($"_PartStageChangedMessage: {_PartStageChangedMessage}");
        GUILayout.Label($"_StageAddedMessage: {_StageAddedMessage}");
        GUILayout.Label($"_StateChangedMessage: {_StateChangedMessage}");
        GUILayout.Label($"_TrackingStationLoadedMessage: {_TrackingStationLoadedMessage}");
        GUILayout.Label($"_TrackingStationUnloadedMessage: {_TrackingStationUnloadedMessage}");
        GUILayout.Label($"_VesselLoadedMessage: {_VesselLoadedMessage}");
        GUILayout.Label($"_CloseEngineersReportWindowMessage: {_CloseEngineersReportWindowMessage}");
        GUILayout.Label($"_StagingMessageBase: {_StagingMessageBase}");
        GUILayout.EndVertical();
        GUI.DragWindow(new Rect(0, 0, 2560, 1440));
    }

    #region MyPrivateFields
    private int _OnGUI = 0;
    private int _Update = 0;
    private int _VesselDeltaVCalculationMessage = 0;
    private int _VesselDeltaVCalculationMessageOabOnly = 0;
    private int _GameStateChangedMessage = 0;
    private int _GameStateEnteredMessage = 0;
    private int _GameStateLeftMessage = 0;
    private int _KSCLoadedMessage = 0;
    private int _MapInitializedMessage = 0;
    private int _MapModeChangedMessage = 0;
    private int _MapShutdownMessage = 0;
    private int _MapViewEnteredMessage = 0;
    private int _MapViewLeftMessage = 0;
    private int _OABLoadedMessage = 0;
    private int _OABLoadFinalizedMessage = 0;
    private int _OABMainAssemblyChanged = 0;
    private int _PartSeparatedMessage = 0;
    private int _PartPlacedOnMainAssemblyMessage = 0;
    private int _PartPlacedMessage = 0;
    private int _PartRemovedFromMainAssemblyMessage = 0;
    private int _PartStageChangedMessage = 0;
    private int _StageAddedMessage = 0;
    private int _StateChangedMessage = 0;
    private int _TrackingStationLoadedMessage = 0;
    private int _TrackingStationUnloadedMessage = 0;
    private int _VesselLoadedMessage = 0;
    private int _CloseEngineersReportWindowMessage = 0;
    private int _StagingMessageBase = 0;
    #endregion


    private void GameStateChangedMessage(MessageCenterMessage obj)
    {
        _GameStateChangedMessage++;
    }
    private void GameStateEnteredMessage(MessageCenterMessage obj)
    {
        _GameStateEnteredMessage++;
    }
    private void GameStateLeftMessage(MessageCenterMessage obj)
    {
        _GameStateLeftMessage++;
    }
    private void KSCLoadedMessage(MessageCenterMessage obj)
    {
        _KSCLoadedMessage++;
    }
    private void MapInitializedMessage(MessageCenterMessage obj)
    {
        _MapInitializedMessage++;
    }
    private void MapModeChangedMessage(MessageCenterMessage obj)
    {
        _MapModeChangedMessage++;
    }
    private void MapShutdownMessage(MessageCenterMessage obj)
    {
        _MapShutdownMessage++;
    }
    private void MapViewEnteredMessage(MessageCenterMessage obj)
    {
        _MapViewEnteredMessage++;
    }
    private void MapViewLeftMessage(MessageCenterMessage obj)
    {
        _MapViewLeftMessage++;
    }
    private void OABLoadedMessage(MessageCenterMessage obj)
    {
        _OABLoadedMessage++;
    }
    private void OABLoadFinalizedMessage(MessageCenterMessage obj)
    {
        _OABLoadFinalizedMessage++;
    }
    private void OABMainAssemblyChanged(MessageCenterMessage obj)
    {
        _OABMainAssemblyChanged++;
    }
    private void PartSeparatedMessage(MessageCenterMessage obj)
    {
        _PartSeparatedMessage++;
    }
    private void PartPlacedOnMainAssemblyMessage(MessageCenterMessage obj)
    {
        _PartPlacedOnMainAssemblyMessage++;
    }
    private void PartPlacedMessage(MessageCenterMessage obj)
    {
        _PartPlacedMessage++;
    }
    private void PartRemovedFromMainAssemblyMessage(MessageCenterMessage obj)
    {
        _PartRemovedFromMainAssemblyMessage++;
    }
    private void PartStageChangedMessage(MessageCenterMessage obj)
    {
        _PartStageChangedMessage++;
    }
    private void StageAddedMessage(MessageCenterMessage obj)
    {
        _StageAddedMessage++;
    }
    private void StateChangedMessage(MessageCenterMessage obj)
    {
        _StateChangedMessage++;
    }
    private void TrackingStationLoadedMessage(MessageCenterMessage obj)
    {
        _TrackingStationLoadedMessage++;
    }
    private void TrackingStationUnloadedMessage(MessageCenterMessage obj)
    {
        _TrackingStationUnloadedMessage++;
    }
    private void VesselDeltaVCalculationMessage(MessageCenterMessage obj)
    {
        _VesselDeltaVCalculationMessage++;

        VesselDeltaVCalculationMessage msg = (VesselDeltaVCalculationMessage)obj;
        if (msg.DeltaVComponent.Ship == null || !msg.DeltaVComponent.Ship.IsLaunchAssembly()) return;
        
        _VesselDeltaVCalculationMessageOabOnly++;
    }
    private void VesselLoadedMessage(MessageCenterMessage obj)
    {
        _VesselLoadedMessage++;
    }
    private void CloseEngineersReportWindowMessage(MessageCenterMessage obj)
    {
        _CloseEngineersReportWindowMessage++;
    }
    private void StagingMessageBase(MessageCenterMessage obj)
    {
        _StagingMessageBase++;
    }


}
