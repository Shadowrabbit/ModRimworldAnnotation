using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6E RID: 7022
	public class PawnColumnWorker_Faction : PawnColumnWorker_Icon
	{
		// Token: 0x06009ABC RID: 39612 RVA: 0x002D7E0C File Offset: 0x002D600C
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Faction factionOrExtraMiniOrHomeFaction = pawn.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null && factionOrExtraMiniOrHomeFaction != Faction.OfPlayer)
			{
				return factionOrExtraMiniOrHomeFaction.def.FactionIcon;
			}
			return null;
		}

		// Token: 0x06009ABD RID: 39613 RVA: 0x002D7E38 File Offset: 0x002D6038
		protected override Color GetIconColor(Pawn pawn)
		{
			Faction factionOrExtraMiniOrHomeFaction = pawn.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null && factionOrExtraMiniOrHomeFaction != Faction.OfPlayer)
			{
				return factionOrExtraMiniOrHomeFaction.Color;
			}
			return Color.white;
		}

		// Token: 0x06009ABE RID: 39614 RVA: 0x002D7E64 File Offset: 0x002D6064
		protected override string GetIconTip(Pawn pawn)
		{
			Faction factionOrExtraMiniOrHomeFaction = pawn.FactionOrExtraMiniOrHomeFaction;
			string text = (factionOrExtraMiniOrHomeFaction != null) ? factionOrExtraMiniOrHomeFaction.Name : null;
			if (!text.NullOrEmpty())
			{
				return "PawnFactionInfo".Translate(text, pawn);
			}
			return null;
		}

		// Token: 0x06009ABF RID: 39615 RVA: 0x002D7EAC File Offset: 0x002D60AC
		protected override void ClickedIcon(Pawn pawn)
		{
			Faction factionOrExtraMiniOrHomeFaction = pawn.FactionOrExtraMiniOrHomeFaction;
			if (factionOrExtraMiniOrHomeFaction != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
				((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(factionOrExtraMiniOrHomeFaction);
			}
		}
	}
}
