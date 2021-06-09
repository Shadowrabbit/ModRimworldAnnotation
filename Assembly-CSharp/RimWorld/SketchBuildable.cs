using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200132C RID: 4908
	public abstract class SketchBuildable : SketchEntity
	{
		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06006A4D RID: 27213
		public abstract BuildableDef Buildable { get; }

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06006A4E RID: 27214
		public abstract ThingDef Stuff { get; }

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06006A4F RID: 27215 RVA: 0x000484D9 File Offset: 0x000466D9
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.Buildable, this.Stuff, 1);
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06006A50 RID: 27216 RVA: 0x000484ED File Offset: 0x000466ED
		public override bool LostImportantReferences
		{
			get
			{
				return this.Buildable == null;
			}
		}

		// Token: 0x06006A51 RID: 27217
		public abstract Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x06006A52 RID: 27218 RVA: 0x000484F8 File Offset: 0x000466F8
		public override bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map)
		{
			return at.InBounds(map) && (this.IsSameSpawned(at, map) || this.GetSpawnedBlueprintOrFrame(at, map) != null);
		}

		// Token: 0x06006A53 RID: 27219 RVA: 0x0020D1B8 File Offset: 0x0020B3B8
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
