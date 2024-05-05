﻿using System.Numerics;
using Dalamud.Interface;
using Dalamud.Interface.Style;
using ImGuiNET;
using KamiLib.Classes;
using KamiLib.CommandManager;
using KamiLib.Configuration;
using KamiLib.TabBar;
using KamiLib.Window;
using SortaKinda.System;
using SortaKinda.Views.Tabs;

namespace SortaKinda.Views.Windows;

public class ConfigurationWindow : Window {
    private readonly AreaPaintController areaPaintController = new();

    private readonly TabBar tabBar = new("SortaKindaConfigTabBar", [
        new MainInventoryTab(),
        new ArmoryInventoryTab(),
        new GeneralConfigurationTab()
    ]);

    public ConfigurationWindow() : base("SortaKinda - Configuration Window", new Vector2(840.0f, 636.0f), true) {
        Flags |= ImGuiWindowFlags.NoScrollbar;
        Flags |= ImGuiWindowFlags.NoScrollWithMouse;
        
        TitleBarButtons.Add(new TitleBarButton {
            Icon = FontAwesomeIcon.Cog,
            ShowTooltip = () => ImGui.SetTooltip("Open Configuration Manager"),
            Click = _ => SortaKindaController.WindowManager.AddWindow(new ConfigurationManagerWindow()),
            IconOffset = new Vector2(2.0f, 2.0f),
        });

        SortaKindaController.CommandManager.RegisterCommand(new CommandHandler {
            Delegate = OpenConfigWindow,
            ActivationPath = "/",
        });
    }

    public override bool IsOpenAllowed() 
        => Service.ClientState.IsLoggedInNotPvP();

    public override void PrintOpenNotAllowed() 
        => Service.ChatGui.PrintError("The configuration menu cannot be opened while in a PvP area");

    public override bool DrawConditions()
        => Service.ClientState.IsLoggedInNotPvP();

    public override void PreDraw() 
        => StyleModelV1.DalamudStandard.Push();

    public override void Draw() {
        tabBar.Draw();
        areaPaintController.Draw();
    }

    public override void PostDraw() 
        => StyleModelV1.DalamudStandard.Pop();

    private void OpenConfigWindow(params string[] args) 
        => Toggle();
}