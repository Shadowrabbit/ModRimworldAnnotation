using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005EF RID: 1519
	public class MentalState_SocialFighting : MentalState
	{
		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06002BBB RID: 11195 RVA: 0x001045F5 File Offset: 0x001027F5
		private bool ShouldStop
		{
			get
			{
				return !this.otherPawn.Spawned || this.otherPawn.Dead || this.otherPawn.Downed || !this.IsOtherPawnSocialFightingWithMe;
			}
		}

		// Token: 0x17000867 RID: 2151
		// (get) Token: 0x06002BBC RID: 11196 RVA: 0x0010462C File Offset: 0x0010282C
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

		// Token: 0x06002BBD RID: 11197 RVA: 0x0010466F File Offset: 0x0010286F
		public override void MentalStateTick()
		{
			if (this.ShouldStop)
			{
				base.RecoverFromState();
				return;
			}
			base.MentalStateTick();
		}

		// Token: 0x06002BBE RID: 11198 RVA: 0x00104688 File Offset: 0x00102888
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.pawn.jobs != null)
			{
				this.pawn.jobs.StopAll(false, true);
			}
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
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(def, this.otherPawn, null);
			}
		}

		// Token: 0x06002BBF RID: 11199 RVA: 0x001047E7 File Offset: 0x001029E7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.otherPawn, "otherPawn", false);
		}

		// Token: 0x06002BC0 RID: 11200 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x04001A9A RID: 6810
		public Pawn otherPawn;
	}
}
