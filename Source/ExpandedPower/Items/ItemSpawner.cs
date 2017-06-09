using Verse;

namespace ExpandedPower {

  public class ItemSpawner : ThingWithComps {

    public void SpawnRandomQuantity(ThingDef TDef, int MinToSpawn, int MaxToSpawn, IntVec3 pos, Map map, ThingPlaceMode mode, bool spawningDestroys = true) {
      int stack = Rand.RangeInclusive(MinToSpawn, MaxToSpawn);
      SpawnExactQuantity(TDef, stack, pos, map, mode, spawningDestroys);
    }


    public void SpawnExactQuantity(ThingDef TDef, int NumToSpawn, IntVec3 pos, Map map, ThingPlaceMode mode, bool spawningDestroys = true) {
      if (spawningDestroys) {
        Destroy(); 
      }

      Thing placedProduct = ThingMaker.MakeThing(TDef);
      placedProduct.stackCount = NumToSpawn;
      GenPlace.TryPlaceThing(placedProduct, pos, map, mode);
    }
  }
}
