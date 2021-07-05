using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200073D RID: 1853
	public class JobDriver_Vomit : JobDriver
	{
		// Token: 0x06003362 RID: 13154 RVA: 0x0000313F File Offset: 0x0000133F
		public override void SetInitialPosture()
		{
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00125083 File Offset: 0x00123283
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x0012509D File Offset: 0x0012329D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil toil = new Toil();
			toil.initAction = delegate()
			{
				this.ticksLeft = Rand.Range(300, 900);
				int num = 0;
				IntVec3 c;
				for (;;)
				{
					c = this.pawn.Position + GenAdj.AdjacentCellsAndInside[Rand.Range(0, 9)];
					num++;
					if (num > 12)
					{
						break;
					}
					if (c.InBounds(this.pawn.Map) && c.Standable(this.pawn.Map))
					{
						goto IL_77;
					}
				}
				c = this.pawn.Position;
				IL_77:
				this.job.targetA = c;
				this.pawn.pather.StopDead();
			};
			toil.tickAction = delegate()
			{
				if (this.ticksLeft % 150 == 149)
				{
					FilthMaker.TryMakeFilth(this.job.targetA.Cell, base.Map, ThingDefOf.Filth_Vomit, this.pawn.LabelIndefinite(), 1, FilthSourceFlags.None);
					if (this.pawn.needs.food.CurLevelPercentage > 0.1f)
					{
						this.pawn.needs.food.CurLevel -= this.pawn.needs.food.MaxLevel * 0.04f;
					}
				}
				this.ticksLeft--;
				if (this.ticksLeft <= 0)
				{
					base.ReadyForNextToil();
					TaleRecorder.RecordTale(TaleDefOf.Vomited, new object[]
					{
						this.pawn
					});
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.WithEffect(EffecterDefOf.Vomit, TargetIndex.A, null);
			toil.PlaySustainerOrSound(() => SoundDefOf.Vomit, 1f);
			yield return toil;
			yield break;
		}

		// Token: 0x04001E06 RID: 7686
		private int ticksLeft;
	}
}
