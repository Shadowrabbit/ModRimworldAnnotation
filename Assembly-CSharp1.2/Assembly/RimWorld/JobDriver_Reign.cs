using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C53 RID: 3155
	public class JobDriver_Reign : JobDriver_Meditate
	{
		// Token: 0x17000BA9 RID: 2985
		// (get) Token: 0x06004A1A RID: 18970 RVA: 0x0003547A File Offset: 0x0003367A
		private Building_Throne Throne
		{
			get
			{
				return base.TargetThingA as Building_Throne;
			}
		}

		// Token: 0x06004A1B RID: 18971 RVA: 0x00035487 File Offset: 0x00033687
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004A1C RID: 18972 RVA: 0x0019FA70 File Offset: 0x0019DC70
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

		// Token: 0x06004A1D RID: 18973 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override bool CanBeginNowWhileLyingDown()
		{
			return false;
		}

		// Token: 0x06004A1E RID: 18974 RVA: 0x000354A9 File Offset: 0x000336A9
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
			toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_Passable)) != null);
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
					if (this.Throne.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.ThroneRoom)
					{
						thoughtDef = ThoughtDefOf.ReignedInThroneroom;
					}
					if (thoughtDef != null)
					{
						int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(this.Throne.GetRoom(RegionType.Set_Passable).GetStat(RoomStatDefOf.Impressiveness));
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

		// Token: 0x0400312E RID: 12590
		protected const TargetIndex FacingInd = TargetIndex.B;

		// Token: 0x0400312F RID: 12591
		protected const int ApplyThoughtInitialTicks = 10000;
	}
}
