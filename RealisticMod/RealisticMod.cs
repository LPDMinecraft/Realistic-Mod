using UnityEngine;
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

    private RecipeManager recipeManager;
    private ItemStackAmountManager itemStackAmountManager;
    private RaftCounter counter;
    private TextManager textManager;

    public void Start()
    {
        Debug.Log("Mod RealisticMod will load.");

        // Recipe Manager
        recipeManager = new RecipeManager();
        recipeManager.registerAllRecipes();

        // Set ItemAmount
        itemStackAmountManager = new ItemStackAmountManager();
        itemStackAmountManager.registerAllItems();

        // TextManager
        textManager = new TextManager();

        // Raft Counter
        counter = new RaftCounter();

        // Loaded
        Debug.Log("Mod RealisticMod has been loaded!");
    }

    public void OnModUnload()
    {
        Debug.Log("Mod RealisticMod has been unloaded!");
    }

    

}

public class RecipeManager
{

    // Set Recipes
    public void registerAllRecipes()
    {
        // 2 Stone > 1 Clay
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Clay"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 2),
        }, CraftingCategory.Resources, 1, true);
        // 1 Stone > 4 Sand
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Sand"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 1),
        }, CraftingCategory.Resources, 4, true);
        // 1 Clay & 1 Sand > 2 Bricks
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Brick_Wet"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Sand") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Clay") }, 1),
        }, CraftingCategory.Resources, 2, true);
        // 8 Planks & 4 Nail & 8 Rope > 1 Storage Medium
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Storage_Medium"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Nail") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 8),
        }, CraftingCategory.Other, 1, true);
        // 6 Planks & 6 Nail & 12 Rope > 2 Item Net
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_ItemNet"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 6),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 12),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Nail") }, 6),
        }, CraftingCategory.Other, 2, true);
        // 2 Thatch > 4 Rope
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Rope"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 2),
        }, CraftingCategory.Resources, 4, true);
        // 1 Scrap > 4 Nail
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Nail"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
        }, CraftingCategory.Resources, 4, true);
        // 1 Scrap & 3 Plank > 2 Lantern Basic
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Basic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 3),
        }, CraftingCategory.Decorations, 2, true);
        // 1 Metal & 4 Plank & 1 Lantern Basic > 1 Lantern Metal
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Metal"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 1, true);
        // 2 Scrap & 4 Plank & 1 Lantern Basic > 1 Lantern Metal
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_FireBasket"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 2, true);
        // 2 Scrap & 2 Lantern Basic > Lantern Fireplace
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Fireplace"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 16),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 2),
        }, CraftingCategory.Decorations, 1, true);
        // 8 Plank & 1 Lantern Basic > 4 Stone
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Stone"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Resources, 4, true);
        // 2 Scrap > 4 Bolt
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Bolt"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 2),
        }, CraftingCategory.Resources, 4, true);
        // 1 MetalIngot > 4 Bolt
        RecipeManager.AddRecipe(ItemManager.GetItemByName("Bolt"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 1),
        }, CraftingCategory.Resources, 4, true);
        // 12 Rope & 8 Plank & 1 Fish > 1 SharkBait
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
        // 4 Plank & 1 (Latern Basic || Lantern Metal) > 6 Plank
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Plank"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic"), ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
        }, CraftingCategory.Resources, 6, true);
        // 4 Rope & 2 Thatch & 1 Plank > 4 Plastic
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Plastic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 1),
        }, CraftingCategory.Resources, 4, true);
        // 4 Plank & 1 Lantern Basic > 2 Scrap
        RecipeManager.SetRecipe(ItemManager.GetItemByName("Scrap"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
        }, CraftingCategory.Resources, 2, true);
    }

    public void SetRecipe(Item_Base item, CostMultiple[] cost, CraftingCategory category = CraftingCategory.Resources, int amountToCraft = 1, bool learnedFromBeginning = false)
    {
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
    }
    public void CreateRecipe(Item_Base item, CostMultiple[] cost, CraftingCategory category = CraftingCategory.Resources, int amountToCraft = 1, bool learnedFromBeginning = true)
    {
        Traverse.Create(item.settings_recipe).Field("newCostToCraft").SetValue(cost);
        Traverse.Create(item.settings_recipe).Field("learned").SetValue(false);
        Traverse.Create(item.settings_recipe).Field("learnedFromBeginning").SetValue(learnedFromBeginning);
        Traverse.Create(item.settings_recipe).Field("craftingCategory").SetValue(category);
        Traverse.Create(item.settings_recipe).Field("amountToCraft").SetValue(amountToCraft);
    }

}
public class ItemStackAmountManager
{
    public void registerAllItems()
    {
        setAmount("Plank", 50);
        setAmount("Bucket_Milk", 20);
        setAmount("Claybowl_CoconutChicken", 20);
        setAmount("Claybowl_FishStew", 20);
        setAmount("Claybowl_HeadBroth", 20);
        setAmount("Claybowl_Leftover", 20);
        setAmount("Claybowl_SimpleFishStew", 20);
        setAmount("Claybowl_RootVegetableSoup", 20);
        setAmount("ClayPlate_CatfishDeluxe", 20);
        setAmount("ClayPlate_DrumstickWithJam", 20);
        setAmount("ClayPlate_FruitCompot", 20);
        setAmount("ClayPlate_MushroomOmelette", 20);
        setAmount("ClayPlate_SalmonSalad", 20);
        setAmount("Placeable_GiantClam", 20);
        setAmount("ClayPlate_SharkDinner", 20);
        setAmount("MetalDetector", 20);
        setAmount("", 20);
        setAmount("", 20);
        setAmount("", 20);
    }
    public void setAmount(string item, int value)
    {
        Traverse.Create(ItemManager.GetItemByName(item).settings_Inventory).Field("stackSize").SetValue(value);
    }

}
public class RaftCounter
{
    public void printItemsCounts(string text, IDictionary<string, int> itemsCounts)
    {
        Debug.Log(text);
        foreach (KeyValuePair<string, int> item in itemsCounts)
        {
            Debug.Log("- " + item.Key + " : x" + item.Value);
        }
    }

