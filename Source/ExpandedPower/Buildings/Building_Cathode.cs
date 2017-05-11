using System.Collections.Generic;

using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Building_Cathode : Building {

    private Building_Diode diode;
    private CompPowerTrader powerComp;
    private CompBreakdownable breakdownComp;
    private CompFlickable flickableComp;
    private List<Thing> thingsInCell;


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      powerComp = GetComp<CompPowerTrader>();
      breakdownComp = GetComp<CompBreakdownable>();
      flickableComp = GetComp<CompFlickable>();

      // Notify all connected diodes that this cathode has been created
      foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(Position, Rotation, def.Size)) {
        if (c.InBounds(Map)) {
          thingsInCell = c.GetThingList(Map);
          for (int t = 0; t < thingsInCell.Count; t++) {
            if (thingsInCell[t] is Building_Diode) {
              diode = thingsInCell[t] as Building_Diode;
              diode.Notify_CathodeSpawned(this);
              break;
            }
          }
        }
      }
    }


    public override void Tick() {
      base.Tick();

      // Reset the power output, forcing the anode to send more
      powerComp.PowerOutput = 0;
    }


    public bool CanSendEnergy() {
      // If this is allowed to send power
      if ((breakdownComp != null &&  breakdownComp.BrokenDown) || (flickableComp != null && !flickableComp.SwitchIsOn)) {
        return false;
      }      
      return true;
    }


    public void SendEnergy(float amount) {
      // Add the desired power to the power output
      // This allows multiple anodes to be used
      powerComp.PowerOutput += amount;
    }


    public override void DeSpawn() {
      // Notify all connected diodes that this cathode has been removed
      foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(Position, Rotation, def.Size)) {
        if (c.InBounds(Map)) {
          thingsInCell = c.GetThingList(Map);
          for (int t = 0; t < thingsInCell.Count; t++) {
            if (thingsInCell[t] is Building_Diode) {
              diode = thingsInCell[t] as Building_Diode;
              diode.Notify_CathodeDespawned(this);
              break;
            }
          }
        }
      }

      base.DeSpawn();
    }
  }
}
