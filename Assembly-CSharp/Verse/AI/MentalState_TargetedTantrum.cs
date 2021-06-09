using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A3B RID: 2619
	public class MentalState_TargetedTantrum : MentalState_Tantrum
	{
		// Token: 0x06003E74 RID: 15988 RVA: 0x00178658 File Offset: 0x00176858
		public override void MentalStateTick()
		{
			if (this.target == null || this.target.Destroyed)
			{
				base.RecoverFromState();
				return;
			}
			if (this.target.Spawned && this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
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

		// Token: 0x06003E75 RID: 15989 RVA: 0x0002EF38 File Offset: 0x0002D138
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00178760 File Offset: 0x00176960
		private bool TryFindNewTarget()
		{
			TantrumMentalStateUtility.GetSmashableThingsNear(this.pawn, this.pawn.Position, MentalState_TargetedTantrum.tmpThings, null, 300, 40);
			bool result = MentalState_TargetedTantrum.tmpThings.TryRandomElementByWeight((Thing x) => x.MarketValue * (float)x.stackCount, out this.target);
			MentalState_TargetedTantrum.tmpThings.Clear();
			return result;
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x001787CC File Offset: 0x001769CC
		public override string GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04002B02 RID: 11010
		public const int MinMarketValue = 300;

		// Token: 0x04002B03 RID: 11011
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
