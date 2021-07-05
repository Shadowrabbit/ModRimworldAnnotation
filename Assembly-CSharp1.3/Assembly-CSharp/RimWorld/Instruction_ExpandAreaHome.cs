using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CA RID: 5066
	public class Instruction_ExpandAreaHome : Instruction_ExpandArea
	{
		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x06007B33 RID: 31539 RVA: 0x002B7D84 File Offset: 0x002B5F84
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.Home;
			}
		}
	}
}
