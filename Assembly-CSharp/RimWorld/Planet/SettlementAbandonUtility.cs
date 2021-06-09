using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x0200214D RID: 8525
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x0600B5A1 RID: 46497 RVA: 0x00348D2C File Offset: 0x00346F2C
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

		// Token: 0x0600B5A2 RID: 46498 RVA: 0x00348DC4 File Offset: 0x00346FC4
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x0600B5A3 RID: 46499 RVA: 0x00348E00 File Offset: 0x00347000
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

		// Token: 0x0600B5A4 RID: 46500 RVA: 0x00348F88 File Offset: 0x00347188
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

		// Token: 0x0600B5A5 RID: 46501 RVA: 0x00348FB8 File Offset: 0x003471B8
		private static void AddAbandonedSettlement(Settlement factionBase)
		{
			WorldObject worldObject = WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.AbandonedSettlement);
			worldObject.Tile = factionBase.Tile;
			worldObject.SetFaction(factionBase.Faction);
			Find.WorldObjects.Add(worldObject);
		}

		// Token: 0x04007C80 RID: 31872
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);
	}
}
