using Verse;

namespace ExpandedPower {

  internal class QuartzItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
			
      SpawnRandomQuantity(ExpDefOf.EXP_Quartz, 10, 35, Position, map, ThingPlaceMode.Direct);
    }
  }
}
