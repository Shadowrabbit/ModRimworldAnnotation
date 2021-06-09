using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B11 RID: 6929
	public class ITab_Pawn_Log : ITab
	{
		// Token: 0x17001805 RID: 6149
		// (get) Token: 0x06009879 RID: 39033 RVA: 0x002CD530 File Offset: 0x002CB730
		private Pawn SelPawnForCombatInfo
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				throw new InvalidOperationException("Social tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		// Token: 0x0600987A RID: 39034 RVA: 0x002CD578 File Offset: 0x002CB778
		public ITab_Pawn_Log()
		{
			this.size = new Vector2(630f, 510f);
			this.labelKey = "TabLog";
		}

		// Token: 0x0600987B RID: 39035 RVA: 0x002CD5D4 File Offset: 0x002CB7D4
		protected override void FillTab()
		{
			Pawn selPawnForCombatInfo = this.SelPawnForCombatInfo;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y);
			Rect rect2 = new Rect(ITab_Pawn_Log.ShowAllX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowAllWidth, 24f);
			bool flag = this.showAll;
			Widgets.CheckboxLabeled(rect2, "ShowAll".Translate(), ref this.showAll, false, null, null, false);
			if (flag != this.showAll)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect3 = new Rect(ITab_Pawn_Log.ShowCombatX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowCombatWidth, 24f);
			bool flag2 = this.showCombat;
			Widgets.CheckboxLabeled(rect3, "ShowCombat".Translate(), ref this.showCombat, false, null, null, false);
			if (flag2 != this.showCombat)
			{
				this.cachedLogDisplay = null;
			}
			Rect rect4 = new Rect(ITab_Pawn_Log.ShowSocialX, ITab_Pawn_Log.ToolbarHeight, ITab_Pawn_Log.ShowSocialWidth, 24f);
			bool flag3 = this.showSocial;
			Widgets.CheckboxLabeled(rect4, "ShowSocial".Translate(), ref this.showSocial, false, null, null, false);
			if (flag3 != this.showSocial)
			{
				this.cachedLogDisplay = null;
			}
			if (this.cachedLogDisplay == null || this.cachedLogDisplayLastTick != selPawnForCombatInfo.records.LastBattleTick || this.cachedLogPlayLastTick != Find.PlayLog.LastTick || this.cachedLogForPawn != selPawnForCombatInfo)
			{
				this.cachedLogDisplay = ITab_Pawn_Log_Utility.GenerateLogLinesFor(selPawnForCombatInfo, this.showAll, this.showCombat, this.showSocial).ToList<ITab_Pawn_Log_Utility.LogLineDisplayable>();
				this.cachedLogDisplayLastTick = selPawnForCombatInfo.records.LastBattleTick;
				this.cachedLogPlayLastTick = Find.PlayLog.LastTick;
				this.cachedLogForPawn = selPawnForCombatInfo;
			}
			Rect rect5 = new Rect(rect.width - ITab_Pawn_Log.ButtonOffset, 0f, 18f, 24f);
			if (Widgets.ButtonImage(rect5, TexButton.Copy, true))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable in this.cachedLogDisplay)
				{
					logLineDisplayable.AppendTo(stringBuilder);
				}
				GUIUtility.systemCopyBuffer = stringBuilder.ToString();
			}
			TooltipHandler.TipRegionByKey(rect5, "CopyLogTip");
			rect.yMin = 24f;
			rect = rect.ContractedBy(10f);
			float width = rect.width - 16f - 10f;
			float num = 0f;
			foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable2 in this.cachedLogDisplay)
			{
				if (logLineDisplayable2.Matches(this.logSeek))
				{
					this.scrollPosition.y = num - (logLineDisplayable2.GetHeight(width) + rect.height) / 2f;
				}
				num += logLineDisplayable2.GetHeight(width);
			}
			this.logSeek = null;
			if (num > 0f)
			{
				Rect viewRect = new Rect(0f, 0f, rect.width - 16f, num);
				this.data.StartNewDraw();
				Widgets.BeginScrollView(rect, ref this.scrollPosition, viewRect, true);
				float num2 = 0f;
				foreach (ITab_Pawn_Log_Utility.LogLineDisplayable logLineDisplayable3 in this.cachedLogDisplay)
				{
					logLineDisplayable3.Draw(num2, width, this.data);
					num2 += logLineDisplayable3.GetHeight(width);
				}
				Widgets.EndScrollView();
				return;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			GUI.color = Color.grey;
			Widgets.Label(new Rect(0f, 0f, this.size.x, this.size.y), "(" + "NoRecentEntries".Translate() + ")");
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x0600987C RID: 39036 RVA: 0x00065964 File Offset: 0x00063B64
		public void SeekTo(LogEntry entry)
		{
			this.logSeek = entry;
		}

		// Token: 0x0600987D RID: 39037 RVA: 0x0006596D File Offset: 0x00063B6D
		public void Highlight(LogEntry entry)
		{
			this.data.highlightEntry = entry;
			this.data.highlightIntensity = 1f;
		}

		// Token: 0x0600987E RID: 39038 RVA: 0x0006598B File Offset: 0x00063B8B
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.cachedLogForPawn = null;
		}

		// Token: 0x0400616B RID: 24939
		public const float Width = 630f;

		// Token: 0x0400616C RID: 24940
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllX = 60f;

		// Token: 0x0400616D RID: 24941
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllWidth = 100f;

		// Token: 0x0400616E RID: 24942
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatX = 445f;

		// Token: 0x0400616F RID: 24943
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatWidth = 115f;

		// Token: 0x04006170 RID: 24944
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialX = 330f;

		// Token: 0x04006171 RID: 24945
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialWidth = 105f;

		// Token: 0x04006172 RID: 24946
		[TweakValue("Interface", 0f, 20f)]
		private static float ToolbarHeight = 2f;

		// Token: 0x04006173 RID: 24947
		[TweakValue("Interface", 0f, 100f)]
		private static float ButtonOffset = 60f;

		// Token: 0x04006174 RID: 24948
		public bool showAll;

		// Token: 0x04006175 RID: 24949
		public bool showCombat = true;

		// Token: 0x04006176 RID: 24950
		public bool showSocial = true;

		// Token: 0x04006177 RID: 24951
		public LogEntry logSeek;

		// Token: 0x04006178 RID: 24952
		public ITab_Pawn_Log_Utility.LogDrawData data = new ITab_Pawn_Log_Utility.LogDrawData();

		// Token: 0x04006179 RID: 24953
		public List<ITab_Pawn_Log_Utility.LogLineDisplayable> cachedLogDisplay;

		// Token: 0x0400617A RID: 24954
		public int cachedLogDisplayLastTick = -1;

		// Token: 0x0400617B RID: 24955
		public int cachedLogPlayLastTick = -1;

		// Token: 0x0400617C RID: 24956
		private Pawn cachedLogForPawn;

		// Token: 0x0400617D RID: 24957
		private Vector2 scrollPosition;
	}
}
