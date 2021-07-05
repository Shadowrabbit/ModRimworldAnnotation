using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200028C RID: 652
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x17000390 RID: 912
		// (get) Token: 0x0600124C RID: 4684 RVA: 0x00069AFD File Offset: 0x00067CFD
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x0600124D RID: 4685 RVA: 0x00069B0A File Offset: 0x00067D0A
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x0600124E RID: 4686 RVA: 0x00069B1E File Offset: 0x00067D1E
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x0600124F RID: 4687 RVA: 0x00069B29 File Offset: 0x00067D29
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x06001250 RID: 4688 RVA: 0x00069B40 File Offset: 0x00067D40
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001251 RID: 4689 RVA: 0x00069B48 File Offset: 0x00067D48
		private void CheckDiscovered()
		{
			if (this.discovered)
			{
				return;
			}
			if (!this.parent.CurStage.becomeVisible)
			{
				return;
			}
			this.discovered = true;
			if (this.Props.sendLetterWhenDiscovered && PawnUtility.ShouldSendNotificationAbout(base.Pawn))
			{
				if (base.Pawn.RaceProps.Humanlike)
				{
					string str;
					if (!this.Props.discoverLetterLabel.NullOrEmpty())
					{
						str = string.Format(this.Props.discoverLetterLabel, base.Pawn.LabelShortCap).CapitalizeFirst();
					}
					else
					{
						str = "LetterLabelNewDisease".Translate() + ": " + base.Def.LabelCap;
					}
					string str2;
					if (!this.Props.discoverLetterText.NullOrEmpty())
					{
						str2 = this.Props.discoverLetterText.Formatted(base.Pawn.LabelIndefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else if (this.parent.Part == null)
					{
						str2 = "NewDisease".Translate(base.Pawn.Named("PAWN"), base.Def.label, base.Pawn.LabelDefinite()).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						str2 = "NewPartDisease".Translate(base.Pawn.Named("PAWN"), this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.label).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
					}
					Find.LetterStack.ReceiveLetter(str, str2, (this.Props.letterType != null) ? this.Props.letterType : LetterDefOf.NegativeEvent, base.Pawn, null, null, null, null);
					return;
				}
				string text;
				if (!this.Props.discoverLetterText.NullOrEmpty())
				{
					string value = base.Pawn.KindLabelIndefinite();
					if (base.Pawn.Name.IsValid && !base.Pawn.Name.Numerical)
					{
						value = string.Concat(new object[]
						{
							base.Pawn.Name,
							" (",
							base.Pawn.KindLabel,
							")"
						});
					}
					text = this.Props.discoverLetterText.Formatted(value, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else if (this.parent.Part == null)
				{
					text = "NewDiseaseAnimal".Translate(base.Pawn.LabelShort, base.Def.LabelCap, base.Pawn.LabelDefinite(), base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				else
				{
					text = "NewPartDiseaseAnimal".Translate(base.Pawn.LabelShort, this.parent.Part.Label, base.Pawn.LabelDefinite(), base.Def.LabelCap, base.Pawn.Named("PAWN")).AdjustedFor(base.Pawn, "PAWN", true).CapitalizeFirst();
				}
				Messages.Message(text, base.Pawn, (this.Props.messageType != null) ? this.Props.messageType : MessageTypeDefOf.NegativeHealthEvent, true);
			}
		}

		// Token: 0x06001252 RID: 4690 RVA: 0x00069B40 File Offset: 0x00067D40
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001253 RID: 4691 RVA: 0x00069F93 File Offset: 0x00068193
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x04000DE3 RID: 3555
		private bool discovered;
	}
}
