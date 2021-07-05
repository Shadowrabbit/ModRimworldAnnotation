using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001373 RID: 4979
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x06007921 RID: 31009 RVA: 0x002AEBCA File Offset: 0x002ACDCA
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x17001557 RID: 5463
		// (get) Token: 0x06007922 RID: 31010 RVA: 0x002AEBD1 File Offset: 0x002ACDD1
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06007923 RID: 31011 RVA: 0x002AEBD8 File Offset: 0x002ACDD8
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			this.DoManualPrioritiesCheckbox();
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			Text.Anchor = TextAnchor.UpperCenter;
			Text.Font = GameFont.Tiny;
			Widgets.Label(new Rect(370f, rect.y + 5f, 160f, 30f), "<= " + "HigherPriority".Translate());
			Widgets.Label(new Rect(630f, rect.y + 5f, 160f, 30f), "LowerPriority".Translate() + " =>");
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06007924 RID: 31012 RVA: 0x002AECB4 File Offset: 0x002ACEB4
		private void DoManualPrioritiesCheckbox()
		{
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
			Rect rect = new Rect(5f, 5f, 140f, 30f);
			bool useWorkPriorities = Current.Game.playSettings.useWorkPriorities;
			Widgets.CheckboxLabeled(rect, "ManualPriorities".Translate(), ref Current.Game.playSettings.useWorkPriorities, false, null, null, false);
			if (useWorkPriorities != Current.Game.playSettings.useWorkPriorities)
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					if (pawn.Faction == Faction.OfPlayer && pawn.workSettings != null)
					{
						pawn.workSettings.Notify_UseWorkPrioritiesChanged();
					}
				}
			}
			if (Current.Game.playSettings.useWorkPriorities)
			{
				GUI.color = new Color(1f, 1f, 1f, 0.5f);
				Text.Font = GameFont.Tiny;
				Widgets.Label(new Rect(rect.x, rect.y + rect.height + 4f, rect.width, 60f), "PriorityOneDoneFirst".Translate());
				Text.Font = GameFont.Small;
				GUI.color = Color.white;
			}
			if (!Current.Game.playSettings.useWorkPriorities)
			{
				UIHighlighter.HighlightOpportunity(rect, "ManualPriorities-Off");
			}
		}

		// Token: 0x04004373 RID: 17267
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;
	}
}
