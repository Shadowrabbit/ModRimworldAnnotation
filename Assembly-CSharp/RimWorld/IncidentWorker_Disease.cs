using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B2 RID: 4530
	public abstract class IncidentWorker_Disease : IncidentWorker
	{
		// Token: 0x060063A5 RID: 25509
		protected abstract IEnumerable<Pawn> PotentialVictimCandidates(IIncidentTarget target);

		// Token: 0x060063A6 RID: 25510 RVA: 0x0004473B File Offset: 0x0004293B
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

		// Token: 0x060063A7 RID: 25511
		protected abstract IEnumerable<Pawn> ActualVictims(IncidentParms parms);

		// Token: 0x060063A8 RID: 25512 RVA: 0x001F02A4 File Offset: 0x001EE4A4
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

		// Token: 0x060063A9 RID: 25513 RVA: 0x00044755 File Offset: 0x00042955
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return this.PotentialVictims(parms.target).Any<Pawn>();
		}

		// Token: 0x060063AA RID: 25514 RVA: 0x001F0314 File Offset: 0x001EE514
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

		// Token: 0x060063AB RID: 25515 RVA: 0x001F0458 File Offset: 0x001EE658
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
