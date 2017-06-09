﻿using UnityEngine;
using RimWorld;
using Verse;
using System.Text;

namespace ExpandedPower {
  /// <summary>
  /// Allows changing power variables in XML
  /// <para>Also uses CompSunlight for added realism</para>
  /// </summary>
  public class CompVariablePowerPlantSolar : CompPowerPlant {

    CompSunlight sunlightComp;

    private float RoofedPowerOutputFactor {
      get {
        int num = 0;
        int num2 = 0;
        foreach (IntVec3 current in parent.OccupiedRect()) {
          num++;
          if (parent.Map.roofGrid.Roofed(current)) {
            num2++;
          }
        }
        return (num - num2) / num;
      }
    }

    /// <summary> Accessor for the CompProperties </summary>
    public new CompProperties_VariableSolarPower Props {
      get {
        return (CompProperties_VariableSolarPower)props;
      }
    }

    /// <summary> Calculate how much power is being outputted </summary>
    protected override float DesiredPowerOutput {
      get {
        return Mathf.Lerp(Props.nightPower, Props.fullSunPower, sunlightComp.SimpleFactoredSunlight) * RoofedPowerOutputFactor;
      }
    }


    /// <summary> Setup the sunlight component, then get sunlight </summary>
    public override void PostSpawnSetup(bool respawningAfterLoad) {
      base.PostSpawnSetup(respawningAfterLoad);

      sunlightComp = parent.GetComp<CompSunlight>();
      sunlightComp.GetSunlight();
    }


    /// <summary> Every TickRare, update the sunlight - this doesn't need to be done every tick </summary>
    public override void CompTick() {
      base.CompTick();

      if (Find.TickManager.TicksGame % 250 == 0) {
        sunlightComp.GetSunlight();
      }
    }


    /// <summary> Draw the power bar </summary>
    public override void PostDraw() {
      base.PostDraw();
      GenDraw.FillableBarRequest r = default(GenDraw.FillableBarRequest);
      r.center = parent.DrawPos + Vector3.up * 0.1f;
      r.size = Static.PowerPlantSolarBarSize;
      r.fillPercent = PowerOutput / Props.fullSunPower;
      r.filledMat = Static.PowerPlantSolarBarFilledMat;
      r.unfilledMat = Static.PowerPlantSolarBarUnfilledMat;
      r.margin = 0.15f;
      Rot4 rotation = parent.Rotation;
      rotation.Rotate(RotationDirection.Clockwise);
      r.rotation = rotation;
      GenDraw.DrawFillableBar(r);
    }


    /// <summary> Handle inspector data </summary> 
    public override string CompInspectStringExtra() {
      StringBuilder stringBuilder = new StringBuilder();
      // Get inherited string data
      stringBuilder.AppendLine(base.CompInspectStringExtra());

      // Inform the player how the weather is affecting this building
      if (sunlightComp.WeatherLight == WeatherLight.Bright) {
        stringBuilder.AppendLine(Static.WeatherReportBright);
      }
      if (sunlightComp.WeatherLight == WeatherLight.Darkened) {
        stringBuilder.AppendLine(Static.WeatherReportDarkened);
      }
      if (sunlightComp.WeatherLight == WeatherLight.Dark) {
        stringBuilder.AppendLine(Static.WeatherReportDark);
      }

      stringBuilder.Append(Static.InspectSunlightLevel + ": ");
      stringBuilder.Append(Mathf.Round(sunlightComp.FactoredSunlight * 100));
      stringBuilder.AppendLine(" / 100");

      return stringBuilder.ToString().TrimEndNewlines();
    }
  }
}
