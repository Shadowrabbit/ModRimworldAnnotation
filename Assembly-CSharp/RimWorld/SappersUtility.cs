using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DEE RID: 3566
	public static class SappersUtility
	{
		// Token: 0x06005140 RID: 20800 RVA: 0x00038ECA File Offset: 0x000370CA
		public static bool IsGoodSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.HasBuildingDestroyerWeapon(p) && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06005141 RID: 20801 RVA: 0x00038EE9 File Offset: 0x000370E9
		public static bool IsGoodBackupSapper(Pawn p)
		{
			return p.kindDef.canBeSapper && SappersUtility.CanMineReasonablyFast(p);
		}

		// Token: 0x06005142 RID: 20802 RVA: 0x001BAD7C File Offset: 0x001B8F7C
		private static bool CanMineReasonablyFast(Pawn p)
		{
			return p.RaceProps.Humanlike && !p.skills.GetSkill(SkillDefOf.Mining).TotallyDisabled && !StatDefOf.MiningSpeed.Worker.IsDisabledFor(p) && p.skills.GetSkill(SkillDefOf.Mining).Level >= 4;
		}

		// Token: 0x06005143 RID: 20803 RVA: 0x001BADDC File Offset: 0x001B8FDC
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
