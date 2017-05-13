using System.Collections.Generic;

using UnityEngine;
using RimWorld;
using Verse;
using System.Text;

namespace ExpandedPower {

  public class Building_Anode : Building {
    
    private TickManager tickMan;
    private CompPowerTrader powerComp;
    private List<Thing> thingsInCell;
    private List<Building_Diode> diodes;
    private int cathodesFound;
    private float currentEnergyToSend = -1f;
    private float maxEnergyToSend = 500f;
    private float energyUsedThisTick;

    public float CurrentEnergyToSend {
      get {
        return currentEnergyToSend;
      }
      set {
        currentEnergyToSend = Mathf.Clamp(value, 0, maxEnergyToSend);
      }
    }


    // Handle loading data
    public override void ExposeData() {
      base.ExposeData();
      Scribe_Values.Look(ref currentEnergyToSend, "EXP_Anode_CurrentEnergyToSend", -1f);
    }


    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      tickMan = Find.TickManager;
      powerComp = GetComp<CompPowerTrader>();

      if (currentEnergyToSend == -1f) {
        currentEnergyToSend = maxEnergyToSend;
      }

      diodes = new List<Building_Diode>();
      UpdateDiodeList();
    }


    // Add buttons for adjusting power usage
    public override IEnumerable<Gizmo> GetGizmos() {

      Command_Action lower = new Command_Action() {
        icon = ContentFinder<Texture2D>.Get("Cupro/UI/Commands/PowerLower", false),
        defaultDesc = "EXP_PowerLower_D".Translate(),
        defaultLabel = "EXP_PowerLower_L".Translate(),
        activateSound = SoundDef.Named("Click"),
        action = () => { CurrentEnergyToSend -= 50f; },
      };
      yield return lower;

      Command_Action raise = new Command_Action() {
        icon = ContentFinder<Texture2D>.Get("Cupro/UI/Commands/PowerRaise", false),
        defaultDesc = "EXP_PowerRaise_D".Translate(),
        defaultLabel = "EXP_PowerRaise_L".Translate(),
        activateSound = SoundDef.Named("Click"),
        action = () => { CurrentEnergyToSend += 50f; },
      };
      yield return raise;

      foreach (Command c in base.GetGizmos()) {
        yield return c;
      }
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
            if (diodes[d].ConnectedCathodes[c].TrySendEnergy(Mathf.FloorToInt(currentEnergyToSend / cathodesFound))) {
              energyUsedThisTick += Mathf.FloorToInt(currentEnergyToSend / cathodesFound);
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


    // Handle inspector data
    public override string GetInspectString() {
      StringBuilder stringBuilder = new StringBuilder();
      // Inform the player how much energy is being sent
      // This is to show power usage while paused
      stringBuilder.AppendLine("EXP_PowerUsage".Translate( new object[] {currentEnergyToSend} ));

      // Get inherited string data
      stringBuilder.Append(base.GetInspectString());

      return stringBuilder.ToString().TrimEndNewlines();
    }
  }
}
