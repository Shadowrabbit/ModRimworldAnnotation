using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB5 RID: 3253
	public class GenStep_ChopTrees : GenStep
	{
		// Token: 0x17000D13 RID: 3347
		// (get) Token: 0x06004BD2 RID: 19410 RVA: 0x00193DD1 File Offset: 0x00191FD1
		public override int SeedPart
		{
			get
			{
				return 874575948;
			}
		}

		// Token: 0x06004BD3 RID: 19411 RVA: 0x00193DD8 File Offset: 0x00191FD8
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect cellRect;
			IntVec3 centerCell;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				centerCell = cellRect.CenterCell;
			}
			else
			{
				int num = 0;
				int num2 = 0;
				List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
				foreach (Thing thing in list)
				{
					num += thing.Position.x;
					num2 += thing.Position.z;
				}
				centerCell = new IntVec3(num / list.Count, 0, num2 / list.Count);
			}
			foreach (IntVec3 c in GenRadial.RadialCellsAround(centerCell, (float)this.radialRange, true))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = thingList.Count - 1; i >= 0; i--)
					{
						Plant plant = thingList[i] as Plant;
						if (plant != null && plant.def.plant.IsTree)
						{
							plant.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
		}

		// Token: 0x04002DE2 RID: 11746
		public int radialRange = 35;
	}
}
