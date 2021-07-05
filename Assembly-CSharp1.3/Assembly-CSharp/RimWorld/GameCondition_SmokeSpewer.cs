using System;

namespace RimWorld
{
	// Token: 0x02000BE2 RID: 3042
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x060047A0 RID: 18336 RVA: 0x0017AEF3 File Offset: 0x001790F3
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
