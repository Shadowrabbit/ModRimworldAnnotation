using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C18 RID: 3096
	public class JobDriver_Vomit : JobDriver
	{
		// Token: 0x060048E0 RID: 18656 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void SetInitialPosture()
		{
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x00034AF5 File Offset: 0x00032CF5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x00034B0F File Offset: 0x00032D0F
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
			toil.WithEffect(EffecterDefOf.Vomit, TargetIndex.A);
			toil.PlaySustainerOrSound(() => SoundDefOf.Vomit);
			yield return toil;
			yield break;
		}

		// Token: 0x0400308A RID: 12426
		private int ticksLeft;
	}
}
