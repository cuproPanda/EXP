using RimWorld;
using Verse;

namespace ExpandedPower {

  public class Designator_PlantsHarvestSecondary : Designator {

    private DesignationDef designationDef;

    public override int DraggableDimensions {
      get { return 2; }
    }


    public Designator_PlantsHarvestSecondary() {
      defaultLabel = Static.LabelHarvestSecondary;
      defaultDesc = Static.DescriptionHarvestSecondary;
      icon = Static.texHarvestSecondary;
      soundDragSustain = SoundDefOf.DesignateDragStandard;
      soundDragChanged = SoundDefOf.DesignateDragStandardChanged;
      useMouseIcon = true;
      soundSucceeded = SoundDefOf.DesignateHarvest;
      designationDef = Static.DesignationHarvestSecondary;
    }
    

    public override AcceptanceReport CanDesignateThing(Thing t) {
      // If the thing isn't a plant
      if (t.def.plant == null) {
        return false;
      }
      // If the thing doesn't have a matching PlantWithSecondaryDef
      if (DefDatabase<PlantWithSecondaryDef>.GetNamed(t.def.defName, false) == null) {
        return Static.ReportDesignatePlantsWithSecondary;
      }
      // If the thing is already designated
      if (Map.designationManager.DesignationOn(t, designationDef) != null) {
        return false;
      }
      // If the secondary resource isn't harvestable
      PlantWithSecondary plant = (PlantWithSecondary)t;
      if (!plant.Sec_HarvestableNow) {
        return Static.ReportDesignateHarvestableSecondary;
      }
      return true;
    }


    public override AcceptanceReport CanDesignateCell(IntVec3 c) {
      if (!c.InBounds(Map) || c.Fogged(Map)) {
        return false;
      }
      Plant plant = c.GetPlant(Map);
      if (plant == null) {
        return Static.ReportDesignatePlantsWithSecondary;
      }
      AcceptanceReport result = CanDesignateThing(plant);
      if (!result.Accepted) {
        return result;
      }
      return true;
    }


    public override void DesignateSingleCell(IntVec3 c) {
      DesignateThing(c.GetPlant(Map));
    }


    public override void DesignateThing(Thing t) {
      Map.designationManager.RemoveAllDesignationsOn(t, false);
      Map.designationManager.AddDesignation(new Designation(t, designationDef));
    }


    public override void SelectedUpdate() {
      GenUI.RenderMouseoverBracket();
    }
  }
}
