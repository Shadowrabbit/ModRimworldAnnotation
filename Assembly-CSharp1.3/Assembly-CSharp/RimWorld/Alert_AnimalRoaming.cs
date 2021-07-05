using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001284 RID: 4740
	public class Alert_AnimalRoaming : Alert
	{
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06007140 RID: 28992 RVA: 0x0025BF69 File Offset: 0x0025A169
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.CalculateTargets();
				return this.targets;
			}
		}

		// Token: 0x06007141 RID: 28993 RVA: 0x0025BF77 File Offset: 0x0025A177
		public Alert_AnimalRoaming()
		{
			this.defaultLabel = "AlertAnimalIsRoaming".Translate();
		}

		// Token: 0x06007142 RID: 28994 RVA: 0x0025BFAC File Offset: 0x0025A1AC
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (pawn.MentalStateDef == MentalStateDefOf.Roaming)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x06007143 RID: 28995 RVA: 0x0025C04C File Offset: 0x0025A24C
		public override TaggedString GetExplanation()
		{
			return "AlertAnimalIsRoamingDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06007144 RID: 28996 RVA: 0x0025C06D File Offset: 0x0025A26D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E4A RID: 15946
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E4B RID: 15947
		private List<string> pawnNames = new List<string>();
	}
}
