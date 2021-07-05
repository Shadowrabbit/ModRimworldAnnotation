using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001008 RID: 4104
	internal class ScenPart_CreateIncident : ScenPart_IncidentBase
	{
		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x060060BD RID: 24765 RVA: 0x0020E712 File Offset: 0x0020C912
		protected override string IncidentTag
		{
			get
			{
				return "CreateIncident";
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x060060BE RID: 24766 RVA: 0x0020E719 File Offset: 0x0020C919
		private float IntervalTicks
		{
			get
			{
				return 60000f * this.intervalDays;
			}
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0020E728 File Offset: 0x0020C928
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.intervalDays, "intervalDays", 0f, false);
			Scribe_Values.Look<bool>(ref this.repeat, "repeat", false, false);
			Scribe_Values.Look<float>(ref this.occurTick, "occurTick", 0f, false);
			Scribe_Values.Look<bool>(ref this.isFinished, "isFinished", false, false);
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x0020E78C File Offset: 0x0020C98C
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

		// Token: 0x060060C1 RID: 24769 RVA: 0x0020E898 File Offset: 0x0020CA98
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

		// Token: 0x060060C2 RID: 24770 RVA: 0x0020E8F6 File Offset: 0x0020CAF6
		protected override IEnumerable<IncidentDef> RandomizableIncidents()
		{
			yield return IncidentDefOf.Eclipse;
			yield return IncidentDefOf.ToxicFallout;
			yield return IncidentDefOf.SolarFlare;
			yield break;
		}

		// Token: 0x060060C3 RID: 24771 RVA: 0x0020E8FF File Offset: 0x0020CAFF
		public override void PostGameStart()
		{
			base.PostGameStart();
			this.occurTick = (float)Find.TickManager.TicksGame + this.IntervalTicks;
		}

		// Token: 0x060060C4 RID: 24772 RVA: 0x0020E920 File Offset: 0x0020CB20
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
				Log.Error("Trying to tick ScenPart_CreateIncident but the incident is null");
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

		// Token: 0x04003742 RID: 14146
		private const float IntervalMidpoint = 30f;

		// Token: 0x04003743 RID: 14147
		private const float IntervalDeviation = 15f;

		// Token: 0x04003744 RID: 14148
		private float intervalDays;

		// Token: 0x04003745 RID: 14149
		private bool repeat;

		// Token: 0x04003746 RID: 14150
		private string intervalDaysBuffer;

		// Token: 0x04003747 RID: 14151
		private float occurTick;

		// Token: 0x04003748 RID: 14152
		private bool isFinished;
	}
}
