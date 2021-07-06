using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1A RID: 3610
	public class LordToil_Party : LordToil_Gathering
	{
		// Token: 0x17000CA8 RID: 3240
		// (get) Token: 0x060051FA RID: 20986 RVA: 0x00039507 File Offset: 0x00037707
		private LordToilData_Party Data
		{
			get
			{
				return (LordToilData_Party)this.data;
			}
		}

		// Token: 0x060051FB RID: 20987 RVA: 0x00039514 File Offset: 0x00037714
		public LordToil_Party(IntVec3 spot, GatheringDef gatheringDef, float joyPerTick = 3.5E-05f) : base(spot, gatheringDef)
		{
			this.joyPerTick = joyPerTick;
			this.data = new LordToilData_Party();
		}

		// Token: 0x060051FC RID: 20988 RVA: 0x001BD2B4 File Offset: 0x001BB4B4
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					ownedPawns[i].needs.joy.GainJoy(this.joyPerTick, JoyKindDefOf.Social);
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x0400347B RID: 13435
		private float joyPerTick = 3.5E-05f;

		// Token: 0x0400347C RID: 13436
		public const float DefaultJoyPerTick = 3.5E-05f;
	}
}
