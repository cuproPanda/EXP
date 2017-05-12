using System.Collections.Generic;

using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Building_Cathode : Building {

    private Building_Diode diode;
    private CompPowerTrader powerComp;
    private CompInternalBattery batteryComp;
    private List<Thing> thingsInCell;


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      powerComp = GetComp<CompPowerTrader>();
      batteryComp = GetComp<CompInternalBattery>();

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

      // Drain power from the internal battery
      // This is to prevent issues with the tick timer -- best solution found, but not ideal
      powerComp.PowerOutput = batteryComp.PowerContained;
      batteryComp.PowerContained = 0;
    }


    public bool TrySendEnergy(float amount) {
      return batteryComp.TryAddEnergy(amount);
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
