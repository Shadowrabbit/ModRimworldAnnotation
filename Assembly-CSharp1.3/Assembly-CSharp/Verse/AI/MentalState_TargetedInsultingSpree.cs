using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005DB RID: 1499
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x1700085D RID: 2141
		// (get) Token: 0x06002B5B RID: 11099 RVA: 0x001030F8 File Offset: 0x001012F8
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002B5C RID: 11100 RVA: 0x00103115 File Offset: 0x00101315
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x06002B5D RID: 11101 RVA: 0x00103120 File Offset: 0x00101320
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) || !this.target.Awake()))
			{
				Pawn target = this.target;
				if (!this.TryFindNewTarget())
				{
					base.RecoverFromState();
					return;
				}
				Messages.Message("MessageTargetedInsultingSpreeChangedTarget".Translate(this.pawn.LabelShort, target.Label, this.target.Label, this.pawn.Named("PAWN"), target.Named("OLDTARGET"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				base.MentalStateTick();
				return;
			}
			else
			{
				if (this.target == null || !InsultingSpreeMentalStateUtility.CanChaseAndInsult(this.pawn, this.target, false, false))
				{
					base.RecoverFromState();
					return;
				}
				base.MentalStateTick();
				return;
			}
		}

		// Token: 0x06002B5E RID: 11102 RVA: 0x00103248 File Offset: 0x00101448
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06002B5F RID: 11103 RVA: 0x00103257 File Offset: 0x00101457
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x06002B60 RID: 11104 RVA: 0x00103284 File Offset: 0x00101484
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06002B61 RID: 11105 RVA: 0x00103314 File Offset: 0x00101514
		public override TaggedString GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.");
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04001A7B RID: 6779
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
