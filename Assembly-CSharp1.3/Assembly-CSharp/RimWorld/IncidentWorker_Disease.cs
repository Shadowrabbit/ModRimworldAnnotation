using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C02 RID: 3074
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		// Token: 0x06004851 RID: 18513
		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		// Token: 0x06004852 RID: 18514 RVA: 0x0017E693 File Offset: 0x0017C893
		protected IEnumerable<Pawn> PotentialVictims(IIncidentTarget target)
		{
			return this.PotentialVictimCandidates(target).Where(delegate(Pawn p)
			{
				if (p.ParentHolder is Building_CryptosleepCasket)
				{
					return false;
				}
				if (!this.def.diseasePartsToAffect.NullOrEmpty<BodyPartDef>())
				{
					bool flag = false;
					for (int i = 0; i < this.def.diseasePartsToAffect.Count; i++)
					{
						if (IncidentWorker_Disease.CanAddHediffToAnyPartOfDef(p, this.def.diseaseIncident, this.def.diseasePartsToAffect[i]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return p.RaceProps.IsFlesh;
			});
		}

		// Token: 0x06004853 RID: 18515
		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		// Token: 0x06004854 RID: 18516 RVA: 0x0017E6B0 File Offset: 0x0017C8B0
		private static bool CanAddHediffToAnyPartOfDef(Pawn pawn, HediffDef hediffDef, BodyPartDef partDef)
		{
			List<BodyPartRecord> allParts = pawn.def.race.body.AllParts;
			for (int i = 0; i < allParts.Count; i++)
			{
				BodyPartRecord bodyPartRecord = allParts[i];
				if (bodyPartRecord.def == partDef && !pawn.health.hediffSet.PartIsMissing(bodyPartRecord) && !pawn.health.hediffSet.HasHediff(hediffDef, bodyPartRecord, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0017E720 File Offset: 0x0017C920
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0017E738 File Offset: 0x0017C938
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			string text;
			List<Pawn> list = this.ApplyToPawns(this.ActualVictims(parms).ToList<Pawn>(), out text);
			if (!list.Any<Pawn>() && text.NullOrEmpty())
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append("  - " + list[i].LabelNoCountColored.Resolve());
			}
			string text2;
			if (list.Any<Pawn>())
			{
				text2 = string.Format(this.def.letterText, new object[]
				{
					list.Count.ToString(),
					Faction.OfPlayer.def.pawnsPlural,
					this.def.diseaseIncident.label,
					stringBuilder.ToString()
				});
			}
			else
			{
				text2 = "";
			}
			if (!text.NullOrEmpty())
			{
				if (!text2.NullOrEmpty())
				{
					text2 += "\n\n";
				}
				text2 += text;
			}
			base.SendStandardLetter(this.def.letterLabel, text2, this.def.letterDef, parms, list, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x0017E87C File Offset: 0x0017CA7C
		public List<Pawn> ApplyToPawns(IEnumerable<Pawn> pawns, out string blockedInfo)
		{
			List<Pawn> list = new List<Pawn>();
			Dictionary<HediffDef, List<Pawn>> dictionary = new Dictionary<HediffDef, List<Pawn>>();
			foreach (Pawn pawn in pawns)
			{
				HediffDef hediffDef = null;
				if (Rand.Chance(pawn.health.immunity.DiseaseContractChanceFactor(this.def.diseaseIncident, out hediffDef, null)))
				{
					HediffGiverUtility.TryApply(pawn, this.def.diseaseIncident, this.def.diseasePartsToAffect, false, 1, null);
					TaleRecorder.RecordTale(TaleDefOf.IllnessRevealed, new object[]
					{
						pawn,
						this.def.diseaseIncident
					});
					list.Add(pawn);
				}
				else if (hediffDef != null)
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = new List<Pawn>();
					}
					dictionary[hediffDef].Add(pawn);
				}
			}
			blockedInfo = "";
			foreach (KeyValuePair<HediffDef, List<Pawn>> keyValuePair in dictionary)
			{
				if (keyValuePair.Key != this.def.diseaseIncident)
				{
					if (blockedInfo.Length != 0)
					{
						blockedInfo += "\n\n";
					}
					blockedInfo += "LetterDisease_Blocked".Translate(keyValuePair.Key.LabelCap, this.def.diseaseIncident.label, (from victim in keyValuePair.Value
					select victim.LabelShort).ToLineList("  - ", false));
				}
			}
			return list;
		}
	}
}
