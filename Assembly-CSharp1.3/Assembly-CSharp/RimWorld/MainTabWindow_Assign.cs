using System;

namespace RimWorld
{
	// Token: 0x02001367 RID: 4967
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x17001534 RID: 5428
		// (get) Token: 0x0600787D RID: 30845 RVA: 0x002A7963 File Offset: 0x002A5B63
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}
	}
}
