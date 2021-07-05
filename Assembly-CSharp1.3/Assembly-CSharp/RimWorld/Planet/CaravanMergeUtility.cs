using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020017A9 RID: 6057
	[StaticConstructorOnStartup]
	public static class CaravanMergeUtility
	{
		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06008C6B RID: 35947 RVA: 0x00326A5E File Offset: 0x00324C5E
		public static bool ShouldShowMergeCommand
		{
			get
			{
				return CaravanMergeUtility.CanMergeAnySelectedCaravans || CaravanMergeUtility.AnySelectedCaravanCloseToAnyOtherMergeableCaravan;
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06008C6C RID: 35948 RVA: 0x00326A70 File Offset: 0x00324C70
		public static bool CanMergeAnySelectedCaravans
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = i + 1; j < selectedObjects.Count; j++)
						{
							Caravan caravan2 = selectedObjects[j] as Caravan;
							if (caravan2 != null && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x06008C6D RID: 35949 RVA: 0x00326AEC File Offset: 0x00324CEC
		public static bool AnySelectedCaravanCloseToAnyOtherMergeableCaravan
		{
			get
			{
				List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < selectedObjects.Count; i++)
				{
					Caravan caravan = selectedObjects[i] as Caravan;
					if (caravan != null && caravan.IsPlayerControlled)
					{
						for (int j = 0; j < caravans.Count; j++)
						{
							Caravan caravan2 = caravans[j];
							if (caravan2 != caravan && caravan2.IsPlayerControlled && CaravanMergeUtility.CloseToEachOther(caravan, caravan2))
							{
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x06008C6E RID: 35950 RVA: 0x00326B74 File Offset: 0x00324D74
		public static Command MergeCommand(Caravan caravan)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandMergeCaravans".Translate();
			command_Action.defaultDesc = "CommandMergeCaravansDesc".Translate();
			command_Action.icon = CaravanMergeUtility.MergeCommandTex;
			command_Action.action = delegate()
			{
				CaravanMergeUtility.TryMergeSelectedCaravans();
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
			};
			if (!CaravanMergeUtility.CanMergeAnySelectedCaravans)
			{
				command_Action.Disable("CommandMergeCaravansFailCaravansNotSelected".Translate());
			}
			return command_Action;
		}

		// Token: 0x06008C6F RID: 35951 RVA: 0x00326C00 File Offset: 0x00324E00
		public static void TryMergeSelectedCaravans()
		{
			CaravanMergeUtility.tmpSelectedPlayerCaravans.Clear();
			List<WorldObject> selectedObjects = Find.WorldSelector.SelectedObjects;
			for (int i = 0; i < selectedObjects.Count; i++)
			{
				Caravan caravan = selectedObjects[i] as Caravan;
				if (caravan != null && caravan.IsPlayerControlled)
				{
					CaravanMergeUtility.tmpSelectedPlayerCaravans.Add(caravan);
				}
			}
			while (CaravanMergeUtility.tmpSelectedPlayerCaravans.Any<Caravan>())
			{
				Caravan caravan2 = CaravanMergeUtility.tmpSelectedPlayerCaravans[0];
				CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(0);
				CaravanMergeUtility.tmpCaravansOnSameTile.Clear();
				CaravanMergeUtility.tmpCaravansOnSameTile.Add(caravan2);
				for (int j = CaravanMergeUtility.tmpSelectedPlayerCaravans.Count - 1; j >= 0; j--)
				{
					if (CaravanMergeUtility.CloseToEachOther(CaravanMergeUtility.tmpSelectedPlayerCaravans[j], caravan2))
					{
						CaravanMergeUtility.tmpCaravansOnSameTile.Add(CaravanMergeUtility.tmpSelectedPlayerCaravans[j]);
						CaravanMergeUtility.tmpSelectedPlayerCaravans.RemoveAt(j);
					}
				}
				if (CaravanMergeUtility.tmpCaravansOnSameTile.Count >= 2)
				{
					CaravanMergeUtility.MergeCaravans(CaravanMergeUtility.tmpCaravansOnSameTile);
				}
			}
		}

		// Token: 0x06008C70 RID: 35952 RVA: 0x00326D00 File Offset: 0x00324F00
		private static bool CloseToEachOther(Caravan c1, Caravan c2)
		{
			if (c1.Tile == c2.Tile)
			{
				return true;
			}
			Vector3 drawPos = c1.DrawPos;
			Vector3 drawPos2 = c2.DrawPos;
			float num = Find.WorldGrid.averageTileSize * 0.5f;
			return (drawPos - drawPos2).sqrMagnitude < num * num;
		}

		// Token: 0x06008C71 RID: 35953 RVA: 0x00326D54 File Offset: 0x00324F54
		private static void MergeCaravans(List<Caravan> caravans)
		{
			Caravan caravan = caravans.MaxBy((Caravan x) => x.PawnsListForReading.Count);
			for (int i = 0; i < caravans.Count; i++)
			{
				Caravan caravan2 = caravans[i];
				if (caravan2 != caravan)
				{
					caravan2.pawns.TryTransferAllToContainer(caravan.pawns, true);
					caravan2.Destroy();
				}
			}
			caravan.Notify_Merged(caravans);
		}

		// Token: 0x0400591F RID: 22815
		private static readonly Texture2D MergeCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/MergeCaravans", true);

		// Token: 0x04005920 RID: 22816
		private static List<Caravan> tmpSelectedPlayerCaravans = new List<Caravan>();

		// Token: 0x04005921 RID: 22817
		private static List<Caravan> tmpCaravansOnSameTile = new List<Caravan>();
	}
}
