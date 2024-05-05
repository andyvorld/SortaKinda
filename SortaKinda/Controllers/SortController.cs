﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Dalamud.Interface;
using KamiLib.Classes;
using SortaKinda.Models;
using SortaKinda.Views.SortControllerViews;

namespace SortaKinda.System;

public class SortingRuleConfig {
    public List<SortingRule> Rules { get; set; } = [
        new SortingRule {
            Color = KnownColor.White.Vector(),
            Id = SortController.DefaultId,
            Name = "Unsorted"
        }
    ];
}

public class SortController {
    public const string DefaultId = "Default";
    
    public int SelectedRuleIndex = 0;
    
    public SortingRuleConfig RuleConfig { get; set; } = new();
    
    public SortingRule SelectedRule 
        => SelectedRuleIndex < RuleConfig.Rules.Count ? RuleConfig.Rules[SelectedRuleIndex] : DefaultRule;
    
    public SortControllerView? View { get; set; }

    private static SortingRule DefaultRule => new() {
        Id = DefaultId,
        Name = "Unsorted",
        Index = 0,
        Color = KnownColor.White.Vector()
    };

    public List<SortingRule> Rules 
        => RuleConfig.Rules;

    public void SortAllInventories() 
        => SortaKindaController.ModuleController.Sort();

    public void Load() {
        // RuleConfig = new SortingRuleConfig();
        RuleConfig = LoadConfig();

        TryMigrate();
        
        View = new SortControllerView(this);
        EnsureDefaultRule();
    }
    
    private void TryMigrate() {
        var needsSaving = false;
        
        foreach (var rule in Rules.Where(rule => rule.AllowedItemNames.Count != 0)) {
            foreach (var oldNameRule in rule.AllowedItemNames) {
                rule.AllowedNameRegexes.Add(new UserRegex(oldNameRule));
            }
                
            rule.AllowedItemNames.Clear();
            needsSaving = true;
        }

        if (needsSaving) {
            SaveConfig();
        }
    }

    public void Draw() => View?.Draw();

    public SortingRule GetRule(string id) => RuleConfig.Rules.FirstOrDefault(rule => rule.Id == id) ?? RuleConfig.Rules[0];

    private void EnsureDefaultRule() {
        if (RuleConfig.Rules.Count is 0) {
            RuleConfig.Rules.Add(DefaultRule);
        }

        if (RuleConfig.Rules[0] is not { Id: DefaultId, Name: "Unsorted", Index: 0 }) {
            RuleConfig.Rules[0] = DefaultRule;
        }
    }
    
    private SortingRuleConfig LoadConfig()
        => Service.PluginInterface.LoadCharacterFile(Service.ClientState.LocalContentId, "SortingRules.config.json", () => new SortingRuleConfig());

    public void SaveConfig() 
        => Service.PluginInterface.SaveCharacterFile(Service.ClientState.LocalContentId, "SortingRules.config.json", RuleConfig);
}