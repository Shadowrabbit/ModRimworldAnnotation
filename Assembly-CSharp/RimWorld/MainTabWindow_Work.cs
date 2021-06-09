using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B5E RID: 7006
	public class MainTabWindow_Work : MainTabWindow_PawnTable
	{
		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06009A65 RID: 39525 RVA: 0x00066CE5 File Offset: 0x00064EE5
		protected override PawnTableDef PawnTableDef
		{
			get
			{
				return PawnTableDefOf.Work;
			}
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06009A66 RID: 39526 RVA: 0x00066CEC File Offset: 0x00064EEC
		protected override float ExtraTopSpace
		{
			get
			{
				return 40f;
			}
		}

		// Token: 0x06009A67 RID: 39527 RVA: 0x0006636A File Offset: 0x0006456A
		public override void PostOpen()
		{
			base.PostOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		// Token: 0x06009A68 RID: 39528 RVA: 0x002D7348 File Offset: 0x002D5548
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

		// Token: 0x06009A69 RID: 39529 RVA: 0x002D7424 File Offset: 0x002D5624
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

		// Token: 0x040062BA RID: 25274
		private const int SpaceBetweenPriorityArrowsAndWorkLabels = 40;
	}
}
