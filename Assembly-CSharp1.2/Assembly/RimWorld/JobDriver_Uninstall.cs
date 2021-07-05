using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B6F RID: 2927
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x00032C3B File Offset: 0x00030E3B
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060044D9 RID: 17625 RVA: 0x0019102C File Offset: 0x0018F22C
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x00032C42 File Offset: 0x00030E42
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
