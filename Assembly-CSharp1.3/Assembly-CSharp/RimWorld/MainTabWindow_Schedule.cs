using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001371 RID: 4977
	public class MainTabWindow_Schedule : MainTabWindow_PawnTable
	{
		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x0600791B RID: 31003 RVA: 0x002AEB60 File Offset: 0x002ACD60
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Restrict;
			}
		}

		// Token: 0x0600791C RID: 31004 RVA: 0x002AEB67 File Offset: 0x002ACD67
		public override void DoWindowContents(Rect fillRect)
		{
			base.DoWindowContents(fillRect);
			TimeAssignmentSelector.DrawTimeAssignmentSelectorGrid(new Rect(0f, 0f, 191f, 65f));
		}

		// Token: 0x04004371 RID: 17265
		private const int TimeAssignmentSelectorWidth = 191;

		// Token: 0x04004372 RID: 17266
		private const int TimeAssignmentSelectorHeight = 65;
	}
}
