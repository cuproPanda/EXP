using Verse;

namespace ExpandedPower {
  [StaticConstructorOnStartup]
  class Building_InvertedDaylightSensor : Building_DaylightSensor {

    // Paths for graphics
    public static readonly Graphic InSensorOn = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/IDS/InDaylightSensor_On");
    public static readonly Graphic InSensorOff = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/IDS/InDaylightSensor_Off");

    // This building is an inverted daylight sensor
    public override bool Inverted {
      get { return true; }
    }

    // Allow power flow when not exposed to sunlight
    public override bool TransmitsPowerNow {
      get { return !InSunlight; }
    }

    // Change graphic depending on current sunlight status
    public override Graphic Graphic {
      get {
        if (!InSunlight) {
          return InSensorOn;
        }
        return InSensorOff;
      }
    }
  }
}
