using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000340 RID: 832
	public class RadialTrigger : PawnTrigger
	{
		// Token: 0x060017BA RID: 6074 RVA: 0x0008DC64 File Offset: 0x0008BE64
		public override void Tick()
		{
			if (this.IsHashIntervalTick(60))
			{
				Map map = base.Map;
				int num = GenRadial.NumCellsInRadius((float)this.maxRadius);
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = base.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(map))
					{
						List<Thing> thingList = c.GetThingList(map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (base.TriggeredBy(thingList[j]) && (!this.lineOfSight || GenSight.LineOfSightToThing(base.Position, thingList[j], map, false, null)))
							{
								base.ActivatedBy((Pawn)thingList[j]);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x0008DD2A File Offset: 0x0008BF2A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.maxRadius, "maxRadius", 0, false);
			Scribe_Values.Look<bool>(ref this.lineOfSight, "lineOfSight", false, false);
		}

		// Token: 0x04001055 RID: 4181
		public int maxRadius;

		// Token: 0x04001056 RID: 4182
		public bool lineOfSight;
	}
}
