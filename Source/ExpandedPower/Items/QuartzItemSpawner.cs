using Verse;

namespace ExpandedPower {

  internal class QuartzItemSpawner : ItemSpawner {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      int chance = Rand.Range(0, 100);

      // Give a 5% chance to spawn amethyst, if it exists
      if (chance < 5){
				if (DefDatabase<ThingDef>.GetNamed("EXP_Amethyst", false) != null){
					SpawnExactQuantity(ThingDef.Named("EXP_Amethyst"), 1);
				}
			}
			
      SpawnRandomQuantity(ThingDef.Named("EXP_Quartz"), 8, 35);
			
      Destroy();
    }
  }
}
