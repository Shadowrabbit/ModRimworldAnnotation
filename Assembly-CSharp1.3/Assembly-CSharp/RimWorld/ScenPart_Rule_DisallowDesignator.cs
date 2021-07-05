using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001006 RID: 4102
	public class ScenPart_Rule_DisallowDesignator : ScenPart_Rule
	{
		// Token: 0x0600609F RID: 24735 RVA: 0x0020E648 File Offset: 0x0020C848
		protected override void ApplyRule()
		{
			Current.Game.Rules.SetAllowDesignator(this.def.designatorType, false);
		}
	}
}
