using System.Collections.Generic;

using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Building_Diode : Building {


    public CompBreakdownable breakdownComp;
    public CompFlickable flickableComp;

    private List<Building_Cathode> connectedCathodes;
    public List<Building_Cathode> ConnectedCathodes {
      get { return connectedCathodes; }
    }


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      breakdownComp = GetComp<CompBreakdownable>();
      flickableComp = GetComp<CompFlickable>();
      connectedCathodes = new List<Building_Cathode>();
      UpdateCathodeList();
    }


    public void Notify_CathodeSpawned(Building_Cathode cathode) {
      connectedCathodes.Add(cathode);
    }


    public void Notify_CathodeDespawned(Building_Cathode oldCathode) {
      connectedCathodes.Remove(oldCathode);
    }


    public void UpdateCathodeList() {
      connectedCathodes.Clear();
      List<Thing> thingsInCell;

      // Scan for existing cathodes
      foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(Position, Rotation, def.Size)) {
        if (c.InBounds(Map)) {
          thingsInCell = c.GetThingList(Map);
          for (int t = 0; t < thingsInCell.Count; t++) {
            if (thingsInCell[t] is Building_Cathode) {
              connectedCathodes.Add(thingsInCell[t] as Building_Cathode);
              break;
            }
          }
        }
      }
    }
  }
}
