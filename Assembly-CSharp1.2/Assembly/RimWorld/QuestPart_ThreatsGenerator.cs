using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001159 RID: 4441
	public class QuestPart_ThreatsGenerator : QuestPartActivable, IIncidentMakerQuestPart
	{
		// Token: 0x06006190 RID: 24976 RVA: 0x00043292 File Offset: 0x00041492
		public IEnumerable<FiringIncident> MakeIntervalIncidents()
		{
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				return ThreatsGenerator.MakeIntervalIncidents(this.parms, this.mapParent.Map, base.EnableTick + this.threatStartTicks);
			}
			return Enumerable.Empty<FiringIncident>();
		}

		// Token: 0x06006191 RID: 24977 RVA: 0x001E8464 File Offset: 0x001E6664
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Log future incidents from " + base.GetType().Name, true, true, true))
			{
				StorytellerUtility.DebugLogTestFutureIncidents(false, null, this, 50);
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06006192 RID: 24978 RVA: 0x000432D2 File Offset: 0x000414D2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThreatsGeneratorParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.threatStartTicks, "threatStartTicks", 0, false);
		}

		// Token: 0x04004131 RID: 16689
		public ThreatsGeneratorParams parms;

		// Token: 0x04004132 RID: 16690
		public MapParent mapParent;

		// Token: 0x04004133 RID: 16691
		public int threatStartTicks;
	}
}
