using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020000E8 RID: 232
	public class PawnInventoryOption
	{
		// Token: 0x06000655 RID: 1621 RVA: 0x0001F58B File Offset: 0x0001D78B
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

		// Token: 0x04000578 RID: 1400
		public ThingDef thingDef;

		// Token: 0x04000579 RID: 1401
		public IntRange countRange = IntRange.one;

		// Token: 0x0400057A RID: 1402
		public float choiceChance = 1f;

		// Token: 0x0400057B RID: 1403
		public float skipChance;

		// Token: 0x0400057C RID: 1404
		public List<PawnInventoryOption> subOptionsTakeAll;

		// Token: 0x0400057D RID: 1405
		public List<PawnInventoryOption> subOptionsChooseOne;
	}
}
