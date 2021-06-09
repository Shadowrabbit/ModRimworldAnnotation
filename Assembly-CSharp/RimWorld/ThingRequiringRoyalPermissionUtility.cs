using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DFD RID: 7677
	public static class ThingRequiringRoyalPermissionUtility
	{
		// Token: 0x0600A64B RID: 42571 RVA: 0x003036D0 File Offset: 0x003018D0
		public static bool IsViolatingRulesOf(Def implantOrWeapon, Pawn pawn, Faction faction, int implantLevel = 0)
		{
			if (faction.def.royalImplantRules == null || faction.def.royalImplantRules.Count == 0)
			{
				return false;
			}
			RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(implantOrWeapon, faction, implantLevel);
			if (minTitleToUse == null)
			{
				return false;
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(faction);
			if (currentTitle == null)
			{
				return true;
			}
			int num = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(currentTitle);
			if (num < 0)
			{
				return false;
			}
			int num2 = faction.def.RoyalTitlesAwardableInSeniorityOrderForReading.IndexOf(minTitleToUse);
			return num < num2;
		}

		// Token: 0x0600A64C RID: 42572 RVA: 0x0030374C File Offset: 0x0030194C
		public static bool IsViolatingRulesOfAnyFaction(Def implantOrWeapon, Pawn pawn, int implantLevel = 0)
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(implantOrWeapon, pawn, faction, implantLevel))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A64D RID: 42573 RVA: 0x0006E020 File Offset: 0x0006C220
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static bool IsViolatingRulesOfAnyFaction_NewTemp(Def implantOrWeapon, Pawn pawn, int implantLevel = 0, bool ignoreSilencer = false)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction(implantOrWeapon, pawn, implantLevel);
		}

		// Token: 0x0600A64E RID: 42574 RVA: 0x003037A8 File Offset: 0x003019A8
		public static RoyalTitleDef GetMinTitleToUse(Def implantOrWeapon, Faction faction, int implantLevel = 0)
		{
			HediffDef implantDef;
			if ((implantDef = (implantOrWeapon as HediffDef)) != null)
			{
				return faction.GetMinTitleForImplant(implantDef, implantLevel);
			}
			return null;
		}

		// Token: 0x0600A64F RID: 42575 RVA: 0x0005E676 File Offset: 0x0005C876
		[Obsolete("Will be removed in the future")]
		public static TaggedString GetEquipWeaponConfirmationDialogText(Thing weapon, Pawn pawn)
		{
			return null;
		}
	}
}
