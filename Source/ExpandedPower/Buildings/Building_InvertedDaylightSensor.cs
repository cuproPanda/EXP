using Verse;

namespace ExpandedPower {

  public class Building_InvertedDaylightSensor : Building_DaylightSensor {

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
          return Static.GraphicInvSensorOn;
        }
        return Static.GraphicInvSensorOff;
      }
    }
  }
}
