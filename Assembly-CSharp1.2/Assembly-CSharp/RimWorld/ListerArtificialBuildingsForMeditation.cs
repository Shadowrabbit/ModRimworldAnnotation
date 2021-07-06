using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001267 RID: 4711
	public class ListerArtificialBuildingsForMeditation
	{
		// Token: 0x060066B4 RID: 26292 RVA: 0x000462F4 File Offset: 0x000444F4
		public ListerArtificialBuildingsForMeditation(Map map)
		{
			this.map = map;
		}

		// Token: 0x060066B5 RID: 26293 RVA: 0x0004630E File Offset: 0x0004450E
		public void Notify_BuildingSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x060066B6 RID: 26294 RVA: 0x0004630E File Offset: 0x0004450E
		public void Notify_BuildingDeSpawned(Building b)
		{
			if (MeditationUtility.CountsAsArtificialBuilding(b))
			{
				this.artificialBuildingsPerCell.Clear();
			}
		}

		// Token: 0x060066B7 RID: 26295 RVA: 0x001F9EB8 File Offset: 0x001F80B8
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

		// Token: 0x0400445C RID: 17500
		private Map map;

		// Token: 0x0400445D RID: 17501
		private Dictionary<CellWithRadius, List<Thing>> artificialBuildingsPerCell = new Dictionary<CellWithRadius, List<Thing>>();
	}
}
