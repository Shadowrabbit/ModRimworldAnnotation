﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C9F RID: 3231
	public class JobGiver_AITrashColonyClose : ThinkNode_JobGiver
	{
		// Token: 0x06004B3B RID: 19259 RVA: 0x001A49A8 File Offset: 0x001A2BA8
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.HostileTo(Faction.OfPlayer))
			{
				return null;
			}
			bool flag = pawn.natives.IgniteVerb != null && pawn.natives.IgniteVerb.IsStillUsableBy(pawn) && pawn.HostileTo(Faction.OfPlayer);
			CellRect cellRect = CellRect.CenteredOn(pawn.Position, 5);
			for (int i = 0; i < 35; i++)
			{
				IntVec3 randomCell = cellRect.RandomCell;
				if (randomCell.InBounds(pawn.Map))
				{
					Building edifice = randomCell.GetEdifice(pawn.Map);
					if (edifice != null && TrashUtility.ShouldTrashBuilding(pawn, edifice, false) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map, false, null, 0, 0))
					{
						if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
						{
							Find.CurrentMap.debugDrawer.FlashCell(randomCell, 1f, "trash bld", 50);
						}
						Job job = TrashUtility.TrashJob(pawn, edifice, false);
						if (job != null)
						{
							return job;
						}
					}
					if (flag)
					{
						Plant plant = randomCell.GetPlant(pawn.Map);
						if (plant != null && TrashUtility.ShouldTrashPlant(pawn, plant) && GenSight.LineOfSight(pawn.Position, randomCell, pawn.Map, false, null, 0, 0))
						{
							if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
							{
								Find.CurrentMap.debugDrawer.FlashCell(randomCell, 0.5f, "trash plant", 50);
							}
							Job job2 = TrashUtility.TrashJob(pawn, plant, false);
							if (job2 != null)
							{
								return job2;
							}
						}
					}
					if (DebugViewSettings.drawDestSearch && Find.CurrentMap == pawn.Map)
					{
						Find.CurrentMap.debugDrawer.FlashCell(randomCell, 0f, "trash no", 50);
					}
				}
			}
			return null;
		}

		// Token: 0x040031C5 RID: 12741
		private const int CloseSearchRadius = 5;
	}
}
