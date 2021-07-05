using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E1 RID: 1505
	public class MentalState_MurderousRage : MentalState
	{
		// Token: 0x06002B87 RID: 11143 RVA: 0x00103E9E File Offset: 0x0010209E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.target, "target", false);
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x0001276E File Offset: 0x0001096E
		public override RandomSocialMode SocialModeMax()
		{
			return RandomSocialMode.Off;
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x00103EB7 File Offset: 0x001020B7
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x00103EC8 File Offset: 0x001020C8
		public override void MentalStateTick()
		{
			base.MentalStateTick();
			if (this.target != null && this.target.Dead)
			{
				base.RecoverFromState();
			}
			if (this.pawn.IsHashIntervalTick(120) && !this.IsTargetStillValidAndReachable())
			{
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageMurderousRageChangedTarget".Translate(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).Resolve().AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick();
			}
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x00103FA4 File Offset: 0x001021A4
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x00104044 File Offset: 0x00102244
		private bool TryFindNewTarget()
		{
			this.target = MurderousRageMentalStateUtility.FindPawnToKill(this.pawn);
			return this.target != null;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x00104060 File Offset: 0x00102260
		public bool IsTargetStillValidAndReachable()
		{
			return this.target != null && this.target.SpawnedParentOrMe != null && (!(this.target.SpawnedParentOrMe is Pawn) || this.target.SpawnedParentOrMe == this.target) && this.pawn.CanReach(this.target.SpawnedParentOrMe, PathEndMode.Touch, Danger.Deadly, true, false, TraverseMode.ByPawn);
		}

		// Token: 0x04001A8E RID: 6798
		public Pawn target;

		// Token: 0x04001A8F RID: 6799
		private const int NoLongerValidTargetCheckInterval = 120;
	}
}
