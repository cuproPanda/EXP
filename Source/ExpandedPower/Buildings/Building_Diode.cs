using System.Collections.Generic;
using Verse;

namespace ExpandedPower {

  public class Building_Diode : Building {

    private List<Building_Cathode> connectedCathodes;
    public List<Building_Cathode> ConnectedCathodes {
      get { return connectedCathodes; }
    }


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      List<Thing> thingsInCell;

      // Scan for existing cathodes
      foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(Position, Rotation, def.Size)) {
        if (c.InBounds(Map)) {
          thingsInCell = c.GetThingList(Map);
          for (int t = 0; t < thingsInCell.Count; t++) {
            if (thingsInCell[t] is Building_Cathode) {
              Notify_CathodeSpawned(thingsInCell[t] as Building_Cathode);
              break;
            }
          }
        }
      }
    }


    public void Notify_CathodeSpawned(Building_Cathode cathode) {
      connectedCathodes.Add(cathode);
    }


    public void Notify_CathodeDespawned(Building_Cathode oldCathode) {
      connectedCathodes.Remove(oldCathode);
    }
  }
}
