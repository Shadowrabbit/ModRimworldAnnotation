using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001382 RID: 4994
	public class PawnColumnWorker_Faction : PawnColumnWorker_Icon
	{
		// Token: 0x0600797D RID: 31101 RVA: 0x002AFBF8 File Offset: 0x002ADDF8
		protected override Texture2D GetIconFor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.def.FactionIcon;
			}
			return null;
		}

		// Token: 0x0600797E RID: 31102 RVA: 0x002AFC24 File Offset: 0x002ADE24
		protected override Color GetIconColor(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null && homeFaction != Faction.OfPlayer)
			{
				return homeFaction.Color;
			}
			return Color.white;
		}

		// Token: 0x0600797F RID: 31103 RVA: 0x002AFC50 File Offset: 0x002ADE50
		protected override string GetIconTip(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			string text = (homeFaction != null) ? homeFaction.Name : null;
			if (!text.NullOrEmpty())
			{
				return "PawnFactionInfo".Translate(text, pawn);
			}
			return null;
		}

		// Token: 0x06007980 RID: 31104 RVA: 0x002AFC98 File Offset: 0x002ADE98
		protected override void ClickedIcon(Pawn pawn)
		{
			Faction homeFaction = pawn.HomeFaction;
			if (homeFaction != null)
			{
				Find.MainTabsRoot.SetCurrentTab(MainButtonDefOf.Factions, true);
				((MainTabWindow_Factions)Find.MainTabsRoot.OpenTab.TabWindow).ScrollToFaction(homeFaction);
			}
		}
	}
}
