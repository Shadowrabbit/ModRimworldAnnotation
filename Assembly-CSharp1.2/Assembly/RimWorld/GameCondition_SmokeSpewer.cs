using System;

namespace RimWorld
{
	// Token: 0x02001179 RID: 4473
	public class GameCondition_SmokeSpewer : GameCondition_VolcanicWinter
	{
		// Token: 0x17000F76 RID: 3958
		// (get) Token: 0x0600628C RID: 25228 RVA: 0x00043D77 File Offset: 0x00041F77
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}
	}
}
