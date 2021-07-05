using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7C RID: 3196
	public class ListerArtificialBuildingsForMeditation
	{
		// Token: 0x06004A81 RID: 19073 RVA: 0x0018A32A File Offset: 0x0018852A
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A82 RID: 19074 RVA: 0x0018A344 File Offset: 0x00188544
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06004A83 RID: 19075 RVA: 0x0018A344 File Offset: 0x00188544
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x06004A84 RID: 19076 RVA: 0x0018A35C File Offset: 0x0018855C
		public List<Thing> GetForCell(IntVec3 cell, float radius)
		{
			CellWithRadius key = new CellWithRadius(cell, radius);
			List<Thing> list;
			if (!this.artificialBuildingsPerCell.TryGetValue(key, out list))
			{
				list = new List<Thing>();
				foreach (Thing thing in GenRadial.RadialDistinctThingsAround(cell, this.map, radius, false))
				{
					if (MeditationUtility.CountsAsArtificialBuilding(thing))
					{
						list.Add(thing);
					}
				}
				this.artificialBuildingsPerCell[key] = list;
			}
			return list;
		}

		// Token: 0x04002D43 RID: 11587
		private Map map;

		// Token: 0x04002D44 RID: 11588
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
