using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001673 RID: 5747
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x06007D57 RID: 32087 RVA: 0x000543F0 File Offset: 0x000525F0
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x06007D58 RID: 32088 RVA: 0x0001B6B7 File Offset: 0x000198B7
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06007D59 RID: 32089 RVA: 0x000543F8 File Offset: 0x000525F8
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x06007D5A RID: 32090 RVA: 0x00256BC0 File Offset: 0x00254DC0
		public override void Print(SectionLayer layer, Thing parent)
		{
			foreach (IntVec3 cell in parent.OccupiedRect())
			{
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), base.LinkedDrawMatFrom(parent, cell), 0f, false, null, null, 0.01f, 0f);
			}
		}
	}
}
