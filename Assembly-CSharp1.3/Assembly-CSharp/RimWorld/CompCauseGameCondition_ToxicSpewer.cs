using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F5 RID: 4341
	public class CompCauseGameCondition_ToxicSpewer : CompCauseGameCondition
	{
		// Token: 0x060067F7 RID: 26615 RVA: 0x002329E8 File Offset: 0x00230BE8
		public override void CompTick()
		{
			base.CompTick();
			if (!base.Active)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				List<Caravan> caravans = Find.WorldObjects.Caravans;
				for (int i = 0; i < caravans.Count; i++)
				{
					if (Find.WorldGrid.ApproxDistanceInTiles(caravans[i].Tile, base.MyTile) < (float)base.Props.worldRange)
					{
						List<Pawn> pawnsListForReading = caravans[i].PawnsListForReading;
						for (int j = 0; j < pawnsListForReading.Count; j++)
						{
							GameCondition_ToxicFallout.DoPawnToxicDamage(pawnsListForReading[j]);
						}
					}
				}
			}
		}
	}
}
