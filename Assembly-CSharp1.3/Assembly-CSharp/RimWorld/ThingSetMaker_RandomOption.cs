using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DC RID: 4316
	public class ThingSetMaker_RandomOption : ThingSetMaker
	{
		// Token: 0x0600673E RID: 26430 RVA: 0x0022E108 File Offset: 0x0022C308
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

		// Token: 0x0600673F RID: 26431 RVA: 0x0022E164 File Offset: 0x0022C364
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

		// Token: 0x06006740 RID: 26432 RVA: 0x0022E1CC File Offset: 0x0022C3CC
		private float GetSelectionWeight(ThingSetMaker_RandomOption.Option option, ThingSetMakerParams parms)
		{
			if (option.weightIfPlayerHasNoItem != null && !PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(option.weightIfPlayerHasNoItemItem, 1))
			{
				return option.weightIfPlayerHasNoItem.Value * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
			}
			return option.weight * option.thingSetMaker.ExtraSelectionWeightFactor(parms);
		}

		// Token: 0x06006741 RID: 26433 RVA: 0x0022E220 File Offset: 0x0022C420
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.options.Count; i++)
			{
				this.options[i].thingSetMaker.ResolveReferences();
			}
		}

		// Token: 0x06006742 RID: 26434 RVA: 0x0022E25F File Offset: 0x0022C45F
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

		// Token: 0x04003A4A RID: 14922
		public List<ThingSetMaker_RandomOption.Option> options;

		// Token: 0x020024F2 RID: 9458
		public class Option
		{
			// Token: 0x04008CAD RID: 36013
			public ThingSetMaker thingSetMaker;

			// Token: 0x04008CAE RID: 36014
			public float weight;

			// Token: 0x04008CAF RID: 36015
			public float? weightIfPlayerHasNoItem;

			// Token: 0x04008CB0 RID: 36016
			public ThingDef weightIfPlayerHasNoItemItem;
		}
	}
}
