using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Building_Anode : Building {

    public static readonly float MaxEnergyToSend = 500f;

    private TickManager tickMan;
    private CompPowerTrader powerComp;
    private List<Thing> thingsInCell;
    private List<Building_Diode> diodes;
    private int cathodesFound;
    private float energyUsedThisTick;


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      tickMan = Find.TickManager;
      powerComp = GetComp<CompPowerTrader>();

      diodes = new List<Building_Diode>();
      UpdateDiodeList();
    }


    public override void Tick() {
      base.Tick();

      // Reset power usage to standby power
      // This will be increased if any cathodes are found
      powerComp.PowerOutput = -5f;
      energyUsedThisTick = 0;
      cathodesFound = 0;

      // Every TickRare, scan for diode changes
      if (tickMan.TicksGame % 250 == 0) {
        UpdateDiodeList();
      }

      // If there is a list of diodes and this anode has power
      if (!diodes.NullOrEmpty() && powerComp.PowerOn) {
        // Do first pass to count connected cathodes
        // This is to divide the sent energy among cathodes
        for (int fd = 0; fd < diodes.Count; fd++) {
          // Remove the diode from the list if it is no longer present,
          // or if it is broken down or turned off
          if ((diodes[fd] == null) || (diodes[fd].breakdownComp != null && diodes[fd].IsBrokenDown()) || (diodes[fd].flickableComp != null && !diodes[fd].flickableComp.SwitchIsOn)) {
            diodes.Remove(diodes[fd]);
            continue;
          }

          for (int fc = 0; fc < diodes[fd].ConnectedCathodes.Count; fc++) {
            cathodesFound++;
          }
        }

        // Do second pass to distribute power between cathodes
        for (int d = 0; d < diodes.Count; d++) {
          for (int c = 0; c < diodes[d].ConnectedCathodes.Count; c++) {
            // Try to send energy to the connected cathodes
            if (diodes[d].ConnectedCathodes[c].TrySendEnergy(Mathf.FloorToInt(MaxEnergyToSend / cathodesFound))) {
              energyUsedThisTick += Mathf.FloorToInt(MaxEnergyToSend / cathodesFound);
            }
          }
        } 
      }

      // Draw power from the grid
      powerComp.PowerOutput -= energyUsedThisTick;
    }


    private void UpdateDiodeList() {
      diodes.Clear();

      foreach (IntVec3 c in GenAdj.CellsAdjacentCardinal(Position, Rotation, def.Size)) {
        if (c.InBounds(Map)) {
          thingsInCell = c.GetThingList(Map);
          for (int t = 0; t < thingsInCell.Count; t++) {
            if (thingsInCell[t] is Building_Diode) {
              diodes.Add(thingsInCell[t] as Building_Diode);
              break;
            }
          }
        }
      }
    }
  }
}
