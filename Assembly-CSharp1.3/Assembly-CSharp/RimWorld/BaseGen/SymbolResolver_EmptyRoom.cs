using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020015B8 RID: 5560
	public class SymbolResolver_EmptyRoom : SymbolResolver
	{
		// Token: 0x06008308 RID: 33544 RVA: 0x002EA034 File Offset: 0x002E8234
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.wallStuff ?? BaseGenUtility.RandomCheapWallStuff(rp.faction, false);
			TerrainDef floorDef = rp.floorDef ?? BaseGenUtility.CorrespondingTerrainDef(thingDef, false, rp.faction);
			if (rp.noRoof == null || !rp.noRoof.Value)
			{
				BaseGen.symbolStack.Push("roof", rp, null);
			}
			ResolveParams resolveParams = rp;
			resolveParams.wallStuff = thingDef;
			BaseGen.symbolStack.Push("edgeWalls", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = floorDef;
			BaseGen.symbolStack.Push("floor", resolveParams2, null);
			BaseGen.symbolStack.Push("clear", rp, null);
			if (rp.addRoomCenterToRootsToUnfog != null && rp.addRoomCenterToRootsToUnfog.Value && Current.ProgramState == ProgramState.MapInitializing)
			{
				MapGenerator.rootsToUnfog.Add(rp.rect.CenterCell);
			}
		}
	}
}
