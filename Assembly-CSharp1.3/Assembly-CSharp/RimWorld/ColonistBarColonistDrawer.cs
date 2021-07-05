using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001295 RID: 4757
	[StaticConstructorOnStartup]
	public class ColonistBarColonistDrawer
	{
		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x0600719F RID: 29087 RVA: 0x0025F217 File Offset: 0x0025D417
		private ColonistBar ColonistBar
		{
			get
			{
				return Find.ColonistBar;
			}
		}

		// Token: 0x060071A0 RID: 29088 RVA: 0x0025F220 File Offset: 0x0025D420
		public void DrawColonist(Rect rect, Pawn colonist, Map pawnMap, bool highlight, bool reordering)
		{
			float num = this.ColonistBar.GetEntryRectAlpha(rect);
			this.ApplyEntryInAnotherMapAlphaFactor(pawnMap, ref num);
			if (reordering)
			{
				num *= 0.5f;
			}
			Color color = new Color(1f, 1f, 1f, num);
			GUI.color = color;
			GUI.DrawTexture(rect, ColonistBar.BGTex);
			if (colonist.needs != null && colonist.needs.mood != null)
			{
				Rect position = rect.ContractedBy(2f);
				float num2 = position.height * colonist.needs.mood.CurLevelPercentage;
				position.yMin = position.yMax - num2;
				position.height = num2;
				GUI.DrawTexture(position, ColonistBarColonistDrawer.MoodBGTex);
			}
			if (highlight)
			{
				int thickness = (rect.width <= 22f) ? 2 : 3;
				GUI.color = Color.white;
				Widgets.DrawBox(rect, thickness, null);
				GUI.color = color;
			}
			Rect rect2 = rect.ContractedBy(-2f * this.ColonistBar.Scale);
			if ((colonist.Dead ? Find.Selector.SelectedObjects.Contains(colonist.Corpse) : Find.Selector.SelectedObjects.Contains(colonist)) && !WorldRendererUtility.WorldRenderedNow)
			{
				this.DrawSelectionOverlayOnGUI(colonist, rect2);
			}
			else if (WorldRendererUtility.WorldRenderedNow && colonist.IsCaravanMember() && Find.WorldSelector.IsSelected(colonist.GetCaravan()))
			{
				this.DrawCaravanSelectionOverlayOnGUI(colonist.GetCaravan(), rect2);
			}
			GUI.DrawTexture(this.GetPawnTextureRect(rect.position), PortraitsCache.Get(colonist, ColonistBarColonistDrawer.PawnTextureSize, Rot4.South, ColonistBarColonistDrawer.PawnTextureCameraOffset, 1.28205f, true, true, true, true, null, false));
			GUI.color = new Color(1f, 1f, 1f, num * 0.8f);
			this.DrawIcons(rect, colonist);
			GUI.color = color;
			if (colonist.Dead)
			{
				GUI.DrawTexture(rect, ColonistBarColonistDrawer.DeadColonistTex);
			}
			float num3 = 4f * this.ColonistBar.Scale;
			Vector2 pos = new Vector2(rect.center.x, rect.yMax - num3);
			GenMapUI.DrawPawnLabel(colonist, pos, num, rect.width + this.ColonistBar.SpaceBetweenColonistsHorizontal - 2f, this.pawnLabelsCache, GameFont.Tiny, true, true);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
		}

		// Token: 0x060071A1 RID: 29089 RVA: 0x0025F470 File Offset: 0x0025D670
		private Rect GroupFrameRect(int group)
		{
			float num = 99999f;
			float num2 = 0f;
			float num3 = 0f;
			List<ColonistBar.Entry> entries = this.ColonistBar.Entries;
			List<Vector2> drawLocs = this.ColonistBar.DrawLocs;
			for (int i = 0; i < entries.Count; i++)
			{
				if (entries[i].group == group)
				{
					num = Mathf.Min(num, drawLocs[i].x);
					num2 = Mathf.Max(num2, drawLocs[i].x + this.ColonistBar.Size.x);
					num3 = Mathf.Max(num3, drawLocs[i].y + this.ColonistBar.Size.y);
				}
			}
			return new Rect(num, 0f, num2 - num, num3 - 0f).ContractedBy(-12f * this.ColonistBar.Scale);
		}

		// Token: 0x060071A2 RID: 29090 RVA: 0x0025F55C File Offset: 0x0025D75C
		public void DrawGroupFrame(int group)
		{
			Rect position = this.GroupFrameRect(group);
			Map map = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group).map;
			float num;
			if (map == null)
			{
				if (WorldRendererUtility.WorldRenderedNow)
				{
					num = 1f;
				}
				else
				{
					num = 0.75f;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				num = 0.75f;
			}
			else
			{
				num = 1f;
			}
			Widgets.DrawRectFast(position, new Color(0.5f, 0.5f, 0.5f, 0.4f * num), null);
		}

		// Token: 0x060071A3 RID: 29091 RVA: 0x0025F5FC File Offset: 0x0025D7FC
		private void ApplyEntryInAnotherMapAlphaFactor(Map map, ref float alpha)
		{
			if (map == null)
			{
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					alpha = Mathf.Min(alpha, 0.4f);
					return;
				}
			}
			else if (map != Find.CurrentMap || WorldRendererUtility.WorldRenderedNow)
			{
				alpha = Mathf.Min(alpha, 0.4f);
			}
		}

		// Token: 0x060071A4 RID: 29092 RVA: 0x0025F634 File Offset: 0x0025D834
		public void HandleClicks(Rect rect, Pawn colonist, int reorderableGroup, out bool reordering)
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Event.current.clickCount == 2 && Mouse.IsOver(rect))
			{
				Event.current.Use();
				CameraJumper.TryJump(colonist);
			}
			reordering = ReorderableWidget.Reorderable(reorderableGroup, rect, true);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x060071A5 RID: 29093 RVA: 0x0025F6BC File Offset: 0x0025D8BC
		public void HandleGroupFrameClicks(int group)
		{
			Rect rect = this.GroupFrameRect(group);
			if (Event.current.type == EventType.MouseUp && Event.current.button == 0 && Mouse.IsOver(rect) && !this.ColonistBar.AnyColonistOrCorpseAt(UI.MousePositionOnUIInverted))
			{
				bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
				if ((!worldRenderedNow && !Find.Selector.dragBox.IsValidAndActive) || (worldRenderedNow && !Find.WorldSelector.dragBox.IsValidAndActive))
				{
					Find.Selector.dragBox.active = false;
					Find.WorldSelector.dragBox.active = false;
					ColonistBar.Entry entry = this.ColonistBar.Entries.Find((ColonistBar.Entry x) => x.group == group);
					Map map = entry.map;
					if (map == null)
					{
						if (WorldRendererUtility.WorldRenderedNow)
						{
							CameraJumper.TrySelect(entry.pawn);
						}
						else
						{
							CameraJumper.TryJumpAndSelect(entry.pawn);
						}
					}
					else
					{
						if (!CameraJumper.TryHideWorld() && Find.CurrentMap != map)
						{
							SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
						}
						Current.Game.CurrentMap = map;
					}
				}
			}
			if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && Mouse.IsOver(rect))
			{
				Event.current.Use();
			}
		}

		// Token: 0x060071A6 RID: 29094 RVA: 0x0025F81C File Offset: 0x0025DA1C
		public void Notify_RecachedEntries()
		{
			this.pawnLabelsCache.Clear();
		}

		// Token: 0x060071A7 RID: 29095 RVA: 0x0025F82C File Offset: 0x0025DA2C
		public Rect GetPawnTextureRect(Vector2 pos)
		{
			float x = pos.x;
			float y = pos.y;
			Vector2 vector = ColonistBarColonistDrawer.PawnTextureSize * this.ColonistBar.Scale;
			return new Rect(x + 1f, y - (vector.y - this.ColonistBar.Size.y) - 1f, vector.x, vector.y).ContractedBy(1f);
		}

		// Token: 0x060071A8 RID: 29096 RVA: 0x0025F8A0 File Offset: 0x0025DAA0
		private void DrawIcons(Rect rect, Pawn colonist)
		{
			if (colonist.Dead)
			{
				return;
			}
			ColonistBarColonistDrawer.tmpIconsToDraw.Clear();
			bool flag = false;
			if (colonist.CurJob != null)
			{
				JobDef def = colonist.CurJob.def;
				if (def == JobDefOf.AttackMelee || def == JobDefOf.AttackStatic)
				{
					flag = true;
				}
				else if (def == JobDefOf.Wait_Combat)
				{
					Stance_Busy stance_Busy = colonist.stances.curStance as Stance_Busy;
					if (stance_Busy != null && stance_Busy.focusTarg.IsValid)
					{
						flag = true;
					}
				}
			}
			if (colonist.IsFormingCaravan())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_FormingCaravan, "ActivityIconFormingCaravan".Translate(), null));
			}
			if (colonist.InAggroMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InMentalState)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MentalStateNonAggro, colonist.MentalStateDef.LabelCap, null));
			}
			else if (colonist.InBed() && colonist.CurrentBed().Medical)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_MedicalRest, "ActivityIconMedicalRest".Translate(), null));
			}
			else if (colonist.CurJob != null && colonist.jobs.curDriver.asleep)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Sleeping, "ActivityIconSleeping".Translate(), null));
			}
			else if (colonist.CurJob != null && colonist.CurJob.def == JobDefOf.FleeAndCower)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Fleeing, "ActivityIconFleeing".Translate(), null));
			}
			else if (flag)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Attacking, "ActivityIconAttacking".Translate(), null));
			}
			else if (colonist.mindState.IsIdle && GenDate.DaysPassed >= 1)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Idle, "ActivityIconIdle".Translate(), null));
			}
			if (colonist.IsBurning())
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Burning, "ActivityIconBurning".Translate(), null));
			}
			if (colonist.Inspired)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(ColonistBarColonistDrawer.Icon_Inspired, colonist.InspirationDef.LabelCap, null));
			}
			if (colonist.IsSlaveOfColony)
			{
				ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(colonist.guest.GetIcon(), null, null));
			}
			else
			{
				bool flag2 = false;
				if (colonist.Ideo != null)
				{
					Ideo ideo = colonist.Ideo;
					Precept_Role role = ideo.GetRole(colonist);
					if (role != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(role.Icon, null, new Color?(ideo.Color)));
						flag2 = true;
					}
				}
				if (!flag2)
				{
					Faction faction = null;
					if (colonist.HasExtraMiniFaction(null))
					{
						faction = colonist.GetExtraMiniFaction(null);
					}
					else if (colonist.HasExtraHomeFaction(null))
					{
						faction = colonist.GetExtraHomeFaction(null);
					}
					if (faction != null)
					{
						ColonistBarColonistDrawer.tmpIconsToDraw.Add(new ColonistBarColonistDrawer.IconDrawCall(faction.def.FactionIcon, null, new Color?(faction.Color)));
					}
				}
			}
			float num = Mathf.Min(ColonistBarColonistDrawer.BaseIconAreaWidth / (float)ColonistBarColonistDrawer.tmpIconsToDraw.Count, ColonistBarColonistDrawer.BaseIconMaxSize) * this.ColonistBar.Scale;
			Vector2 vector = new Vector2(rect.x + 1f, rect.yMax - num - 1f);
			foreach (ColonistBarColonistDrawer.IconDrawCall iconDrawCall in ColonistBarColonistDrawer.tmpIconsToDraw)
			{
				GUI.color = (iconDrawCall.color ?? Color.white);
				this.DrawIcon(iconDrawCall.texture, ref vector, num, iconDrawCall.tooltip);
				GUI.color = Color.white;
			}
		}

		// Token: 0x060071A9 RID: 29097 RVA: 0x0025FD24 File Offset: 0x0025DF24
		private void DrawIcon(Texture2D icon, ref Vector2 pos, float iconSize, string tooltip = null)
		{
			Rect rect = new Rect(pos.x, pos.y, iconSize, iconSize);
			GUI.DrawTexture(rect, icon);
			if (tooltip != null)
			{
				TooltipHandler.TipRegion(rect, tooltip);
			}
			pos.x += iconSize;
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x0025FD6C File Offset: 0x0025DF6C
		private void DrawSelectionOverlayOnGUI(Pawn colonist, Rect rect)
		{
			Thing obj = colonist;
			if (colonist.Dead)
			{
				obj = colonist.Corpse;
			}
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<object>(ColonistBarColonistDrawer.bracketLocs, obj, rect, SelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x0025FDEC File Offset: 0x0025DFEC
		private void DrawCaravanSelectionOverlayOnGUI(Caravan caravan, Rect rect)
		{
			float num = 0.4f * this.ColonistBar.Scale;
			Vector2 textureSize = new Vector2((float)SelectionDrawerUtility.SelectedTexGUI.width * num, (float)SelectionDrawerUtility.SelectedTexGUI.height * num);
			SelectionDrawerUtility.CalculateSelectionBracketPositionsUI<WorldObject>(ColonistBarColonistDrawer.bracketLocs, caravan, rect, WorldSelectionDrawer.SelectTimes, textureSize, 20f * this.ColonistBar.Scale);
			this.DrawSelectionOverlayOnGUI(ColonistBarColonistDrawer.bracketLocs, num);
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x0025FE5C File Offset: 0x0025E05C
		private void DrawSelectionOverlayOnGUI(Vector2[] bracketLocs, float selectedTexScale)
		{
			int num = 90;
			for (int i = 0; i < 4; i++)
			{
				Widgets.DrawTextureRotated(bracketLocs[i], SelectionDrawerUtility.SelectedTexGUI, (float)num, selectedTexScale);
				num += 90;
			}
		}

		// Token: 0x04003E8C RID: 16012
		private Dictionary<string, string> pawnLabelsCache = new Dictionary<string, string>();

		// Token: 0x04003E8D RID: 16013
		private static readonly Texture2D MoodBGTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.4f, 0.47f, 0.53f, 0.44f));

		// Token: 0x04003E8E RID: 16014
		private static readonly Texture2D DeadColonistTex = ContentFinder<Texture2D>.Get("UI/Misc/DeadColonist", true);

		// Token: 0x04003E8F RID: 16015
		private static readonly Texture2D Icon_FormingCaravan = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/FormingCaravan", true);

		// Token: 0x04003E90 RID: 16016
		private static readonly Texture2D Icon_MentalStateNonAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateNonAggro", true);

		// Token: 0x04003E91 RID: 16017
		private static readonly Texture2D Icon_MentalStateAggro = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MentalStateAggro", true);

		// Token: 0x04003E92 RID: 16018
		private static readonly Texture2D Icon_MedicalRest = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/MedicalRest", true);

		// Token: 0x04003E93 RID: 16019
		private static readonly Texture2D Icon_Sleeping = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x04003E94 RID: 16020
		private static readonly Texture2D Icon_Fleeing = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Fleeing", true);

		// Token: 0x04003E95 RID: 16021
		private static readonly Texture2D Icon_Attacking = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Attacking", true);

		// Token: 0x04003E96 RID: 16022
		private static readonly Texture2D Icon_Idle = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Idle", true);

		// Token: 0x04003E97 RID: 16023
		private static readonly Texture2D Icon_Burning = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Burning", true);

		// Token: 0x04003E98 RID: 16024
		private static readonly Texture2D Icon_Inspired = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Inspired", true);

		// Token: 0x04003E99 RID: 16025
		public static readonly Vector2 PawnTextureSize = new Vector2(ColonistBar.BaseSize.x - 2f, 75f);

		// Token: 0x04003E9A RID: 16026
		public static readonly Vector3 PawnTextureCameraOffset = new Vector3(0f, 0f, 0.3f);

		// Token: 0x04003E9B RID: 16027
		public const float PawnTextureCameraZoom = 1.28205f;

		// Token: 0x04003E9C RID: 16028
		private const float PawnTextureHorizontalPadding = 1f;

		// Token: 0x04003E9D RID: 16029
		private static readonly float BaseIconAreaWidth = ColonistBarColonistDrawer.PawnTextureSize.x;

		// Token: 0x04003E9E RID: 16030
		private static readonly float BaseIconMaxSize = 20f;

		// Token: 0x04003E9F RID: 16031
		private const float BaseGroupFrameMargin = 12f;

		// Token: 0x04003EA0 RID: 16032
		public const float DoubleClickTime = 0.5f;

		// Token: 0x04003EA1 RID: 16033
		public const float FactionIconSpacing = 2f;

		// Token: 0x04003EA2 RID: 16034
		public const float IdeoRoleIconSpacing = 2f;

		// Token: 0x04003EA3 RID: 16035
		public const float SlaveIconSpacing = 2f;

		// Token: 0x04003EA4 RID: 16036
		private static List<ColonistBarColonistDrawer.IconDrawCall> tmpIconsToDraw = new List<ColonistBarColonistDrawer.IconDrawCall>();

		// Token: 0x04003EA5 RID: 16037
		private static Vector2[] bracketLocs = new Vector2[4];

		// Token: 0x02002605 RID: 9733
		private struct IconDrawCall
		{
			// Token: 0x0600D4D9 RID: 54489 RVA: 0x00405FBE File Offset: 0x004041BE
			public IconDrawCall(Texture2D texture, string tooltip = null, Color? color = null)
			{
				this.texture = texture;
				this.tooltip = tooltip;
				this.color = color;
			}

			// Token: 0x040090F9 RID: 37113
			public Texture2D texture;

			// Token: 0x040090FA RID: 37114
			public string tooltip;

			// Token: 0x040090FB RID: 37115
			public Color? color;
		}
	}
}
