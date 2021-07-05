using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134A RID: 4938
	public class ITab_Pawn_Log : ITab
	{
		// Token: 0x17001502 RID: 5378
		// (get) Token: 0x06007794 RID: 30612 RVA: 0x002A1E38 File Offset: 0x002A0038
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

		// Token: 0x06007795 RID: 30613 RVA: 0x002A1E80 File Offset: 0x002A0080
		public ITab_Pawn_Log()
		{
			this.size = new Vector2(630f, 510f);
			this.labelKey = "TabLog";
		}

		// Token: 0x06007796 RID: 30614 RVA: 0x002A1EDC File Offset: 0x002A00DC
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

		// Token: 0x06007797 RID: 30615 RVA: 0x002A22E8 File Offset: 0x002A04E8
		public void SeekTo(LogEntry entry)
		{
			this.logSeek = entry;
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x002A22F1 File Offset: 0x002A04F1
		public void Highlight(LogEntry entry)
		{
			this.data.highlightEntry = entry;
			this.data.highlightIntensity = 1f;
		}

		// Token: 0x06007799 RID: 30617 RVA: 0x002A230F File Offset: 0x002A050F
		public override void Notify_ClearingAllMapsMemory()
		{
			base.Notify_ClearingAllMapsMemory();
			this.cachedLogForPawn = null;
		}

		// Token: 0x04004277 RID: 17015
		public const float Width = 630f;

		// Token: 0x04004278 RID: 17016
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllX = 60f;

		// Token: 0x04004279 RID: 17017
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowAllWidth = 100f;

		// Token: 0x0400427A RID: 17018
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatX = 445f;

		// Token: 0x0400427B RID: 17019
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowCombatWidth = 115f;

		// Token: 0x0400427C RID: 17020
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialX = 330f;

		// Token: 0x0400427D RID: 17021
		[TweakValue("Interface", 0f, 1000f)]
		private static float ShowSocialWidth = 105f;

		// Token: 0x0400427E RID: 17022
		[TweakValue("Interface", 0f, 20f)]
		private static float ToolbarHeight = 2f;

		// Token: 0x0400427F RID: 17023
		[TweakValue("Interface", 0f, 100f)]
		private static float ButtonOffset = 60f;

		// Token: 0x04004280 RID: 17024
		public bool showAll;

		// Token: 0x04004281 RID: 17025
		public bool showCombat = true;

		// Token: 0x04004282 RID: 17026
		public bool showSocial = true;

		// Token: 0x04004283 RID: 17027
		public LogEntry logSeek;

		// Token: 0x04004284 RID: 17028
		public ITab_Pawn_Log_Utility.LogDrawData data = new ITab_Pawn_Log_Utility.LogDrawData();

		// Token: 0x04004285 RID: 17029
		public List<ITab_Pawn_Log_Utility.LogLineDisplayable> cachedLogDisplay;

		// Token: 0x04004286 RID: 17030
		public int cachedLogDisplayLastTick = -1;

		// Token: 0x04004287 RID: 17031
		public int cachedLogPlayLastTick = -1;

		// Token: 0x04004288 RID: 17032
		private Pawn cachedLogForPawn;

		// Token: 0x04004289 RID: 17033
		private Vector2 scrollPosition;
	}
}
