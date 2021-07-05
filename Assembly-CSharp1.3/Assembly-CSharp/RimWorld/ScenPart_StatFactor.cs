using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001013 RID: 4115
	public class ScenPart_StatFactor : ScenPart
	{
		// Token: 0x0600610A RID: 24842 RVA: 0x0020FAD4 File Offset: 0x0020DCD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<StatDef>(ref this.stat, "stat");
			Scribe_Values.Look<float>(ref this.factor, "factor", 0f, false);
		}

		// Token: 0x0600610B RID: 24843 RVA: 0x0020FB04 File Offset: 0x0020DD04
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 2f);
			if (Widgets.ButtonText(scenPartRect.TopHalf(), this.stat.LabelCap, true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (StatDef statDef in DefDatabase<StatDef>.AllDefs)
				{
					if (!statDef.forInformationOnly && statDef.CanShowWithLoadedMods())
					{
						StatDef localSd = statDef;
						list.Add(new FloatMenuOption(localSd.LabelForFullStatListCap, delegate()
						{
							this.stat = localSd;
						}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
					}
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			Rect rect = scenPartRect.BottomHalf();
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, "multiplier".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.TextFieldPercent(rect3, ref this.factor, ref this.factorBuf, 0f, 100f);
		}

		// Token: 0x0600610C RID: 24844 RVA: 0x0020FC44 File Offset: 0x0020DE44
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StatFactor".Translate(this.stat.label, this.factor.ToStringPercent());
		}

		// Token: 0x0600610D RID: 24845 RVA: 0x0020FC78 File Offset: 0x0020DE78
		public override void Randomize()
		{
			this.stat = (from d in DefDatabase<StatDef>.AllDefs
			where d.scenarioRandomizable
			select d).RandomElement<StatDef>();
			this.factor = GenMath.RoundedHundredth(Rand.Range(0.1f, 3f));
		}

		// Token: 0x0600610E RID: 24846 RVA: 0x0020FCD4 File Offset: 0x0020DED4
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_StatFactor scenPart_StatFactor = other as ScenPart_StatFactor;
			if (scenPart_StatFactor != null && scenPart_StatFactor.stat == this.stat)
			{
				this.factor *= scenPart_StatFactor.factor;
				return true;
			}
			return false;
		}

		// Token: 0x0600610F RID: 24847 RVA: 0x0020FD0F File Offset: 0x0020DF0F
		public float GetStatFactor(StatDef stat)
		{
			if (stat == this.stat)
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x0400375E RID: 14174
		private StatDef stat;

		// Token: 0x0400375F RID: 14175
		private float factor;

		// Token: 0x04003760 RID: 14176
		private string factorBuf;
	}
}
