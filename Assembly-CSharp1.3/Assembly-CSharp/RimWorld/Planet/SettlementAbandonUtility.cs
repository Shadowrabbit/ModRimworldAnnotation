using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020017CF RID: 6095
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x06008DCB RID: 36299 RVA: 0x0032F0B4 File Offset: 0x0032D2B4
		public static Command AbandonCommand(MapParent settlement)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandAbandonHome".Translate();
			command_Action.defaultDesc = "CommandAbandonHomeDesc".Translate();
			command_Action.icon = SettlementAbandonUtility.AbandonCommandTex;
			command_Action.action = delegate()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(settlement);
			};
			command_Action.order = 30f;
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		// Token: 0x06008DCC RID: 36300 RVA: 0x0032F14C File Offset: 0x0032D34C
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x06008DCD RID: 36301 RVA: 0x0032F188 File Offset: 0x0032D388
		public static void TryAbandonViaInterface(MapParent settlement)
		{
			Map map = settlement.Map;
			if (map == null)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> source = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
			if (source.Count<Pawn>() != 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (Pawn pawn in from x in source
				orderby x.IsColonist descending
				select x)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.AppendLine();
					}
					stringBuilder2.Append("    " + pawn.LabelCap);
				}
				stringBuilder.Append("ConfirmAbandonHomeWithColonyPawns".Translate(stringBuilder2));
			}
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(map.mapPawns.AllPawns, PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, null, "\n\n" + "ConfirmAbandonHomeNegativeThoughts_Everyone".Translate(), "ConfirmAbandonHomeNegativeThoughts");
			if (stringBuilder.Length == 0)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate
			{
				SettlementAbandonUtility.Abandon(settlement);
			}, false, null));
		}

		// Token: 0x06008DCE RID: 36302 RVA: 0x0032F310 File Offset: 0x0032D510
		private static void Abandon(MapParent settlement)
		{
			settlement.Destroy();
			Settlement settlement2 = settlement as Settlement;
			if (settlement2 != null)
			{
				SettlementAbandonUtility.AddAbandonedSettlement(settlement2);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x06008DCF RID: 36303 RVA: 0x0032F340 File Offset: 0x0032D540
		private static void AddAbandonedSettlement(Settlement factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}

		// Token: 0x040059A3 RID: 22947
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);
	}
}
