using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001600 RID: 5632
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x06007A7A RID: 31354 RVA: 0x0005254D File Offset: 0x0005074D
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x06007A7B RID: 31355
		protected abstract string IncidentTag { get; }

		// Token: 0x06007A7C RID: 31356 RVA: 0x0024EC3C File Offset: 0x0024CE3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.incident == null)
			{
				this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
				Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>(), false);
			}
		}

		// Token: 0x06007A7D RID: 31357 RVA: 0x0024EC9C File Offset: 0x0024CE9C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x06007A7E RID: 31358 RVA: 0x0024ECC0 File Offset: 0x0024CEC0
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x06007A7F RID: 31359 RVA: 0x00052555 File Offset: 0x00050755
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x06007A80 RID: 31360 RVA: 0x0005256C File Offset: 0x0005076C
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x06007A81 RID: 31361 RVA: 0x0024ECF8 File Offset: 0x0024CEF8
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x06007A82 RID: 31362 RVA: 0x0024ED20 File Offset: 0x0024CF20
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x06007A83 RID: 31363 RVA: 0x0005257F File Offset: 0x0005077F
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x06007A84 RID: 31364 RVA: 0x0024ED48 File Offset: 0x0024CF48
		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate()
				{
					this.incident = id;
				});
			}
		}

		// Token: 0x0400504D RID: 20557
		protected IncidentDef incident;
	}
}
