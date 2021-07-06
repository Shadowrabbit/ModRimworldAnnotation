using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200015B RID: 347
	public class PawnInventoryOption
	{
		// Token: 0x060008D1 RID: 2257 RVA: 0x0000CF61 File Offset: 0x0000B161
		public IEnumerable<Thing> GenerateThings()
		{
			if (Rand.Value < this.skipChance)
			{
				yield break;
			}
			if (this.thingDef != null && this.countRange.max > 0)
			{
				Thing thing = ThingMaker.MakeThing(this.thingDef, null);
				thing.stackCount = this.countRange.RandomInRange;
				yield return thing;
			}
			if (this.subOptionsTakeAll != null)
			{
				foreach (PawnInventoryOption pawnInventoryOption in this.subOptionsTakeAll)
				{
					foreach (Thing thing2 in pawnInventoryOption.GenerateThings())
					{
						yield return thing2;
					}
					IEnumerator<Thing> enumerator2 = null;
				}
				List<PawnInventoryOption>.Enumerator enumerator = default(List<PawnInventoryOption>.Enumerator);
			}
			if (this.subOptionsChooseOne != null)
			{
				PawnInventoryOption pawnInventoryOption2 = this.subOptionsChooseOne.RandomElementByWeight((PawnInventoryOption o) => o.choiceChance);
				foreach (Thing thing3 in pawnInventoryOption2.GenerateThings())
				{
					yield return thing3;
				}
				IEnumerator<Thing> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04000769 RID: 1897
		public ThingDef thingDef;

		// Token: 0x0400076A RID: 1898
		public IntRange countRange = IntRange.one;

		// Token: 0x0400076B RID: 1899
		public float choiceChance = 1f;

		// Token: 0x0400076C RID: 1900
		public float skipChance;

		// Token: 0x0400076D RID: 1901
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x0400076E RID: 1902
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
