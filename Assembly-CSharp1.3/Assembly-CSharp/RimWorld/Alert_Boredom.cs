using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001273 RID: 4723
	public class Alert_Boredom : Alert
	{
		// Token: 0x06007102 RID: 28930 RVA: 0x0025A733 File Offset: 0x00258933
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		// Token: 0x170013B5 RID: 5045
		// (get) Token: 0x06007103 RID: 28931 RVA: 0x0025A764 File Offset: 0x00258964
		private List<Pawn> BoredPawns
		{
			get
			{
				this.boredPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.needs.joy != null && (pawn.needs.joy.CurLevelPercentage < 0.24000001f || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(pawn))
					{
						this.boredPawnsResult.Add(pawn);
					}
				}
				return this.boredPawnsResult;
			}
		}

		// Token: 0x06007104 RID: 28932 RVA: 0x0025A818 File Offset: 0x00258A18
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		// Token: 0x06007105 RID: 28933 RVA: 0x0025A828 File Offset: 0x00258A28
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn = null;
			foreach (Pawn pawn2 in this.BoredPawns)
			{
				stringBuilder.AppendLine("   " + pawn2.Label);
				if (pawn == null)
				{
					pawn = pawn2;
				}
			}
			string value = (pawn != null) ? JoyUtility.JoyKindsOnMapString(pawn.Map) : "";
			return "BoredomDesc".Translate(stringBuilder.ToString().TrimEndNewlines(), pawn.LabelShort, value, pawn.Named("PAWN"));
		}

		// Token: 0x04003E32 RID: 15922
		private const float JoyNeedThreshold = 0.24000001f;

		// Token: 0x04003E33 RID: 15923
		private List<Pawn> boredPawnsResult = new List<Pawn>();
	}
}
