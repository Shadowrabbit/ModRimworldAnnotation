using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A2C RID: 2604
	public class MentalState_BingingDrug : MentalState_Binging
	{
		// Token: 0x170009CA RID: 2506
		// (get) Token: 0x06003E34 RID: 15924 RVA: 0x0002ECCC File Offset: 0x0002CECC
		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		// Token: 0x06003E35 RID: 15925 RVA: 0x0002ECE4 File Offset: 0x0002CEE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		// Token: 0x06003E36 RID: 15926 RVA: 0x00177F6C File Offset: 0x0017616C
		public override void PostStart(string reason)
		{
			base.PostStart(reason);
			this.ChooseRandomChemical();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				string str = "LetterLabelDrugBinge".Translate(this.chemical.label).CapitalizeFirst() + ": " + this.pawn.LabelShortCap;
				string text = "LetterDrugBinge".Translate(this.pawn.Label, this.chemical.label, this.pawn).CapitalizeFirst();
				if (!reason.NullOrEmpty())
				{
					text = text + "\n\n" + reason;
				}
				Find.LetterStack.ReceiveLetter(str, text, LetterDefOf.ThreatSmall, this.pawn, null, null, null, null);
			}
		}

		// Token: 0x06003E37 RID: 15927 RVA: 0x0017805C File Offset: 0x0017625C
		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(this.pawn.LabelShort, this.chemical.label, this.pawn), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06003E38 RID: 15928 RVA: 0x001780CC File Offset: 0x001762CC
		private void ChooseRandomChemical()
		{
			MentalState_BingingDrug.addictions.Clear();
			List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_Addiction hediff_Addiction = hediffs[i] as Hediff_Addiction;
				if (hediff_Addiction != null && AddictionUtility.CanBingeOnNow(this.pawn, hediff_Addiction.Chemical, DrugCategory.Any))
				{
					MentalState_BingingDrug.addictions.Add(hediff_Addiction.Chemical);
				}
			}
			if (MentalState_BingingDrug.addictions.Count > 0)
			{
				this.chemical = MentalState_BingingDrug.addictions.RandomElement<ChemicalDef>();
				this.drugCategory = DrugCategory.Any;
				MentalState_BingingDrug.addictions.Clear();
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, this.def.drugCategory)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = this.def.drugCategory;
				return;
			}
			this.chemical = (from x in DefDatabase<ChemicalDef>.AllDefsListForReading
			where AddictionUtility.CanBingeOnNow(this.pawn, x, DrugCategory.Any)
			select x).RandomElementWithFallback(null);
			if (this.chemical != null)
			{
				this.drugCategory = DrugCategory.Any;
				return;
			}
			this.chemical = DefDatabase<ChemicalDef>.AllDefsListForReading.RandomElement<ChemicalDef>();
			this.drugCategory = DrugCategory.Any;
		}

		// Token: 0x04002AF5 RID: 10997
		public ChemicalDef chemical;

		// Token: 0x04002AF6 RID: 10998
		public DrugCategory drugCategory;

		// Token: 0x04002AF7 RID: 10999
		private static List<ChemicalDef> addictions = new List<ChemicalDef>();
	}
}
