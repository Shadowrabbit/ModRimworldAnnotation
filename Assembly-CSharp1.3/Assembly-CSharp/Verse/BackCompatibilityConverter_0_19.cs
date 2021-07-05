using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000463 RID: 1123
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x0600221F RID: 8735 RVA: 0x000D790C File Offset: 0x000D5B0C
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x06002220 RID: 8736 RVA: 0x00002688 File Offset: 0x00000888
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x06002221 RID: 8737 RVA: 0x00002688 File Offset: 0x00000888
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x06002222 RID: 8738 RVA: 0x000D791C File Offset: 0x000D5B1C
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.foodRestrictionDatabase == null)
				{
					game.foodRestrictionDatabase = new FoodRestrictionDatabase();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.foodRestriction == null && pawn.RaceProps.Humanlike && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
			}
		}
	}
}
