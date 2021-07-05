using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFA RID: 4090
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06006045 RID: 24645 RVA: 0x0020D03B File Offset: 0x0020B23B
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x06006046 RID: 24646 RVA: 0x0020D052 File Offset: 0x0020B252
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06006047 RID: 24647 RVA: 0x0020D070 File Offset: 0x0020B270
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		// Token: 0x06006048 RID: 24648 RVA: 0x0020D0E1 File Offset: 0x0020B2E1
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x06006049 RID: 24649 RVA: 0x0020D0FE File Offset: 0x0020B2FE
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x0600604A RID: 24650 RVA: 0x0020D136 File Offset: 0x0020B336
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600604B RID: 24651 RVA: 0x0020D156 File Offset: 0x0020B356
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x0600604C RID: 24652 RVA: 0x0020D17A File Offset: 0x0020B37A
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		// Token: 0x0600604D RID: 24653 RVA: 0x0020D19C File Offset: 0x0020B39C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x04003728 RID: 14120
		private float durationDays;

		// Token: 0x04003729 RID: 14121
		private string durationDaysBuf;
	}
}
