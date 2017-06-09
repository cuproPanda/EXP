using RimWorld;
using Verse;

namespace ExpandedPower {

  internal class GalenaItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
			
			int chance = Rand.Range(0, 100);

      // Give a 25% chance to spawn silver
      if (chance < 25) {
        SpawnRandomQuantity(ThingDefOf.Silver, 10, 15, Position, map, ThingPlaceMode.Near, false);
      }

      SpawnRandomQuantity(ExpDefOf.EXP_Lead, 20, 40, Position, map, ThingPlaceMode.Direct);
    }
  }
}
