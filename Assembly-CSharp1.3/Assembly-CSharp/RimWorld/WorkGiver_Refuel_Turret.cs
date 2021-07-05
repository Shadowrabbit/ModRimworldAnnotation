using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000859 RID: 2137
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x06003874 RID: 14452 RVA: 0x0013D497 File Offset: 0x0013B697
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x17000A0F RID: 2575
		// (get) Token: 0x06003875 RID: 14453 RVA: 0x0013D49E File Offset: 0x0013B69E
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x0013D4A5 File Offset: 0x0013B6A5
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
