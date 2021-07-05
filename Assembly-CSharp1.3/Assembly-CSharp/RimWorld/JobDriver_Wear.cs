using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200075E RID: 1886
	public class JobDriver_Wear : JobDriver
	{
		// Token: 0x170009B0 RID: 2480
		// (get) Token: 0x0600343B RID: 13371 RVA: 0x00128394 File Offset: 0x00126594
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x0600343C RID: 13372 RVA: 0x001283BA File Offset: 0x001265BA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
			Scribe_Values.Look<int>(ref this.unequipBuffer, "unequipBuffer", 0, false);
		}

		// Token: 0x0600343D RID: 13373 RVA: 0x001283E6 File Offset: 0x001265E6
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x0600343E RID: 13374 RVA: 0x00128408 File Offset: 0x00126608
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true) * 60f);
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			for (int i = wornApparel.Count - 1; i >= 0; i--)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					this.duration += (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
				}
			}
		}

		// Token: 0x0600343F RID: 13375 RVA: 0x001284B3 File Offset: 0x001266B3
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			Toil toil = new Toil();
			toil.tickAction = delegate()
			{
				this.unequipBuffer++;
				this.TryUnequipSomething();
			};
			toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.duration;
			yield return toil;
			yield return Toils_General.Do(delegate
			{
				Apparel apparel = this.Apparel;
				this.pawn.apparel.Wear(apparel, true, false);
				if (this.pawn.outfits != null && this.job.playerForced)
				{
					this.pawn.outfits.forcedHandler.SetForced(apparel, true);
				}
			});
			yield break;
		}

		// Token: 0x06003440 RID: 13376 RVA: 0x001284C4 File Offset: 0x001266C4
		private void TryUnequipSomething()
		{
			Apparel apparel = this.Apparel;
			List<Apparel> wornApparel = this.pawn.apparel.WornApparel;
			int i = wornApparel.Count - 1;
			while (i >= 0)
			{
				if (!ApparelUtility.CanWearTogether(apparel.def, wornApparel[i].def, this.pawn.RaceProps.body))
				{
					int num = (int)(wornApparel[i].GetStatValue(StatDefOf.EquipDelay, true) * 60f);
					if (this.unequipBuffer < num)
					{
						break;
					}
					bool forbid = this.pawn.Faction != null && this.pawn.Faction.HostileTo(Faction.OfPlayer);
					Apparel apparel2;
					if (!this.pawn.apparel.TryDrop(wornApparel[i], out apparel2, this.pawn.PositionHeld, forbid))
					{
						Log.Error(this.pawn + " could not drop " + wornApparel[i].ToStringSafe<Apparel>());
						base.EndJobWith(JobCondition.Errored);
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x04001E3C RID: 7740
		private int duration;

		// Token: 0x04001E3D RID: 7741
		private int unequipBuffer;

		// Token: 0x04001E3E RID: 7742
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
