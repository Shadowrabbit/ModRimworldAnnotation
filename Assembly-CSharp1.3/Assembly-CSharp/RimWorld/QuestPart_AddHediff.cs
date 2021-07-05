using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B5F RID: 2911
	public class QuestPart_AddHediff : QuestPart
	{
		// Token: 0x17000BEE RID: 3054
		// (get) Token: 0x06004411 RID: 17425 RVA: 0x00169D54 File Offset: 0x00167F54
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				int num;
				for (int i = 0; i < this.pawns.Count; i = num + 1)
				{
					yield return this.pawns[i];
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x00169D64 File Offset: 0x00167F64
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				foreach (Dialog_InfoCard.Hyperlink hyperlink in base.Hyperlinks)
				{
					yield return hyperlink;
				}
				IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
				if (this.addToHyperlinks)
				{
					yield return new Dialog_InfoCard.Hyperlink(this.hediffDef, -1);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x00169D74 File Offset: 0x00167F74
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				for (int i = 0; i < this.pawns.Count; i++)
				{
					if (!this.pawns[i].DestroyedOrNull() && (!this.checkDiseaseContractChance || Rand.Chance(this.pawns[i].health.immunity.DiseaseContractChanceFactor(this.hediffDef, null))))
					{
						HediffGiverUtility.TryApply(this.pawns[i], this.hediffDef, this.partsToAffect, false, 1, null);
					}
				}
			}
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x00169E18 File Offset: 0x00168018
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Pawn>(ref this.pawns, "pawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<BodyPartDef>(ref this.partsToAffect, "partsToAffect", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<HediffDef>(ref this.hediffDef, "hediffDef");
			Scribe_Values.Look<bool>(ref this.checkDiseaseContractChance, "checkDiseaseContractChance", false, false);
			Scribe_Values.Look<bool>(ref this.addToHyperlinks, "addToHyperlinks", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawns.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00169ED0 File Offset: 0x001680D0
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x00169F1D File Offset: 0x0016811D
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x0400294D RID: 10573
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x0400294E RID: 10574
		public List<BodyPartDef> partsToAffect;

		// Token: 0x0400294F RID: 10575
		public string inSignal;

		// Token: 0x04002950 RID: 10576
		public HediffDef hediffDef;

		// Token: 0x04002951 RID: 10577
		public bool checkDiseaseContractChance;

		// Token: 0x04002952 RID: 10578
		public bool addToHyperlinks;
	}
}
