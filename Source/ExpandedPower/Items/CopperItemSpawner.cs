using Verse;

namespace ExpandedPower {

  internal class CopperItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
			
			int chance = Rand.Range(0, 100);
			
			// Give a 5% chance to spawn malachite, if it exists
			if (chance < 5){
				if (DefDatabase<ThingDef>.GetNamed("EXP_Malachite", false) != null){
					SpawnExactQuantity(ThingDef.Named("EXP_Malachite"), 1);
				}
			}

      SpawnRandomQuantity(ThingDef.Named("EXP_Copper"), 10, 20);
      Destroy();
    }
  }
}
