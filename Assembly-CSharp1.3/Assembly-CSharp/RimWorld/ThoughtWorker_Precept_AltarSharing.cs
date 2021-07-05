using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000963 RID: 2403
	public class ThoughtWorker_Precept_AltarSharing : ThoughtWorker_Precept
	{
		// Token: 0x06003D2E RID: 15662 RVA: 0x00151644 File Offset: 0x0014F844
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			return this.SharedAltar(p) != null;
		}

		// Token: 0x06003D2F RID: 15663 RVA: 0x00151655 File Offset: 0x0014F855
		public override string PostProcessDescription(Pawn p, string description)
		{
			return description.Formatted(this.SharedAltar(p).Named("ALTAR"));
		}

		// Token: 0x06003D30 RID: 15664 RVA: 0x00151674 File Offset: 0x0014F874
		private Thing SharedAltar(Pawn pawn)
		{
			if (!pawn.Spawned || pawn.Ideo == null)
			{
				return null;
			}
			foreach (Thing thing in pawn.Map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial)))
			{
				CompStyleable compStyleable = thing.TryGetComp<CompStyleable>();
				if (compStyleable != null && compStyleable.SourcePrecept != null && compStyleable.SourcePrecept.ideo == pawn.Ideo)
				{
					Room room = thing.GetRoom(RegionType.Set_All);
					if (room != null && !room.TouchesMapEdge)
					{
						foreach (Thing thing2 in room.ContainedAndAdjacentThings)
						{
							if (thing2 != thing)
							{
								CompStyleable compStyleable2 = thing2.TryGetComp<CompStyleable>();
								if (compStyleable2 != null && compStyleable2.SourcePrecept != null && compStyleable2.SourcePrecept.ideo != pawn.Ideo)
								{
									return thing;
								}
							}
						}
					}
				}
			}
			return null;
		}
	}
}
