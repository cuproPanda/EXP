using Verse;

namespace ExpandedPower {

  public class ItemSpawner : ThingWithComps {

    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = stack;
      GenPlace.TryPlaceThing(placedProduct, Position, Map, ThingPlaceMode.Near);
    }


    public void SpawnExactQuantity(ThingDef TDef, int NumToSpawn) {
      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = NumToSpawn;
      GenPlace.TryPlaceThing(placedProduct, Position, Map, ThingPlaceMode.Near);
    }
  }
}
