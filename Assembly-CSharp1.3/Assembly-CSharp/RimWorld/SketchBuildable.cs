using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF7 RID: 3319
	public abstract class SketchBuildable : SketchEntity
	{
		// Token: 0x17000D4A RID: 3402
		// (get) Token: 0x06004D65 RID: 19813
		public abstract BuildableDef Buildable { get; }

		// Token: 0x17000D4B RID: 3403
		// (get) Token: 0x06004D66 RID: 19814
		public abstract ThingDef Stuff { get; }

		// Token: 0x17000D4C RID: 3404
		// (get) Token: 0x06004D67 RID: 19815 RVA: 0x0019F26D File Offset: 0x0019D46D
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		// Token: 0x17000D4D RID: 3405
		// (get) Token: 0x06004D68 RID: 19816 RVA: 0x0019F281 File Offset: 0x0019D481
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		// Token: 0x06004D69 RID: 19817
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06004D6A RID: 19818 RVA: 0x0019F28C File Offset: 0x0019D48C
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}

		// Token: 0x06004D6B RID: 19819 RVA: 0x0019F2B0 File Offset: 0x0019D4B0
		public Thing FirstPermanentBlockerAt(IntVec3 at, Map map)
		{
			foreach (IntVec3 c in GenAdj.OccupiedRect(at, Rot4.North, this.Buildable.Size))
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (!thingList[i].def.destroyable && !GenConstruct.CanPlaceBlueprintOver(this.Buildable, thingList[i].def))
						{
							return thingList[i];
						}
					}
				}
			}
			return null;
		}
	}
}
