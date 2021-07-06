using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020015F3 RID: 5619
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x06007A17 RID: 31255 RVA: 0x0005229D File Offset: 0x0005049D
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
