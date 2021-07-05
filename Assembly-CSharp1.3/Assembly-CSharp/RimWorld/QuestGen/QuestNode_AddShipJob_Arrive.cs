using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02001656 RID: 5718
	public class QuestNode_AddShipJob_Arrive : QuestNode_AddShipJob
	{
		// Token: 0x17001612 RID: 5650
		// (get) Token: 0x0600856C RID: 34156 RVA: 0x002FE5D6 File Offset: 0x002FC7D6
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Arrive;
			}
		}

		// Token: 0x0600856D RID: 34157 RVA: 0x002FE5E0 File Offset: 0x002FC7E0
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Arrive shipJob_Arrive;
			if ((shipJob_Arrive = (shipJob as ShipJob_Arrive)) != null)
			{
				Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
				shipJob_Arrive.mapParent = map.Parent;
				if (this.landingCell.GetValue(slate) != null)
				{
					shipJob_Arrive.cell = this.landingCell.GetValue(slate).Value;
				}
			}
		}

		// Token: 0x0400534E RID: 21326
		public SlateRef<Map> map;

		// Token: 0x0400534F RID: 21327
		public SlateRef<IntVec3?> landingCell;
	}
}
