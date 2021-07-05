using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020003CE RID: 974
	public class HediffComp_Discoverable : HediffComp
	{
		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x00016EF3 File Offset: 0x000150F3
		public HediffCompProperties_Discoverable Props
		{
			get
			{
				return (HediffCompProperties_Discoverable)this.props;
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00016F00 File Offset: 0x00015100
		public override void CompExposeData()
		{
			Scribe_Values.Look<bool>(ref this.discovered, "discovered", false, false);
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00016F14 File Offset: 0x00015114
		public override bool CompDisallowVisible()
		{
			return !this.discovered;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x00016F1F File Offset: 0x0001511F
		public override void CompPostTick(ref float severityAdjustment)
		{
			if (Find.TickManager.TicksGame % 103 == 0)
			{
				this.CheckDiscovered();
			}
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x00016F36 File Offset: 0x00015136
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			this.CheckDiscovered();
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000DE3AC File Offset: 0x000DC5AC
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

		// Token: 0x0600181F RID: 6175 RVA: 0x00016F36 File Offset: 0x00015136
		public override void Notify_PawnDied()
		{
			this.CheckDiscovered();
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x00016F3E File Offset: 0x0001513E
		public override string CompDebugString()
		{
			return "discovered: " + this.discovered.ToString();
		}

		// Token: 0x0400124D RID: 4685
		private bool discovered;
	}
}
