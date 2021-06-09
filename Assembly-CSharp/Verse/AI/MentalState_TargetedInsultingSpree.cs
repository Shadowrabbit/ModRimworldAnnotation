using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A24 RID: 2596
	public class MentalState_TargetedInsultingSpree : MentalState_InsultingSpree
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0002EAB5 File Offset: 0x0002CCB5
		public override string InspectLine
		{
			get
			{
				return string.Format(this.def.baseInspectLine, this.target.LabelShort);
			}
		}

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003DFF RID: 15871 RVA: 0x0002EAD2 File Offset: 0x0002CCD2
		protected override bool CanEndBeforeMaxDurationNow
		{
			get
			{
				return this.insultedTargetAtLeastOnce;
			}
		}

		// Token: 0x06003E00 RID: 15872 RVA: 0x00177620 File Offset: 0x00175820
		public override void MentalStateTick()
		{
			if (this.target != null && (!this.target.Spawned || !this.pawn.CanReach(this.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn) || !this.target.Awake()))
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

		// Token: 0x06003E01 RID: 15873 RVA: 0x0002EADA File Offset: 0x0002CCDA
		public override void PreStart()
		{
			base.PreStart();
			this.TryFindNewTarget();
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0002EAE9 File Offset: 0x0002CCE9
		private bool TryFindNewTarget()
		{
			InsultingSpreeMentalStateUtility.GetInsultCandidatesFor(this.pawn, MentalState_TargetedInsultingSpree.candidates, false);
			bool result = MentalState_TargetedInsultingSpree.candidates.TryRandomElement(out this.target);
			MentalState_TargetedInsultingSpree.candidates.Clear();
			return result;
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x00177748 File Offset: 0x00175948
		public override void PostEnd()
		{
			base.PostEnd();
			if (this.target != null && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerOnTargetedInsultingSpree".Translate(this.pawn.LabelShort, this.target.Label, this.pawn.Named("PAWN"), this.target.Named("TARGET")), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x001777D8 File Offset: 0x001759D8
		public override string GetBeginLetterText()
		{
			if (this.target == null)
			{
				Log.Error("No target. This should have been checked in this mental state's worker.", false);
				return "";
			}
			return this.def.beginLetter.Formatted(this.pawn.NameShortColored, this.target.NameShortColored, this.pawn.Named("PAWN"), this.target.Named("TARGET")).AdjustedFor(this.pawn, "PAWN", true).Resolve().CapitalizeFirst();
		}

		// Token: 0x04002AE3 RID: 10979
		private static List<Pawn> candidates = new List<Pawn>();
	}
}
