using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAF RID: 2991
	public class QuestPart_SubquestGenerator_RelicHunt : QuestPart_SubquestGenerator
	{
		// Token: 0x060045CE RID: 17870 RVA: 0x00171B88 File Offset: 0x0016FD88
		protected override Slate InitSlate()
		{
			Slate slate = new Slate();
			slate.Set<float>("points", StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map), false);
			slate.Set<Precept_Relic>(this.relicSlateName, this.relic, false);
			return slate;
		}

		// Token: 0x060045CF RID: 17871 RVA: 0x00171BC0 File Offset: 0x0016FDC0
		protected override QuestScriptDef GetNextSubquestDef()
		{
			this.ShuffleQueue();
			QuestScriptDef questScriptDef = this.questQueue.First<QuestScriptDef>();
			if (!questScriptDef.CanRun(this.InitSlate()))
			{
				return null;
			}
			this.questQueue.RemoveAt(0);
			return questScriptDef;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x00171BFC File Offset: 0x0016FDFC
		private void ShuffleQueue()
		{
			this.questQueue.Clear();
			if (this.subquestDefs.Count == 1)
			{
				this.questQueue.AddRange(this.subquestDefs);
				return;
			}
			Quest quest = (from q in this.quest.GetSubquests(null)
			orderby q.appearanceTick descending
			select q).FirstOrDefault<Quest>();
			QuestScriptDef questScriptDef = (quest != null) ? quest.root : null;
			int i = 100;
			while (i > 0)
			{
				i--;
				this.questQueue.Clear();
				this.questQueue.AddRange(this.subquestDefs.InRandomOrder(null));
				if (questScriptDef == null || this.questQueue.First<QuestScriptDef>() != questScriptDef)
				{
					break;
				}
			}
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x00171CC0 File Offset: 0x0016FEC0
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			base.DoDebugWindowContents(innerRect, ref curY);
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Shuffle subquest queue", true, true, true))
			{
				this.ShuffleQueue();
			}
			curY += 29f;
			if (Widgets.ButtonText(new Rect(innerRect.x, curY, 500f, 25f), "Log subquest queue", true, true, true))
			{
				if (this.questQueue.Count == 0)
				{
					this.ShuffleQueue();
				}
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < this.questQueue.Count; i++)
				{
					stringBuilder.AppendLine(string.Format("{0} ->", i) + this.questQueue[i].defName);
				}
				Log.Message(stringBuilder.ToString());
			}
			curY += 29f;
		}

		// Token: 0x060045D2 RID: 17874 RVA: 0x00171DA4 File Offset: 0x0016FFA4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Precept_Relic>(ref this.relic, "relic", false);
			Scribe_Values.Look<string>(ref this.relicSlateName, "relicSlateName", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.questQueue, "questQueue", LookMode.Def, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.questQueue == null)
			{
				this.questQueue = new List<QuestScriptDef>();
			}
		}

		// Token: 0x04002A86 RID: 10886
		private List<QuestScriptDef> questQueue = new List<QuestScriptDef>();

		// Token: 0x04002A87 RID: 10887
		public Precept_Relic relic;

		// Token: 0x04002A88 RID: 10888
		public string relicSlateName;

		// Token: 0x04002A89 RID: 10889
		public MapParent useMapParentThreatPoints;
	}
}
