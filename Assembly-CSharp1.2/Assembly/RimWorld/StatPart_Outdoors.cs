using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D43 RID: 7491
	public class StatPart_Outdoors : StatPart
	{
		// Token: 0x0600A2C3 RID: 41667 RVA: 0x0006C1C1 File Offset: 0x0006A3C1
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.OutdoorsFactor(req);
		}

		// Token: 0x0600A2C4 RID: 41668 RVA: 0x002F5C74 File Offset: 0x002F3E74
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

		// Token: 0x0600A2C5 RID: 41669 RVA: 0x0006C1CF File Offset: 0x0006A3CF
		private float OutdoorsFactor(StatRequest req)
		{
			if (this.ConsideredOutdoors(req))
			{
				return this.factorOutdoors;
			}
			return this.factorIndoors;
		}

		// Token: 0x0600A2C6 RID: 41670 RVA: 0x002F5CE0 File Offset: 0x002F3EE0
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

		// Token: 0x04006E85 RID: 28293
		private float factorIndoors = 1f;

		// Token: 0x04006E86 RID: 28294
		private float factorOutdoors = 1f;
	}
}
