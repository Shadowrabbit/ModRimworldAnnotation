using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006EA RID: 1770
	public class JobDriver_EatAtCannibalPlatter : JobDriver
	{
		// Token: 0x0600314F RID: 12623 RVA: 0x0011FA19 File Offset: 0x0011DC19
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.ReserveSittableOrSpot(this.job.targetB.Cell, this.job, errorOnFailed);
		}

		// Token: 0x06003150 RID: 12624 RVA: 0x0011FA3D File Offset: 0x0011DC3D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckIdeology("Cannibal eat job"))
			{
				yield break;
			}
			this.EndOnDespawnedOrNull(TargetIndex.A, JobCondition.Incompletable);
			yield return Toils_Goto.Goto(TargetIndex.B, PathEndMode.OnCell);
			float totalBuildingNutrition = base.TargetA.Thing.def.CostList.Sum((ThingDefCountClass x) => x.thingDef.GetStatValueAbstract(StatDefOf.Nutrition, null) * (float)x.count);
			Toil eat = new Toil();
			eat.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceCell(this.TargetA.Thing.OccupiedRect().ClosestCellTo(this.pawn.Position));
				this.pawn.GainComfortFromCellIfPossible(false);
				if (this.pawn.needs.food != null)
				{
					this.pawn.needs.food.CurLevel += totalBuildingNutrition / (float)this.pawn.GetLord().ownedPawns.Count / (float)eat.defaultDuration;
				}
				if (this.pawn.IsHashIntervalTick(40) && Rand.Value < 0.25f)
				{
					IntVec3 c = Rand.Bool ? this.pawn.Position : this.pawn.RandomAdjacentCellCardinal();
					if (c.InBounds(this.pawn.Map))
					{
						FilthMaker.TryMakeFilth(c, this.pawn.Map, ThingDefOf.Human.race.BloodDef, 1, FilthSourceFlags.None);
					}
				}
			};
			eat.AddFinishAction(delegate
			{
				if (this.pawn.mindState != null)
				{
					this.pawn.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
				}
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, this.pawn.Named(HistoryEventArgsNames.Doer)), true);
			});
			eat.WithEffect(EffecterDefOf.EatMeat, TargetIndex.A, null);
			eat.PlaySustainerOrSound(SoundDefOf.RawMeat_Eat, 1f);
			eat.handlingFacing = true;
			eat.defaultCompleteMode = ToilCompleteMode.Delay;
			eat.defaultDuration = (this.job.doUntilGatheringEnded ? this.job.expiryInterval : this.job.def.joyDuration);
			yield return eat;
			yield break;
		}

		// Token: 0x04001D74 RID: 7540
		private const TargetIndex PlatterIndex = TargetIndex.A;

		// Token: 0x04001D75 RID: 7541
		private const TargetIndex CellIndex = TargetIndex.B;

		// Token: 0x04001D76 RID: 7542
		private const int BloodFilthIntervalTick = 40;

		// Token: 0x04001D77 RID: 7543
		private const float ChanceToProduceBloodFilth = 0.25f;
	}
}
