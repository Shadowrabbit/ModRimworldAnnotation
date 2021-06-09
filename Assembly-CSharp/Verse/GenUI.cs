using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000766 RID: 1894
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06002FB6 RID: 12214 RVA: 0x0002585A File Offset: 0x00023A5A
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06002FB7 RID: 12215 RVA: 0x00025862 File Offset: 0x00023A62
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06002FB8 RID: 12216 RVA: 0x0013C114 File Offset: 0x0013A314
		public static float BackgroundDarkAlphaForText()
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			float num = GenCelestial.CurCelestialSunGlow(Find.CurrentMap);
			float num2 = (Find.CurrentMap.Biome == BiomeDefOf.IceSheet) ? 1f : Mathf.Clamp01(Find.CurrentMap.snowGrid.TotalDepth / 1000f);
			return num * num2 * 0.41f;
		}

		// Token: 0x06002FB9 RID: 12217 RVA: 0x0013C174 File Offset: 0x0013A374
		public static void DrawTextWinterShadow(Rect rect)
		{
			float num = GenUI.BackgroundDarkAlphaForText();
			if (num > 0.001f)
			{
				GUI.color = new Color(1f, 1f, 1f, num);
				GUI.DrawTexture(rect, GenUI.UnderShadowTex);
				GUI.color = Color.white;
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x0013C1C0 File Offset: 0x0013A3C0
		public static void DrawTextureWithMaterial(Rect rect, Texture texture, Material material, Rect texCoords = default(Rect))
		{
			if (texCoords == default(Rect))
			{
				if (material == null)
				{
					GUI.DrawTexture(rect, texture);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, 0.5f), material);
					return;
				}
			}
			else
			{
				if (material == null)
				{
					GUI.DrawTextureWithTexCoords(rect, texture, texCoords);
					return;
				}
				if (Event.current.type == EventType.Repaint)
				{
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, 0.5f), material);
				}
			}
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x0013C2C8 File Offset: 0x0013A4C8
		public static float IconDrawScale(ThingDef tDef)
		{
			float num = tDef.uiIconScale;
			if (tDef.uiIconPath.NullOrEmpty() && tDef.graphicData != null)
			{
				IntVec2 intVec = (!tDef.defaultPlacingRot.IsHorizontal) ? tDef.Size : tDef.Size.Rotated();
				num *= Mathf.Min(tDef.graphicData.drawSize.x / (float)intVec.x, tDef.graphicData.drawSize.y / (float)intVec.z);
			}
			return num;
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x0013C350 File Offset: 0x0013A550
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x0013C384 File Offset: 0x0013A584
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x0013C3D0 File Offset: 0x0013A5D0
		public static float GetWidthCached(this string s)
		{
			if (GenUI.labelWidthCache.Count > 2000 || (Time.frameCount % 40000 == 0 && GenUI.labelWidthCache.Count > 100))
			{
				GenUI.labelWidthCache.Clear();
			}
			s = s.StripTags();
			float x;
			if (GenUI.labelWidthCache.TryGetValue(s, out x))
			{
				return x;
			}
			x = Text.CalcSize(s).x;
			GenUI.labelWidthCache.Add(s, x);
			return x;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x0002586A File Offset: 0x00023A6A
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x00025876 File Offset: 0x00023A76
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x06002FC1 RID: 12225 RVA: 0x000258A1 File Offset: 0x00023AA1
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x06002FC2 RID: 12226 RVA: 0x0013C448 File Offset: 0x0013A648
		public static float DistFromRect(Rect r, Vector2 p)
		{
			float num = Mathf.Abs(p.x - r.center.x) - r.width / 2f;
			if (num < 0f)
			{
				num = 0f;
			}
			float num2 = Mathf.Abs(p.y - r.center.y) - r.height / 2f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			return Mathf.Sqrt(num * num + num2 * num2);
		}

		// Token: 0x06002FC3 RID: 12227 RVA: 0x0013C4CC File Offset: 0x0013A6CC
		public static void DrawMouseAttachment(Texture iconTex, string text = "", float angle = 0f, Vector2 offset = default(Vector2), Rect? customRect = null, bool drawTextBackground = false, Color textBgColor = default(Color))
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float num = mousePosition.y + 12f;
			if (drawTextBackground && text != "")
			{
				Rect value;
				if (customRect != null)
				{
					value = customRect.Value;
				}
				else
				{
					Vector2 vector = Text.CalcSize(text);
					float num2 = (iconTex != null) ? 42f : 0f;
					value = new Rect(mousePosition.x + 12f - 4f, num + num2, Text.CalcSize(text).x + 8f, vector.y);
				}
				Widgets.DrawBoxSolid(value, textBgColor);
			}
			if (iconTex != null)
			{
				Rect mouseRect;
				if (customRect != null)
				{
					mouseRect = customRect.Value;
				}
				else
				{
					mouseRect = new Rect(mousePosition.x + 8f, num + 8f, 32f, 32f);
				}
				Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
				{
					Rect rect = mouseRect.AtZero();
					rect.position += new Vector2(offset.x * rect.size.x, offset.y * rect.size.y);
					Widgets.DrawTextureRotated(rect, iconTex, angle);
				}, false, false, 0f);
				num += mouseRect.height + 10f;
			}
			if (text != "")
			{
				Rect textRect = new Rect(mousePosition.x + 12f, num, 200f, 9999f);
				Find.WindowStack.ImmediateWindow(34003429, textRect, WindowLayer.Super, delegate
				{
					GameFont font = Text.Font;
					Text.Font = GameFont.Small;
					Widgets.Label(textRect.AtZero(), text);
					Text.Font = font;
				}, false, false, 0f);
			}
		}

		// Token: 0x06002FC4 RID: 12228 RVA: 0x0013C6BC File Offset: 0x0013A8BC
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f);
		}

		// Token: 0x06002FC5 RID: 12229 RVA: 0x0013C738 File Offset: 0x0013A938
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06002FC6 RID: 12230 RVA: 0x0013C76C File Offset: 0x0013A96C
		public static void DrawStatusLevel(Need status, Rect rect)
		{
			GUI.BeginGroup(rect);
			Widgets.Label(new Rect(0f, 2f, rect.width, 25f), status.LabelCap);
			Rect rect2 = new Rect(100f, 3f, GenUI.PieceBarSize.x, GenUI.PieceBarSize.y);
			Widgets.FillableBar(rect2, status.CurLevelPercentage);
			Widgets.FillableBarChangeArrows(rect2, status.GUIChangeArrow);
			GUI.EndGroup();
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, status.GetTipString());
			}
			if (Mouse.IsOver(rect))
			{
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
		}

		// Token: 0x06002FC7 RID: 12231 RVA: 0x000258B8 File Offset: 0x00023AB8
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAtMouse_NewTemp(clickParams, thingsOnly, null);
		}

		// Token: 0x06002FC8 RID: 12232 RVA: 0x000258C2 File Offset: 0x00023AC2
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse_NewTemp(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt_NewTemp(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x06002FC9 RID: 12233 RVA: 0x000258D1 File Offset: 0x00023AD1
		[Obsolete("Only need this overload to not break mod compatibility.")]
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false)
		{
			return GenUI.TargetsAt_NewTemp(clickPos, clickParams, thingsOnly, null);
		}

		// Token: 0x06002FCA RID: 12234 RVA: 0x000258DC File Offset: 0x00023ADC
		public static IEnumerable<LocalTargetInfo> TargetsAt_NewTemp(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams);
			Thing caster = (source != null) ? source.Caster : null;
			int num;
			for (int i = 0; i < clickableList.Count; i = num + 1)
			{
				Pawn pawn = clickableList[i] as Pawn;
				if (pawn == null || !pawn.IsInvisible() || caster == null || caster.Faction == pawn.Faction)
				{
					yield return clickableList[i];
				}
				num = i;
			}
			if (!thingsOnly)
			{
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(intVec, Find.CurrentMap, false)))
				{
					yield return intVec;
				}
			}
			yield break;
		}

		// Token: 0x06002FCB RID: 12235 RVA: 0x0013C814 File Offset: 0x0013AA14
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn))
				{
					GenUI.clickedPawns.Add(pawn);
				}
			}
			GenUI.clickedPawns.Sort(new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int j = 0; j < GenUI.clickedPawns.Count; j++)
			{
				list.Add(GenUI.clickedPawns[j]);
			}
			List<Thing> list2 = new List<Thing>();
			foreach (Thing thing in Find.CurrentMap.thingGrid.ThingsAt(c))
			{
				if (!list.Contains(thing) && clickParams.CanTarget(thing))
				{
					list2.Add(thing);
				}
			}
			List<Thing> list3 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list3.Count; k++)
			{
				Thing thing2 = list3[k];
				if (thing2.CustomRectForSelector != null && thing2.CustomRectForSelector.Value.Contains(c) && !list.Contains(thing2) && clickParams.CanTarget(thing2))
				{
					list2.Add(thing2);
				}
			}
			list2.Sort(new Comparison<Thing>(GenUI.CompareThingsByDrawAltitude));
			list.AddRange(list2);
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned2 = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int l = 0; l < allPawnsSpawned2.Count; l++)
			{
				Pawn pawn2 = allPawnsSpawned2[l];
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2))
				{
					GenUI.clickedPawns.Add(pawn2);
				}
			}
			GenUI.clickedPawns.Sort(new Comparison<Pawn>(GenUI.CompareThingsByDistanceToMousePointer));
			for (int m = 0; m < GenUI.clickedPawns.Count; m++)
			{
				if (!list.Contains(GenUI.clickedPawns[m]))
				{
					list.Add(GenUI.clickedPawns[m]);
				}
			}
			list.RemoveAll((Thing t) => !t.Spawned);
			GenUI.clickedPawns.Clear();
			return list;
		}

		// Token: 0x06002FCC RID: 12236 RVA: 0x0013CADC File Offset: 0x0013ACDC
		private static int CompareThingsByDistanceToMousePointer(Thing a, Thing b)
		{
			Vector3 b2 = UI.MouseMapPosition();
			float num = (a.DrawPos - b2).MagnitudeHorizontalSquared();
			float num2 = (b.DrawPos - b2).MagnitudeHorizontalSquared();
			if (num < num2)
			{
				return -1;
			}
			if (num == num2)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x06002FCD RID: 12237 RVA: 0x00025901 File Offset: 0x00023B01
		private static int CompareThingsByDrawAltitude(Thing A, Thing B)
		{
			if (A.def.Altitude < B.def.Altitude)
			{
				return 1;
			}
			if (A.def.Altitude == B.def.Altitude)
			{
				return 0;
			}
			return -1;
		}

		// Token: 0x06002FCE RID: 12238 RVA: 0x00025938 File Offset: 0x00023B38
		public static int CurrentAdjustmentMultiplier()
		{
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent && KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 1000;
			}
			if (KeyBindingDefOf.ModifierIncrement_100x.IsDownEvent)
			{
				return 100;
			}
			if (KeyBindingDefOf.ModifierIncrement_10x.IsDownEvent)
			{
				return 10;
			}
			return 1;
		}

		// Token: 0x06002FCF RID: 12239 RVA: 0x00025977 File Offset: 0x00023B77
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06002FD0 RID: 12240 RVA: 0x00025984 File Offset: 0x00023B84
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x000259BB File Offset: 0x00023BBB
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x06002FD2 RID: 12242 RVA: 0x0013CB20 File Offset: 0x0013AD20
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x06002FD3 RID: 12243 RVA: 0x000259F2 File Offset: 0x00023BF2
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x00025A2B File Offset: 0x00023C2B
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x00025A64 File Offset: 0x00023C64
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x00025A83 File Offset: 0x00023C83
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x00025AAE File Offset: 0x00023CAE
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x00025AD7 File Offset: 0x00023CD7
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06002FD9 RID: 12249 RVA: 0x00025AFC File Offset: 0x00023CFC
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x06002FDA RID: 12250 RVA: 0x00025B19 File Offset: 0x00023D19
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x06002FDB RID: 12251 RVA: 0x00025B50 File Offset: 0x00023D50
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x06002FDC RID: 12252 RVA: 0x00025B85 File Offset: 0x00023D85
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x00025BAC File Offset: 0x00023DAC
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06002FDE RID: 12254 RVA: 0x00025BD5 File Offset: 0x00023DD5
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06002FDF RID: 12255 RVA: 0x00025BFA File Offset: 0x00023DFA
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06002FE0 RID: 12256 RVA: 0x00025C17 File Offset: 0x00023E17
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x06002FE1 RID: 12257 RVA: 0x00025C4E File Offset: 0x00023E4E
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06002FE2 RID: 12258 RVA: 0x00025C83 File Offset: 0x00023E83
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x06002FE3 RID: 12259 RVA: 0x0013CB94 File Offset: 0x0013AD94
		public static Color LerpColor(List<Pair<float, Color>> colors, float value)
		{
			if (colors.Count == 0)
			{
				return Color.white;
			}
			int i = 0;
			while (i < colors.Count)
			{
				if (value < colors[i].First)
				{
					if (i == 0)
					{
						return colors[i].Second;
					}
					return Color.Lerp(colors[i - 1].Second, colors[i].Second, Mathf.InverseLerp(colors[i - 1].First, colors[i].First, value));
				}
				else
				{
					i++;
				}
			}
			return colors.Last<Pair<float, Color>>().Second;
		}

		// Token: 0x06002FE4 RID: 12260 RVA: 0x0013CC40 File Offset: 0x0013AE40
		public static Vector2 GetMouseAttachedWindowPos(float width, float height)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			float y;
			if (mousePosition.y + 14f + height < (float)UI.screenHeight)
			{
				y = mousePosition.y + 14f;
			}
			else if (mousePosition.y - 5f - height >= 0f)
			{
				y = mousePosition.y - 5f - height;
			}
			else
			{
				y = 0f;
			}
			float x;
			if (mousePosition.x + 16f + width < (float)UI.screenWidth)
			{
				x = mousePosition.x + 16f;
			}
			else
			{
				x = mousePosition.x - 4f - width;
			}
			return new Vector2(x, y);
		}

		// Token: 0x06002FE5 RID: 12261 RVA: 0x0013CCF0 File Offset: 0x0013AEF0
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x06002FE6 RID: 12262 RVA: 0x0013CD20 File Offset: 0x0013AF20
		public static void DrawArrowPointingAt(Rect rect)
		{
			Vector2 vector = new Vector2((float)UI.screenWidth, (float)UI.screenHeight) / 2f;
			float angle = Mathf.Atan2(rect.center.x - vector.x, vector.y - rect.center.y) * 57.29578f;
			Vector2 vector2 = new Bounds(rect.center, rect.size).ClosestPoint(vector);
			Rect position = new Rect(vector2 + Vector2.left * (float)GenUI.ArrowTex.width * 0.5f, new Vector2((float)GenUI.ArrowTex.width, (float)GenUI.ArrowTex.height));
			Matrix4x4 matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;
			Vector2 center = GUIUtility.GUIToScreenPoint(vector2);
			GUI.matrix = matrix;
			UI.RotateAroundPivot(angle, center);
			GUI.DrawTexture(position, GenUI.ArrowTex);
			GUI.matrix = matrix;
		}

		// Token: 0x06002FE7 RID: 12263 RVA: 0x0013CE2C File Offset: 0x0013B02C
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x06002FE8 RID: 12264 RVA: 0x0013CE90 File Offset: 0x0013B090
		public static Rect DrawElementStack<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float rowMargin = 4f, float elementMargin = 5f, bool allowOrderOptimization = true)
		{
			GenUI.tmpRects.Clear();
			GenUI.tmpRects2.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int num = Mathf.FloorToInt(rect.height / rowHeight);
			List<GenUI.StackedElementRect> list = GenUI.tmpRects;
			float num3;
			float num2;
			if (allowOrderOptimization)
			{
				num2 = (num3 = 0f);
				while (num2 < (float)num)
				{
					GenUI.StackedElementRect item = default(GenUI.StackedElementRect);
					int num4 = -1;
					for (int j = 0; j < list.Count; j++)
					{
						GenUI.StackedElementRect stackedElementRect = list[j];
						if (num4 == -1 || (item.rect.width < stackedElementRect.rect.width && stackedElementRect.rect.width < rect.width - num3))
						{
							num4 = j;
							item = stackedElementRect;
						}
					}
					if (num4 == -1)
					{
						if (num3 == 0f)
						{
							break;
						}
						num3 = 0f;
						num2 += 1f;
					}
					else
					{
						num3 += item.rect.width + elementMargin;
						GenUI.tmpRects2.Add(item);
					}
					list.RemoveAt(num4);
					if (list.Count <= 0)
					{
						break;
					}
				}
				list = GenUI.tmpRects2;
			}
			num2 = (num3 = 0f);
			while (list.Count > 0)
			{
				GenUI.StackedElementRect stackedElementRect2 = list[0];
				if (num3 + stackedElementRect2.rect.width > rect.width)
				{
					num3 = 0f;
					num2 += rowHeight + rowMargin;
				}
				drawer(new Rect(rect.x + num3, rect.y + num2, stackedElementRect2.rect.width, stackedElementRect2.rect.height), elements[stackedElementRect2.elementIndex]);
				num3 += stackedElementRect2.rect.width + elementMargin;
				list.RemoveAt(0);
			}
			return new Rect(rect.x, rect.y, rect.width, num2 + rowHeight);
		}

		// Token: 0x06002FE9 RID: 12265 RVA: 0x0013D0A0 File Offset: 0x0013B2A0
		public static Rect DrawElementStackVertical<T>(Rect rect, float rowHeight, List<T> elements, GenUI.StackElementDrawer<T> drawer, GenUI.StackElementWidthGetter<T> widthGetter, float elementMargin = 5f)
		{
			GenUI.tmpRects.Clear();
			for (int i = 0; i < elements.Count; i++)
			{
				GenUI.tmpRects.Add(new GenUI.StackedElementRect(new Rect(0f, 0f, widthGetter(elements[i]), rowHeight), i));
			}
			int elem = Mathf.FloorToInt(rect.height / rowHeight);
			GenUI.spacingCache.Reset(elem);
			int num = 0;
			float num2 = 0f;
			float num3 = 0f;
			for (int j = 0; j < GenUI.tmpRects.Count; j++)
			{
				GenUI.StackedElementRect stackedElementRect = GenUI.tmpRects[j];
				if (num3 + stackedElementRect.rect.height > rect.height)
				{
					num3 = 0f;
					num = 0;
				}
				drawer(new Rect(rect.x + GenUI.spacingCache.GetSpaceFor(num), rect.y + num3, stackedElementRect.rect.width, stackedElementRect.rect.height), elements[stackedElementRect.elementIndex]);
				num3 += stackedElementRect.rect.height + elementMargin;
				GenUI.spacingCache.AddSpace(num, stackedElementRect.rect.width + elementMargin);
				num2 = Mathf.Max(num2, GenUI.spacingCache.GetSpaceFor(num));
				num++;
			}
			return new Rect(rect.x, rect.y, num2, num3 + rowHeight);
		}

		// Token: 0x04002055 RID: 8277
		public const float Pad = 10f;

		// Token: 0x04002056 RID: 8278
		public const float GapTiny = 4f;

		// Token: 0x04002057 RID: 8279
		public const float GapSmall = 10f;

		// Token: 0x04002058 RID: 8280
		public const float Gap = 17f;

		// Token: 0x04002059 RID: 8281
		public const float GapWide = 26f;

		// Token: 0x0400205A RID: 8282
		public const float ListSpacing = 28f;

		// Token: 0x0400205B RID: 8283
		public const float MouseAttachIconSize = 32f;

		// Token: 0x0400205C RID: 8284
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x0400205D RID: 8285
		public const float ScrollBarWidth = 16f;

		// Token: 0x0400205E RID: 8286
		public const float HorizontalSliderHeight = 16f;

		// Token: 0x0400205F RID: 8287
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04002060 RID: 8288
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04002061 RID: 8289
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04002062 RID: 8290
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x04002063 RID: 8291
		public const float SmallIconSize = 24f;

		// Token: 0x04002064 RID: 8292
		public const int RootGUIDepth = 50;

		// Token: 0x04002065 RID: 8293
		private const float MouseIconSize = 32f;

		// Token: 0x04002066 RID: 8294
		private const float MouseIconOffset = 12f;

		// Token: 0x04002067 RID: 8295
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x04002068 RID: 8296
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x04002069 RID: 8297
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x0400206A RID: 8298
		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		// Token: 0x0400206B RID: 8299
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x0400206C RID: 8300
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x0400206D RID: 8301
		private static List<Pawn> clickedPawns = new List<Pawn>();

		// Token: 0x0400206E RID: 8302
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x0400206F RID: 8303
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x04002070 RID: 8304
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x04002071 RID: 8305
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x04002072 RID: 8306
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x02000767 RID: 1895
		private struct StackedElementRect
		{
			// Token: 0x06002FEB RID: 12267 RVA: 0x00025CAA File Offset: 0x00023EAA
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x04002073 RID: 8307
			public Rect rect;

			// Token: 0x04002074 RID: 8308
			public int elementIndex;
		}

		// Token: 0x02000768 RID: 1896
		public class AnonymousStackElement
		{
			// Token: 0x04002075 RID: 8309
			public Action<Rect> drawer;

			// Token: 0x04002076 RID: 8310
			public float width;
		}

		// Token: 0x02000769 RID: 1897
		private struct SpacingCache
		{
			// Token: 0x06002FED RID: 12269 RVA: 0x0013D300 File Offset: 0x0013B500
			public void Reset(int elem = 16)
			{
				if (this.spaces == null || this.maxElements != elem)
				{
					this.maxElements = elem;
					this.spaces = new float[this.maxElements];
					return;
				}
				for (int i = 0; i < this.maxElements; i++)
				{
					this.spaces[i] = 0f;
				}
			}

			// Token: 0x06002FEE RID: 12270 RVA: 0x00025CBA File Offset: 0x00023EBA
			public float GetSpaceFor(int elem)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					return this.spaces[elem];
				}
				return 0f;
			}

			// Token: 0x06002FEF RID: 12271 RVA: 0x00025CF0 File Offset: 0x00023EF0
			public void AddSpace(int elem, float space)
			{
				if (this.spaces == null || this.maxElements < 1)
				{
					this.Reset(16);
				}
				if (elem >= 0 && elem < this.maxElements)
				{
					this.spaces[elem] += space;
				}
			}

			// Token: 0x04002077 RID: 8311
			private int maxElements;

			// Token: 0x04002078 RID: 8312
			private float[] spaces;
		}

		// Token: 0x0200076A RID: 1898
		// (Invoke) Token: 0x06002FF1 RID: 12273
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x0200076B RID: 1899
		// (Invoke) Token: 0x06002FF5 RID: 12277
		public delegate float StackElementWidthGetter<T>(T element);
	}
}
