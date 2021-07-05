using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BAE RID: 2990
	public abstract class QuestPart_SubquestGenerator : QuestPartActivable
	{
		// Token: 0x17000C31 RID: 3121
		// (get) Token: 0x060045C3 RID: 17859 RVA: 0x00171782 File Offset: 0x0016F982
		private bool CanGenerateSubquest
		{
			get
			{
				return this.PendingSubquestCount < this.maxActiveSubquests && this.SuccessfulSubquestCount + this.PendingSubquestCount < this.maxSuccessfulSubquests;
			}
		}

		// Token: 0x17000C32 RID: 3122
		// (get) Token: 0x060045C4 RID: 17860 RVA: 0x001717A9 File Offset: 0x0016F9A9
		private int SuccessfulSubquestCount
		{
			get
			{
				return this.quest.GetSubquests(new QuestState?(QuestState.EndedSuccess)).Count<Quest>();
			}
		}

		// Token: 0x17000C33 RID: 3123
		// (get) Token: 0x060045C5 RID: 17861 RVA: 0x001717C4 File Offset: 0x0016F9C4
		private int PendingSubquestCount
		{
			get
			{
				return (from q in this.quest.GetSubquests(null)
				where q.State == QuestState.Ongoing || q.State == QuestState.NotYetAccepted
				select q).Count<Quest>();
			}
		}

		// Token: 0x17000C34 RID: 3124
		// (get) Token: 0x060045C6 RID: 17862 RVA: 0x00171810 File Offset: 0x0016FA10
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.expiryInfoPartKey.NullOrEmpty())
				{
					return null;
				}
				return string.Format("{0} {1} / {2}", this.expiryInfoPartKey.Translate(), this.SuccessfulSubquestCount, this.maxSuccessfulSubquests);
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0017185C File Offset: 0x0016FA5C
		public override void QuestPartTick()
		{
			if (this.subquestDefs.Count == 0)
			{
				return;
			}
			if (this.maxSuccessfulSubquests > 0 && this.SuccessfulSubquestCount >= this.maxSuccessfulSubquests)
			{
				base.Complete();
				return;
			}
			if (this.currentInterval != null)
			{
				this.currentInterval--;
				int? num = this.currentInterval;
				int num2 = 0;
				if (num.GetValueOrDefault() < num2 & num != null)
				{
					this.GenerateSubquest();
					this.currentInterval = null;
				}
			}
			if (this.currentInterval == null && this.CanGenerateSubquest)
			{
				this.currentInterval = new int?(this.interval.RandomInRange);
			}
		}

		// Token: 0x060045C8 RID: 17864
		protected abstract QuestScriptDef GetNextSubquestDef();

		// Token: 0x060045C9 RID: 17865
		protected abstract Slate InitSlate();

		// Token: 0x060045CA RID: 17866 RVA: 0x0017192C File Offset: 0x0016FB2C
		private void GenerateSubquest()
		{
			QuestScriptDef nextSubquestDef = this.GetNextSubquestDef();
			if (nextSubquestDef == null)
			{
				return;
			}
			Slate vars = this.InitSlate();
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(nextSubquestDef, vars);
			quest.parent = this.quest;
			if (!quest.hidden && quest.root.sendAvailableLetter)
			{
				QuestUtility.SendLetterQuestAvailable(quest);
			}
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x0017197C File Offset: 0x0016FB7C
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "Generate " + this.ToString(), true, true, true) && this.CanGenerateSubquest)
			{
				this.GenerateSubquest();
			}
			curY += rect.height + 4f;
			Rect rect2 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect2, "Remove Active " + this.ToString(), true, true, true))
			{
				foreach (Quest quest in this.quest.GetSubquests(null))
				{
					Find.QuestManager.Remove(quest);
				}
			}
			curY += rect2.height + 4f;
			Rect rect3 = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect3, "Complete " + this.ToString(), true, true, true))
			{
				base.Complete();
			}
			curY += rect3.height + 4f;
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00171AD4 File Offset: 0x0016FCD4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int?>(ref this.currentInterval, "currentInterval", null, false);
			Scribe_Collections.Look<QuestScriptDef>(ref this.subquestDefs, "subquestDefs", LookMode.Def, Array.Empty<object>());
			Scribe_Values.Look<IntRange>(ref this.interval, "interval", default(IntRange), false);
			Scribe_Values.Look<int>(ref this.maxActiveSubquests, "maxActiveSubquests", 0, false);
			Scribe_Values.Look<int>(ref this.maxSuccessfulSubquests, "maxSuccessfulSubquests", -1, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartKey, "expiryInfoPartKey", null, false);
		}

		// Token: 0x04002A80 RID: 10880
		public List<QuestScriptDef> subquestDefs = new List<QuestScriptDef>();

		// Token: 0x04002A81 RID: 10881
		public IntRange interval;

		// Token: 0x04002A82 RID: 10882
		public int maxActiveSubquests = 2;

		// Token: 0x04002A83 RID: 10883
		public string expiryInfoPartKey;

		// Token: 0x04002A84 RID: 10884
		public int maxSuccessfulSubquests = -1;

		// Token: 0x04002A85 RID: 10885
		private int? currentInterval;
	}
}
