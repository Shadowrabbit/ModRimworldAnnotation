using System;

namespace RimWorld
{
	// Token: 0x02001447 RID: 5191
	public static class PawnPostureUtility
	{
		// Token: 0x06007004 RID: 28676 RVA: 0x0004B898 File Offset: 0x00049A98
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
