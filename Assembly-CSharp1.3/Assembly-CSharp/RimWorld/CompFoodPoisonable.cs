using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001137 RID: 4407
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x1700121F RID: 4639
		// (get) Token: 0x060069D9 RID: 27097 RVA: 0x0023A7EA File Offset: 0x002389EA
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x0023A7F2 File Offset: 0x002389F2
		public void SetPoisoned(FoodPoisonCause newCause)
		{
			this.poisonPct = 1f;
			this.cause = newCause;
		}

		// Token: 0x060069DB RID: 27099 RVA: 0x0023A806 File Offset: 0x00238A06
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
			Scribe_Values.Look<FoodPoisonCause>(ref this.cause, "cause", FoodPoisonCause.Unknown, false);
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x0023A836 File Offset: 0x00238A36
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
			compFoodPoisonable.cause = this.cause;
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x0023A85C File Offset: 0x00238A5C
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			base.PreAbsorbStack(otherStack, count);
			CompFoodPoisonable compFoodPoisonable = otherStack.TryGetComp<CompFoodPoisonable>();
			if (this.cause == FoodPoisonCause.Unknown && compFoodPoisonable.cause != FoodPoisonCause.Unknown)
			{
				this.cause = compFoodPoisonable.cause;
			}
			else if (compFoodPoisonable.cause != FoodPoisonCause.Unknown || this.cause != FoodPoisonCause.Unknown)
			{
				float num = this.poisonPct * (float)this.parent.stackCount;
				float num2 = compFoodPoisonable.poisonPct * (float)count;
				this.cause = ((num > num2) ? this.cause : compFoodPoisonable.cause);
			}
			this.poisonPct = GenMath.WeightedAverage(this.poisonPct, (float)this.parent.stackCount, compFoodPoisonable.poisonPct, (float)count);
		}

		// Token: 0x060069DE RID: 27102 RVA: 0x0023A901 File Offset: 0x00238B01
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, this.cause);
			}
		}

		// Token: 0x04003B1E RID: 15134
		private float poisonPct;

		// Token: 0x04003B1F RID: 15135
		public FoodPoisonCause cause;
	}
}
