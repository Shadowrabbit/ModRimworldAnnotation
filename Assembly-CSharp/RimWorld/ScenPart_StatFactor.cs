using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001617 RID: 5655
	public class ScenPart_StatFactor : ScenPart
	{
		// Token: 0x06007AF5 RID: 31477 RVA: 0x00052ADA File Offset: 0x00050CDA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<StatDef>(ref this.stat, "stat");
			Scribe_Values.Look<float>(ref this.factor, "factor", 0f, false);
		}

		// Token: 0x06007AF6 RID: 31478 RVA: 0x0024FBB4 File Offset: 0x0024DDB4
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
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
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

		// Token: 0x06007AF7 RID: 31479 RVA: 0x00052B08 File Offset: 0x00050D08
		public override string Summary(Scenario scen)
		{
			return "ScenPart_StatFactor".Translate(this.stat.label, this.factor.ToStringPercent());
		}

		// Token: 0x06007AF8 RID: 31480 RVA: 0x0024FCF0 File Offset: 0x0024DEF0
		public override void Randomize()
		{
			this.stat = (from d in DefDatabase<StatDef>.AllDefs
			where d.scenarioRandomizable
			select d).RandomElement<StatDef>();
			this.factor = GenMath.RoundedHundredth(Rand.Range(0.1f, 3f));
		}

		// Token: 0x06007AF9 RID: 31481 RVA: 0x0024FD4C File Offset: 0x0024DF4C
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

		// Token: 0x06007AFA RID: 31482 RVA: 0x00052B39 File Offset: 0x00050D39
		public float GetStatFactor(StatDef stat)
		{
			if (stat == this.stat)
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x0400508D RID: 20621
		private StatDef stat;

		// Token: 0x0400508E RID: 20622
		private float factor;

		// Token: 0x0400508F RID: 20623
		private string factorBuf;
	}
}
