using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FA8 RID: 4008
	public class RitualTargetFilterDef : Def
	{
		// Token: 0x06005EAB RID: 24235 RVA: 0x00206E37 File Offset: 0x00205037
		public RitualTargetFilter GetInstance()
		{
			return (RitualTargetFilter)Activator.CreateInstance(this.workerClass, new object[]
			{
				this
			});
		}

		// Token: 0x04003694 RID: 13972
		public Type workerClass;

		// Token: 0x04003695 RID: 13973
		public bool fallBackToGatherSpot;

		// Token: 0x04003696 RID: 13974
		public bool fallbackToRitualSpot;
	}
}
