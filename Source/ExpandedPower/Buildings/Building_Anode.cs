using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Building_Anode : Building {

    private TickManager tickMan;
    private CompPowerTrader powerComp;
    private List<Thing> thingsInCell;
    private List<Building_Diode> diodes;
    private int cathodesFound;
    private float energyToSend = 100f;
    private float energyUsedThisTick;


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      tickMan = Find.TickManager;
      powerComp = GetComp<CompPowerTrader>();

      UpdateDiodeList();
    }


    public override void Tick() {
      base.Tick();

      // Turn the power off to stop drawing from the grid
      // This will be turned on if any cathodes are found
      powerComp.PowerOn = false;
      powerComp.PowerOutput = 0;
      energyUsedThisTick = 0;
      cathodesFound = 0;

      // Every TickRare, scan for diode changes
      if (tickMan.TicksGame % 250 == 0) {
        UpdateDiodeList();
      }

      // Do first pass to count connected cathodes
      // This is to divide the sent energy among cathodes
      for (int fd = 0; fd < diodes.Count; fd++) {
        // Remove the diode from the list if it is no longer present
        if (diodes[fd] == null) {
          diodes.Remove(diodes[fd]);
          continue;
        }

        for (int fc = 0; fc < diodes[fd].ConnectedCathodes.Count; fc++) {
          if (diodes[fd].ConnectedCathodes[fc].CanSendEnergy()) {
            cathodesFound++;
          }
        }
      }

      // Do second pass to distribute power between cathodes
      for (int d = 0; d < diodes.Count; d++) {
        for (int c = 0; c < diodes[d].ConnectedCathodes.Count; c++) {
          powerComp.PowerOn = true;
          diodes[d].ConnectedCathodes[c].SendEnergy(Mathf.FloorToInt(energyToSend / cathodesFound));
          energyUsedThisTick = energyToSend;
        }
      }

      // Draw power from the grid
      // The anode draws 5 to run
      powerComp.PowerOutput = -5f - energyUsedThisTick;
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
