using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System;

public class RealisticMod : Mod
{

    private RecipeManager recipeManager;
    private RaftCounter counter;
    private Harmony harmony;
    public static int ItemCollectorSize;

    public void Start()
    {
        Debug.Log("Mod RealisticMod will load.");

        // Edit maxmium capaticy in item net to 40
        ChangeItemNetMaxValue(40);
        Logger.log("Edited the maxmium capicity of itemnets to 50");

        // Listener and Command Manager
        harmony = new Harmony("de.lcraft.raft.realisticmod.listeners");
        harmony.PatchAll();

        // Recipe Manager
        recipeManager = new RecipeManager();
        recipeManager.registerAllRecipes(this);

        // Raft Counter
        counter = new RaftCounter();

        // Loaded
        Logger.log("Mod RealisticMod has been loaded!");
    }

    public void OnModUnload()
    {
        harmony.UnpatchAll();
        Logger.log("Mod RealisticMod has been unloaded!");
    }

    public void ChangeItemNetMaxValue(int result)
    {
        Traverse.Create(FindObjectOfType<ItemCollector>()).Field("maxNumberOfItems").SetValue(result);
        ItemCollectorSize = result;
    }

}

public class Logger
{

    public static void log(String c)
    {
        Debug.Log(c);
    }

}

[HarmonyPatch(typeof(Inventory), "AddItem", new Type[] { typeof(string), typeof(int) })]
public class DropTreeModifierListener : Logger
{

    static void Prefix(ref string uniqueItemName, ref int amount)
    {
        if (Environment.StackTrace.Contains("at HarvestableTree"))
        {
            amount = (int)amount * 2;
        }
    }
}

[HarmonyPatch(typeof(ItemCollector))]
[HarmonyPatch("OnTriggerEnter")]
public class ItemNetModifier : Logger
{

    public static void Prefix(ItemCollector __instance)
    {
        //On Item Picked up, it checks the value stored in the static script of how many the max should be and it sets the private variable to that
        //On Unload it calls a custom method on all of them to reset them.
        int value = (int)Traverse.Create(__instance).Field("maxNumberOfItems").GetValue();
        if (value != RealisticMod.ItemCollectorSize)
        {
            Traverse.Create(__instance).Field("maxNumberOfItems").SetValue(RealisticMod.ItemCollectorSize);
        }
    }

    
}


public class RecipeManager : Logger
{

    // Set Recipes
    public void registerAllRecipes(RealisticMod mod)
    {
        // 2 Stone > 1 Clay
        SetRecipe(ItemManager.GetItemByName("Clay"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 2),
        }, CraftingCategory.Resources, 1, false);
        log("Loaded recipe: 2 Stone > 1 Clay");

