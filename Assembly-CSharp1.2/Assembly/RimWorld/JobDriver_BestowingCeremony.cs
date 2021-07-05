using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000BAD RID: 2989
	public class JobDriver_BestowingCeremony : JobDriver
	{
		// Token: 0x17000AFF RID: 2815
		// (get) Token: 0x0600463C RID: 17980 RVA: 0x00194AF8 File Offset: 0x00192CF8
		private Pawn Bestower
		{
			get
			{
				return base.TargetA.Pawn;
			}
		}

		// Token: 0x17000B00 RID: 2816
		// (get) Token: 0x0600463D RID: 17981 RVA: 0x0003360E File Offset: 0x0003180E
		private LordJob_BestowingCeremony CeremonyJob
		{
			get
			{
				return (LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob;
			}
		}

		// Token: 0x17000B01 RID: 2817
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x0002DE50 File Offset: 0x0002C050
		private LocalTargetInfo BestowSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x0600463F RID: 17983 RVA: 0x00194B14 File Offset: 0x00192D14
		public static bool AnalyzeThroneRoom(Pawn bestower, Pawn target)
		{
			RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
			if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null)
			{
				using (List<RoomRequirement>.Enumerator enumerator = titleAwardedWhenUpdating.throneRoomRequirements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Met(bestower.GetRoom(RegionType.Set_Passable), target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06004640 RID: 17984 RVA: 0x00194BA4 File Offset: 0x00192DA4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.BestowSpot, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bestower, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004641 RID: 17985 RVA: 0x00033625 File Offset: 0x00031825
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Bestowing cermonies are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 5464564, false);
				yield break;
			}
			base.AddFailCondition(() => this.Bestower.GetLord() == null || this.Bestower.GetLord().CurLordToil == null || !(this.Bestower.GetLord().CurLordToil is LordToil_BestowingCeremony_Wait));
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil waitToil = Toils_General.Wait(471, TargetIndex.None);
			waitToil.AddPreInitAction(delegate
			{
				Messages.Message("MessageBestowingCeremonyStarted".Translate(this.pawn.Named("PAWN")), this.Bestower, MessageTypeDefOf.PositiveEvent, true);
			});
			waitToil.AddPreInitAction(delegate
			{
				if (!JobDriver_BestowingCeremony.AnalyzeThroneRoom(this.Bestower, this.pawn))
				{
					Messages.Message("BestowingCeremonyThroneroomRequirementsNotSatisfied".Translate(this.pawn.Named("PAWN"), this.pawn.royalty.GetTitleAwardedWhenUpdating(this.Bestower.Faction, this.pawn.royalty.GetFavor(this.Bestower.Faction)).label.Named("TITLE")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
					((LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob).MakeCeremonyFail();
				}
			});
			waitToil.AddPreInitAction(delegate
			{
				SoundDefOf.Bestowing_Start.PlayOneShot(this.pawn);
			});
			waitToil.tickAction = delegate()
			{
				this.pawn.rotationTracker.FaceTarget(this.Bestower);
				if (this.mote == null || this.mote.Destroyed)
				{
					Vector3 loc = (this.pawn.TrueCenter() + this.Bestower.TrueCenter()) / 2f;
					this.mote = MoteMaker.MakeStaticMote(loc, this.pawn.Map, ThingDefOf.Mote_Bestow, 1f);
				}
				this.mote.Maintain();
				if ((this.sound == null || this.sound.Ended) && waitToil.actor.jobs.curDriver.ticksLeftThisToil <= 307)
				{
					this.sound = SoundDefOf.Bestowing_Warmup.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(this.pawn.Position, this.pawn.Map, false), MaintenanceType.PerTick));
				}
				if (this.sound != null)
				{
					this.sound.Maintain();
				}
			};
			waitToil.handlingFacing = false;
			waitToil.socialMode = RandomSocialMode.Off;
			waitToil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return waitToil;
			yield return Toils_General.Do(delegate
			{
				this.CeremonyJob.FinishCeremony(this.pawn);
				MoteMaker.MakeStaticMote((this.pawn.TrueCenter() + this.Bestower.TrueCenter()) / 2f, this.pawn.Map, ThingDefOf.Mote_PsycastAreaEffect, 2f);
				SoundDefOf.Bestowing_Finished.PlayOneShot(this.pawn);
			});
			yield break;
		}

		// Token: 0x04002F3E RID: 12094
		public const float WarmupSoundLength = 5.125f;

		// Token: 0x04002F3F RID: 12095
		public const int BestowTimeTicks = 471;

		// Token: 0x04002F40 RID: 12096
		public const int PlayWarmupSoundAfterTicks = 307;

		// Token: 0x04002F41 RID: 12097
		protected const TargetIndex BestowerInd = TargetIndex.A;

		// Token: 0x04002F42 RID: 12098
		protected const TargetIndex BestowSpotInd = TargetIndex.B;

		// Token: 0x04002F43 RID: 12099
		private Mote mote;

		// Token: 0x04002F44 RID: 12100
		private Sustainer sound;
	}
}
