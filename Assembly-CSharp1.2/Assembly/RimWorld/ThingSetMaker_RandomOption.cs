using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001742 RID: 5954
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x0600834A RID: 33610 RVA: 0x0026EB50 File Offset: 0x0026CD50
		protected override bool CanGenerateSub(ThingSetMakerParams parms)
		{
			for (int i = 0; i < this.options.Count; i++)
			{
				if (this.options[i].thingSetMaker.CanGenerate(parms) && this.GetSelectionWeight(this.options[i], parms) > 0f)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600834B RID: 33611 RVA: 0x0026EBAC File Offset: 0x0026CDAC
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			ThingSetMaker_RandomOption.Option option;
			if (!(from x in this.options
			where x.thingSetMaker.CanGenerate(parms)
			select x).TryRandomElementByWeight((ThingSetMaker_RandomOption.Option x) => this.GetSelectionWeight(x, parms), out option))
			{
				return;
			}
			outThings.AddRange(option.thingSetMaker.Generate(parms));
		}

		// Token: 0x0600834C RID: 33612 RVA: 0x0026EC14 File Offset: 0x0026CE14
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option, ThingSetMakerParams parms)
		{
			if (option.weightIfPlayerHasNoItem != null && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem, 1))
			{
				return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}

		// Token: 0x0600834D RID: 33613 RVA: 0x0026EC68 File Offset: 0x0026CE68
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x0600834E RID: 33614 RVA: 0x00058298 File Offset: 0x00056498
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			int num2;
			for (int i = 0; i < this.options.Count; i = num2 + 1)
			{
				float num = this.options[i].weight;
				if (this.options[i].weightIfPlayerHasNoItem != null)
				{
					num = Mathf.Max(num, this.options[i].weightIfPlayerHasNoItem.Value);
				}
				if (num > 0f)
				{
					foreach (ThingDef thingDef in this.options[i].thingSetMaker.AllGeneratableThingsDebug(parms))
					{
						yield return thingDef;
					}
					IEnumerator<ThingDef> enumerator = null;
				}
				num2 = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x04005515 RID: 21781
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x02001743 RID: 5955
		public class Option
		{
			// Token: 0x04005516 RID: 21782
			public ThingSetMaker thingSetMaker;

			// Token: 0x04005517 RID: 21783
			public float weight;

			// Token: 0x04005518 RID: 21784
			public float? weightIfPlayerHasNoItem;

			// Token: 0x04005519 RID: 21785
			public ThingDef weightIfPlayerHasNoItemItem;
		}
	}
}
