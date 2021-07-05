using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02001658 RID: 5720
	public class QuestNode_AddShipJob_Unload : QuestNode_AddShipJob
	{
		// Token: 0x17001613 RID: 5651
		// (get) Token: 0x06008571 RID: 34161 RVA: 0x002FE6B3 File Offset: 0x002FC8B3
		protected override ShipJobDef DefaultShipJobDef
		{
			get
			{
				return ShipJobDefOf.Unload;
			}
		}

		// Token: 0x06008572 RID: 34162 RVA: 0x002FE6BC File Offset: 0x002FC8BC
		protected override void AddJobVars(ShipJob shipJob, Slate slate)
		{
			ShipJob_Unload shipJob_Unload;
			if ((shipJob_Unload = (shipJob as ShipJob_Unload)) != null)
			{
				shipJob_Unload.dropMode = (this.dropMode.GetValue(slate) ?? TransportShipDropMode.All);
			}
		}

		// Token: 0x04005353 RID: 21331
		public SlateRef<TransportShipDropMode?> dropMode;
	}
}
