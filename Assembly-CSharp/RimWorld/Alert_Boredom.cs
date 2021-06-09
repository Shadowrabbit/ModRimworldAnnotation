using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001962 RID: 6498
	public class Alert_Boredom : Alert
	{
		// Token: 0x06008FCD RID: 36813 RVA: 0x0006063F File Offset: 0x0005E83F
		public Alert_Boredom()
		{
			this.defaultLabel = "Boredom".Translate();
			this.defaultPriority = AlertPriority.Medium;
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06008FCE RID: 36814 RVA: 0x00296640 File Offset: 0x00294840
		private List<Pawn> BoredPawns
		{
			get
			{
				this.boredPawnsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if ((pawn.needs.joy.CurLevelPercentage < 0.24000001f || pawn.GetTimeAssignment() == TimeAssignmentDefOf.Joy) && pawn.needs.joy.tolerances.BoredOfAllAvailableJoyKinds(pawn))
					{
						this.boredPawnsResult.Add(pawn);
					}
				}
				return this.boredPawnsResult;
			}
		}

		// Token: 0x06008FCF RID: 36815 RVA: 0x0006066E File Offset: 0x0005E86E
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BoredPawns);
		}

		// Token: 0x06008FD0 RID: 36816 RVA: 0x002966E4 File Offset: 0x002948E4
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

		// Token: 0x04005B83 RID: 23427
		private const float JoyNeedThreshold = 0.24000001f;

		// Token: 0x04005B84 RID: 23428
		private List<Pawn> boredPawnsResult = new List<Pawn>();
	}
}
