using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200100B RID: 4107
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x060060CD RID: 24781 RVA: 0x0020EB76 File Offset: 0x0020CD76
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x060060CE RID: 24782
		protected abstract string IncidentTag { get; }

		// Token: 0x060060CF RID: 24783 RVA: 0x0020EB80 File Offset: 0x0020CD80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.incident == null)
			{
				this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
				Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>());
			}
		}

		// Token: 0x060060D0 RID: 24784 RVA: 0x0020EBE0 File Offset: 0x0020CDE0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x060060D1 RID: 24785 RVA: 0x0020EC04 File Offset: 0x0020CE04
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x060060D2 RID: 24786 RVA: 0x0020EC39 File Offset: 0x0020CE39
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x060060D3 RID: 24787 RVA: 0x0020EC50 File Offset: 0x0020CE50
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x0020EC64 File Offset: 0x0020CE64
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x060060D5 RID: 24789 RVA: 0x0020EC8C File Offset: 0x0020CE8C
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x0020ECB4 File Offset: 0x0020CEB4
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x060060D7 RID: 24791 RVA: 0x0020ECBC File Offset: 0x0020CEBC
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

		// Token: 0x0400374C RID: 14156
		protected IncidentDef incident;
	}
}
