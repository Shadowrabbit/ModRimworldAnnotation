using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020002A5 RID: 677
	public abstract class SectionLayer_Things : SectionLayer
	{
		// Token: 0x06001166 RID: 4454 RVA: 0x00012B04 File Offset: 0x00010D04
		public SectionLayer_Things(Section section) : base(section)
		{
		}

		// Token: 0x06001167 RID: 4455 RVA: 0x00012B0D File Offset: 0x00010D0D
		public override void DrawLayer()
		{
			if (!DebugViewSettings.drawThingsPrinted)
			{
				return;
			}
			base.DrawLayer();
		}

		// Token: 0x06001168 RID: 4456 RVA: 0x000C2168 File Offset: 0x000C0368
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

		// Token: 0x06001169 RID: 4457
		protected abstract void TakePrintFrom(Thing t);

		// Token: 0x04000E1C RID: 3612
		protected bool requireAddToMapMesh;
	}
}
