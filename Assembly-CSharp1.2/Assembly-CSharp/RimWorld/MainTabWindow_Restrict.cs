using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5B RID: 7003
	public class MainTabWindow_Restrict : MainTabWindow_PawnTable
	{
		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06009A5A RID: 39514 RVA: 0x00066C6F File Offset: 0x00064E6F
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x06009A5B RID: 39515 RVA: 0x0006636A File Offset: 0x0006456A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06009A5C RID: 39516 RVA: 0x00066C76 File Offset: 0x00064E76
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		// Token: 0x040062B6 RID: 25270
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x040062B7 RID: 25271
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
