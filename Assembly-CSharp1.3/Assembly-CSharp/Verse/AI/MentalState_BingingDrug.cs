using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x020005E6 RID: 1510
	public class MentalState_BingingDrug : MentalState_Binging
	{
		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06002B9D RID: 11165 RVA: 0x00104201 File Offset: 0x00102401
		public override string InspectLine
		{
			get
			{
				return string.Format(base.InspectLine, this.chemical.label);
			}
		}

		// Token: 0x06002B9E RID: 11166 RVA: 0x00104219 File Offset: 0x00102419
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ChemicalDef>(ref this.chemical, "chemical");
			Scribe_Values.Look<DrugCategory>(ref this.drugCategory, "drugCategory", DrugCategory.None, false);
		}

		// Token: 0x06002B9F RID: 11167 RVA: 0x00104244 File Offset: 0x00102444
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

		// Token: 0x06002BA0 RID: 11168 RVA: 0x00104334 File Offset: 0x00102534
		public override void PostEnd()
		{
			base.PostEnd();
			if (PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				Messages.Message("MessageNoLongerBingingOnDrug".Translate(this.pawn.LabelShort, this.chemical.label, this.pawn), this.pawn, MessageTypeDefOf.SituationResolved, true);
			}
		}

		// Token: 0x06002BA1 RID: 11169 RVA: 0x001043A4 File Offset: 0x001025A4
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

		// Token: 0x04001A96 RID: 6806
		public ChemicalDef chemical;

		// Token: 0x04001A97 RID: 6807
		public DrugCategory drugCategory;

		// Token: 0x04001A98 RID: 6808
		private static List<ChemicalDef> addictions = new List<ChemicalDef>();
	}
}
