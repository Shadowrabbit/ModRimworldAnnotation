using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001251 RID: 4689
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06007073 RID: 28787 RVA: 0x002573A0 File Offset: 0x002555A0
		private List<Pawn> HypothermicAnimals
		{
			get
			{
				this.hypothermicAnimalsResult.Clear();
				List<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				for (int i = 0; i < allMaps_Spawned.Count; i++)
				{
					if (allMaps_Spawned[i].RaceProps.Animal && allMaps_Spawned[i].Faction == null && allMaps_Spawned[i].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null)
					{
						this.hypothermicAnimalsResult.Add(allMaps_Spawned[i]);
					}
				}
				return this.hypothermicAnimalsResult;
			}
		}

		// Token: 0x06007074 RID: 28788 RVA: 0x00257426 File Offset: 0x00255626
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x06007075 RID: 28789 RVA: 0x00257430 File Offset: 0x00255630
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Debug alert:\n\nThese wild animals are hypothermic. This may indicate a bug (but it may not, if the animals are trapped or in some other wierd but legitimate situation):");
			foreach (Pawn pawn in this.HypothermicAnimals)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"    ",
					pawn,
					" at ",
					pawn.Position
				}));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06007076 RID: 28790 RVA: 0x002574D0 File Offset: 0x002556D0
		public override AlertReport GetReport()
		{
			if (!Prefs.DevMode)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.HypothermicAnimals);
		}

		// Token: 0x04003E0F RID: 15887
		private List<Pawn> hypothermicAnimalsResult = new List<Pawn>();
	}
}
