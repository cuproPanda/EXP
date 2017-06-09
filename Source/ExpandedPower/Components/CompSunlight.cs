using Verse;

namespace ExpandedPower {

  /// <summary>
  /// Handles sunlight based on weather
  /// </summary>
  public class CompSunlight : ThingComp {

    private WeatherDef weatherDef;        // Weather reference
    private float sunStrength;            // Strength of the sun multiplier, affected by weather

    // Used for inspect string weather reporting
    private WeatherLight wLight = WeatherLight.None;  

    /// <summary>
    /// Used for determining if the weather is affecting the current sunlight
    /// </summary>
    public WeatherLight WeatherLight { get { return wLight; } }

    /// <summary>
    /// Returns 0.0f to 1.0f based on time and weather
    /// <para>Requires GetSunlight() to have already been called</para>
    /// </summary>
    public float SimpleFactoredSunlight { // Final sunlight value based on time and weather, returns cached value
      get { return parent.Map.skyManager.CurSkyGlow * sunStrength; }
    }

    /// <summary>
    /// Gets the current sunlight, then returns 0.0f to 1.0f based on time and weather
    /// </summary>
    public float FactoredSunlight {
      get { GetSunlight();  return parent.Map.skyManager.CurSkyGlow * sunStrength; }
    }


    /// <summary>
    /// Updates the sunlight based on weather
    /// </summary>
    public virtual void GetSunlight() {
      // Get the current weather
      weatherDef = parent.Map.weatherManager.curWeather;

      // Clear weather provides the maximum sunlight
      if (weatherDef == ExpDefOf.Clear) {
        wLight = WeatherLight.Bright;
        sunStrength = 1f;
        return;
      }
      // These weathers provide 60% sunlight
      else if (weatherDef == ExpDefOf.Fog ||
               weatherDef == ExpDefOf.Rain ||
               weatherDef == ExpDefOf.SnowGentle) {
        wLight = WeatherLight.Darkened;
        sunStrength = 0.6f;
        return;
      }
      // These weathers get only 25% sunlight
      else if (weatherDef == ExpDefOf.FoggyRain ||
               weatherDef == ExpDefOf.SnowHard ||
               weatherDef == ExpDefOf.DryThunderstorm ||
               weatherDef == ExpDefOf.RainyThunderstorm) {
        wLight = WeatherLight.Dark;
        sunStrength = 0.35f;
        return;
      }
      // Default variable. Prevents issues when other mods add custom weather
      else {
        wLight = WeatherLight.Bright;
        sunStrength = 1f;
      }
    }
  }
}