    [ConsoleCommand(name: "raftCost", docs: "List all the ressources needed to build your current raft.")]
    public static void raftCommand(string[] args)
    {
        IDictionary<string, int> itemsRequired = new Dictionary<string, int>();
        IDictionary<string, int> itemsIgnored = new Dictionary<string, int>();

        var blocks = UnityEngine.Object.FindObjectsOfType<Block>();
        for (int i = 0; i < blocks.Length; i++)
        {
            registerBlock(blocks[i], itemsRequired, itemsIgnored);
        }
        if (itemsIgnored.ContainsKey("RespawnPointBed"))
        {
            itemsIgnored.Remove("RespawnPointBed");
        }

        printItemsCounts("Required items : ", itemsRequired);

        if (itemsIgnored.Count == 0) return;
        printItemsCounts("Items ignored in count : ", itemsIgnored);
    }

    public void registerBlock(Block block, IDictionary<string, int> itemsRequired, IDictionary<string, int> itemsIgnored)
    {
        Item_Base baseItem = block.buildableItem;
        string blockName = "";
        if (baseItem == null)
        {
            blockName = block.name.Replace("(Clone)", "");
            baseItem = ItemManager.GetItemByName(blockName);
        }

        if (baseItem == null)
        {
            if (itemsIgnored.ContainsKey(blockName))
            {
                itemsIgnored[blockName]++;
            }
            else
            {
                itemsIgnored.Add(blockName, 1);
            }
            return;
        }

        if (block.Reinforced)
        {
            string metalIngot = "MetalIngot";
            if (!itemsRequired.ContainsKey(metalIngot))
            {
                itemsRequired.Add(metalIngot, 1);
            }
            else
            {
                itemsRequired[metalIngot] += 1;
            }

            string nail = "Nail";
            if (!itemsRequired.ContainsKey(nail))
            {
                itemsRequired.Add(nail, 2);
            }
            else
            {
                itemsRequired[nail] += 2;
            }
        }
        ItemInstance_Recipe recipe = baseItem.settings_recipe;
        CostMultiple[] costs = recipe.NewCost;
        if (costs == null) return;
        if (costs.Length == 0) return;

        for (int i = 0; i < costs.Length; i++)
        {
            Item_Base[] items = costs[i].items;
            int amount = costs[i].amount;
            if (items.Length == 0) continue;
            for (int j = 0; j < items.Length; j++)
            {
                if (itemsRequired.ContainsKey(items[j].name))
                {
                    itemsRequired[items[j].name] += amount;
                }
                else
                {
                    itemsRequired.Add(items[j].name, amount);
                }
            }

        }
    }
}
public class TextManager {
    public GameObject CreateText(Transform canvas_transform, float x, float y, string text_to_print, int font_size, Color text_color, float width, float height, Font font, string name = "Text")
    {
        GameObject UItextGO = new GameObject("Text");
        UItextGO.transform.SetParent(canvas_transform, false);
        RectTransform trans = UItextGO.AddComponent<RectTransform>();
        trans.sizeDelta = new Vector2(width, height);
        trans.anchoredPosition = new Vector2(x, y);
        Text text = UItextGO.AddComponent<Text>();
        text.text = text_to_print;
        text.font = font;
        text.fontSize = font_size;
        text.color = text_color;
        text.name = name;
        Shadow shadow = UItextGO.AddComponent<Shadow>();
        shadow.effectColor = new Color();
        return UItextGO;
    }
    public void AddTextShadow(GameObject textObject, Color shadowColor, Vector2 shadowOffset)
    {
        Shadow shadow = textObject.AddComponent<Shadow>();
        shadow.effectColor = shadowColor;
        shadow.effectDistance = shadowOffset;
    }
    public void CopyTextShadow(GameObject textObject, GameObject shadowSource)
    {
        Shadow sourcesShadow = shadowSource.GetComponent<Shadow>();
        if (sourcesShadow == null)
            sourcesShadow = shadowSource.GetComponentInChildren<Shadow>();
        AddTextShadow(textObject, sourcesShadow.effectColor, sourcesShadow.effectDistance);
    }
    public GameObject CreateLine(Canvas canvas, float length, float width, float x, float y, Color lineColor, float angle)
    {
        GameObject lineContainer = new GameObject();
        lineContainer.transform.SetParent(canvas.transform, false);
        RectTransform trans = lineContainer.AddComponent<RectTransform>();
        trans.sizeDelta = canvas.pixelRect.size;
        trans.anchoredPosition = new Vector2(0, 0);
        UILine.setCreationValues(new Vector2(x, y), length, width, angle);
        UILine lineObj = lineContainer.AddComponent<UILine>();
        lineObj.color = lineColor;
        return lineContainer;
    }
}
