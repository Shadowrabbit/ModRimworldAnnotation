using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DA RID: 5338
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x06007F43 RID: 32579 RVA: 0x002CFDEC File Offset: 0x002CDFEC
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x06007F44 RID: 32580 RVA: 0x002CFDFC File Offset: 0x002CDFFC
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && req.Thing.GetRoom(RegionType.Set_All) != null)
			{
				string str;
				if (this.ConsideredOutdoors(req))
				{
					str = "Outdoors".Translate();
				}
				else
				{
					str = "Indoors".Translate();
				}
				return str + ": x" + this.OutdoorsFactor(req).ToStringPercent();
			}
			return null;
		}

		// Token: 0x06007F45 RID: 32581 RVA: 0x002CFE66 File Offset: 0x002CE066
		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		// Token: 0x06007F46 RID: 32582 RVA: 0x002CFE80 File Offset: 0x002CE080
		private bool ConsideredOutdoors(StatRequest req)
		{
			if (req.HasThing)
			{
				Room room = req.Thing.GetRoom(RegionType.Set_All);
				if (room != null)
				{
					return room.OutdoorsForWork || (req.HasThing && req.Thing.Spawned && !req.Thing.Map.roofGrid.Roofed(req.Thing.Position));
				}
			}
			return false;
		}

		// Token: 0x04004F74 RID: 20340
		private float factorIndoors = 1f;

		// Token: 0x04004F75 RID: 20341
		private float factorOutdoors = 1f;
	}
}
