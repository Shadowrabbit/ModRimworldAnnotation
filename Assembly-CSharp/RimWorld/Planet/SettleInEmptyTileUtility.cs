using System;
using System.Text;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02002115 RID: 8469
	public static class SettleInEmptyTileUtility
	{
		// Token: 0x0600B3D5 RID: 46037 RVA: 0x003438B8 File Offset: 0x00341AB8
		public static void Settle(Caravan caravan)
		{
			Faction faction = caravan.Faction;
			if (faction != Faction.OfPlayer)
			{
				Log.Error("Cannot settle with non-player faction.", false);
				return;
			}
			Settlement newHome = SettleUtility.AddNewHome(caravan.Tile, faction);
			LongEventHandler.QueueLongEvent(delegate()
			{
				GetOrGenerateMapUtility.GetOrGenerateMap(caravan.Tile, Find.World.info.initialMapSize, null);
			}, "GeneratingMap", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
			LongEventHandler.QueueLongEvent(delegate()
			{
				Map map = newHome.Map;
				Thing t = caravan.PawnsListForReading[0];
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Center, CaravanDropInventoryMode.DropInstantly, false, (IntVec3 x) => x.GetRoom(map, RegionType.Set_Passable).CellCount >= 600);
				CameraJumper.TryJump(t);
			}, "SpawningColonists", true, new Action<Exception>(GameAndMapInitExceptionHandlers.ErrorWhileGeneratingMap), true);
		}

		// Token: 0x0600B3D6 RID: 46038 RVA: 0x00343954 File Offset: 0x00341B54
		public static Command SettleCommand(Caravan caravan)
		{
			Command_Settle command_Settle = new Command_Settle();
			command_Settle.defaultLabel = "CommandSettle".Translate();
			command_Settle.defaultDesc = "CommandSettleDesc".Translate();
			command_Settle.icon = SettleUtility.SettleCommandTex;
			Action <>9__1;
			command_Settle.action = delegate()
			{
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				int tile = caravan.Tile;
				Action settleAction;
				if ((settleAction = <>9__1) == null)
				{
					settleAction = (<>9__1 = delegate()
					{
						SettleInEmptyTileUtility.Settle(caravan);
					});
				}
				SettlementProximityGoodwillUtility.CheckConfirmSettle(tile, settleAction);
			};
			SettleInEmptyTileUtility.tmpSettleFailReason.Length = 0;
			if (!TileFinder.IsValidTileForNewSettlement(caravan.Tile, SettleInEmptyTileUtility.tmpSettleFailReason))
			{
				command_Settle.Disable(SettleInEmptyTileUtility.tmpSettleFailReason.ToString());
			}
			else if (SettleUtility.PlayerSettlementsCountLimitReached)
			{
				if (Prefs.MaxNumberOfPlayerSettlements > 1)
				{
					command_Settle.Disable("CommandSettleFailReachedMaximumNumberOfBases".Translate());
				}
				else
				{
					command_Settle.Disable("CommandSettleFailAlreadyHaveBase".Translate());
				}
			}
			return command_Settle;
		}

		// Token: 0x04007B9E RID: 31646
		private const int MinStartingLocCellsCount = 600;

		// Token: 0x04007B9F RID: 31647
		private static StringBuilder tmpSettleFailReason = new StringBuilder();
	}
}
