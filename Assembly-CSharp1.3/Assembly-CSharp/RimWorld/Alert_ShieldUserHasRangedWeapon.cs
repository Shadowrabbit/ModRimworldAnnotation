using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001278 RID: 4728
	public class Alert_ShieldUserHasRangedWeapon : Alert
	{
		// Token: 0x170013B8 RID: 5048
		// (get) Token: 0x06007114 RID: 28948 RVA: 0x0025AD28 File Offset: 0x00258F28
		private List<Pawn> ShieldUsersWithRangedWeapon
		{
			get
			{
				this.shieldUsersWithRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (pawn.equipment.Primary != null && pawn.equipment.Primary.def.IsRangedWeapon)
					{
						List<Apparel> wornApparel = pawn.apparel.WornApparel;
						for (int i = 0; i < wornApparel.Count; i++)
						{
							if (wornApparel[i] is ShieldBelt)
							{
								this.shieldUsersWithRangedWeaponResult.Add(pawn);
								break;
							}
						}
					}
				}
				return this.shieldUsersWithRangedWeaponResult;
			}
		}

		// Token: 0x06007115 RID: 28949 RVA: 0x0025ADE4 File Offset: 0x00258FE4
		public Alert_ShieldUserHasRangedWeapon()
		{
			this.defaultLabel = "ShieldUserHasRangedWeapon".Translate();
			this.defaultExplanation = "ShieldUserHasRangedWeaponDesc".Translate();
		}

		// Token: 0x06007116 RID: 28950 RVA: 0x0025AE21 File Offset: 0x00259021
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ShieldUsersWithRangedWeapon);
		}

		// Token: 0x04003E36 RID: 15926
		private List<Pawn> shieldUsersWithRangedWeaponResult = new List<Pawn>();
	}
}
