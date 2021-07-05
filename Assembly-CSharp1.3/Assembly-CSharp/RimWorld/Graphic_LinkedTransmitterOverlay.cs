using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001048 RID: 4168
	public class Graphic_LinkedTransmitterOverlay : Graphic_Linked
	{
		// Token: 0x0600628F RID: 25231 RVA: 0x002169E4 File Offset: 0x00214BE4
		public Graphic_LinkedTransmitterOverlay()
		{
		}

		// Token: 0x06006290 RID: 25232 RVA: 0x0008FE34 File Offset: 0x0008E034
		public Graphic_LinkedTransmitterOverlay(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06006291 RID: 25233 RVA: 0x002169EC File Offset: 0x00214BEC
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null;
		}

		// Token: 0x06006292 RID: 25234 RVA: 0x00216A14 File Offset: 0x00214C14
		public override void Print(SectionLayer layer, Thing parent, float extraRotation)
		{
			foreach (IntVec3 cell in parent.OccupiedRect())
			{
				Vector3 center = cell.ToVector3ShiftedWithAltitude(AltitudeLayer.MapDataOverlay);
				Printer_Plane.PrintPlane(layer, center, new Vector2(1f, 1f), this.LinkedDrawMatFrom(parent, cell), extraRotation, false, null, null, 0.01f, 0f);
			}
		}
	}
}
