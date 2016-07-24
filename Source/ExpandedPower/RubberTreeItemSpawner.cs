using CorePanda;
using Verse;

namespace ExpandedPower {

  public class RubberTreeItemSpawner : ItemSpawner {

    public override void SpawnSetup() {
      base.SpawnSetup();
      SpawnRandomQuantity(ThingDef.Named("EXP_RubberWood"), 10, 20);
      SpawnRandomQuantity(ThingDef.Named("EXP_LatexBucket"), 1, 2);
      Destroy();
    }
  }
}
