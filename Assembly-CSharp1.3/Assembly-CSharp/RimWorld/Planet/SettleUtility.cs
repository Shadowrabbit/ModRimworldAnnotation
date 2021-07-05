using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017BF RID: 6079
	[StaticConstructorOnStartup]
	public static class SettleUtility
	{
		// Token: 0x170016F6 RID: 5878
		// (get) Token: 0x06008D07 RID: 36103 RVA: 0x0032C3BC File Offset: 0x0032A5BC
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

		// Token: 0x06008D08 RID: 36104 RVA: 0x0032C414 File Offset: 0x0032A614
		public static Settlement AddNewHome(int tile, Faction faction)
		{
			Settlement settlement = (Settlement)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Settlement);
			settlement.Tile = tile;
			settlement.SetFaction(faction);
			settlement.Name = SettlementNameGenerator.GenerateSettlementName(settlement, null);
			Find.WorldObjects.Add(settlement);
			return settlement;
		}

		// Token: 0x04005973 RID: 22899
		public static readonly Texture2D SettleCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/Settle", true);
	}
}
