using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000708 RID: 1800
	public class JobDriver_BestowingCeremony : JobDriver
	{
		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x060031FB RID: 12795 RVA: 0x00121AD0 File Offset: 0x0011FCD0
		private Pawn Bestower
		{
			get
			{
				return base.TargetA.Pawn;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x060031FC RID: 12796 RVA: 0x00121AEB File Offset: 0x0011FCEB
		private LordJob_BestowingCeremony CeremonyJob
		{
			get
			{
				return (LordJob_BestowingCeremony)this.Bestower.GetLord().LordJob;
			}
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x060031FD RID: 12797 RVA: 0x000FE3EF File Offset: 0x000FC5EF
		private LocalTargetInfo BestowSpot
		{
			get
			{
				return this.job.targetB;
			}
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x00121B04 File Offset: 0x0011FD04
		public static bool AnalyzeThroneRoom(Pawn bestower, Pawn target)
		{
			RoyalTitleDef titleAwardedWhenUpdating = target.royalty.GetTitleAwardedWhenUpdating(bestower.Faction, target.royalty.GetFavor(bestower.Faction));
			if (titleAwardedWhenUpdating != null && titleAwardedWhenUpdating.throneRoomRequirements != null)
			{
				using (List<RoomRequirement>.Enumerator enumerator = titleAwardedWhenUpdating.throneRoomRequirements.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.MetOrDisabled(bestower.GetRoom(RegionType.Set_All), target))
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x00121B94 File Offset: 0x0011FD94
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.BestowSpot, this.job, 1, -1, null, errorOnFailed) && this.pawn.Reserve(this.Bestower, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x00121BE0 File Offset: 0x0011FDE0
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (!ModLister.CheckRoyalty("Bestowing ceremony"))
			{
				yield break;
			}
			base.AddFailCondition(() => this.Bestower.GetLord() == null || this.Bestower.GetLord().CurLordToil == null || !(this.Bestower.GetLord().CurLordToil is LordToil_BestowingCeremony_Perform));
			yield return Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
			Toil waitToil = Toils_General.Wait(5000, TargetIndex.None);
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
				Mote mote = this.mote;
				if (mote != null)
				{
					mote.Maintain();
				}
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
				FleckMaker.Static((this.pawn.TrueCenter() + this.Bestower.TrueCenter()) / 2f, this.pawn.Map, FleckDefOf.PsycastAreaEffect, 2f);
				SoundDefOf.Bestowing_Finished.PlayOneShot(this.pawn);
			});
			yield break;
		}

		// Token: 0x04001DA4 RID: 7588
		public const float WarmupSoundLength = 5.125f;

		// Token: 0x04001DA5 RID: 7589
		public const int BestowTimeTicks = 5000;

		// Token: 0x04001DA6 RID: 7590
		public const int PlayWarmupSoundAfterTicks = 307;

		// Token: 0x04001DA7 RID: 7591
		protected const TargetIndex BestowerInd = TargetIndex.A;

		// Token: 0x04001DA8 RID: 7592
		protected const TargetIndex BestowSpotInd = TargetIndex.B;

		// Token: 0x04001DA9 RID: 7593
		private Mote mote;

		// Token: 0x04001DAA RID: 7594
		private Sustainer sound;
	}
}
