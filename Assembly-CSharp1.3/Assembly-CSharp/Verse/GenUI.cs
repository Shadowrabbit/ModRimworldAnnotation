using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200042F RID: 1071
	[StaticConstructorOnStartup]
	public static class GenUI
	{
		// Token: 0x06002028 RID: 8232 RVA: 0x000C7258 File Offset: 0x000C5458
		public static void SetLabelAlign(TextAnchor a)
		{
			Text.Anchor = a;
		}

		// Token: 0x06002029 RID: 8233 RVA: 0x000C7260 File Offset: 0x000C5460
		public static void ResetLabelAlign()
		{
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600202A RID: 8234 RVA: 0x000C7268 File Offset: 0x000C5468
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

		// Token: 0x0600202B RID: 8235 RVA: 0x000C72C8 File Offset: 0x000C54C8
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

		// Token: 0x0600202C RID: 8236 RVA: 0x000C7314 File Offset: 0x000C5514
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
					Graphics.DrawTexture(rect, texture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a * 0.5f), material);
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
					Graphics.DrawTexture(rect, texture, texCoords, 0, 0, 0, 0, new Color(GUI.color.r * 0.5f, GUI.color.g * 0.5f, GUI.color.b * 0.5f, GUI.color.a * 0.5f), material);
				}
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x000C7434 File Offset: 0x000C5634
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

		// Token: 0x0600202E RID: 8238 RVA: 0x000C74BC File Offset: 0x000C56BC
		public static void ErrorDialog(string message)
		{
			if (Find.WindowStack != null)
			{
				Find.WindowStack.Add(new Dialog_MessageBox(message, null, null, null, null, null, false, null, null));
			}
		}

		// Token: 0x0600202F RID: 8239 RVA: 0x000C74F0 File Offset: 0x000C56F0
		public static void DrawFlash(float centerX, float centerY, float size, float alpha, Color color)
		{
			Rect position = new Rect(centerX - size / 2f, centerY - size / 2f, size, size);
			Color color2 = color;
			color2.a = alpha;
			GUI.color = color2;
			GUI.DrawTexture(position, GenUI.UIFlash);
			GUI.color = Color.white;
		}

		// Token: 0x06002030 RID: 8240 RVA: 0x000C753C File Offset: 0x000C573C
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

		// Token: 0x06002031 RID: 8241 RVA: 0x000C75B1 File Offset: 0x000C57B1
		public static void ClearLabelWidthCache()
		{
			GenUI.labelWidthCache.Clear();
		}

		// Token: 0x06002032 RID: 8242 RVA: 0x000C75BD File Offset: 0x000C57BD
		public static Rect Rounded(this Rect r)
		{
			return new Rect((float)((int)r.x), (float)((int)r.y), (float)((int)r.width), (float)((int)r.height));
		}

		// Token: 0x06002033 RID: 8243 RVA: 0x000C75E8 File Offset: 0x000C57E8
		public static Rect RoundedCeil(this Rect r)
		{
			return new Rect((float)Mathf.CeilToInt(r.x), (float)Mathf.CeilToInt(r.y), (float)Mathf.CeilToInt(r.width), (float)Mathf.CeilToInt(r.height));
		}

		// Token: 0x06002034 RID: 8244 RVA: 0x000C7623 File Offset: 0x000C5823
		public static Vector2 Rounded(this Vector2 v)
		{
			return new Vector2((float)((int)v.x), (float)((int)v.y));
		}

		// Token: 0x06002035 RID: 8245 RVA: 0x000C763C File Offset: 0x000C583C
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

		// Token: 0x06002036 RID: 8246 RVA: 0x000C76C0 File Offset: 0x000C58C0
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
				}, false, false, 0f, null);
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
				}, false, false, 0f, null);
			}
		}

		// Token: 0x06002037 RID: 8247 RVA: 0x000C78B4 File Offset: 0x000C5AB4
		public static void DrawMouseAttachment(Texture2D icon)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect mouseRect = new Rect(mousePosition.x + 8f, mousePosition.y + 8f, 32f, 32f);
			Find.WindowStack.ImmediateWindow(34003428, mouseRect, WindowLayer.Super, delegate
			{
				GUI.DrawTexture(mouseRect.AtZero(), icon);
			}, false, false, 0f, null);
		}

		// Token: 0x06002038 RID: 8248 RVA: 0x000C7930 File Offset: 0x000C5B30
		public static void RenderMouseoverBracket()
		{
			Vector3 position = UI.MouseCell().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, GenUI.MouseoverBracketMaterial, 0);
		}

		// Token: 0x06002039 RID: 8249 RVA: 0x000C7964 File Offset: 0x000C5B64
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

		// Token: 0x0600203A RID: 8250 RVA: 0x000C7A09 File Offset: 0x000C5C09
		public static IEnumerable<LocalTargetInfo> TargetsAtMouse(TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			return GenUI.TargetsAt(UI.MouseMapPosition(), clickParams, thingsOnly, source);
		}

		// Token: 0x0600203B RID: 8251 RVA: 0x000C7A18 File Offset: 0x000C5C18
		public static IEnumerable<LocalTargetInfo> TargetsAt(Vector3 clickPos, TargetingParameters clickParams, bool thingsOnly = false, ITargetingSource source = null)
		{
			List<Thing> clickableList = GenUI.ThingsUnderMouse(clickPos, 0.8f, clickParams, source);
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
				if (intVec.InBounds(Find.CurrentMap) && clickParams.CanTarget(new TargetInfo(intVec, Find.CurrentMap, false), source))
				{
					yield return intVec;
				}
			}
			yield break;
		}

		// Token: 0x0600203C RID: 8252 RVA: 0x000C7A40 File Offset: 0x000C5C40
		public static List<Thing> ThingsUnderMouse(Vector3 clickPos, float pawnWideClickRadius, TargetingParameters clickParams, ITargetingSource source = null)
		{
			IntVec3 c = IntVec3.FromVector3(clickPos);
			List<Thing> list = new List<Thing>();
			GenUI.clickedPawns.Clear();
			List<Pawn> allPawnsSpawned = Find.CurrentMap.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((pawn.DrawPos - clickPos).MagnitudeHorizontal() < 0.4f && clickParams.CanTarget(pawn, source))
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
				if (!list.Contains(thing) && clickParams.CanTarget(thing, source))
				{
					list2.Add(thing);
				}
			}
			List<Thing> list3 = Find.CurrentMap.listerThings.ThingsInGroup(ThingRequestGroup.WithCustomRectForSelector);
			for (int k = 0; k < list3.Count; k++)
			{
				Thing thing2 = list3[k];
				if (thing2.CustomRectForSelector != null && thing2.CustomRectForSelector.Value.Contains(c) && !list.Contains(thing2) && clickParams.CanTarget(thing2, source))
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
				if ((pawn2.DrawPos - clickPos).MagnitudeHorizontal() < pawnWideClickRadius && clickParams.CanTarget(pawn2, source))
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

		// Token: 0x0600203D RID: 8253 RVA: 0x000C7D0C File Offset: 0x000C5F0C
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

		// Token: 0x0600203E RID: 8254 RVA: 0x000C7D50 File Offset: 0x000C5F50
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

		// Token: 0x0600203F RID: 8255 RVA: 0x000C7D87 File Offset: 0x000C5F87
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

		// Token: 0x06002040 RID: 8256 RVA: 0x000C7DC6 File Offset: 0x000C5FC6
		public static Rect GetInnerRect(this Rect rect)
		{
			return rect.ContractedBy(17f);
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000C7DD3 File Offset: 0x000C5FD3
		public static Rect ExpandedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x - margin, rect.y - margin, rect.width + margin * 2f, rect.height + margin * 2f);
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000C7E0A File Offset: 0x000C600A
		public static Rect ContractedBy(this Rect rect, float margin)
		{
			return new Rect(rect.x + margin, rect.y + margin, rect.width - margin * 2f, rect.height - margin * 2f);
		}

		// Token: 0x06002043 RID: 8259 RVA: 0x000C7E41 File Offset: 0x000C6041
		public static Rect ContractedBy(this Rect rect, float marginX, float marginY)
		{
			return new Rect(rect.x + marginX, rect.y + marginY, rect.width - marginX * 2f, rect.height - marginY * 2f);
		}

		// Token: 0x06002044 RID: 8260 RVA: 0x000C7E78 File Offset: 0x000C6078
		public static Rect ScaledBy(this Rect rect, float scale)
		{
			rect.x -= rect.width * (scale - 1f) / 2f;
			rect.y -= rect.height * (scale - 1f) / 2f;
			rect.width *= scale;
			rect.height *= scale;
			return rect;
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000C7EEA File Offset: 0x000C60EA
		public static Rect CenteredOnXIn(this Rect rect, Rect otherRect)
		{
			return new Rect(otherRect.x + (otherRect.width - rect.width) / 2f, rect.y, rect.width, rect.height);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000C7F23 File Offset: 0x000C6123
		public static Rect CenteredOnYIn(this Rect rect, Rect otherRect)
		{
			return new Rect(rect.x, otherRect.y + (otherRect.height - rect.height) / 2f, rect.width, rect.height);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x000C7F5C File Offset: 0x000C615C
		public static Rect AtZero(this Rect rect)
		{
			return new Rect(0f, 0f, rect.width, rect.height);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000C7F7C File Offset: 0x000C617C
		public static Rect Union(this Rect a, Rect b)
		{
			return new Rect
			{
				min = Vector2.Min(a.min, b.min),
				max = Vector2.Max(a.max, b.max)
			};
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000C7FC6 File Offset: 0x000C61C6
		public static void AbsorbClicksInRect(Rect r)
		{
			if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000C7FF1 File Offset: 0x000C61F1
		public static Rect LeftHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000C801A File Offset: 0x000C621A
		public static Rect LeftPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x000C803F File Offset: 0x000C623F
		public static Rect LeftPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x, rect.y, width, rect.height);
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000C805C File Offset: 0x000C625C
		public static Rect RightHalf(this Rect rect)
		{
			return new Rect(rect.x + rect.width / 2f, rect.y, rect.width / 2f, rect.height);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000C8093 File Offset: 0x000C6293
		public static Rect RightPart(this Rect rect, float pct)
		{
			return new Rect(rect.x + rect.width * (1f - pct), rect.y, rect.width * pct, rect.height);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000C80C8 File Offset: 0x000C62C8
		public static Rect RightPartPixels(this Rect rect, float width)
		{
			return new Rect(rect.x + rect.width - width, rect.y, width, rect.height);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000C80EF File Offset: 0x000C62EF
		public static Rect TopHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height / 2f);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000C8118 File Offset: 0x000C6318
		public static Rect TopPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y, rect.width, rect.height * pct);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000C813D File Offset: 0x000C633D
		public static Rect TopPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y, rect.width, height);
		}

		// Token: 0x06002053 RID: 8275 RVA: 0x000C815A File Offset: 0x000C635A
		public static Rect BottomHalf(this Rect rect)
		{
			return new Rect(rect.x, rect.y + rect.height / 2f, rect.width, rect.height / 2f);
		}

		// Token: 0x06002054 RID: 8276 RVA: 0x000C8191 File Offset: 0x000C6391
		public static Rect BottomPart(this Rect rect, float pct)
		{
			return new Rect(rect.x, rect.y + rect.height * (1f - pct), rect.width, rect.height * pct);
		}

		// Token: 0x06002055 RID: 8277 RVA: 0x000C81C6 File Offset: 0x000C63C6
		public static Rect BottomPartPixels(this Rect rect, float height)
		{
			return new Rect(rect.x, rect.y + rect.height - height, rect.width, height);
		}

		// Token: 0x06002056 RID: 8278 RVA: 0x000C81F0 File Offset: 0x000C63F0
		public static void SplitHorizontally(this Rect rect, float topHeight, out Rect top, out Rect bottom, float margin = 0f)
		{
			top = new Rect(rect.x, rect.y, rect.width, Mathf.Clamp(topHeight - margin, 0f, rect.height));
			bottom = new Rect(rect.x, Mathf.Clamp(rect.y + topHeight + margin, rect.y, rect.yMax), rect.width, Mathf.Clamp(rect.height - topHeight - margin, 0f, rect.height));
		}

		// Token: 0x06002057 RID: 8279 RVA: 0x000C8288 File Offset: 0x000C6488
		public static void SplitVertically(this Rect rect, float leftWidth, out Rect top, out Rect bottom, float margin = 0f)
		{
			top = new Rect(rect.x, rect.y, Mathf.Clamp(leftWidth - margin, 0f, rect.width), rect.height);
			bottom = new Rect(Mathf.Clamp(rect.x + leftWidth + margin, rect.x, rect.xMax), rect.y, Mathf.Clamp(rect.width - leftWidth - margin, 0f, rect.width), rect.height);
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000C8320 File Offset: 0x000C6520
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

		// Token: 0x06002059 RID: 8281 RVA: 0x000C83CC File Offset: 0x000C65CC
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
				y = (float)UI.screenHeight - (14f + height);
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

		// Token: 0x0600205A RID: 8282 RVA: 0x000C8488 File Offset: 0x000C6688
		public static float GetCenteredButtonPos(int buttonIndex, int buttonsCount, float totalWidth, float buttonWidth, float pad = 10f)
		{
			float num = (float)buttonsCount * buttonWidth + (float)(buttonsCount - 1) * pad;
			return Mathf.Floor((totalWidth - num) / 2f + (float)buttonIndex * (buttonWidth + pad));
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000C84B8 File Offset: 0x000C66B8
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

		// Token: 0x0600205C RID: 8284 RVA: 0x000C85C4 File Offset: 0x000C67C4
		public static void DrawArrowPointingAtWorldspace(Vector3 worldspace, Camera camera)
		{
			Vector3 vector = camera.WorldToScreenPoint(worldspace) / Prefs.UIScale;
			GenUI.DrawArrowPointingAt(new Rect(new Vector2(vector.x, (float)UI.screenHeight - vector.y) + new Vector2(-2f, 2f), new Vector2(4f, 4f)));
		}

		// Token: 0x0600205D RID: 8285 RVA: 0x000C8628 File Offset: 0x000C6828
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

		// Token: 0x0600205E RID: 8286 RVA: 0x000C8838 File Offset: 0x000C6A38
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

		// Token: 0x04001386 RID: 4998
		public const float Pad = 10f;

		// Token: 0x04001387 RID: 4999
		public const float GapTiny = 4f;

		// Token: 0x04001388 RID: 5000
		public const float GapSmall = 10f;

		// Token: 0x04001389 RID: 5001
		public const float Gap = 17f;

		// Token: 0x0400138A RID: 5002
		public const float GapWide = 26f;

		// Token: 0x0400138B RID: 5003
		public const float ListSpacing = 28f;

		// Token: 0x0400138C RID: 5004
		public const float MouseAttachIconSize = 32f;

		// Token: 0x0400138D RID: 5005
		public const float MouseAttachIconOffset = 8f;

		// Token: 0x0400138E RID: 5006
		public const float ScrollBarWidth = 16f;

		// Token: 0x0400138F RID: 5007
		public const float HorizontalSliderHeight = 16f;

		// Token: 0x04001390 RID: 5008
		public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

		// Token: 0x04001391 RID: 5009
		public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

		// Token: 0x04001392 RID: 5010
		public static readonly Color SubtleMouseoverColor = new Color(0.7f, 0.7f, 0.7f);

		// Token: 0x04001393 RID: 5011
		public static readonly Vector2 MaxWinSize = new Vector2(1010f, 754f);

		// Token: 0x04001394 RID: 5012
		public const float SmallIconSize = 24f;

		// Token: 0x04001395 RID: 5013
		public const int RootGUIDepth = 50;

		// Token: 0x04001396 RID: 5014
		private const float MouseIconSize = 32f;

		// Token: 0x04001397 RID: 5015
		private const float MouseIconOffset = 12f;

		// Token: 0x04001398 RID: 5016
		private static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", ShaderDatabase.MetaOverlay);

		// Token: 0x04001399 RID: 5017
		private static readonly Texture2D UnderShadowTex = ContentFinder<Texture2D>.Get("UI/Misc/ScreenCornerShadow", true);

		// Token: 0x0400139A RID: 5018
		private static readonly Texture2D UIFlash = ContentFinder<Texture2D>.Get("UI/Misc/Flash", true);

		// Token: 0x0400139B RID: 5019
		private static Dictionary<string, float> labelWidthCache = new Dictionary<string, float>();

		// Token: 0x0400139C RID: 5020
		private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

		// Token: 0x0400139D RID: 5021
		public const float PawnDirectClickRadius = 0.4f;

		// Token: 0x0400139E RID: 5022
		private static List<Pawn> clickedPawns = new List<Pawn>();

		// Token: 0x0400139F RID: 5023
		private static readonly Texture2D ArrowTex = ContentFinder<Texture2D>.Get("UI/Overlays/Arrow", true);

		// Token: 0x040013A0 RID: 5024
		private static List<GenUI.StackedElementRect> tmpRects = new List<GenUI.StackedElementRect>();

		// Token: 0x040013A1 RID: 5025
		private static List<GenUI.StackedElementRect> tmpRects2 = new List<GenUI.StackedElementRect>();

		// Token: 0x040013A2 RID: 5026
		public const float ElementStackDefaultElementMargin = 5f;

		// Token: 0x040013A3 RID: 5027
		private static GenUI.SpacingCache spacingCache;

		// Token: 0x02001C5E RID: 7262
		private struct StackedElementRect
		{
			// Token: 0x0600A709 RID: 42761 RVA: 0x00382E18 File Offset: 0x00381018
			public StackedElementRect(Rect rect, int elementIndex)
			{
				this.rect = rect;
				this.elementIndex = elementIndex;
			}

			// Token: 0x04006DA6 RID: 28070
			public Rect rect;

			// Token: 0x04006DA7 RID: 28071
			public int elementIndex;
		}

		// Token: 0x02001C5F RID: 7263
		public class AnonymousStackElement
		{
			// Token: 0x04006DA8 RID: 28072
			public Action<Rect> drawer;

			// Token: 0x04006DA9 RID: 28073
			public float width;
		}

		// Token: 0x02001C60 RID: 7264
		private struct SpacingCache
		{
			// Token: 0x0600A70B RID: 42763 RVA: 0x00382E28 File Offset: 0x00381028
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

			// Token: 0x0600A70C RID: 42764 RVA: 0x00382E7D File Offset: 0x0038107D
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

			// Token: 0x0600A70D RID: 42765 RVA: 0x00382EB3 File Offset: 0x003810B3
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

			// Token: 0x04006DAA RID: 28074
			private int maxElements;

			// Token: 0x04006DAB RID: 28075
			private float[] spaces;
		}

		// Token: 0x02001C61 RID: 7265
		// (Invoke) Token: 0x0600A70F RID: 42767
		public delegate void StackElementDrawer<T>(Rect rect, T element);

		// Token: 0x02001C62 RID: 7266
		// (Invoke) Token: 0x0600A713 RID: 42771
		public delegate float StackElementWidthGetter<T>(T element);
	}
}
