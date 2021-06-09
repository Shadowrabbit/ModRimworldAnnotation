using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D35 RID: 7477
	public class StatPart_Food : StatPart
	{
		// Token: 0x0600A27E RID: 41598 RVA: 0x002F5608 File Offset: 0x002F3808
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					val *= this.FoodMultiplier(pawn.needs.food.CurCategory);
				}
			}
		}

		// Token: 0x0600A27F RID: 41599 RVA: 0x002F5658 File Offset: 0x002F3858
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.needs.food != null)
				{
					return pawn.needs.food.CurCategory.GetLabel() + ": x" + this.FoodMultiplier(pawn.needs.food.CurCategory).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0600A280 RID: 41600 RVA: 0x0006BF2D File Offset: 0x0006A12D
		private float FoodMultiplier(HungerCategory hunger)
		{
			switch (hunger)
			{
			case HungerCategory.Fed:
				return this.factorFed;
			case HungerCategory.Hungry:
				return this.factorHungry;
			case HungerCategory.UrgentlyHungry:
				return this.factorUrgentlyHungry;
			case HungerCategory.Starving:
				return this.factorStarving;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x04006E6D RID: 28269
		public float factorStarving = 1f;

		// Token: 0x04006E6E RID: 28270
		public float factorUrgentlyHungry = 1f;

		// Token: 0x04006E6F RID: 28271
		public float factorHungry = 1f;

		// Token: 0x04006E70 RID: 28272
		public float factorFed = 1f;
	}
}
