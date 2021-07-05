using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005F6 RID: 1526
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x06002BE5 RID: 11237 RVA: 0x00104D6C File Offset: 0x00102F6C
		public override void MentalStateTick()
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
				return;
			}
			if (this.target.Spawned && this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				base.MentalStateTick();
				return;
			}
			Thing target = this.target;
			if (!this.TryFindNewTarget())
			{
				base.RecoverFromState();
				return;
			}
			Messages.Message("MessageTargetedTantrumChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
			base.MentalStateTick();
		}

		// Token: 0x06002BE6 RID: 11238 RVA: 0x00104E74 File Offset: 0x00103074
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06002BE7 RID: 11239 RVA: 0x00104E84 File Offset: 0x00103084
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06002BE8 RID: 11240 RVA: 0x00104EF0 File Offset: 0x001030F0
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04001AA8 RID: 6824
		public const int MinMarketValue = 300;

		// Token: 0x04001AA9 RID: 6825
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
