using Verse;

namespace ExpandedPower {

  internal class ItemSpawner : ThingWithComps {

    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = stack;
      GenPlace.TryPlaceThing(placedProduct, Position, Map, ThingPlaceMode.Near);
    }
  }
}
