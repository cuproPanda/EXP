using Verse;

namespace ExpandedPower {

  internal class GalenaItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
			
			int chance = Rand.Range(0, 100);
			
			// Give a 5% chance to spawn cerussite, if it exists
			if (chance < 5){
				if (DefDatabase<ThingDef>.GetNamed("EXP_Cerussite", false) != null){
					SpawnExactQuantity(ThingDef.Named("EXP_Cerussite"), 1);
				}
			}

      // Give a 25% chance to spawn silver
      if (chance < 25) {
        SpawnRandomQuantity(ThingDef.Named("Silver"), 10, 15);
      }

      SpawnRandomQuantity(ThingDef.Named("EXP_Lead"), 10, 20);
      Destroy();
    }
  }
}
