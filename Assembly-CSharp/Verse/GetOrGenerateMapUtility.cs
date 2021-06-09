using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000258 RID: 600
	public static class GetOrGenerateMapUtility
	{
		// Token: 0x06000F3C RID: 3900 RVA: 0x000B5D8C File Offset: 0x000B3F8C
		public static Map GetOrGenerateMap(int tile, IntVec3 size, WorldObjectDef suggestedMapParentDef)
		{
			Map map = Current.Game.FindMap(tile);
			if (map == null)
			{
				MapParent mapParent = Find.WorldObjects.MapParentAt(tile);
				if (mapParent == null)
				{
					if (suggestedMapParentDef == null)
					{
						Log.Error("Tried to get or generate map at " + tile + ", but there isn't any MapParent world object here and map parent def argument is null.", false);
						return null;
					}
					mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(suggestedMapParentDef);
					mapParent.Tile = tile;
					Find.WorldObjects.Add(mapParent);
				}
				map = MapGenerator.GenerateMap(size, mapParent, mapParent.MapGeneratorDef, mapParent.ExtraGenStepDefs, null);
			}
			return map;
		}

		// Token: 0x06000F3D RID: 3901 RVA: 0x000116C6 File Offset: 0x0000F8C6
		public static Map GetOrGenerateMap(int tile, WorldObjectDef suggestedMapParentDef)
		{
			return GetOrGenerateMapUtility.GetOrGenerateMap(tile, Find.World.info.initialMapSize, suggestedMapParentDef);
		}
	}
}
