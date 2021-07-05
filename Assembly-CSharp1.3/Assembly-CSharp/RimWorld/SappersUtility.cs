using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A9 RID: 2217
	public static class SappersUtility
	{
		// Token: 0x06003AAC RID: 15020 RVA: 0x0014848E File Offset: 0x0014668E
		public static bool IsGoodSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.HasBuildingDestroyerWeapon(p) && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06003AAD RID: 15021 RVA: 0x001484AD File Offset: 0x001466AD
		public static bool IsGoodBackupSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06003AAE RID: 15022 RVA: 0x001484C4 File Offset: 0x001466C4
		private static bool CanMineReasonablyFast(Pawn p)
		{
			return p.RaceProps.Humanlike && !p.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled && !StatDefOf.MiningSpeed.Worker.IsDisabledFor(p) && p.skills.GetSkill(SkillDefOf.Mining).Level >= 4;
		}

		// Token: 0x06003AAF RID: 15023 RVA: 0x00148524 File Offset: 0x00146724
		public static bool HasBuildingDestroyerWeapon(Pawn p)
		{
			if (p.equipment == null || p.equipment.Primary == null)
			{
				return false;
			}
			List<Verb> allVerbs = p.equipment.Primary.GetComp<CompEquippable>().AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].verbProps.ai_IsBuildingDestroyer)
				{
					return true;
				}
			}
			return false;
		}
	}
}
