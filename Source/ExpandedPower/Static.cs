using UnityEngine;
using Verse;

namespace ExpandedPower {
  [StaticConstructorOnStartup]
  public static class Static {

    public static readonly Vector2 PowerPlantSolarBarSize = new Vector2(2.3f, 0.14f);

    public static Texture2D TexPowerLower = ContentFinder<Texture2D>.Get("Cupro/UI/Commands/PowerLower", false);
    public static Texture2D TexPowerRaise = ContentFinder<Texture2D>.Get("Cupro/UI/Commands/PowerRaise", false);
    public static Texture2D texHarvestSecondary = ContentFinder<Texture2D>.Get("Cupro/UI/Designations/HarvestSecondary");

    public static readonly Graphic GraphicSensorOn = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/DS/DaylightSensor_On");
    public static readonly Graphic GraphicSensorOff = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/DS/DaylightSensor_Off");
    public static readonly Graphic GraphicInvSensorOn = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/IDS/InDaylightSensor_On");
    public static readonly Graphic GraphicInvSensorOff = GraphicDatabase.Get<Graphic_Single>("Cupro/Object/Utility/IDS/InDaylightSensor_Off");

    public static readonly Material PowerPlantSolarBarFilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.5f, 0.475f, 0.1f));
    public static readonly Material PowerPlantSolarBarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.15f, 0.15f, 0.15f));

    public static DesignationDef DesignationHarvestSecondary = DefDatabase<DesignationDef>.GetNamed("EXP_Designator_PlantsHarvestSecondary");

    public static string DescriptionPowerLower = "EXP_PowerLower_D".Translate();
    public static string LabelPowerLower = "EXP_PowerLower_L".Translate();
    public static string DescriptionPowerRaise = "EXP_PowerRaise_D".Translate();
    public static string LabelPowerRaise = "EXP_PowerRaise_L".Translate();
    public static string WeatherReportBright = "EXP_Bright".Translate();
    public static string WeatherReportDarkened = "EXP_Darkened".Translate();
    public static string WeatherReportDark = "EXP_Dark".Translate();
    public static string InspectSunlightLevel = "EXP_SunlightLevel".Translate();
    public static string InspectUnder30 = "EXP_Under30".Translate();
    public static string InspectAbove30 = "EXP_Above30".Translate();
    public static string LabelHarvestSecondary = "EXP_DesignatorHarvestSecondary".Translate();
    public static string DescriptionHarvestSecondary = "EXP_DesignatorHarvestSecondaryDesc".Translate();
    public static string ReportDesignatePlantsWithSecondary = "EXP_MustDesignatePlantsWithSecondary".Translate();
    public static string ReportDesignateHarvestableSecondary = "EXP_MustDesignateHarvestableSecondary".Translate();
    public static string ReportBadSeason = "EXP_CannotGrowBadSeason".Translate();
  }
}
