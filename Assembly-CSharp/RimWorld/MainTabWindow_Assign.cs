using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B39 RID: 6969
	public class MainTabWindow_Assign : MainTabWindow_PawnTable
	{
		// Token: 0x17001837 RID: 6199
		// (get) Token: 0x06009974 RID: 39284 RVA: 0x00066478 File Offset: 0x00064678
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Assign;
			}
		}

		// Token: 0x06009975 RID: 39285 RVA: 0x0006636A File Offset: 0x0006456A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}
	}
}
