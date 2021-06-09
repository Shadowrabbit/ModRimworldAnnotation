using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9B RID: 3483
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x17000C26 RID: 3110
		// (get) Token: 0x06004F63 RID: 20323 RVA: 0x00037D75 File Offset: 0x00035F75
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x17000C27 RID: 3111
		// (get) Token: 0x06004F64 RID: 20324 RVA: 0x00037D7C File Offset: 0x00035F7C
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x06004F65 RID: 20325 RVA: 0x00037D83 File Offset: 0x00035F83
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
