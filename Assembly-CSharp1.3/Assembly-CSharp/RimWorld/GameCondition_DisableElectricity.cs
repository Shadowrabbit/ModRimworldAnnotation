using System;

namespace RimWorld
{
	// Token: 0x02000BDD RID: 3037
	public class GameCondition_DisableElectricity : GameCondition
	{
		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06004782 RID: 18306 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ElectricityDisabled
		{
			get
			{
				return true;
			}
		}
	}
}
