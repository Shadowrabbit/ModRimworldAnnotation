using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000378 RID: 888
	public interface IThingHolderWithDrawnPawn : IThingHolder
	{
		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001996 RID: 6550
		float HeldPawnDrawPos_Y { get; }

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001997 RID: 6551
		float HeldPawnBodyAngle { get; }

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001998 RID: 6552
		PawnPosture HeldPawnPosture { get; }
	}
}
