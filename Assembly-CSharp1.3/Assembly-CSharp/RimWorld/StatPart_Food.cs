using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014CE RID: 5326
	public class StatPart_Food : StatPart
	{
		// Token: 0x06007F0E RID: 32526 RVA: 0x002CF45C File Offset: 0x002CD65C
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

		// Token: 0x06007F0F RID: 32527 RVA: 0x002CF4AC File Offset: 0x002CD6AC
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

		// Token: 0x06007F10 RID: 32528 RVA: 0x002CF51B File Offset: 0x002CD71B
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

		// Token: 0x04004F69 RID: 20329
		public float factorStarving = 1f;

		// Token: 0x04004F6A RID: 20330
		public float factorUrgentlyHungry = 1f;

		// Token: 0x04004F6B RID: 20331
		public float factorHungry = 1f;

		// Token: 0x04004F6C RID: 20332
		public float factorFed = 1f;
	}
}
