﻿using UnityEngine;
using UnityEngine.UI;
using HarmonyLib;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using UnityEngine.AzureSky;
using Steamworks;
using System.Collections;

public class RealisticMod : Mod
{

    private static RecipeManager recipeManager;
    private static MoreSpawnManager moreSpawnManager;
    private static DrawManager drawManager;

    Harmony harmony;
    static ModData otherMod = null;
    public static void StartModCheck(ModData md)
    {
        modInstance.StartCoroutine(WaitForModLoad(md));
    }

    static IEnumerator WaitForModLoad(ModData md)
    {
        while (md.modinfo == null)
            yield return new WaitForEndOfFrame();
        while (md.modinfo.modState == ModInfo.ModStateEnum.compiling || md.modinfo.modState == ModInfo.ModStateEnum.idle)
            yield return new WaitForEndOfFrame();
        if (md.modinfo.modState != ModInfo.ModStateEnum.running)
            yield break;
        while (md.modinfo.mainClass == null)
            yield return new WaitForEndOfFrame();
        var t = md.modinfo.mainClass.GetType();
        if (t.Name != "MoreTrashRedux")
            yield break;
        otherMod = md;
        yield break;
    }

    public static void EndModCheck(ModData md)
    {
        if (md == otherMod)
            otherMod = null;
    }

    public void Start()
    {
        Debug.Log("Mod RealisticMod will load.");
        foreach (ModData mod in HMLLibrary.ModManagerPage.modList)
            StartModCheck(mod);
        harmony = new Harmony("de.lpd.RealisticMod");
        harmony.PatchAll();

        // Recipe Manager
        recipeManager = new RecipeManager();

        // Spawner Manager (in Arbeit)
        moreSpawnManager = new MoreSpawnManager(0.50f, 1.00f, 0.30f, 0.80f);
        ChangeSpawnRate(0.02f);

        // Warnanzeige wenn der Hei angreift (in Arbeit)

        // 10x Wood (in Arbeit)

        // Loaded
        Debug.Log("Mod RealisticMod has been loaded!");
    }

    public void OnModUnload()
    {
        Debug.Log("Mod RealisticMod has been unloaded!");
    }

    static void ChangeSpawnRate(float multiplier)
    {
        if (otherMod == null)
            return;
        var access = Traverse.Create(otherMod.modinfo.mainClass);
        access.Field("WorldCheck").SetValue(true);
        access.Field("WorldDelay").SetValue(multiplier);
        access.Method("RemodifyAll").GetValue();
    }
}

[HarmonyPatch(typeof(HMLLibrary.BaseModHandler))]
public class Patch_ModLoader
{
    [HarmonyPatch("LoadMod")]
    [HarmonyPostfix]
    static void LoadMod(ModData moddata) => RealisticMod.StartModCheck(moddata);


    [HarmonyPatch("UnloadMod")]
    [HarmonyPostfix]
    static void UnloadMod(ModData moddata) => RealisticMod.EndModCheck(moddata);
}

public class RecipeManager
{

    public RecipeManager()
    {
        // Set Recipes
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Clay"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 2),
        }, CraftingCategory.Resources, 1, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Sand"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 1),
        }, CraftingCategory.Resources, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Brick_Wet"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Sand") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Clay") }, 1),
        }, CraftingCategory.Resources, 2, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Storage_Medium"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 5),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Nail") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 8),
        }, CraftingCategory.Other, 1, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_ItemNet"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 5),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 9),
        }, CraftingCategory.Other, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Rope"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 1),
        }, CraftingCategory.Resources, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Nail"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
        }, CraftingCategory.Resources, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Basic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 3),
        }, CraftingCategory.Decorations, 5, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Metal"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 8, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_FireBasket"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 2, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Fireplace"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 16),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 2),
        }, CraftingCategory.Decorations, 1, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Stone"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Resources, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Bolt"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap"), ItemManager.GetItemByName("MetalIngot") }, 2),
        }, CraftingCategory.Resources, 4, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("SharkBait"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 12),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plastic") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Raw_Pomfret"),
                ItemManager.GetItemByName("Raw_Herring"),
                ItemManager.GetItemByName("Raw_Macherel"),
                ItemManager.GetItemByName("Raw_Tilapia"),
                ItemManager.GetItemByName("Raw_Catfish"),
                ItemManager.GetItemByName("Raw_Salmon") }, 1),
        }, CraftingCategory.Tools, 2, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Plank"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic"), ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 1),
        }, CraftingCategory.Resources, 10, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Plastic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 2),
        }, CraftingCategory.Resources, 8, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Thatch"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 2),
        }, CraftingCategory.Resources, 6, true);
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Scrap"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 1),
        }, CraftingCategory.Resources, 6, true);
    }

    public static void SetRecipe(Item_Base item, CostMultiple[] cost, CraftingCategory category = CraftingCategory.Resources, int amountToCraft = 1, bool learnedFromBeginning = false)
    {
        //Debug.Log(item);
        Traverse recipe = Traverse.Create(item.settings_recipe);
        if (recipe != null)
        {
            recipe.Field("craftingCategory").SetValue(category);
            recipe.Field("amountToCraft").SetValue(amountToCraft);
            recipe.Field("learnedFromBeginning").SetValue(learnedFromBeginning);
            item.settings_recipe.NewCost = cost;
        }
        else
        {
            CreateRecipe(item, cost, category, amountToCraft, learnedFromBeginning);
        }
        //CreateRecipe(item, cost, category, amountToCraft, learnedFromBeginning);
    }

    public static void CreateRecipe(Item_Base item, CostMultiple[] cost, CraftingCategory category = CraftingCategory.Resources, int amountToCraft = 1, bool learnedFromBeginning = true)
    {
        Traverse.Create(item.settings_recipe).Field("newCostToCraft").SetValue(cost);
        Traverse.Create(item.settings_recipe).Field("learned").SetValue(false);
        Traverse.Create(item.settings_recipe).Field("learnedFromBeginning").SetValue(learnedFromBeginning);
        Traverse.Create(item.settings_recipe).Field("craftingCategory").SetValue(category);
        Traverse.Create(item.settings_recipe).Field("amountToCraft").SetValue(amountToCraft);
    }

}

public class MoreSpawnManager
{

    private static Interval_Float defaultTime;
    private static float timer = 10f;
    private static ObjectSpawnerManager spawner;
    private static ObjectSpawnerAssetSettings plankSpawnerSettings, itemSpawnerSettings;

    public MoreSpawnManager(float min_all, float max_all, float min_wood, float max_food)
    {

    }

}

public class DrawManager {

    public void drawText(Vector2 pos, string text)
    {
        /*Text GUIText = CanavasHelper.transform.GetText(text);
        GUIText.rectTransform.localPosition = pos;*/
    }

}