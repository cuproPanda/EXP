using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ExpandedPower {

  public class PlantWithSecondaryDef : Def {

    // Make sure to have a duplicate defName for the secondary def and the parent def!

    // Customize how quickly this grows and when it is first harvestable
    public float growDays;
    public float parentMinGrowth = 0.5f;

    // Customize what seasons this is allowed to grow in
    // Useful for fruits that should only grow in spring, etc.
    public List<Season> limitedGrowSeasons;

    // What to harvest
    public ThingDef harvestedThingDef;
    // If the thing harvested doesn't match up with what is growing (e.g. latex bucket vs latex), use a special label
    public string specialThingDefLabel;
    // If an exact amount is desired, only put a value for maxToHarvest. minToHarvest gets checked, and if it's MaxValue only maxToHarvest is used
    public int minToHarvest = int.MaxValue;
    public int maxToHarvest;
  }
}
