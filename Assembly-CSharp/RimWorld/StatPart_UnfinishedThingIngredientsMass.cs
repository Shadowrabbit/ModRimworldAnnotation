using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4D RID: 7501
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x0600A2F2 RID: 41714 RVA: 0x002F6774 File Offset: 0x002F4974
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x0600A2F3 RID: 41715 RVA: 0x002F6794 File Offset: 0x002F4994
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x0600A2F4 RID: 41716 RVA: 0x002F67D4 File Offset: 0x002F49D4
		private bool TryGetValue(StatRequest req, out float value)
		{
			UnfinishedThing unfinishedThing = req.Thing as UnfinishedThing;
			if (unfinishedThing == null)
			{
				value = 0f;
				return false;
			}
			float num = 0f;
			for (int i = 0; i < unfinishedThing.ingredients.Count; i++)
			{
				num += unfinishedThing.ingredients[i].GetStatValue(StatDefOf.Mass, true) * (float)unfinishedThing.ingredients[i].stackCount;
			}
			value = num;
			return true;
		}
	}
}
