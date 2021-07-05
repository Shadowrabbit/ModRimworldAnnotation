using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200125F RID: 4703
	public class Alert_HunterLacksRangedWeapon : Alert
	{
		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x060070B1 RID: 28849 RVA: 0x00258C78 File Offset: 0x00256E78
		private List<Pawn> HuntersWithoutRangedWeapon
		{
			get
			{
				this.huntersWithoutRangedWeaponResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonists)
				{
					if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Hunting) && !WorkGiver_HunterHunt.HasHuntingWeapon(pawn) && !pawn.Downed && (!HealthAIUtility.ShouldSeekMedicalRest(pawn) || !pawn.InBed()))
					{
						this.huntersWithoutRangedWeaponResult.Add(pawn);
					}
				}
				return this.huntersWithoutRangedWeaponResult;
			}
		}

		// Token: 0x060070B2 RID: 28850 RVA: 0x00258D24 File Offset: 0x00256F24
		public Alert_HunterLacksRangedWeapon()
		{
			this.defaultLabel = "HunterLacksWeapon".Translate();
			this.defaultExplanation = "HunterLacksWeaponDesc".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x060070B3 RID: 28851 RVA: 0x00258D73 File Offset: 0x00256F73
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HuntersWithoutRangedWeapon);
		}

		// Token: 0x04003E1F RID: 15903
		private List<Pawn> huntersWithoutRangedWeaponResult = new List<Pawn>();
	}
}
