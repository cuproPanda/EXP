using Verse;

namespace ExpandedPower {

  internal class QuartzItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
      SpawnRandomQuantity(ThingDef.Named("EXP_Quartz"), 8, 35);
      Destroy();
    }
  }
}
