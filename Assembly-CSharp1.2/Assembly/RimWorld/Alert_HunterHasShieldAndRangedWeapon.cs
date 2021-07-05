using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200196D RID: 6509
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x170016BD RID: 5821
		// (get) Token: 0x06008FF2 RID: 36850 RVA: 0x00296F04 File Offset: 0x00295104
		private List<Pawn> BadHunters
		{
			get
			{
				this.badHuntersResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && WorkGiver_HunterHunt.HasShieldAndRangedWeapon(pawn))
					{
						this.badHuntersResult.Add(pawn);
					}
				}
				return this.badHuntersResult;
			}
		}

		// Token: 0x06008FF3 RID: 36851 RVA: 0x000608C4 File Offset: 0x0005EAC4
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x06008FF4 RID: 36852 RVA: 0x00060901 File Offset: 0x0005EB01
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		// Token: 0x04005B90 RID: 23440
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
