using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006E2 RID: 1762
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06003124 RID: 12580 RVA: 0x0011F2C2 File Offset: 0x0011D4C2
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06003125 RID: 12581 RVA: 0x0011F2CC File Offset: 0x0011D4CC
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		// Token: 0x06003126 RID: 12582 RVA: 0x0011F2F6 File Offset: 0x0011D4F6
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
