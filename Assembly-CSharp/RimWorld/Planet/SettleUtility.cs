using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200211C RID: 8476
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x17001A7C RID: 6780
		// (get) Token: 0x0600B3E9 RID: 46057 RVA: 0x00343D3C File Offset: 0x00341F3C
		public static bool PlayerSettlementsCountLimitReached
		{
			get
			{
				int num = 0;
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome && maps[i].Parent is Settlement)
					{
						num++;
					}
				}
				return num >= Prefs.MaxNumberOfPlayerSettlements;
			}
		}

		// Token: 0x0600B3EA RID: 46058 RVA: 0x00343D94 File Offset: 0x00341F94
		public static Settlement AddNewHome(int tile, Faction faction)
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.Tile = tile;
			settlement.SetFaction(faction);
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
			Find.WorldObjects.Add(settlement);
			return settlement;
		}

		// Token: 0x04007BAA RID: 31658
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);
	}
}
