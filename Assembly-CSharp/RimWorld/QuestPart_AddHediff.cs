using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001098 RID: 4248
	public class QuestPart_AddHediff : QuestPart
	{
		// Token: 0x17000E5D RID: 3677
		// (get) Token: 0x06005C98 RID: 23704 RVA: 0x00040411 File Offset: 0x0003E611
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

		// Token: 0x17000E5E RID: 3678
		// (get) Token: 0x06005C99 RID: 23705 RVA: 0x00040421 File Offset: 0x0003E621
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

		// Token: 0x06005C9A RID: 23706 RVA: 0x001DAD9C File Offset: 0x001D8F9C
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

		// Token: 0x06005C9B RID: 23707 RVA: 0x001DAE40 File Offset: 0x001D9040
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

		// Token: 0x06005C9C RID: 23708 RVA: 0x001DAEF8 File Offset: 0x001D90F8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			this.hediffDef = HediffDefOf.Anesthetic;
			this.pawns.Add(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
		}

		// Token: 0x06005C9D RID: 23709 RVA: 0x00040431 File Offset: 0x0003E631
		public override void ReplacePawnReferences(Pawn replace, Pawn with)
		{
			this.pawns.Replace(replace, with);
		}

		// Token: 0x04003E00 RID: 15872
		public List<Pawn> pawns = new List<Pawn>();

		// Token: 0x04003E01 RID: 15873
		public List<BodyPartDef> partsToAffect;

		// Token: 0x04003E02 RID: 15874
		public string inSignal;

		// Token: 0x04003E03 RID: 15875
		public HediffDef hediffDef;

		// Token: 0x04003E04 RID: 15876
		public bool checkDiseaseContractChance;

		// Token: 0x04003E05 RID: 15877
		public bool addToHyperlinks;
	}
}
