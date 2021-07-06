using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C51 RID: 3153
	public class JobDriver_Meditate : JobDriver
	{
		// Token: 0x17000BA5 RID: 2981
		// (get) Token: 0x06004A03 RID: 18947 RVA: 0x000353D8 File Offset: 0x000335D8
		public LocalTargetInfo Focus
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C);
			}
		}

		// Token: 0x17000BA6 RID: 2982
		// (get) Token: 0x06004A04 RID: 18948 RVA: 0x0019F3B0 File Offset: 0x0019D5B0
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).IsValid;
			}
		}

		// Token: 0x06004A05 RID: 18949 RVA: 0x0019F3D4 File Offset: 0x0019D5D4
		protected string PsyfocusPerDayReport()
		{
			if (!this.pawn.HasPsylink)
			{
				return "";
			}
			Thing thing = this.Focus.Thing;
			float f = MeditationUtility.PsyfocusGainPerTick(this.pawn, thing) * 60000f;
			return "\n" + "PsyfocusPerDayOfMeditation".Translate(f.ToStringPercent()).CapitalizeFirst();
		}

		// Token: 0x06004A06 RID: 18950 RVA: 0x0019F444 File Offset: 0x0019D644
		public override string GetReport()
		{
			if (!ModsConfig.RoyaltyActive)
			{
				return base.GetReport();
			}
			Thing thing = this.Focus.Thing;
			if (thing != null && !thing.Destroyed)
			{
				return "MeditatingAt".Translate() + ": " + thing.LabelShort.CapitalizeFirst() + "." + this.PsyfocusPerDayReport();
			}
			return base.GetReport() + this.PsyfocusPerDayReport();
		}

		// Token: 0x06004A07 RID: 18951 RVA: 0x0002DA01 File Offset: 0x0002BC01
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004A08 RID: 18952 RVA: 0x000353E6 File Offset: 0x000335E6
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil meditate = new Toil();
			meditate.socialMode = RandomSocialMode.Off;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.B);
				meditate = Toils_LayDown.LayDown(TargetIndex.B, this.job.GetTarget(TargetIndex.B).Thing is Building_Bed, false, false, true);
			}
			else
			{
				yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				meditate.initAction = delegate()
				{
					LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
					if (target.IsValid)
					{
						this.faceDir = target.Cell - this.pawn.Position;
						return;
					}
					this.faceDir = (this.job.def.faceDir.IsValid ? this.job.def.faceDir : Rot4.Random).FacingCell;
				};
				if (this.Focus != null)
				{
					meditate.FailOnDespawnedNullOrForbidden(TargetIndex.C);
					if (this.pawn.HasPsylink && this.Focus.Thing != null)
					{
						meditate.FailOn(() => this.Focus.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, this.pawn, true) < float.Epsilon);
					}
				}
				meditate.handlingFacing = true;
			}
			meditate.defaultCompleteMode = ToilCompleteMode.Delay;
			meditate.defaultDuration = this.job.def.joyDuration;
			meditate.FailOn(() => !MeditationUtility.CanMeditateNow(this.pawn) || !MeditationUtility.SafeEnvironmentalConditions(this.pawn, base.TargetLocA, base.Map));
			meditate.AddPreTickAction(delegate
			{
				bool flag = this.pawn.GetTimeAssignment() == TimeAssignmentDefOf.Meditate;
				if (this.job.ignoreJoyTimeAssignment)
				{
					Pawn_PsychicEntropyTracker psychicEntropy = this.pawn.psychicEntropy;
					if (!flag && psychicEntropy.TargetPsyfocus < psychicEntropy.CurrentPsyfocus && (psychicEntropy.TargetPsyfocus < this.job.psyfocusTargetLast || this.job.wasOnMeditationTimeAssignment))
					{
						base.EndJobWith(JobCondition.InterruptForced);
						return;
					}
					this.job.psyfocusTargetLast = psychicEntropy.TargetPsyfocus;
					this.job.wasOnMeditationTimeAssignment = flag;
				}
				if (this.faceDir.IsValid && !this.FromBed)
				{
					this.pawn.rotationTracker.FaceCell(this.pawn.Position + this.faceDir);
				}
				this.MeditationTick();
				if (ModLister.RoyaltyInstalled && MeditationFocusDefOf.Natural.CanPawnUse(this.pawn))
				{
					int num = GenRadial.NumCellsInRadius(MeditationUtility.FocusObjectSearchRadius);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = this.pawn.Position + GenRadial.RadialPattern[i];
						if (c.InBounds(this.pawn.Map))
						{
							Plant plant = c.GetPlant(this.pawn.Map);
							if (plant != null && plant.def == ThingDefOf.Plant_TreeAnima)
							{
								CompSpawnSubplant compSpawnSubplant = plant.TryGetComp<CompSpawnSubplant>();
								if (compSpawnSubplant != null)
								{
									compSpawnSubplant.AddProgress_NewTmp(JobDriver_Meditate.AnimaTreeSubplantProgressPerTick, false);
								}
							}
						}
					}
				}
			});
			yield return meditate;
			yield break;
		}

		// Token: 0x06004A09 RID: 18953 RVA: 0x000353F6 File Offset: 0x000335F6
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.job.psyfocusTargetLast = this.pawn.psychicEntropy.TargetPsyfocus;
		}

		// Token: 0x06004A0A RID: 18954 RVA: 0x0019F4CC File Offset: 0x0019D6CC
		protected void MeditationTick()
		{
			this.pawn.skills.Learn(SkillDefOf.Intellectual, 0.018000001f, false);
			this.pawn.GainComfortFromCellIfPossible(false);
			if (this.pawn.needs.joy != null)
			{
				JoyUtility.JoyTickCheckEnd(this.pawn, JoyTickFullJoyAction.None, 1f, null);
			}
			if (this.pawn.IsHashIntervalTick(100))
			{
				MoteMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, ThingDefOf.Mote_Meditating);
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.pawn.psychicEntropy.Notify_Meditated();
				if (this.pawn.HasPsylink && this.pawn.psychicEntropy.PsychicSensitivity > 1E-45f)
				{
					if (this.psyfocusMote == null || this.psyfocusMote.Destroyed)
					{
						this.psyfocusMote = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero, 1f, -1f);
					}
					this.psyfocusMote.Maintain();
					if (this.sustainer == null || this.sustainer.Ended)
					{
						this.sustainer = SoundDefOf.MeditationGainPsyfocus.TrySpawnSustainer(SoundInfo.InMap(this.pawn, MaintenanceType.PerTick));
					}
					this.sustainer.Maintain();
					this.pawn.psychicEntropy.GainPsyfocus(this.Focus.Thing);
				}
			}
		}

		// Token: 0x06004A0B RID: 18955 RVA: 0x0019F63C File Offset: 0x0019D83C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.faceDir, "faceDir", default(IntVec3), false);
		}

		// Token: 0x04003121 RID: 12577
		protected IntVec3 faceDir;

		// Token: 0x04003122 RID: 12578
		private Mote psyfocusMote;

		// Token: 0x04003123 RID: 12579
		protected Sustainer sustainer;

		// Token: 0x04003124 RID: 12580
		protected const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x04003125 RID: 12581
		protected const TargetIndex BedInd = TargetIndex.B;

		// Token: 0x04003126 RID: 12582
		protected const TargetIndex FocusInd = TargetIndex.C;

		// Token: 0x04003127 RID: 12583
		public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

		// Token: 0x04003128 RID: 12584
		private const int TicksBetweenMotesBase = 100;
	}
}
