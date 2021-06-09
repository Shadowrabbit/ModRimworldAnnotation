using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001672 RID: 5746
	public class Graphic_LinkedTransmitter : Graphic_Linked
	{
		// Token: 0x06007D54 RID: 32084 RVA: 0x0001B6B7 File Offset: 0x000198B7
		public Graphic_LinkedTransmitter(Graphic subGraphic) : base(subGraphic)
		{
		}

		// Token: 0x06007D55 RID: 32085 RVA: 0x000543BE File Offset: 0x000525BE
		public override bool ShouldLinkWith(IntVec3 c, Thing parent)
		{
			return c.InBounds(parent.Map) && (base.ShouldLinkWith(c, parent) || parent.Map.powerNetGrid.TransmittedPowerNetAt(c) != null);
		}

		// Token: 0x06007D56 RID: 32086 RVA: 0x00256B18 File Offset: 0x00254D18
		public override void Print(SectionLayer layer, Thing thing)
		{
			base.Print(layer, thing);
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = thing.Position + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(thing.Map))
				{
					Building transmitter = intVec.GetTransmitter(thing.Map);
					if (transmitter != null && !transmitter.def.graphicData.Linked)
					{
						Material mat = base.LinkedDrawMatFrom(thing, intVec);
						Printer_Plane.PrintPlane(layer, intVec.ToVector3ShiftedWithAltitude(thing.def.Altitude), Vector2.one, mat, 0f, false, null, null, 0.01f, 0f);
					}
				}
			}
		}
	}
}
