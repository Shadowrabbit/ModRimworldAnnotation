using System;
using System.Collections.Generic;
using RimWorld.BaseGen;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB2 RID: 3250
	public class GenStep_AncientAltar : GenStep_ScattererBestFit
	{
		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004BBF RID: 19391 RVA: 0x0019396D File Offset: 0x00191B6D
		public override int SeedPart
		{
			get
			{
				return 572334943;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06004BC0 RID: 19392 RVA: 0x00193974 File Offset: 0x00191B74
		protected override IntVec2 Size
		{
			get
			{
				return SymbolResolver_AncientAltar.Size;
			}
		}

		// Token: 0x06004BC1 RID: 19393 RVA: 0x0019397B File Offset: 0x00191B7B
		public override void Generate(Map map, GenStepParams parms)
		{
			this.count = 1;
			this.warnOnFail = false;
			base.Generate(map, parms);
		}

		// Token: 0x06004BC2 RID: 19394 RVA: 0x00193994 File Offset: 0x00191B94
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

		// Token: 0x06004BC3 RID: 19395 RVA: 0x00193A18 File Offset: 0x00191C18
		protected override bool TryFindScatterCell(Map map, out IntVec3 result)
		{
			if (!base.TryFindScatterCell(map, out result))
			{
				result = map.Center;
			}
			return true;
		}

		// Token: 0x06004BC4 RID: 19396 RVA: 0x00193A34 File Offset: 0x00191C34
		protected override void ScatterAt(IntVec3 c, Map map, GenStepParams parms, int stackCount = 1)
		{
			SitePartParams parms2 = parms.sitePart.parms;
			ResolveParams resolveParams = default(ResolveParams);
			resolveParams.triggerSecuritySignal = parms2.triggerSecuritySignal;
			resolveParams.threatPoints = new float?(parms2.threatPoints);
			resolveParams.relicThing = parms2.relicThing;
			resolveParams.interiorThreatPoints = ((parms2.interiorThreatPoints > 0f) ? new float?(parms2.interiorThreatPoints) : null);
			resolveParams.exteriorThreatPoints = ((parms2.exteriorThreatPoints > 0f) ? new float?(parms2.exteriorThreatPoints) : null);
			resolveParams.rect = CellRect.CenteredOn(c, this.Size.x, this.Size.z);
			BaseGen.globalSettings.map = map;
			BaseGen.symbolStack.Push("ancientAltar", resolveParams, null);
			BaseGen.Generate();
			parms.sitePart.relicWasSpawned = true;
		}
	}
}
