using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001581 RID: 5505
	public class SketchResolver_AddCornerThings : SketchResolver
	{
		// Token: 0x06008225 RID: 33317 RVA: 0x002E0518 File Offset: 0x002DE718
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

		// Token: 0x06008226 RID: 33318 RVA: 0x000126F5 File Offset: 0x000108F5
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x0400510D RID: 20749
		private HashSet<IntVec3> wallPositions = new HashSet<IntVec3>();

		// Token: 0x0400510E RID: 20750
		private const float Chance = 0.09f;
	}
}
