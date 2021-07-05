using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013CB RID: 5067
	public class Instruction_ExpandAreaBuildRoof : Instruction_ExpandArea
	{
		// Token: 0x17001591 RID: 5521
		// (get) Token: 0x06007B35 RID: 31541 RVA: 0x002B7D9E File Offset: 0x002B5F9E
		protected override Area MyArea
		{
			get
			{
				return base.Map.areaManager.BuildRoof;
			}
		}
	}
}
