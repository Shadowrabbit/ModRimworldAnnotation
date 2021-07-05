using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127C RID: 4732
	public class Alert_HunterHasShieldAndRangedWeapon : Alert
	{
		// Token: 0x170013BC RID: 5052
		// (get) Token: 0x06007121 RID: 28961 RVA: 0x0025B27C File Offset: 0x0025947C
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

		// Token: 0x06007122 RID: 28962 RVA: 0x0025B300 File Offset: 0x00259500
		public Alert_HunterHasShieldAndRangedWeapon()
		{
			this.defaultLabel = "HunterHasShieldAndRangedWeapon".Translate();
			this.defaultExplanation = "HunterHasShieldAndRangedWeaponDesc".Translate();
		}

		// Token: 0x06007123 RID: 28963 RVA: 0x0025B33D File Offset: 0x0025953D
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.BadHunters);
		}

		// Token: 0x04003E3B RID: 15931
		private List<Pawn> badHuntersResult = new List<Pawn>();
	}
}
