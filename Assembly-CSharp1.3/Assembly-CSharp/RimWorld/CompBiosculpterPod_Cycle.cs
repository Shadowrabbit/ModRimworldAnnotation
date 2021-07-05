using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200110A RID: 4362
	public abstract class CompBiosculpterPod_Cycle : ThingComp
	{
		// Token: 0x170011F1 RID: 4593
		// (get) Token: 0x060068D7 RID: 26839 RVA: 0x00236508 File Offset: 0x00234708
		public CompProperties_BiosculpterPod_BaseCycle Props
		{
			get
			{
				return (CompProperties_BiosculpterPod_BaseCycle)this.props;
			}
		}

		// Token: 0x060068D8 RID: 26840
		public abstract void CycleCompleted(Pawn occupant);
	}
}
