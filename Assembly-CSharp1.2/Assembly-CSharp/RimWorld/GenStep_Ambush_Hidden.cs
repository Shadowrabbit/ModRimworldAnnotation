using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C0 RID: 4800
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06006819 RID: 26649 RVA: 0x00046E7A File Offset: 0x0004507A
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x0600681A RID: 26650 RVA: 0x00046E81 File Offset: 0x00045081
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x0600681B RID: 26651 RVA: 0x00201F2C File Offset: 0x0020012C
		protected override SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = base.MakeAmbushSignalAction(rectToDefend, root, parms);
			if (root.IsValid)
			{
				signalAction_Ambush.spawnNear = root;
			}
			else
			{
				signalAction_Ambush.spawnAround = rectToDefend;
			}
			return signalAction_Ambush;
		}
	}
}
