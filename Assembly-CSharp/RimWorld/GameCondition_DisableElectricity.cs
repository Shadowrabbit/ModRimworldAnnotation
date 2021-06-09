using System;

namespace RimWorld
{
	// Token: 0x02001173 RID: 4467
	public class GameCondition_DisableElectricity : GameCondition
	{
		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x06006266 RID: 25190 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
