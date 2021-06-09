using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E09 RID: 7689
	public class SketchResolver_AddCornerThings : SketchResolver
	{
		// Token: 0x0600A68A RID: 42634 RVA: 0x00304C9C File Offset: 0x00302E9C
		protected override void ResolveInt(ResolveParams parms)
		{
			this.wallPositions.Clear();
			for (int i = 0; i < parms.sketch.Things.Count; i++)
			{
				if (parms.sketch.Things[i].def == ThingDefOf.Wall)
				{
					this.wallPositions.Add(parms.sketch.Things[i].pos);
				}
			}
			bool allowWood = parms.allowWood ?? true;
			ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(parms.cornerThing, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, true, parms.cornerThing));
			bool flag = parms.requireFloor ?? false;
			try
			{
				foreach (IntVec3 intVec in this.wallPositions)
				{
					if (Rand.Chance(0.09f))
					{
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z - 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z - 1)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z - 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z - 1), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x, 0, intVec.z + 1)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x, 0, intVec.z + 1)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x, 0, intVec.z + 1), Rot4.North, stuff, 1, null, null, false);
						}
						if (this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z + 1)) && !this.wallPositions.Contains(new IntVec3(intVec.x + 1, 0, intVec.z)) && (!flag || (parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)) != null && parms.sketch.TerrainAt(new IntVec3(intVec.x + 1, 0, intVec.z)).layerable)))
						{
							parms.sketch.AddThing(parms.cornerThing, new IntVec3(intVec.x + 1, 0, intVec.z), Rot4.North, stuff, 1, null, null, false);
						}
					}
				}
			}
			finally
			{
				this.wallPositions.Clear();
			}
		}

		// Token: 0x0600A68B RID: 42635 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x040070ED RID: 28909
		private HashSet<IntVec3> wallPositions = new HashSet<IntVec3>();

		// Token: 0x040070EE RID: 28910
		private const float Chance = 0.09f;
	}
}
