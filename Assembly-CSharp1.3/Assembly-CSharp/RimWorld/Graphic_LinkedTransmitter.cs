using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001047 RID: 4167
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x0600628C RID: 25228 RVA: 0x0008FE34 File Offset: 0x0008E034
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x0600628D RID: 25229 RVA: 0x0021690E File Offset: 0x00214B0E
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x00216940 File Offset: 0x00214B40
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			base.Print(layer, thing, extraRotation);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(thing.Map))
				{
					Building transmitter = intVec.GetTransmitter(thing.Map);
					if (transmitter != null && !transmitter.def.graphicData.Linked)
					{
						Material mat = this.LinkedDrawMatFrom(thing, intVec);
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, extraRotation, false, null, null, 0.01f, 0f);
					}
				}
			}
		}
	}
}
