using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CB1 RID: 3249
	public class GenStep_Ambush_Hidden : GenStep_Ambush
	{
		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004BBB RID: 19387 RVA: 0x00193924 File Offset: 0x00191B24
		public override int SeedPart
		{
			get
			{
				return 921085483;
			}
		}

		// Token: 0x06004BBC RID: 19388 RVA: 0x0019392B File Offset: 0x00191B2B
		protected override RectTrigger MakeRectTrigger()
		{
			RectTrigger rectTrigger = base.MakeRectTrigger();
			rectTrigger.activateOnExplosion = true;
			return rectTrigger;
		}

		// Token: 0x06004BBD RID: 19389 RVA: 0x0019393C File Offset: 0x00191B3C
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
