using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D44 RID: 7492
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x0600A2C8 RID: 41672 RVA: 0x002F5D54 File Offset: 0x002F3F54
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0600A2C9 RID: 41673 RVA: 0x002F5D74 File Offset: 0x002F3F74
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

		// Token: 0x0600A2CA RID: 41674 RVA: 0x002F5E08 File Offset: 0x002F4008
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
			if (plant.def.plant.Sowable)
			{
				factor = plant.Growth;
				return true;
			}
			factor = Mathf.Lerp(0.5f, 1f, plant.Growth);
			return true;
		}
	}
}
