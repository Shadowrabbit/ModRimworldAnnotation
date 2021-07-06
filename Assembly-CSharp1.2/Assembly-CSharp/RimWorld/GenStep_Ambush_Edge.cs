using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BF RID: 4799
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06006816 RID: 26646 RVA: 0x00046E59 File Offset: 0x00045059
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x06006817 RID: 26647 RVA: 0x00046E60 File Offset: 0x00045060
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
