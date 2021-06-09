using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020007B6 RID: 1974
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x060031D5 RID: 12757 RVA: 0x00027231 File Offset: 0x00025431
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x060031D6 RID: 12758 RVA: 0x0000C32E File Offset: 0x0000A52E
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x060031D7 RID: 12759 RVA: 0x0000C32E File Offset: 0x0000A52E
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x060031D8 RID: 12760 RVA: 0x0014A710 File Offset: 0x00148910
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
