using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FFB RID: 4091
	public class ScenPart_PermaGameCondition : ScenPart
	{
		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x0600604F RID: 24655 RVA: 0x0020D1DC File Offset: 0x0020B3DC
		public override string Label
		{
			get
			{
				return "Permanent".Translate().CapitalizeFirst() + ": " + this.gameCondition.label.CapitalizeFirst();
			}
		}

		// Token: 0x06006050 RID: 24656 RVA: 0x0020D220 File Offset: 0x0020B420
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.gameCondition.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<GameConditionDef>(this.AllowedGameConditions(), (GameConditionDef d) => d.LabelCap, (GameConditionDef d) => delegate()
				{
					this.gameCondition = d;
				});
			}
		}

		// Token: 0x06006051 RID: 24657 RVA: 0x0020D289 File Offset: 0x0020B489
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<GameConditionDef>(ref this.gameCondition, "gameCondition");
		}

		// Token: 0x06006052 RID: 24658 RVA: 0x0020D2A1 File Offset: 0x0020B4A1
		public override void Randomize()
		{
			this.gameCondition = this.AllowedGameConditions().RandomElement<GameConditionDef>();
		}

		// Token: 0x06006053 RID: 24659 RVA: 0x0020D2B4 File Offset: 0x0020B4B4
		private IEnumerable<GameConditionDef> AllowedGameConditions()
		{
			return from d in DefDatabase<GameConditionDef>.AllDefs
			where d.canBePermanent
			select d;
		}

		// Token: 0x06006054 RID: 24660 RVA: 0x0020D2DF File Offset: 0x0020B4DF
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PermaGameCondition", "ScenPart_PermaGameCondition".Translate());
		}

		// Token: 0x06006055 RID: 24661 RVA: 0x0020D2FB File Offset: 0x0020B4FB
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PermaGameCondition")
			{
				yield return this.gameCondition.LabelCap + ": " + this.gameCondition.description.CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x06006056 RID: 24662 RVA: 0x0020D314 File Offset: 0x0020B514
		public override void GenerateIntoMap(Map map)
		{
			GameCondition cond = GameConditionMaker.MakeConditionPermanent(this.gameCondition);
			map.gameConditionManager.RegisterCondition(cond);
		}

		// Token: 0x06006057 RID: 24663 RVA: 0x0020D33C File Offset: 0x0020B53C
		public override bool CanCoexistWith(ScenPart other)
		{
			if (this.gameCondition == null)
			{
				return true;
			}
			ScenPart_PermaGameCondition scenPart_PermaGameCondition = other as ScenPart_PermaGameCondition;
			return scenPart_PermaGameCondition == null || this.gameCondition.CanCoexistWith(scenPart_PermaGameCondition.gameCondition);
		}

		// Token: 0x0400372A RID: 14122
		private GameConditionDef gameCondition;

		// Token: 0x0400372B RID: 14123
		public const string PermaGameConditionTag = "PermaGameCondition";
	}
}
