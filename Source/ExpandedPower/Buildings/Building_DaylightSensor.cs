﻿using System.Text;

using UnityEngine;
using Verse;

namespace ExpandedPower {

  public class Building_DaylightSensor : Building {

    public bool InSunlight;          // Current sunlight status
    private bool inSunlightOld;      // Backup status for loading

    // Comps
    private CompSunlight sunlightComp;    

    // Used for Building_InvertedDaylightSensor, since it inherits this class
    public virtual bool Inverted {
      get { return false; }
    }

    // Allow power flow when exposed to sunlight
    public override bool TransmitsPowerNow {
      get { return InSunlight; }
    }

    // Change graphic depending on current sunlight status
    public override Graphic Graphic {
      get {
        if (InSunlight) {
          return Static.GraphicSensorOn;
        }
        return Static.GraphicSensorOff;
      }
    }

    // Handle loading data
    public override void ExposeData() {
      base.ExposeData(); 
      if (Scribe.mode == LoadSaveMode.PostLoadInit) { 
        inSunlightOld = !InSunlight;
        UpdatePowerGrid();
      }
    }


    // Handle initiation
    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
      base.SpawnSetup(map, respawningAfterLoad);

      sunlightComp = GetComp<CompSunlight>();
      // Call TickRare() immediately when spawned to prevent latency issues
      TickRare(); 
    }


    public override void TickRare() {
      base.TickRare();

      // When under a roof, prevent sunlight from reaching the sensor
      if (Map.roofGrid.Roofed(Position)) {
        InSunlight = false;
        return;
      }
      else {
        ReceiveDaylightSignal();
      }
    }

    // Handle sunlight calculations
    public void ReceiveDaylightSignal() {
      // If calculated sunlight level is above 30%, set to true. Otherwise, set to false.
      if (sunlightComp.FactoredSunlight > 0.3f) {
        InSunlight = true;
        UpdatePowerGrid();
        return;
      }
      else {
        InSunlight = false;
        UpdatePowerGrid();
      }
    }

    // Handle inspector data
    public override string GetInspectString() {
      StringBuilder stringBuilder = new StringBuilder();
      // Get inherited string data
      stringBuilder.Append(base.GetInspectString());

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
      // Add a helpful tracker for current sunlight levels
      stringBuilder.Append(Mathf.Round(sunlightComp.FactoredSunlight * 1000) / 10);
      stringBuilder.AppendLine(" / 100");

      // If this is not receiving > 30% light, and is a daylight sensor
      if (InSunlight == false && Inverted == false) {
        stringBuilder.Append(Static.InspectUnder30);
      }
      // If this is receiving > 30% light, but is an inverted daylight sensor
      if (InSunlight == true && Inverted == true) {
        stringBuilder.Append(Static.InspectAbove30);
      }

      return stringBuilder.ToString().TrimEndNewlines();
    }

    // Notify the power net if a change has been made, then backup sunlight variable
    public void UpdatePowerGrid() {
      if (InSunlight != inSunlightOld) {
        Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(PowerComp);
        inSunlightOld = InSunlight;
      }
    }
  }
}
