using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BCC RID: 3020
	public class QuestPart_ThreatsGenerator : QuestPartActivable, IIncidentMakerQuestPart
	{
		// Token: 0x060046BC RID: 18108 RVA: 0x00176297 File Offset: 0x00174497
		public IEnumerable<FiringIncident> MakeIntervalIncidents()
		{
			if (this.mapParent != null && this.mapParent.HasMap)
			{
				return ThreatsGenerator.MakeIntervalIncidents(this.parms, this.mapParent.Map, base.EnableTick + this.threatStartTicks);
			}
			return Enumerable.Empty<FiringIncident>();
		}

		// Token: 0x060046BD RID: 18109 RVA: 0x001762D8 File Offset: 0x001744D8
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

		// Token: 0x060046BE RID: 18110 RVA: 0x00176345 File Offset: 0x00174545
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThreatsGeneratorParams>(ref this.parms, "parms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.threatStartTicks, "threatStartTicks", 0, false);
		}

		// Token: 0x04002B33 RID: 11059
		public ThreatsGeneratorParams parms;

		// Token: 0x04002B34 RID: 11060
		public MapParent mapParent;

		// Token: 0x04002B35 RID: 11061
		public int threatStartTicks;
	}
}
