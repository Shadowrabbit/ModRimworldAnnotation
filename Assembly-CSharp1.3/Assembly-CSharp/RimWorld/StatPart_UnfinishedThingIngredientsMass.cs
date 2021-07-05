using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E8 RID: 5352
	public class StatPart_UnfinishedThingIngredientsMass : StatPart
	{
		// Token: 0x06007F7D RID: 32637 RVA: 0x002D0D34 File Offset: 0x002CEF34
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06007F7E RID: 32638 RVA: 0x002D0D54 File Offset: 0x002CEF54
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_IngredientsMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06007F7F RID: 32639 RVA: 0x002D0D94 File Offset: 0x002CEF94
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
