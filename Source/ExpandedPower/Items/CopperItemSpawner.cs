using Verse;

namespace ExpandedPower {

  internal class CopperItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      SpawnRandomQuantity(ExpDefOf.EXP_Copper, 20, 40, Position, map, ThingPlaceMode.Direct);
    }
  }
}
