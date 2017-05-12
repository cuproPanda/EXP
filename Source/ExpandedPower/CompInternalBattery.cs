using Verse;

namespace ExpandedPower {

  internal class CompInternalBattery : ThingComp {

    private float maxEnergy = Building_Anode.MaxEnergyToSend * 3;

    private float powerContained;
    public float PowerContained {
      get { return powerContained; }
      set { powerContained = value; }
    }


    public override void PostExposeData() {
      base.PostExposeData();
      Scribe_Values.Look(ref powerContained, "EXP_InternalBatteryPower", 0);
    }


    public bool TryAddEnergy(float amount) {
      if ((powerContained + amount) <= maxEnergy) {
        powerContained += amount;
        return true;
      }
      return false;
    }
  }
}
