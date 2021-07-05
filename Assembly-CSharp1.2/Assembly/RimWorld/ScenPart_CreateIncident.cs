using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020015F9 RID: 5625
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06007A54 RID: 31316 RVA: 0x0005240F File Offset: 0x0005060F
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06007A55 RID: 31317 RVA: 0x00052416 File Offset: 0x00050616
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x06007A56 RID: 31318 RVA: 0x0024E68C File Offset: 0x0024C88C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x06007A57 RID: 31319 RVA: 0x0024E6F0 File Offset: 0x0024C8F0
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight * 3f);
			Rect rect = new Rect(scenPartRect.x, scenPartRect.y, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect2 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height / 3f, scenPartRect.width, scenPartRect.height / 3f);
			Rect rect3 = new Rect(scenPartRect.x, scenPartRect.y + scenPartRect.height * 2f / 3f, scenPartRect.width, scenPartRect.height / 3f);
			base.DoIncidentEditInterface(rect);
			Widgets.TextFieldNumericLabeled<float>(rect2, "intervalDays".Translate(), ref this.intervalDays, ref this.intervalDaysBuffer, 0f, 1E+09f);
			Widgets.CheckboxLabeled(rect3, "repeat".Translate(), ref this.repeat, false, null, null, false);
		}

		// Token: 0x06007A58 RID: 31320 RVA: 0x0024E7FC File Offset: 0x0024C9FC
		public override void Randomize()
		{
			base.Randomize();
			this.intervalDays = 15f * Rand.Gaussian(0f, 1f) + 30f;
			if (this.intervalDays < 0f)
			{
				this.intervalDays = 0f;
			}
			this.repeat = (Rand.Range(0, 100) < 50);
		}

		// Token: 0x06007A59 RID: 31321 RVA: 0x00052424 File Offset: 0x00050624
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x06007A5A RID: 31322 RVA: 0x0005242D File Offset: 0x0005062D
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x06007A5B RID: 31323 RVA: 0x0024E85C File Offset: 0x0024CA5C
		public override void Tick()
		{
			base.Tick();
			if (Find.AnyPlayerHomeMap == null)
			{
				return;
			}
			if (this.isFinished)
			{
				return;
			}
			if (this.incident == null)
			{
				Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null", false);
				this.isFinished = true;
				return;
			}
			if ((float)Find.TickManager.TicksGame >= this.occurTick)
			{
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(this.incident.category, (from x in Find.Maps
				where x.IsPlayerHome
				select x).RandomElement<Map>());
				if (!this.incident.Worker.TryExecute(parms))
				{
					this.isFinished = true;
					return;
				}
				if (this.repeat && this.intervalDays > 0f)
				{
					this.occurTick += this.IntervalTicks;
					return;
				}
				this.isFinished = true;
			}
		}

		// Token: 0x04005039 RID: 20537
		private const float IntervalMidpoint = 30f;

		// Token: 0x0400503A RID: 20538
		private const float IntervalDeviation = 15f;

		// Token: 0x0400503B RID: 20539
		private float intervalDays;

		// Token: 0x0400503C RID: 20540
		private bool repeat;

		// Token: 0x0400503D RID: 20541
		private string intervalDaysBuffer;

		// Token: 0x0400503E RID: 20542
		private float occurTick;

		// Token: 0x0400503F RID: 20543
		private bool isFinished;
	}
}
