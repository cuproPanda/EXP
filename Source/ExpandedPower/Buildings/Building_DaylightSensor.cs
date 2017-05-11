using System.Text;

using UnityEngine;
using Verse;

namespace ExpandedPower {
  [StaticConstructorOnStartup]
  public class Building_DaylightSensor : Building {

    public bool InSunlight;          // Current sunlight status
    private bool inSunlightOld;      // Backup status for loading

    // Comps
    private CompSunlight sunlightComp;

    // Paths for graphics
    public static readonly Graphic SensorOn = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/DS/DaylightSensor_On");
    public static readonly Graphic SensorOff = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/DS/DaylightSensor_Off");

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
          return SensorOn;
        }
        return SensorOff;
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
        stringBuilder.AppendLine("EXP_Bright".Translate());
      }
      if (sunlightComp.WeatherLight == WeatherLight.Darkened) {
        stringBuilder.AppendLine("EXP_Darkened".Translate());
      }
      if (sunlightComp.WeatherLight == WeatherLight.Dark) {
        stringBuilder.AppendLine("EXP_Dark".Translate());
      }

      stringBuilder.Append("EXP_SunlightLevel".Translate() + ": ");
      // Add a helpful tracker for current sunlight levels
      stringBuilder.Append(Mathf.Round(sunlightComp.FactoredSunlight * 1000) / 10);
      stringBuilder.AppendLine(" / 100");

      // If this is not receiving > 30% light, and is a daylight sensor
      if (InSunlight == false && Inverted == false) {
        stringBuilder.Append("EXP_Under30".Translate());
      }
      // If this is receiving > 30% light, but is an inverted daylight sensor
      if (InSunlight == true && Inverted == true) {
        stringBuilder.Append("EXP_Above30".Translate());
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
