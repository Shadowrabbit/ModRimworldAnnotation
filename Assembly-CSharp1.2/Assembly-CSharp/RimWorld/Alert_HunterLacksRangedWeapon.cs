using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194E RID: 6478
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x170016A8 RID: 5800
		// (get) Token: 0x06008F80 RID: 36736 RVA: 0x00295070 File Offset: 0x00293270
		private List<Pawn> HuntersWithoutRangedWeapon
		{
			get
			{
				this.huntersWithoutRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
				{
					if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) && !pawn.Downed)
					{
						this.huntersWithoutRangedWeaponResult.Add(pawn);
					}
				}
				return this.huntersWithoutRangedWeaponResult;
			}
		}

		// Token: 0x06008F81 RID: 36737 RVA: 0x0029510C File Offset: 0x0029330C
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x06008F82 RID: 36738 RVA: 0x00060237 File Offset: 0x0005E437
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}

		// Token: 0x04005B6C RID: 23404
		private List<Pawn> huntersWithoutRangedWeaponResult = new List<Pawn>();
	}
}
