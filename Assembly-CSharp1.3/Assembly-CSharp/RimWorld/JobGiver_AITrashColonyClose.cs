using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000783 RID: 1923
	public class JobGiver_AITrashColonyClose : ThinkNode_JobGiver
	{
		// Token: 0x060034E1 RID: 13537 RVA: 0x0012B6A8 File Offset: 0x001298A8
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
						Job job = TrashUtility.TrashJob(pawn, edifice, false, false);
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
							Job job2 = TrashUtility.TrashJob(pawn, plant, false, false);
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

		// Token: 0x04001E70 RID: 7792
		private const int CloseSearchRadius = 5;
	}
}
