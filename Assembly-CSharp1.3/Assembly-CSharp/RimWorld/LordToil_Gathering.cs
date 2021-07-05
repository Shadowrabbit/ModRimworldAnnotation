using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008CF RID: 2255
	public abstract class LordToil_Gathering : LordToil
	{
		// Token: 0x17000AAD RID: 2733
		// (get) Token: 0x06003B4F RID: 15183 RVA: 0x0013F411 File Offset: 0x0013D611
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x06003B50 RID: 15184 RVA: 0x0014B72F File Offset: 0x0014992F
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x0014B750 File Offset: 0x00149950
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x0014B764 File Offset: 0x00149964
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		// Token: 0x06003B53 RID: 15187 RVA: 0x0014B7C8 File Offset: 0x001499C8
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
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

		// Token: 0x04002053 RID: 8275
		protected IntVec3 spot;

		// Token: 0x04002054 RID: 8276
		protected GatheringDef gatheringDef;
	}
}
