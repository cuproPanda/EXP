using Verse;

namespace ExpandedPower {

  public class RubberTreeItemSpawner : ThingWithComps {

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);
      SpawnRandomQuantity(ThingDef.Named("EXP_RubberWood"), 10, 20);
      SpawnRandomQuantity(ThingDef.Named("EXP_LatexBucket"), 1, 2);
      Destroy();
    }


    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = stack;
      GenPlace.TryPlaceThing(placedProduct, Position, Map, ThingPlaceMode.Near);
    }
  }
}
