using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001E0 RID: 480
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06000DAC RID: 3500 RVA: 0x0004D5B6 File Offset: 0x0004B7B6
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0004D5BF File Offset: 0x0004B7BF
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0004D5D0 File Offset: 0x0004B7D0
		public override void Regenerate()
		{
			base.ClearSubMeshes(MeshParts.All);
			foreach (IntVec3 intVec in this.section.CellRect)
			{
				List<Thing> list = base.Map.thingGrid.ThingsListAt(intVec);
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					Thing thing = list[i];
					if ((thing.def.seeThroughFog || !base.Map.fogGrid.fogGrid[CellIndicesUtility.CellToIndex(thing.Position, base.Map.Size.x)]) && thing.def.drawerType != DrawerType.None && (thing.def.drawerType != DrawerType.RealtimeOnly || !this.requireAddToMapMesh) && (thing.def.hideAtSnowDepth >= 1f || base.Map.snowGrid.GetDepth(thing.Position) <= thing.def.hideAtSnowDepth) && thing.Position.x == intVec.x && thing.Position.z == intVec.z)
					{
						this.TakePrintFrom(thing);
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
		}

		// Token: 0x06000DAF RID: 3503
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04000B33 RID: 2867
		protected bool requireAddToMapMesh;
	}
}
