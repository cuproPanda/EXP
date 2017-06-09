using RimWorld;
using Verse;

namespace ExpandedPower {

  internal class MachineryItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      // Give a 10% chance to spawn copper or rubber
      if (Rand.Range(0, 100) < 10) {
        if (Rand.Bool) {
          SpawnRandomQuantity(ExpDefOf.EXP_Copper, 5, 10, Position, map, ThingPlaceMode.Near, false);
        }
        else {
          SpawnRandomQuantity(ExpDefOf.EXP_Rubber, 2, 5, Position, map, ThingPlaceMode.Near, false);
        }
      }

      SpawnExactQuantity(ThingDefOf.Component, 2, Position, map, ThingPlaceMode.Direct);
    }
  }
}
