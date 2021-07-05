using System;

namespace RimWorld
{
	// Token: 0x02000DCA RID: 3530
	public static class PawnPostureUtility
	{
		// Token: 0x060051AD RID: 20909 RVA: 0x001B8CA2 File Offset: 0x001B6EA2
		public static bool Laying(this PawnPosture posture)
		{
			return posture == PawnPosture.LayingOnGroundFaceUp || posture == PawnPosture.LayingOnGroundNormal || posture == PawnPosture.LayingInBed;
		}
	}
}