        // 1 Stone > 4 Sand
        SetRecipe(ItemManager.GetItemByName("Sand"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Stone") }, 1),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 1 Stone > 4 Sand");

        // 1 Clay & 1 Sand > 1 Bricks
        SetRecipe(ItemManager.GetItemByName("Placeable_Brick_Wet"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Sand") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Clay") }, 1),
        }, CraftingCategory.Resources, 1, false);
        log("Loaded recipe: 1 Clay & 1 Sand > 1 Bricks");

        // 8 Planks & 4 Nail & 8 Rope > 1 Storage Medium
        SetRecipe(ItemManager.GetItemByName("Placeable_Storage_Medium"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Nail") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 8),
        }, CraftingCategory.Other, 1, false);
        log("Loaded recipe: 8 Planks & 4 Nail & 8 Rope > 1 Storage Medium");

        // 6 Planks & 6 Nail & 12 Rope > 2 Item Net
        SetRecipe(ItemManager.GetItemByName("Placeable_CollectionNet_Basic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 6),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 12),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Nail") }, 6),
        }, CraftingCategory.Other, 2, false);
        log("Loaded recipe: 6 Planks & 6 Nail & 12 Rope > 2 Item Net");

        // 2 Thatch > 4 Rope
        SetRecipe(ItemManager.GetItemByName("Rope"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 2),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 2 Thatch > 4 Rope");

        // 1 Scrap > 6 Nail
        SetRecipe(ItemManager.GetItemByName("Nail"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
        }, CraftingCategory.Resources, 6, false);
        log("Loaded recipe: 1 Scrap > 4 Nail");

        // 1 Metalingot > 3 Hinge
        SetRecipe(ItemManager.GetItemByName("Hinge"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 1),
        }, CraftingCategory.Resources, 3, true);
        log("Loaded recipe: 1 Metalingot > 3 Hinge");

        // 1 Scrap & 3 Plank > 2 Lantern Basic
        SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Basic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 3),
        }, CraftingCategory.Decorations, 2, false);
        log("Loaded recipe: 1 Scrap & 3 Plank > 2 Lantern Basic");

        // 1 Metal & 4 Plank & 1 Lantern Basic > 1 Lantern Metal
        SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Metal"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 1, false);

        log("Loaded recipe: 1 Metal & 4 Plank & 1 Lantern Basic > 1 Lantern Metal");
        // 2 Scrap & 4 Plank & 1 Lantern Basic > 1 Lantern Metal
        SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_FireBasket"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Decorations, 2, false);
        log("Loaded recipe: 2 Scrap & 4 Plank & 1 Lantern Basic > 1 Lantern Metal");

        // 2 Scrap & 2 Lantern Basic > Lantern Fireplace
        SetRecipe(ItemManager.GetItemByName("Placeable_Lantern_Fireplace"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 16),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 2),
        }, CraftingCategory.Decorations, 1, false);
        log("Loaded recipe: 2 Scrap & 2 Lantern Basic > Lantern Fireplace");

        // 8 Plank & 1 Lantern Basic > 4 Stone
        SetRecipe(ItemManager.GetItemByName("Stone"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic") }, 1),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 8 Plank & 1 Lantern Basic > 4 Stone");

        // 2 Scrap > 4 Bolt
        SetRecipe(ItemManager.GetItemByName("Bolt"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Scrap") }, 2),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 2 Scrap > 4 Bolt");

        // 1 MetalIngot > 4 Bolt
        CreateRecipe(ItemManager.GetItemByName("Bolt"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 1),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 1 MetalIngot > 4 Bolt");

        // 4 MetalIngot > 1 TitaniumOre
        CreateRecipe(ItemManager.GetItemByName("TitaniumOre"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("MetalIngot") }, 4),
        }, CraftingCategory.Resources, 1, false);
        log("Loaded recipe: 4 MetalIngot > 1 TitaniumOre");

        // 12 Rope & 8 Plank & 1 Fish > 1 SharkBait
        SetRecipe(ItemManager.GetItemByName("SharkBait"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 12),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 8),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plastic") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Raw_Herring"),
                ItemManager.GetItemByName("Raw_Mackerel"),
                ItemManager.GetItemByName("Raw_Pomfret"),
                ItemManager.GetItemByName("Raw_Salmon"),
                ItemManager.GetItemByName("Raw_Tilapia"), }, 1),
        }, CraftingCategory.Tools, 2, true);
        log("Loaded recipe: 12 Rope & 8 Plank & 1 Fish > 1 SharkBait");

        // 4 Plank & 1 (Latern Basic || Lantern Metal) > 6 Plank
        SetRecipe(ItemManager.GetItemByName("Plank"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic"), ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
        }, CraftingCategory.Resources, 6, false);
        log("Loaded recipe: 4 Plank & 1 (Latern Basic || Lantern Metal) > 6 Plank");

        // 4 Rope & 2 Thatch & 1 Plank > 4 Plastic
        SetRecipe(ItemManager.GetItemByName("Plastic"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Rope") }, 4),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Thatch") }, 2),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 1),
        }, CraftingCategory.Resources, 4, false);
        log("Loaded recipe: 4 Rope & 2 Thatch & 1 Plank > 4 Plastic");

        // 4 Plank & 1 (Latern Basic || Lantern Metal) > 2 Scrap
        SetRecipe(ItemManager.GetItemByName("Scrap"), new CostMultiple[] {
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Placeable_Lantern_Basic"), ItemManager.GetItemByName("Placeable_Lantern_Metal") }, 1),
            new CostMultiple(new Item_Base[] { ItemManager.GetItemByName("Plank") }, 4),
        }, CraftingCategory.Resources, 2, false);
        log("Loaded recipe: 4 Plank & 1 Lantern Basic > 2 Scrap");
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
public class RaftCounter : Logger
{
    public void printItemsCounts(string text, IDictionary<string, int> itemsCounts)
    {
        Debug.Log(text);
        foreach (KeyValuePair<string, int> item in itemsCounts)
        {
            log("- " + item.Key + " : x" + item.Value);
        }
    }

    [ConsoleCommand(name: "raftCost", docs: "List all the ressources needed to build your current raft.")]
    public void raftCommand(string[] args)
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