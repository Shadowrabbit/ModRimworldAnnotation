using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000755 RID: 1877
	public class JobDriver_Reign : JobDriver_Meditate
	{
		// Token: 0x170009AB RID: 2475
		// (get) Token: 0x06003403 RID: 13315 RVA: 0x00127313 File Offset: 0x00125513
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00127320 File Offset: 0x00125520
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x00127344 File Offset: 0x00125544
		public override string GetReport()
		{
			return string.Concat(new string[]
			{
				this.ReportStringProcessed(this.job.def.reportString),
				": ",
				this.Throne.LabelShort.CapitalizeFirst(),
				".",
				base.PsyfocusPerDayReport()
			});
		}

		// Token: 0x06003406 RID: 13318 RVA: 0x0001276E File Offset: 0x0001096E
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06003407 RID: 13319 RVA: 0x001273A1 File Offset: 0x001255A1
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, this.Throne.InteractionCell + this.Throne.Rotation.FacingCell);
			});
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
			Toil toil = new Toil();
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_All)) != null);
			toil.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = this.job.def.joyDuration;
			toil.tickAction = delegate()
			{
				if (this.pawn.mindState.applyThroneThoughtsTick == 0)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 10000;
				}
				else if (this.pawn.mindState.applyThroneThoughtsTick <= Find.TickManager.TicksGame)
				{
					this.pawn.mindState.applyThroneThoughtsTick = Find.TickManager.TicksGame + 60000;
					ThoughtDef thoughtDef = null;
					if (this.Throne.GetRoom(RegionType.Set_All).Role == RoomRoleDefOf.ThroneRoom)
					{
						thoughtDef = ThoughtDefOf.ReignedInThroneroom;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(this.Throne.GetRoom(RegionType.Set_All).GetStat(RoomStatDefOf.Impressiveness));
						if (thoughtDef.stages[scoreStageIndex] != null)
						{
							this.pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(thoughtDef, scoreStageIndex), null);
						}
					}
				}
				this.rotateToFace = TargetIndex.B;
				base.MeditationTick();
			};
			yield return toil;
			yield break;
		}

		// Token: 0x04001E2F RID: 7727
		protected const TargetIndex FacingInd = TargetIndex.B;

		// Token: 0x04001E30 RID: 7728
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
