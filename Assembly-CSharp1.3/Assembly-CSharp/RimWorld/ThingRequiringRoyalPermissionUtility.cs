using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001574 RID: 5492
	public static class ThingRequiringRoyalPermissionUtility
	{
		// Token: 0x060081E3 RID: 33251 RVA: 0x002DE7E4 File Offset: 0x002DC9E4
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

		// Token: 0x060081E4 RID: 33252 RVA: 0x002DE860 File Offset: 0x002DCA60
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

		// Token: 0x060081E5 RID: 33253 RVA: 0x002DE8BC File Offset: 0x002DCABC
		public static RoyalTitleDef GetMinTitleToUse(Def implantOrWeapon, Faction faction, int implantLevel = 0)
		{
			HediffDef implantDef;
			if ((implantDef = (implantOrWeapon as HediffDef)) != null)
			{
				return faction.GetMinTitleForImplant(implantDef, implantLevel);
			}
			return null;
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x002514C1 File Offset: 0x0024F6C1
		[Obsolete("Will be removed in the future")]
		public static TaggedString GetEquipWeaponConfirmationDialogText(Thing weapon, Pawn pawn)
		{
			return null;
		}
	}
}
