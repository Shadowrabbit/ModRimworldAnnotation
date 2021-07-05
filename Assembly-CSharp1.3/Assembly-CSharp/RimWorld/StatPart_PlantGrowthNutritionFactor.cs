using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DC RID: 5340
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x06007F4C RID: 32588 RVA: 0x002CFFC8 File Offset: 0x002CE1C8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06007F4D RID: 32589 RVA: 0x002CFFE8 File Offset: 0x002CE1E8
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				Plant plant = (Plant)req.Thing;
				TaggedString taggedString = "StatsReport_PlantGrowth".Translate(plant.Growth.ToStringPercent()) + ": x" + f.ToStringPercent();
				if (!plant.def.plant.Sowable)
				{
					taggedString += " (" + "StatsReport_PlantGrowth_Wild".Translate() + ")";
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x06007F4E RID: 32590 RVA: 0x002D007C File Offset: 0x002CE27C
		private bool TryGetFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Plant plant = req.Thing as Plant;
			if (plant == null)
			{
				factor = 1f;
				return false;
			}
			factor = PlantUtility.NutritionFactorFromGrowth(plant.def, plant.Growth);
			return true;
		}
	}
}
