using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000754 RID: 1876
	public class JobDriver_Meditate : JobDriver
	{
		// Token: 0x170009A9 RID: 2473
		// (get) Token: 0x060033F4 RID: 13300 RVA: 0x00126D73 File Offset: 0x00124F73
		public LocalTargetInfo Focus
		{
			get
			{
				return this.job.GetTarget(TargetIndex.C);
			}
		}

		// Token: 0x170009AA RID: 2474
		// (get) Token: 0x060033F5 RID: 13301 RVA: 0x00126D84 File Offset: 0x00124F84
		private bool FromBed
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).IsValid;
			}
		}

		// Token: 0x060033F6 RID: 13302 RVA: 0x00126DA8 File Offset: 0x00124FA8
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

		// Token: 0x060033F7 RID: 13303 RVA: 0x00126E18 File Offset: 0x00125018
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

		// Token: 0x060033F8 RID: 13304 RVA: 0x000FA68B File Offset: 0x000F888B
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x00126E9D File Offset: 0x0012509D
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Toil meditate = new Toil();
			meditate.socialMode = RandomSocialMode.Off;
			if (this.FromBed)
			{
				this.KeepLyingDown(TargetIndex.B);
				meditate = Toils_LayDown.LayDown(TargetIndex.B, this.job.GetTarget(TargetIndex.B).Thing is Building_Bed, false, false, true, PawnPosture.LayingOnGroundNormal);
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
					meditate.FailOnDespawnedOrNull(TargetIndex.C);
					meditate.FailOnForbidden(TargetIndex.A);
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
				if (ModsConfig.RoyaltyActive && MeditationFocusDefOf.Natural.CanPawnUse(this.pawn))
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
									compSpawnSubplant.AddProgress(JobDriver_Meditate.AnimaTreeSubplantProgressPerTick, false);
								}
							}
						}
					}
				}
			});
			yield return meditate;
			yield break;
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00126EAD File Offset: 0x001250AD
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.job.psyfocusTargetLast = this.pawn.psychicEntropy.TargetPsyfocus;
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x00126ED0 File Offset: 0x001250D0
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
				FleckMaker.ThrowMetaIcon(this.pawn.Position, this.pawn.Map, FleckDefOf.Meditating, 0.42f);
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.pawn.psychicEntropy.Notify_Meditated();
				if (this.pawn.HasPsylink && this.pawn.psychicEntropy.PsychicSensitivity > 1E-45f)
				{
					float yOffset = (float)(this.pawn.Position.x % 2 + this.pawn.Position.z % 2) / 10f;
					if (this.psyfocusMote == null || this.psyfocusMote.Destroyed)
					{
						this.psyfocusMote = MoteMaker.MakeAttachedOverlay(this.pawn, ThingDefOf.Mote_PsyfocusPulse, Vector3.zero, 1f, -1f);
						this.psyfocusMote.yOffset = yOffset;
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

		// Token: 0x060033FC RID: 13308 RVA: 0x00127080 File Offset: 0x00125280
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<IntVec3>(ref this.faceDir, "faceDir", default(IntVec3), false);
		}

		// Token: 0x04001E27 RID: 7719
		protected IntVec3 faceDir;

		// Token: 0x04001E28 RID: 7720
		private Mote psyfocusMote;

		// Token: 0x04001E29 RID: 7721
		protected Sustainer sustainer;

		// Token: 0x04001E2A RID: 7722
		protected const TargetIndex SpotInd = TargetIndex.A;

		// Token: 0x04001E2B RID: 7723
		protected const TargetIndex BedInd = TargetIndex.B;

		// Token: 0x04001E2C RID: 7724
		protected const TargetIndex FocusInd = TargetIndex.C;

		// Token: 0x04001E2D RID: 7725
		public static float AnimaTreeSubplantProgressPerTick = 6.666667E-05f;

		// Token: 0x04001E2E RID: 7726
		private const int TicksBetweenMotesBase = 100;
	}
}
