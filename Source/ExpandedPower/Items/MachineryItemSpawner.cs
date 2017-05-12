using Verse;

namespace ExpandedPower {

  internal class MachineryItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      // Give a 10% chance to spawn copper or rubber
      if (Rand.Range(0, 100) < 10) {
        if (Rand.Bool) {
          SpawnRandomQuantity(ThingDef.Named("EXP_Copper"), 5, 10);
        }
        else {
          SpawnRandomQuantity(ThingDef.Named("EXP_Rubber"), 1, 5);
        }
      }

      SpawnExactQuantity(ThingDef.Named("Component"), 2);
      Destroy();
    }
  }
}
