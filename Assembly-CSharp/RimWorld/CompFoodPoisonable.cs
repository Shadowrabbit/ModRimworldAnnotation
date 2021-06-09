using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017C1 RID: 6081
	public class CompFoodPoisonable : ThingComp
	{
		// Token: 0x170014D7 RID: 5335
		// (get) Token: 0x06008677 RID: 34423 RVA: 0x0005A330 File Offset: 0x00058530
		public float PoisonPercent
		{
			get
			{
				return this.poisonPct;
			}
		}

		// Token: 0x06008678 RID: 34424 RVA: 0x0005A338 File Offset: 0x00058538
		public void SetPoisoned(FoodPoisonCause newCause)
		{
			this.poisonPct = 1f;
			this.cause = newCause;
		}

		// Token: 0x06008679 RID: 34425 RVA: 0x0005A34C File Offset: 0x0005854C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.poisonPct, "poisonPct", 0f, false);
			Scribe_Values.Look<FoodPoisonCause>(ref this.cause, "cause", FoodPoisonCause.Unknown, false);
		}

		// Token: 0x0600867A RID: 34426 RVA: 0x0005A37C File Offset: 0x0005857C
		public override void PostSplitOff(Thing piece)
		{
			base.PostSplitOff(piece);
			CompFoodPoisonable compFoodPoisonable = piece.TryGetComp<CompFoodPoisonable>();
			compFoodPoisonable.poisonPct = this.poisonPct;
			compFoodPoisonable.cause = this.cause;
		}

		// Token: 0x0600867B RID: 34427 RVA: 0x00278F64 File Offset: 0x00277164
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

		// Token: 0x0600867C RID: 34428 RVA: 0x0005A3A2 File Offset: 0x000585A2
		public override void PostIngested(Pawn ingester)
		{
			if (Rand.Chance(this.poisonPct * FoodUtility.GetFoodPoisonChanceFactor(ingester)))
			{
				FoodUtility.AddFoodPoisoningHediff(ingester, this.parent, this.cause);
			}
		}

		// Token: 0x04005692 RID: 22162
		private float poisonPct;

		// Token: 0x04005693 RID: 22163
		public FoodPoisonCause cause;
	}
}
