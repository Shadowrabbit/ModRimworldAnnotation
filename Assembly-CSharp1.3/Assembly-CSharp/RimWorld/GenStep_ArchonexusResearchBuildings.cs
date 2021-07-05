using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C89 RID: 3209
	public abstract class GenStep_ArchonexusResearchBuildings : GenStep_ScattererBestFit
	{
		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x06004ADC RID: 19164 RVA: 0x0018BB52 File Offset: 0x00189D52
		public override int SeedPart
		{
			get
			{
				return 746395733;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x06004ADD RID: 19165 RVA: 0x0018BB59 File Offset: 0x00189D59
		protected override IntVec2 Size
		{
			get
			{
				return new IntVec2(20, 20);
			}
		}

		// Token: 0x06004ADE RID: 19166 RVA: 0x0018BB64 File Offset: 0x00189D64
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.nearMapCenter = false;
			base.Generate(map, parms);
		}

		// Token: 0x06004ADF RID: 19167 RVA: 0x0018BB7C File Offset: 0x00189D7C
		public override bool CollisionAt(IntVec3 cell, Map map)
		{
			TerrainDef terrain = cell.GetTerrain(map);
			if (terrain != null && (terrain.IsWater || terrain.IsRoad))
			{
				return true;
			}
			List<Thing> thingList = cell.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.IsBuildingArtificial || (thingList[i].def.building != null && thingList[i].def.building.isNaturalRock))
				{
					return true;
				}
			}
			return false;
		}
	}
}
