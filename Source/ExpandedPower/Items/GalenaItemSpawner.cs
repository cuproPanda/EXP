using Verse;

namespace ExpandedPower {

  internal class GalenaItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      // Give a 25% chance to spawn silver
      if (Rand.Range(0, 100) < 25) {
        SpawnRandomQuantity(ThingDef.Named("Silver"), 10, 15);
      }

      SpawnRandomQuantity(ThingDef.Named("EXP_Lead"), 10, 20);
      Destroy();
    }
  }
}
