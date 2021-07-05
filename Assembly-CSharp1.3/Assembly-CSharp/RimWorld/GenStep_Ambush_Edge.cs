using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB0 RID: 3248
	public class GenStep_Ambush_Edge : GenStep_Ambush
	{
		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06004BB8 RID: 19384 RVA: 0x00193903 File Offset: 0x00191B03
		public override int SeedPart
		{
			get
			{
				return 1412216193;
			}
		}

		// Token: 0x06004BB9 RID: 19385 RVA: 0x0019390A File Offset: 0x00191B0A
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.spawnPawnsOnEdge = true;
			return signalAction_Ambush;
		}
	}
}
