using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A35 RID: 2613
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x170009CB RID: 2507
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x0002ED8F File Offset: 0x0002CF8F
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x170009CC RID: 2508
		// (get) Token: 0x06003E53 RID: 15955 RVA: 0x00178278 File Offset: 0x00176478
		private bool IsOtherPawnSocialFightingWithMe
		{
			get
			{
				if (!this.otherPawn.InMentalState)
				{
					return false;
				}
				MentalState_SocialFighting mentalState_SocialFighting = this.otherPawn.MentalState as MentalState_SocialFighting;
				return mentalState_SocialFighting != null && mentalState_SocialFighting.otherPawn == this.pawn;
			}
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0002EDC5 File Offset: 0x0002CFC5
		public override void MentalStateTick()
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
			}
			base.MentalStateTick();
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x001782BC File Offset: 0x001764BC
		public override void PostEnd()
		{
			base.PostEnd();
			this.pawn.jobs.StopAll(false, true);
			this.pawn.mindState.meleeThreat = null;
			if (this.IsOtherPawnSocialFightingWithMe)
			{
				this.otherPawn.MentalState.RecoverFromState();
			}
			if ((PawnUtility.ShouldSendNotificationAbout(this.pawn) || PawnUtility.ShouldSendNotificationAbout(this.otherPawn)) && this.pawn.thingIDNumber < this.otherPawn.thingIDNumber)
			{
				Messages.Message("MessageNoLongerSocialFighting".Translate(this.pawn.LabelShort, this.otherPawn.LabelShort, this.pawn.Named("PAWN1"), this.otherPawn.Named("PAWN2")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
			if (!this.pawn.Dead && this.pawn.needs.mood != null && !this.otherPawn.Dead)
			{
				ThoughtDef def;
				if (Rand.Value < 0.5f)
				{
					def = ThoughtDefOf.HadAngeringFight;
				}
				else
				{
					def = ThoughtDefOf.HadCatharticFight;
				}
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn);
			}
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0002EDDC File Offset: 0x0002CFDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06003E57 RID: 15959 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04002AF9 RID: 11001
		public Pawn otherPawn;
	}
}
