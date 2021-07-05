using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020006EB RID: 1771
	[StaticConstructorOnStartup]
	public class JobDriver_GiveSpeech : JobDriver
	{
		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x06003152 RID: 12626 RVA: 0x0011FA4D File Offset: 0x0011DC4D
		private Building_Throne Throne
		{
			get
			{
				return (Building_Throne)base.TargetThingA;
			}
		}

		// Token: 0x06003153 RID: 12627 RVA: 0x0011FA5C File Offset: 0x0011DC5C
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.Throne != null)
			{
				return this.pawn.Reserve(this.Throne, this.job, 1, -1, null, errorOnFailed);
			}
			return this.pawn.Reserve(base.TargetLocA, this.job, 1, -1, null, errorOnFailed);
		}

		// Token: 0x06003154 RID: 12628 RVA: 0x0011FAB2 File Offset: 0x0011DCB2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.A);
			yield return Toils_General.Do(delegate
			{
				LocalTargetInfo pack = (this.Throne != null) ? (this.Throne.InteractionCell + this.Throne.Rotation.FacingCell) : base.TargetB;
				this.job.SetTarget(TargetIndex.B, pack);
			});
			Toil toil = new Toil();
			if (this.Throne != null)
			{
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				toil.FailOn(() => this.Throne.AssignedPawn != this.pawn);
				toil.FailOn(() => RoomRoleWorker_ThroneRoom.Validate(this.Throne.GetRoom(RegionType.Set_All)) != null);
			}
			else
			{
				toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.OnCell);
			}
			toil.tickAction = delegate()
			{
				this.pawn.GainComfortFromCellIfPossible(false);
				this.pawn.skills.Learn(SkillDefOf.Social, 0.3f, false);
				if (this.ticksTillSocialInteraction <= 0)
				{
					if (this.job.showSpeechBubbles)
					{
						MoteMaker.MakeSpeechBubble(this.pawn, JobDriver_GiveSpeech.moteIcon);
					}
					if (this.job.interaction != null)
					{
						Lord lord = this.pawn.GetLord();
						LordJob_Ritual lordJob_Ritual;
						if ((lordJob_Ritual = (((lord != null) ? lord.LordJob : null) as LordJob_Ritual)) != null)
						{
							InteractionUtility.ImitateSocialInteractionWithManyPawns(this.pawn, lordJob_Ritual.lord.ownedPawns, this.job.interaction);
						}
					}
					this.ticksTillSocialInteraction = JobDriver_GiveSpeech.SocialInteractionInterval;
				}
				this.rotateToFace = TargetIndex.B;
				this.ticksTillSocialInteraction--;
			};
			if (ModsConfig.IdeologyActive)
			{
				toil.PlaySustainerOrSound(delegate()
				{
					if (this.pawn.gender != Gender.Female)
					{
						return this.job.speechSoundMale;
					}
					return this.job.speechSoundFemale;
				}, this.pawn.story.VoicePitchFactor);
			}
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			yield return toil;
			yield break;
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0011FAC2 File Offset: 0x0011DCC2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksTillSocialInteraction, "ticksTillSocialInteraction", 0, false);
		}

		// Token: 0x04001D78 RID: 7544
		private int ticksTillSocialInteraction = 60;

		// Token: 0x04001D79 RID: 7545
		private const TargetIndex StandIndex = TargetIndex.A;

		// Token: 0x04001D7A RID: 7546
		private const TargetIndex FacingIndex = TargetIndex.B;

		// Token: 0x04001D7B RID: 7547
		private static readonly int SocialInteractionInterval = 480;

		// Token: 0x04001D7C RID: 7548
		public static readonly Texture2D moteIcon = ContentFinder<Texture2D>.Get("Things/Mote/SpeechSymbols/Speech", true);
	}
}
