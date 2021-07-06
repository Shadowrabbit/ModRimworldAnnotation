using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015D4 RID: 5588
	public class ScenPart_GameCondition : ScenPart
	{
		// Token: 0x170012C3 RID: 4803
		// (get) Token: 0x06007972 RID: 31090 RVA: 0x00051B67 File Offset: 0x0004FD67
		public override string Label
		{
			get
			{
				return this.def.gameCondition.LabelCap;
			}
		}

		// Token: 0x06007973 RID: 31091 RVA: 0x00051B7E File Offset: 0x0004FD7E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.durationDays, "durationDayS", 0f, false);
		}

		// Token: 0x06007974 RID: 31092 RVA: 0x0024D02C File Offset: 0x0024B22C
		public override string Summary(Scenario scen)
		{
			return this.def.gameCondition.LabelCap + ": " + this.def.gameCondition.description + " (" + ((int)(this.durationDays * 60000f)).ToStringTicksToDays("F1") + ")";
		}

		// Token: 0x06007975 RID: 31093 RVA: 0x00051B9C File Offset: 0x0004FD9C
		public override void Randomize()
		{
			this.durationDays = Mathf.Round(this.def.durationRandomRange.RandomInRange);
		}

		// Token: 0x06007976 RID: 31094 RVA: 0x00051BB9 File Offset: 0x0004FDB9
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Widgets.TextFieldNumericLabeled<float>(listing.GetScenPartRect(this, ScenPart.RowHeight), "durationDays".Translate(), ref this.durationDays, ref this.durationDaysBuf, 0f, 1E+09f);
		}

		// Token: 0x06007977 RID: 31095 RVA: 0x00051BF1 File Offset: 0x0004FDF1
		public override void GenerateIntoMap(Map map)
		{
			if (!this.def.gameConditionTargetsWorld)
			{
				map.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06007978 RID: 31096 RVA: 0x00051C11 File Offset: 0x0004FE11
		public override void PostWorldGenerate()
		{
			if (this.def.gameConditionTargetsWorld)
			{
				Find.World.gameConditionManager.RegisterCondition(this.MakeCondition());
			}
		}

		// Token: 0x06007979 RID: 31097 RVA: 0x00051C35 File Offset: 0x0004FE35
		private GameCondition MakeCondition()
		{
			return GameConditionMaker.MakeCondition(this.def.gameCondition, (int)(this.durationDays * 60000f));
		}

		// Token: 0x0600797A RID: 31098 RVA: 0x0024D0A0 File Offset: 0x0024B2A0
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_GameCondition scenPart_GameCondition = other as ScenPart_GameCondition;
			return scenPart_GameCondition == null || scenPart_GameCondition.def.gameCondition.CanCoexistWith(this.def.gameCondition);
		}

		// Token: 0x04004FDE RID: 20446
		private float durationDays;

		// Token: 0x04004FDF RID: 20447
		private string durationDaysBuf;
	}
}
