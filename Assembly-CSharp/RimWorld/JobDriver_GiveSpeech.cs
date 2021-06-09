using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B78 RID: 2936
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x06004503 RID: 17667 RVA: 0x00032D13 File Offset: 0x00030F13
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x00032D20 File Offset: 0x00030F20
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x00032D42 File Offset: 0x00030F42
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				this.job.SetTarget(TargetIndex.B, this.Throne.InteractionCell + this.Throne.Rotation.FacingCell);
			});
			Toil toil = new Toil();
			toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
			toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
			toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_Passable)) != null);
			toil.tickAction = delegate()
			{
				this.pawn.GainComfortFromCellIfPossible(false);
				this.pawn.skills.Learn(SkillDefOf.Social, 0.3f, false);
				if (this.pawn.IsHashIntervalTick(JobDriver_GiveSpeech.MoteInterval.RandomInRange))
				{
					MoteMaker.MakeSpeechBubble(this.pawn, JobDriver_GiveSpeech.moteIcon);
				}
				this.rotateToFace = TargetIndex.B;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x04002EB6 RID: 11958
		private const TargetIndex ThroneIndex = TargetIndex.A;

		// Token: 0x04002EB7 RID: 11959
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x04002EB8 RID: 11960
		private static readonly IntRange MoteInterval = new IntRange(300, 500);

		// Token: 0x04002EB9 RID: 11961
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
