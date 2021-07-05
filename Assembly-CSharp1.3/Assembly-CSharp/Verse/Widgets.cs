using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000443 RID: 1091
	[StaticConstructorOnStartup]
	public static class Widgets
	{
		// Token: 0x060020A6 RID: 8358 RVA: 0x000CAF50 File Offset: 0x000C9150
		static Widgets()
		{
			Color color = new Color(1f, 1f, 1f, 0f);
			Widgets.LineTexAA = new Texture2D(1, 3, TextureFormat.ARGB32, false);
			Widgets.LineTexAA.name = "LineTexAA";
			Widgets.LineTexAA.SetPixel(0, 0, color);
			Widgets.LineTexAA.SetPixel(0, 1, Color.white);
			Widgets.LineTexAA.SetPixel(0, 2, color);
			Widgets.LineTexAA.Apply();
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000CB3AC File Offset: 0x000C95AC
		public static bool CanDrawIconFor(Def def)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				return buildableDef.uiIcon != null;
			}
			FactionDef factionDef;
			return (factionDef = (def as FactionDef)) != null && factionDef.FactionIcon != null;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000CB3E8 File Offset: 0x000C95E8
		public static void DefIcon(Rect rect, Def def, ThingDef stuffDef = null, float scale = 1f, ThingStyleDef thingStyleDef = null, bool drawPlaceholder = false, Color? color = null)
		{
			BuildableDef buildableDef;
			if ((buildableDef = (def as BuildableDef)) != null)
			{
				rect.position += new Vector2(buildableDef.uiIconOffset.x * rect.size.x, buildableDef.uiIconOffset.y * rect.size.y);
			}
			ThingDef thingDef;
			if ((thingDef = (def as ThingDef)) != null)
			{
				Widgets.ThingIcon(rect, thingDef, stuffDef, thingStyleDef, scale, color);
				return;
			}
			PawnKindDef pawnKindDef;
			if ((pawnKindDef = (def as PawnKindDef)) != null)
			{
				Widgets.ThingIcon(rect, pawnKindDef.race, stuffDef, thingStyleDef, scale, color);
				return;
			}
			RecipeDef recipeDef;
			if ((recipeDef = (def as RecipeDef)) != null && recipeDef.UIIconThing != null)
			{
				Widgets.ThingIcon(rect, recipeDef.UIIconThing, null, null, scale, color);
				return;
			}
			TerrainDef terrainDef;
			if ((terrainDef = (def as TerrainDef)) != null && terrainDef.uiIcon != null)
			{
				GUI.color = terrainDef.uiIconColor;
				Widgets.DrawTextureFitted(rect, terrainDef.uiIcon, scale, Vector2.one, Widgets.CroppedTerrainTextureRect(terrainDef.uiIcon), 0f, null);
				GUI.color = Color.white;
				return;
			}
			FactionDef factionDef;
			if ((factionDef = (def as FactionDef)) != null)
			{
				if (!factionDef.colorSpectrum.NullOrEmpty<Color>())
				{
					GUI.color = factionDef.colorSpectrum.FirstOrDefault<Color>();
				}
				Widgets.DrawTextureFitted(rect, factionDef.FactionIcon, scale);
				GUI.color = Color.white;
				return;
			}
			StyleItemDef styleItemDef;
			if ((styleItemDef = (def as StyleItemDef)) != null)
			{
				Widgets.DrawTextureFitted(rect, styleItemDef.Icon, scale);
				return;
			}
			if (drawPlaceholder)
			{
				Widgets.DrawTextureFitted(rect, Widgets.PlaceholderIconTex, scale);
			}
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000CB564 File Offset: 0x000C9764
		public static void ThingIcon(Rect rect, Thing thing, float alpha = 1f, Rot4? rot = null)
		{
			thing = thing.GetInnerIfMinified();
			GUI.color = thing.DrawColor;
			float resolvedIconAngle = 0f;
			ThingStyleDef styleDef = thing.StyleDef;
			Texture resolvedIcon;
			if (styleDef != null && styleDef.UIIcon != null)
			{
				resolvedIcon = styleDef.UIIcon;
				resolvedIconAngle = thing.def.uiIconAngle;
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			else if (!thing.def.uiIconPath.NullOrEmpty())
			{
				resolvedIcon = thing.def.uiIcon;
				resolvedIconAngle = thing.def.uiIconAngle;
				rect.position += new Vector2(thing.def.uiIconOffset.x * rect.size.x, thing.def.uiIconOffset.y * rect.size.y);
			}
			else if (thing is Pawn || thing is Corpse)
			{
				Pawn pawn = thing as Pawn;
				if (pawn == null)
				{
					pawn = ((Corpse)thing).InnerPawn;
				}
				if (!pawn.RaceProps.Humanlike)
				{
					if (!pawn.Drawer.renderer.graphics.AllResolved)
					{
						pawn.Drawer.renderer.graphics.ResolveAllGraphics();
					}
					Material material = pawn.Drawer.renderer.graphics.nakedGraphic.MatAt(rot ?? Rot4.East, null);
					resolvedIcon = material.mainTexture;
					GUI.color = material.color;
				}
				else
				{
					rect = rect.ScaledBy(1.8f);
					rect.y += 3f;
					rect = rect.Rounded();
					resolvedIcon = PortraitsCache.Get(pawn, new Vector2(rect.width, rect.height), rot ?? Rot4.South, default(Vector3), 1f, true, true, true, true, null, false);
				}
			}
			else
			{
				resolvedIcon = thing.Graphic.ExtractInnerGraphicFor(thing).MatAt(thing.def.defaultPlacingRot, null).mainTexture;
			}
			if (alpha != 1f)
			{
				Color color = GUI.color;
				color.a *= alpha;
				GUI.color = color;
			}
			Widgets.ThingIconWorker(rect, thing.def, resolvedIcon, resolvedIconAngle, 1f);
			GUI.color = Color.white;
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x000CB810 File Offset: 0x000C9A10
		public static void ThingIcon(Rect rect, ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null, float scale = 1f, Color? color = null)
		{
			if (thingDef.uiIcon == null || thingDef.uiIcon == BaseContent.BadTex)
			{
				return;
			}
			Texture2D iconFor = Widgets.GetIconFor(thingDef, stuffDef, thingStyleDef);
			if (color != null)
			{
				GUI.color = color.Value;
			}
			else if (stuffDef != null)
			{
				GUI.color = thingDef.GetColorForStuff(stuffDef);
			}
			else
			{
				GUI.color = (thingDef.MadeFromStuff ? thingDef.GetColorForStuff(GenStuff.DefaultStuffFor(thingDef)) : thingDef.uiIconColor);
			}
			Widgets.ThingIconWorker(rect, thingDef, iconFor, thingDef.uiIconAngle, scale);
			GUI.color = Color.white;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000CB8AC File Offset: 0x000C9AAC
		public static Texture2D GetIconFor(ThingDef thingDef, ThingDef stuffDef = null, ThingStyleDef thingStyleDef = null)
		{
			Texture2D result = thingDef.uiIcon;
			Graphic_Appearances graphic_Appearances;
			if (thingStyleDef != null && thingStyleDef.UIIcon != null)
			{
				result = thingStyleDef.UIIcon;
			}
			else if ((graphic_Appearances = (thingDef.graphic as Graphic_Appearances)) != null)
			{
				result = (Texture2D)graphic_Appearances.SubGraphicFor(stuffDef ?? GenStuff.DefaultStuffFor(thingDef)).MatAt(thingDef.defaultPlacingRot, null).mainTexture;
			}
			return result;
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000CB914 File Offset: 0x000C9B14
		private static void ThingIconWorker(Rect rect, ThingDef thingDef, Texture resolvedIcon, float resolvedIconAngle, float scale = 1f)
		{
			Vector2 texProportions = new Vector2((float)resolvedIcon.width, (float)resolvedIcon.height);
			Rect texCoords = Widgets.DefaultTexCoords;
			if (thingDef.graphicData != null)
			{
				texProportions = thingDef.graphicData.drawSize.RotatedBy(thingDef.defaultPlacingRot);
				if (thingDef.uiIconPath.NullOrEmpty() && thingDef.graphicData.linkFlags != LinkFlags.None)
				{
					texCoords = Widgets.LinkedTexCoords;
				}
			}
			Widgets.DrawTextureFitted(rect, resolvedIcon, GenUI.IconDrawScale(thingDef) * scale, texProportions, texCoords, resolvedIconAngle, null);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x000CB98F File Offset: 0x000C9B8F
		public static Rect CroppedTerrainTextureRect(Texture2D tex)
		{
			return new Rect(0f, 0f, 64f / (float)tex.width, 64f / (float)tex.height);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000CB9BA File Offset: 0x000C9BBA
		public static void DrawAltRect(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.AltTexture);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000CB9C8 File Offset: 0x000C9BC8
		public static void ListSeparator(ref float curY, float width, string label)
		{
			Color color = GUI.color;
			curY += 3f;
			GUI.color = Widgets.SeparatorLabelColor;
			Rect rect = new Rect(0f, curY, width, 30f);
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.Label(rect, label);
			curY += 20f;
			GUI.color = Widgets.SeparatorLineColor;
			Widgets.DrawLineHorizontal(0f, curY, width);
			curY += 2f;
			GUI.color = color;
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000CBA3C File Offset: 0x000C9C3C
		public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
		{
			float num = end.x - start.x;
			float num2 = end.y - start.y;
			float num3 = Mathf.Sqrt(num * num + num2 * num2);
			if (num3 < 0.01f)
			{
				return;
			}
			width *= 3f;
			float num4 = width * num2 / num3;
			float num5 = width * num / num3;
			float z = -Mathf.Atan2(-num2, num) * 57.29578f;
			Vector2 vector = start + new Vector2(0.5f * num4, -0.5f * num5);
			Matrix4x4 m = Matrix4x4.TRS(vector, Quaternion.Euler(0f, 0f, z), Vector3.one) * Matrix4x4.TRS(-vector, Quaternion.identity, Vector3.one);
			Rect position = new Rect(start.x, start.y - 0.5f * num5, num3, width);
			GL.PushMatrix();
			GL.MultMatrix(m);
			GUI.DrawTexture(position, Widgets.LineTexAA, ScaleMode.StretchToFill, true, 0f, color, 0f, 0f);
			GL.PopMatrix();
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000CBB4B File Offset: 0x000C9D4B
		public static void DrawLineHorizontal(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, length, 1f), BaseContent.WhiteTex);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x000CBB64 File Offset: 0x000C9D64
		public static void DrawLineVertical(float x, float y, float length)
		{
			GUI.DrawTexture(new Rect(x, y, 1f, length), BaseContent.WhiteTex);
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000CBB7D File Offset: 0x000C9D7D
		public static void DrawBoxSolid(Rect rect, Color color)
		{
			Color color2 = GUI.color;
			GUI.color = color;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = color2;
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000CBB9A File Offset: 0x000C9D9A
		public static void DrawBoxSolidWithOutline(Rect rect, Color solidColor, Color outlineColor, int outlineThickness = 1)
		{
			Widgets.DrawBoxSolid(rect, solidColor);
			Color color = GUI.color;
			GUI.color = outlineColor;
			Widgets.DrawBox(rect, outlineThickness, null);
			GUI.color = color;
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000CBBBC File Offset: 0x000C9DBC
		public static void DrawBox(Rect rect, int thickness = 1, Texture2D lineTexture = null)
		{
			Vector2 vector = new Vector2(rect.x, rect.y);
			Vector2 vector2 = new Vector2(rect.x + rect.width, rect.y + rect.height);
			if (vector.x > vector2.x)
			{
				float x = vector.x;
				vector.x = vector2.x;
				vector2.x = x;
			}
			if (vector.y > vector2.y)
			{
				float y = vector.y;
				vector.y = vector2.y;
				vector2.y = y;
			}
			Vector3 vector3 = vector2 - vector;
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector2.x - (float)thickness, vector.y, (float)thickness, vector3.y)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector.y, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
			GUI.DrawTexture(Widgets.AdjustRectToUIScaling(new Rect(vector.x + (float)thickness, vector2.y - (float)thickness, vector3.x - (float)(thickness * 2), (float)thickness)), lineTexture ?? BaseContent.WhiteTex);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000CBD30 File Offset: 0x000C9F30
		public static void LabelCacheHeight(ref Rect rect, string label, bool renderLabel = true, bool forceInvalidation = false)
		{
			bool flag = Widgets.LabelCache.ContainsKey(label);
			if (forceInvalidation)
			{
				flag = false;
			}
			float height;
			if (flag)
			{
				height = Widgets.LabelCache[label];
			}
			else
			{
				height = Text.CalcHeight(label, rect.width);
			}
			rect.height = height;
			if (renderLabel)
			{
				Widgets.Label(rect, label);
			}
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000CBD88 File Offset: 0x000C9F88
		public static void Label(Rect rect, GUIContent content)
		{
			GUI.Label(rect, content, Text.CurFontStyle);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000CBD98 File Offset: 0x000C9F98
		public static void Label(Rect rect, string label)
		{
			Rect position = rect;
			float num = Prefs.UIScale / 2f;
			if (Prefs.UIScale > 1f && Math.Abs(num - Mathf.Floor(num)) > 1E-45f)
			{
				position.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
				position.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
				position.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax + 1E-05f);
				position.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax + 1E-05f);
			}
			GUI.Label(position, label, Text.CurFontStyle);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x000CBE37 File Offset: 0x000CA037
		public static void Label(Rect rect, TaggedString label)
		{
			Widgets.Label(rect, label.Resolve());
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x000CBE48 File Offset: 0x000CA048
		public static void Label(float x, ref float curY, float width, string text, TipSignal tip = default(TipSignal))
		{
			if (text.NullOrEmpty())
			{
				return;
			}
			float num = Text.CalcHeight(text, width);
			Rect rect = new Rect(x, curY, width, num);
			if (!tip.text.NullOrEmpty() || tip.textGetter != null)
			{
				float x2 = Text.CalcSize(text).x;
				Rect rect2 = new Rect(rect.x, rect.y, x2, num);
				Widgets.DrawHighlightIfMouseover(rect2);
				TooltipHandler.TipRegion(rect2, tip);
			}
			Widgets.Label(rect, text);
			curY += num;
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x000CBEC4 File Offset: 0x000CA0C4
		public static void LongLabel(float x, float width, string label, ref float curY, bool draw = true)
		{
			if (label.Length < 2500)
			{
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 1000f), label);
				}
				curY += Text.CalcHeight(label, width);
				return;
			}
			int num = 0;
			int num2 = -1;
			bool flag = false;
			for (int i = 0; i < label.Length; i++)
			{
				if (label[i] == '\n')
				{
					num++;
					if (num >= 50)
					{
						string text = label.Substring(num2 + 1, i - num2 - 1);
						num2 = i;
						num = 0;
						if (flag)
						{
							curY += Text.SpaceBetweenLines;
						}
						if (draw)
						{
							Widgets.Label(new Rect(x, curY, width, 10000f), text);
						}
						curY += Text.CalcHeight(text, width);
						flag = true;
					}
				}
			}
			if (num2 != label.Length - 1)
			{
				if (flag)
				{
					curY += Text.SpaceBetweenLines;
				}
				string text2 = label.Substring(num2 + 1);
				if (draw)
				{
					Widgets.Label(new Rect(x, curY, width, 10000f), text2);
				}
				curY += Text.CalcHeight(text2, width);
			}
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x000CBFC4 File Offset: 0x000CA1C4
		public static void LabelScrollable(Rect rect, string label, ref Vector2 scrollbarPosition, bool dontConsumeScrollEventsIfNoScrollbar = false, bool takeScrollbarSpaceEvenIfNoScrollbar = true, bool longLabel = false)
		{
			bool flag = takeScrollbarSpaceEvenIfNoScrollbar || Text.CalcHeight(label, rect.width) > rect.height;
			bool flag2 = flag && (!dontConsumeScrollEventsIfNoScrollbar || Text.CalcHeight(label, rect.width - 16f) > rect.height);
			float num = rect.width;
			if (flag)
			{
				num -= 16f;
			}
			float num2;
			if (longLabel)
			{
				num2 = 0f;
				Widgets.LongLabel(0f, num, label, ref num2, false);
			}
			else
			{
				num2 = Text.CalcHeight(label, num);
			}
			Rect rect2 = new Rect(0f, 0f, num, Mathf.Max(num2 + 5f, rect.height));
			if (flag2)
			{
				Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			}
			else
			{
				GUI.BeginGroup(rect);
			}
			if (longLabel)
			{
				float y = rect2.y;
				Widgets.LongLabel(rect2.x, rect2.width, label, ref y, true);
			}
			else
			{
				Widgets.Label(rect2, label);
			}
			if (flag2)
			{
				Widgets.EndScrollView();
				return;
			}
			GUI.EndGroup();
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x000CC0C0 File Offset: 0x000CA2C0
		public static void DefLabelWithIcon(Rect rect, Def def, float iconMargin = 2f, float textOffsetX = 6f)
		{
			Widgets.DrawHighlightIfMouseover(rect);
			TooltipHandler.TipRegion(rect, def.description);
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			Widgets.DefIcon(rect2, def, null, 1f, null, true, null);
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.Label(rect3, def.LabelCap);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.EndGroup();
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000CC17C File Offset: 0x000CA37C
		public static bool LabelFit(Rect rect, string label)
		{
			bool result = false;
			GameFont font = Text.Font;
			Text.Font = GameFont.Small;
			if (Text.CalcSize(label).x <= rect.width)
			{
				Widgets.Label(rect, label);
			}
			else
			{
				Text.Font = GameFont.Tiny;
				if (Text.CalcSize(label).x <= rect.width)
				{
					Widgets.Label(rect, label);
				}
				else
				{
					Widgets.Label(rect, label.Truncate(rect.width, null));
					result = true;
				}
				Text.Font = GameFont.Small;
			}
			Text.Font = font;
			return result;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000CC1F8 File Offset: 0x000CA3F8
		public static void HyperlinkWithIcon(Rect rect, Dialog_InfoCard.Hyperlink hyperlink, string text = null, float iconMargin = 2f, float textOffsetX = 6f, Color? color = null, bool truncateLabel = false, string textSuffix = null)
		{
			string text2 = text ?? hyperlink.Label.CapitalizeFirst();
			if (textSuffix != null)
			{
				text2 += textSuffix;
			}
			GUI.BeginGroup(rect);
			Rect rect2 = new Rect(0f, 0f, rect.height, rect.height);
			if (iconMargin != 0f)
			{
				rect2 = rect2.ContractedBy(iconMargin);
			}
			if (hyperlink.thing != null)
			{
				Widgets.ThingIcon(rect2, hyperlink.thing, 1f, null);
			}
			else
			{
				Widgets.DefIcon(rect2, hyperlink.def, null, 1f, null, true, null);
			}
			float num = rect2.xMax + textOffsetX;
			Rect rect3 = new Rect(rect2.xMax + textOffsetX, 0f, rect.width - num, rect.height);
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Widgets.ButtonText(rect3, truncateLabel ? text2.Truncate(rect3.width, null) : text2, false, false, color ?? Widgets.NormalOptionColor, false);
			if (Widgets.ButtonInvisible(rect3, true))
			{
				hyperlink.ActivateHyperlink();
			}
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.EndGroup();
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000CC334 File Offset: 0x000CA534
		public static void DrawNumberOnMap(Vector2 screenPos, int number, Color textColor)
		{
			Text.Anchor = TextAnchor.MiddleCenter;
			Text.Font = GameFont.Medium;
			Rect rect = new Rect(screenPos.x - 20f, screenPos.y - 15f, 40f, 30f);
			GUI.DrawTexture(rect, TexUI.GrayBg);
			GUI.color = textColor;
			Widgets.Label(rect, number.ToStringCached());
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000CC3A6 File Offset: 0x000CA5A6
		public static void Checkbox(Vector2 topLeft, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Widgets.Checkbox(topLeft.x, topLeft.y, ref checkOn, size, disabled, paintable, texChecked, texUnchecked);
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x000CC3C4 File Offset: 0x000CA5C4
		public static void Checkbox(float x, float y, ref bool checkOn, float size = 24f, bool disabled = false, bool paintable = false, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Rect rect = new Rect(x, y, size, size);
			Widgets.CheckboxDraw(x, y, checkOn, disabled, size, texChecked, texUnchecked);
			if (!disabled)
			{
				MouseoverSounds.DoRegion(rect);
				bool flag = false;
				Widgets.DraggableResult draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
				if (draggableResult == Widgets.DraggableResult.Pressed)
				{
					checkOn = !checkOn;
					flag = true;
				}
				else if (draggableResult == Widgets.DraggableResult.Dragged && paintable)
				{
					checkOn = !checkOn;
					flag = true;
					Widgets.checkboxPainting = true;
					Widgets.checkboxPaintingState = checkOn;
				}
				if (paintable && Mouse.IsOver(rect) && Widgets.checkboxPainting && Input.GetMouseButton(0) && checkOn != Widgets.checkboxPaintingState)
				{
					checkOn = Widgets.checkboxPaintingState;
					flag = true;
				}
				if (flag)
				{
					if (checkOn)
					{
						SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
					}
					else
					{
						SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
					}
				}
			}
			if (disabled)
			{
				GUI.color = Color.white;
			}
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x000CC49C File Offset: 0x000CA69C
		public static void CheckboxLabeled(Rect rect, string label, ref bool checkOn, bool disabled = false, Texture2D texChecked = null, Texture2D texUnchecked = null, bool placeCheckboxNearText = false)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (placeCheckboxNearText)
			{
				rect.width = Mathf.Min(rect.width, Text.CalcSize(label).x + 24f + 10f);
			}
			Widgets.Label(rect, label);
			if (!disabled && Widgets.ButtonInvisible(rect, true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			Widgets.CheckboxDraw(rect.x + rect.width - 24f, rect.y, checkOn, disabled, 24f, null, null);
			Text.Anchor = anchor;
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x000CC548 File Offset: 0x000CA748
		public static bool CheckboxLabeledSelectable(Rect rect, string label, ref bool selected, ref bool checkOn, Texture2D labelIcon = null, float labelIconScale = 1f)
		{
			if (selected)
			{
				Widgets.DrawHighlight(rect);
			}
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			if (labelIcon != null)
			{
				Rect outerRect = new Rect(rect.x, rect.y, (float)labelIcon.width, rect.height);
				rect.xMin += (float)labelIcon.width;
				Widgets.DrawTextureFitted(outerRect, labelIcon, labelIconScale);
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			bool flag = selected;
			Rect butRect = rect;
			butRect.width -= 24f;
			if (!selected && Widgets.ButtonInvisible(butRect, true))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				selected = true;
			}
			Color color = GUI.color;
			GUI.color = Color.white;
			Widgets.CheckboxDraw(rect.xMax - 24f, rect.y, checkOn, false, 24f, null, null);
			GUI.color = color;
			if (Widgets.ButtonInvisible(new Rect(rect.xMax - 24f, rect.y, 24f, 24f), true))
			{
				checkOn = !checkOn;
				if (checkOn)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
			}
			return selected && !flag;
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x000CC684 File Offset: 0x000CA884
		private static void CheckboxDraw(float x, float y, bool active, bool disabled, float size = 24f, Texture2D texChecked = null, Texture2D texUnchecked = null)
		{
			Color color = GUI.color;
			if (disabled)
			{
				GUI.color = Widgets.InactiveColor;
			}
			Texture2D image;
			if (active)
			{
				image = ((texChecked != null) ? texChecked : Widgets.CheckboxOnTex);
			}
			else
			{
				image = ((texUnchecked != null) ? texUnchecked : Widgets.CheckboxOffTex);
			}
			GUI.DrawTexture(new Rect(x, y, size, size), image);
			if (disabled)
			{
				GUI.color = color;
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x000CC6EC File Offset: 0x000CA8EC
		public static MultiCheckboxState CheckboxMulti(Rect rect, MultiCheckboxState state, bool paintable = false)
		{
			Texture2D tex;
			if (state == MultiCheckboxState.On)
			{
				tex = Widgets.CheckboxOnTex;
			}
			else if (state == MultiCheckboxState.Off)
			{
				tex = Widgets.CheckboxOffTex;
			}
			else
			{
				tex = Widgets.CheckboxPartialTex;
			}
			MouseoverSounds.DoRegion(rect);
			MultiCheckboxState multiCheckboxState = (state == MultiCheckboxState.Off) ? MultiCheckboxState.On : MultiCheckboxState.Off;
			bool flag = false;
			Widgets.DraggableResult draggableResult = Widgets.ButtonImageDraggable(rect, tex);
			if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
			{
				Widgets.checkboxPainting = true;
				Widgets.checkboxPaintingState = (multiCheckboxState == MultiCheckboxState.On);
				flag = true;
			}
			else if (draggableResult.AnyPressed())
			{
				flag = true;
			}
			else if (paintable && Widgets.checkboxPainting && Mouse.IsOver(rect))
			{
				multiCheckboxState = (Widgets.checkboxPaintingState ? MultiCheckboxState.On : MultiCheckboxState.Off);
				if (state != multiCheckboxState)
				{
					flag = true;
				}
			}
			if (flag)
			{
				if (multiCheckboxState == MultiCheckboxState.On)
				{
					SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
				}
				else
				{
					SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
				}
				return multiCheckboxState;
			}
			return state;
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x000CC79A File Offset: 0x000CA99A
		public static bool RadioButton(Vector2 topLeft, bool chosen)
		{
			return Widgets.RadioButton(topLeft.x, topLeft.y, chosen);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x000CC7AE File Offset: 0x000CA9AE
		public static bool RadioButton(float x, float y, bool chosen)
		{
			Rect butRect = new Rect(x, y, 24f, 24f);
			Widgets.RadioButtonDraw(x, y, chosen);
			bool flag = Widgets.ButtonInvisible(butRect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			return flag;
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x000CC7E0 File Offset: 0x000CA9E0
		public static bool RadioButtonLabeled(Rect rect, string labelText, bool chosen)
		{
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect, labelText);
			Text.Anchor = anchor;
			bool flag = Widgets.ButtonInvisible(rect, true);
			if (flag && !chosen)
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.RadioButtonDraw(rect.x + rect.width - 24f, rect.y + rect.height / 2f - 12f, chosen);
			return flag;
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x000CC854 File Offset: 0x000CAA54
		private static void RadioButtonDraw(float x, float y, bool chosen)
		{
			Color color = GUI.color;
			GUI.color = Color.white;
			Texture2D image;
			if (chosen)
			{
				image = Widgets.RadioButOnTex;
			}
			else
			{
				image = Widgets.RadioButOffTex;
			}
			GUI.DrawTexture(new Rect(x, y, 24f, 24f), image);
			GUI.color = color;
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x000CC89D File Offset: 0x000CAA9D
		public static bool ButtonText(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = true, bool active = true)
		{
			return Widgets.ButtonText(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x000CC8AF File Offset: 0x000CAAAF
		public static bool ButtonText(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, textColor, active, false).AnyPressed();
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x000CC8C4 File Offset: 0x000CAAC4
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground = true, bool doMouseoverSound = false, bool active = true)
		{
			return Widgets.ButtonTextDraggable(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x000CC8D6 File Offset: 0x000CAAD6
		public static Widgets.DraggableResult ButtonTextDraggable(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active = true)
		{
			return Widgets.ButtonTextWorker(rect, label, drawBackground, doMouseoverSound, Widgets.NormalOptionColor, active, true);
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x000CC8EC File Offset: 0x000CAAEC
		private static Widgets.DraggableResult ButtonTextWorker(Rect rect, string label, bool drawBackground, bool doMouseoverSound, Color textColor, bool active, bool draggable)
		{
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (drawBackground)
			{
				Texture2D atlas = Widgets.ButtonBGAtlas;
				if (Mouse.IsOver(rect))
				{
					atlas = Widgets.ButtonBGAtlasMouseover;
					if (Input.GetMouseButton(0))
					{
						atlas = Widgets.ButtonBGAtlasClick;
					}
				}
				Widgets.DrawAtlas(rect, atlas);
			}
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			if (!drawBackground)
			{
				GUI.color = textColor;
				if (Mouse.IsOver(rect))
				{
					GUI.color = Widgets.MouseoverOptionColor;
				}
			}
			if (drawBackground)
			{
				Text.Anchor = TextAnchor.MiddleCenter;
			}
			else
			{
				Text.Anchor = TextAnchor.MiddleLeft;
			}
			bool wordWrap = Text.WordWrap;
			if (rect.height < Text.LineHeight * 2f)
			{
				Text.WordWrap = false;
			}
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			Text.WordWrap = wordWrap;
			if (active && draggable)
			{
				return Widgets.ButtonInvisibleDraggable(rect, false);
			}
			if (!active)
			{
				return Widgets.DraggableResult.Idle;
			}
			if (!Widgets.ButtonInvisible(rect, false))
			{
				return Widgets.DraggableResult.Idle;
			}
			return Widgets.DraggableResult.Pressed;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x000CC9BF File Offset: 0x000CABBF
		public static void DrawRectFast(Rect position, Color color, GUIContent content = null)
		{
			Color backgroundColor = GUI.backgroundColor;
			GUI.backgroundColor = color;
			GUI.Box(position, content ?? GUIContent.none, TexUI.FastFillStyle);
			GUI.backgroundColor = backgroundColor;
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x000CC9E8 File Offset: 0x000CABE8
		public static bool CustomButtonText(ref Rect rect, string label, Color bgColor, Color textColor, Color borderColor, bool cacheHeight = false, int borderSize = 1, bool doMouseoverSound = true, bool active = true)
		{
			if (cacheHeight)
			{
				Widgets.LabelCacheHeight(ref rect, label, false, false);
			}
			Rect position = new Rect(rect);
			position.x += (float)borderSize;
			position.y += (float)borderSize;
			position.width -= (float)(borderSize * 2);
			position.height -= (float)(borderSize * 2);
			Widgets.DrawRectFast(rect, borderColor, null);
			Widgets.DrawRectFast(position, bgColor, null);
			TextAnchor anchor = Text.Anchor;
			Color color = GUI.color;
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(rect);
			}
			GUI.color = textColor;
			if (Mouse.IsOver(rect))
			{
				GUI.color = Widgets.MouseoverOptionColor;
			}
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, label);
			Text.Anchor = anchor;
			GUI.color = color;
			return active && Widgets.ButtonInvisible(rect, false);
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x000CCAD4 File Offset: 0x000CACD4
		public static bool ButtonTextSubtle(Rect rect, string label, float barPercent = 0f, float textLeftMargin = -1f, SoundDef mouseoverSound = null, Vector2 functionalSizeOffset = default(Vector2), Color? labelColor = null, bool highlight = false)
		{
			Rect rect2 = rect;
			rect2.width += functionalSizeOffset.x;
			rect2.height += functionalSizeOffset.y;
			bool flag = false;
			if (Mouse.IsOver(rect2))
			{
				flag = true;
				GUI.color = GenUI.MouseoverColor;
			}
			if (mouseoverSound != null)
			{
				MouseoverSounds.DoRegion(rect2, mouseoverSound);
			}
			Widgets.DrawAtlas(rect, Widgets.ButtonSubtleAtlas);
			if (highlight)
			{
				GUI.color = Color.grey;
				Widgets.DrawBox(rect, 2, null);
			}
			GUI.color = Color.white;
			if (barPercent > 0.001f)
			{
				Widgets.FillableBar(rect.ContractedBy(1f), barPercent, Widgets.ButtonBarTex, null, false);
			}
			Rect rect3 = new Rect(rect);
			if (textLeftMargin < 0f)
			{
				textLeftMargin = rect.width * 0.15f;
			}
			rect3.x += textLeftMargin;
			if (flag)
			{
				rect3.x += 2f;
				rect3.y -= 2f;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Text.WordWrap = false;
			Text.Font = GameFont.Small;
			GUI.color = (labelColor ?? Color.white);
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Text.WordWrap = true;
			GUI.color = Color.white;
			return Widgets.ButtonInvisible(rect2, false);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x000CCC27 File Offset: 0x000CAE27
		public static bool ButtonImage(Rect butRect, Texture2D tex, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, Color.white, doMouseoverSound);
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x000CCC36 File Offset: 0x000CAE36
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, bool doMouseoverSound = true)
		{
			return Widgets.ButtonImage(butRect, tex, baseColor, GenUI.MouseoverColor, doMouseoverSound);
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x000CCC46 File Offset: 0x000CAE46
		public static bool ButtonImage(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor, bool doMouseoverSound = true)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, doMouseoverSound);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x000CCC73 File Offset: 0x000CAE73
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, Color.white);
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x000CCC81 File Offset: 0x000CAE81
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageDraggable(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x000CCC90 File Offset: 0x000CAE90
		public static Widgets.DraggableResult ButtonImageDraggable(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			GUI.DrawTexture(butRect, tex);
			GUI.color = baseColor;
			return Widgets.ButtonInvisibleDraggable(butRect, false);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x000CCCBC File Offset: 0x000CAEBC
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex)
		{
			return Widgets.ButtonImageFitted(butRect, tex, Color.white);
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x000CCCCA File Offset: 0x000CAECA
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor)
		{
			return Widgets.ButtonImageFitted(butRect, tex, baseColor, GenUI.MouseoverColor);
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x000CCCD9 File Offset: 0x000CAED9
		public static bool ButtonImageFitted(Rect butRect, Texture2D tex, Color baseColor, Color mouseoverColor)
		{
			if (Mouse.IsOver(butRect))
			{
				GUI.color = mouseoverColor;
			}
			else
			{
				GUI.color = baseColor;
			}
			Widgets.DrawTextureFitted(butRect, tex, 1f);
			GUI.color = baseColor;
			return Widgets.ButtonInvisible(butRect, true);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000CCD0C File Offset: 0x000CAF0C
		public static bool ButtonImageWithBG(Rect butRect, Texture2D image, Vector2? imageSize = null)
		{
			bool result = Widgets.ButtonText(butRect, "", true, true, true);
			Rect position;
			if (imageSize != null)
			{
				position = new Rect(Mathf.Floor(butRect.x + butRect.width / 2f - imageSize.Value.x / 2f), Mathf.Floor(butRect.y + butRect.height / 2f - imageSize.Value.y / 2f), imageSize.Value.x, imageSize.Value.y);
			}
			else
			{
				position = butRect;
			}
			GUI.DrawTexture(position, image);
			return result;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000CCDB4 File Offset: 0x000CAFB4
		public static bool CloseButtonFor(Rect rectToClose)
		{
			return Widgets.ButtonImage(new Rect(rectToClose.x + rectToClose.width - 18f - 4f, rectToClose.y + 4f, 18f, 18f), TexButton.CloseXSmall, true);
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x000CCE04 File Offset: 0x000CB004
		public static bool BackButtonFor(Rect rectToBack)
		{
			return Widgets.ButtonText(new Rect(rectToBack.x + rectToBack.width - 18f - 4f - 120f - 16f, rectToBack.y + 18f, 120f, 40f), "Back".Translate(), true, true, true);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x000CCE6B File Offset: 0x000CB06B
		public static bool ButtonInvisible(Rect butRect, bool doMouseoverSound = true)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			return GUI.Button(butRect, "", Widgets.EmptyStyle);
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000CCE88 File Offset: 0x000CB088
		public static Widgets.DraggableResult ButtonInvisibleDraggable(Rect butRect, bool doMouseoverSound = false)
		{
			if (doMouseoverSound)
			{
				MouseoverSounds.DoRegion(butRect);
			}
			int controlID = GUIUtility.GetControlID(FocusType.Passive, butRect);
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(butRect))
			{
				Widgets.buttonInvisibleDraggable_activeControl = controlID;
				Widgets.buttonInvisibleDraggable_mouseStart = Input.mousePosition;
				Widgets.buttonInvisibleDraggable_dragged = false;
			}
			if (Widgets.buttonInvisibleDraggable_activeControl == controlID)
			{
				if (Input.GetMouseButtonUp(0))
				{
					Widgets.buttonInvisibleDraggable_activeControl = 0;
					if (!Mouse.IsOver(butRect))
					{
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged)
					{
						return Widgets.DraggableResult.Pressed;
					}
					return Widgets.DraggableResult.DraggedThenPressed;
				}
				else
				{
					if (!Input.GetMouseButton(0))
					{
						Widgets.buttonInvisibleDraggable_activeControl = 0;
						return Widgets.DraggableResult.Idle;
					}
					if (!Widgets.buttonInvisibleDraggable_dragged && (Widgets.buttonInvisibleDraggable_mouseStart - Input.mousePosition).sqrMagnitude > Widgets.DragStartDistanceSquared)
					{
						Widgets.buttonInvisibleDraggable_dragged = true;
						return Widgets.DraggableResult.Dragged;
					}
				}
			}
			return Widgets.DraggableResult.Idle;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000CCF35 File Offset: 0x000CB135
		public static string TextField(Rect rect, string text)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextField(rect, text, Text.CurTextFieldStyle);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000CCF50 File Offset: 0x000CB150
		public static string TextField(Rect rect, string text, int maxLength, Regex inputValidator = null)
		{
			string text2 = Widgets.TextField(rect, text);
			if (text2.Length <= maxLength && (inputValidator == null || inputValidator.IsMatch(text2)))
			{
				return text2;
			}
			return text;
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000CCF7D File Offset: 0x000CB17D
		public static string TextArea(Rect rect, string text, bool readOnly = false)
		{
			if (text == null)
			{
				text = "";
			}
			return GUI.TextArea(rect, text, readOnly ? Text.CurTextAreaReadOnlyStyle : Text.CurTextAreaStyle);
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000CCFA0 File Offset: 0x000CB1A0
		public static string TextAreaScrollable(Rect rect, string text, ref Vector2 scrollbarPosition, bool readOnly = false)
		{
			Rect rect2 = new Rect(0f, 0f, rect.width - 16f, Mathf.Max(Text.CalcHeight(text, rect.width) + 10f, rect.height));
			Widgets.BeginScrollView(rect, ref scrollbarPosition, rect2, true);
			string result = Widgets.TextArea(rect2, text, readOnly);
			Widgets.EndScrollView();
			return result;
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000CD000 File Offset: 0x000CB200
		public static string TextEntryLabeled(Rect rect, string label, string text)
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			if (rect.height <= 30f)
			{
				return Widgets.TextField(rect3, text);
			}
			return Widgets.TextArea(rect3, text, false);
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000CD05C File Offset: 0x000CB25C
		public static void TextFieldNumeric<T>(Rect rect, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			if (buffer == null)
			{
				buffer = val.ToString();
			}
			string text = "TextField" + rect.y.ToString("F0") + rect.x.ToString("F0");
			GUI.SetNextControlName(text);
			string text2 = GUI.TextField(rect, buffer, Text.CurTextFieldStyle);
			if (GUI.GetNameOfFocusedControl() != text)
			{
				Widgets.ResolveParseNow<T>(buffer, ref val, ref buffer, min, max, true);
				return;
			}
			if (text2 != buffer && Widgets.IsPartiallyOrFullyTypedNumber<T>(ref val, text2, min, max))
			{
				buffer = text2;
				if (text2.IsFullyTypedNumber<T>())
				{
					Widgets.ResolveParseNow<T>(text2, ref val, ref buffer, min, max, false);
				}
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000CD10C File Offset: 0x000CB30C
		private static void ResolveParseNow<T>(string edited, ref T val, ref string buffer, float min, float max, bool force)
		{
			if (typeof(T) == typeof(int))
			{
				if (edited.NullOrEmpty())
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
				int num;
				if (int.TryParse(edited, out num))
				{
					val = (T)((object)Mathf.RoundToInt(Mathf.Clamp((float)num, min, max)));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else if (typeof(T) == typeof(float))
			{
				float value;
				if (float.TryParse(edited, out value))
				{
					val = (T)((object)Mathf.Clamp(value, min, max));
					buffer = Widgets.ToStringTypedIn<T>(val);
					return;
				}
				if (force)
				{
					Widgets.ResetValue<T>(edited, ref val, ref buffer, min, max);
					return;
				}
			}
			else
			{
				Log.Error("TextField<T> does not support " + typeof(T));
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000CD208 File Offset: 0x000CB408
		private static void ResetValue<T>(string edited, ref T val, ref string buffer, float min, float max)
		{
			val = default(T);
			if (min > 0f)
			{
				val = (T)((object)Mathf.RoundToInt(min));
			}
			if (max < 0f)
			{
				val = (T)((object)Mathf.RoundToInt(max));
			}
			buffer = Widgets.ToStringTypedIn<T>(val);
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000CD268 File Offset: 0x000CB468
		private static string ToStringTypedIn<T>(T val)
		{
			if (typeof(T) == typeof(float))
			{
				return ((float)((object)val)).ToString("0.##########");
			}
			return val.ToString();
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000CD2B8 File Offset: 0x000CB4B8
		private static bool IsPartiallyOrFullyTypedNumber<T>(ref T val, string s, float min, float max)
		{
			return s == "" || ((s[0] != '-' || min < 0f) && (s.Length <= 1 || s[s.Length - 1] != '-') && !(s == "00") && s.Length <= 12 && ((typeof(T) == typeof(float) && s.CharacterCount('.') <= 1 && s.ContainsOnlyCharacters("-.0123456789")) || s.IsFullyTypedNumber<T>()));
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000CD364 File Offset: 0x000CB564
		private static bool IsFullyTypedNumber<T>(this string s)
		{
			if (s == "")
			{
				return false;
			}
			if (typeof(T) == typeof(float))
			{
				string[] array = s.Split(new char[]
				{
					'.'
				});
				if (array.Length > 2 || array.Length < 1)
				{
					return false;
				}
				if (!array[0].ContainsOnlyCharacters("-0123456789"))
				{
					return false;
				}
				if (array.Length == 2 && (array[1].Length == 0 || !array[1].ContainsOnlyCharacters("0123456789")))
				{
					return false;
				}
			}
			return !(typeof(T) == typeof(int)) || s.ContainsOnlyCharacters("-0123456789");
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000CD418 File Offset: 0x000CB618
		private static bool ContainsOnlyCharacters(this string s, string allowedChars)
		{
			for (int i = 0; i < s.Length; i++)
			{
				if (!allowedChars.Contains(s[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000CD448 File Offset: 0x000CB648
		private static int CharacterCount(this string s, char c)
		{
			int num = 0;
			for (int i = 0; i < s.Length; i++)
			{
				if (s[i] == c)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000CD478 File Offset: 0x000CB678
		public static void TextFieldNumericLabeled<T>(Rect rect, string label, ref T val, ref string buffer, float min = 0f, float max = 1E+09f) where T : struct
		{
			Rect rect2 = rect.LeftHalf().Rounded();
			Rect rect3 = rect.RightHalf().Rounded();
			TextAnchor anchor = Text.Anchor;
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect2, label);
			Text.Anchor = anchor;
			Widgets.TextFieldNumeric<T>(rect3, ref val, ref buffer, min, max);
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000CD4C0 File Offset: 0x000CB6C0
		public static void TextFieldPercent(Rect rect, ref float val, ref string buffer, float min = 0f, float max = 1f)
		{
			Rect rect2 = new Rect(rect.x, rect.y, rect.width - 25f, rect.height);
			Widgets.Label(new Rect(rect2.xMax, rect.y, 25f, rect2.height), "%");
			float num = val * 100f;
			Widgets.TextFieldNumeric<float>(rect2, ref num, ref buffer, min * 100f, max * 100f);
			val = num / 100f;
			if (val > max)
			{
				val = max;
				buffer = val.ToString();
			}
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000CD55C File Offset: 0x000CB75C
		public static T ChangeType<T>(this object obj)
		{
			CultureInfo invariantCulture = CultureInfo.InvariantCulture;
			return (T)((object)Convert.ChangeType(obj, typeof(T), invariantCulture));
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000CD588 File Offset: 0x000CB788
		public static float HorizontalSlider(Rect rect, float value, float leftValue, float rightValue, bool middleAlignment = false, string label = null, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f)
		{
			if (middleAlignment || !label.NullOrEmpty())
			{
				rect.y += Mathf.Round((rect.height - 16f) / 2f);
			}
			if (!label.NullOrEmpty())
			{
				rect.y += 5f;
			}
			float num = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
			if (!label.NullOrEmpty() || !leftAlignedLabel.NullOrEmpty() || !rightAlignedLabel.NullOrEmpty())
			{
				TextAnchor anchor = Text.Anchor;
				GameFont font = Text.Font;
				Text.Font = GameFont.Tiny;
				float num2 = label.NullOrEmpty() ? 18f : Text.CalcSize(label).y;
				rect.y = rect.y - num2 + 3f;
				if (!leftAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperLeft;
					Widgets.Label(rect, leftAlignedLabel);
				}
				if (!rightAlignedLabel.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperRight;
					Widgets.Label(rect, rightAlignedLabel);
				}
				if (!label.NullOrEmpty())
				{
					Text.Anchor = TextAnchor.UpperCenter;
					Widgets.Label(rect, label);
				}
				Text.Anchor = anchor;
				Text.Font = font;
			}
			if (roundTo > 0f)
			{
				num = (float)Mathf.RoundToInt(num / roundTo) * roundTo;
			}
			if (value != num)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			return num;
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000CD6C8 File Offset: 0x000CB8C8
		public static float FrequencyHorizontalSlider(Rect rect, float freq, float minFreq, float maxFreq, bool roundToInt = false)
		{
			float num;
			if (freq < 1f)
			{
				float x = 1f / freq;
				num = GenMath.LerpDouble(1f, 1f / minFreq, 0.5f, 1f, x);
			}
			else
			{
				num = GenMath.LerpDouble(maxFreq, 1f, 0f, 0.5f, freq);
			}
			string label;
			if (freq == 1f)
			{
				label = "EveryDay".Translate();
			}
			else if (freq < 1f)
			{
				label = "TimesPerDay".Translate((1f / freq).ToString("0.##"));
			}
			else
			{
				label = "EveryDays".Translate(freq.ToString("0.##"));
			}
			float num2 = Widgets.HorizontalSlider(rect, num, 0f, 1f, true, label, null, null, -1f);
			if (num != num2)
			{
				float num3;
				if (num2 < 0.5f)
				{
					num3 = GenMath.LerpDouble(0.5f, 0f, 1f, maxFreq, num2);
					if (roundToInt)
					{
						num3 = Mathf.Round(num3);
					}
				}
				else
				{
					float num4 = GenMath.LerpDouble(1f, 0.5f, 1f / minFreq, 1f, num2);
					if (roundToInt)
					{
						num4 = Mathf.Round(num4);
					}
					num3 = 1f / num4;
				}
				freq = num3;
			}
			return freq;
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000CD814 File Offset: 0x000CBA14
		public static void IntEntry(Rect rect, ref int value, ref string editBuffer, int multiplier = 1)
		{
			int num = Mathf.Min(Widgets.IntEntryButtonWidth, (int)rect.width / 5);
			if (Widgets.ButtonText(new Rect(rect.xMin, rect.yMin, (float)num, rect.height), (-10 * multiplier).ToStringCached(), true, true, true))
			{
				value -= 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMin + (float)num, rect.yMin, (float)num, rect.height), (-1 * multiplier).ToStringCached(), true, true, true))
			{
				value -= multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOff.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)num, rect.yMin, (float)num, rect.height), "+" + (10 * multiplier).ToStringCached(), true, true, true))
			{
				value += 10 * multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			if (Widgets.ButtonText(new Rect(rect.xMax - (float)(num * 2), rect.yMin, (float)num, rect.height), "+" + multiplier.ToStringCached(), true, true, true))
			{
				value += multiplier * GenUI.CurrentAdjustmentMultiplier();
				editBuffer = value.ToStringCached();
				SoundDefOf.Checkbox_TurnedOn.PlayOneShotOnCamera(null);
			}
			Widgets.TextFieldNumeric<int>(new Rect(rect.xMin + (float)(num * 2), rect.yMin, rect.width - (float)(num * 4), rect.height), ref value, ref editBuffer, 0f, 1E+09f);
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000CD9D0 File Offset: 0x000CBBD0
		public static void FloatRange(Rect rect, int id, ref FloatRange range, float min = 0f, float max = 1f, string labelKey = null, ToStringStyle valueStyle = ToStringStyle.FloatTwo)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute) + " - " + range.max.ToStringByStyle(valueStyle, ToStringNumberSense.Absolute);
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			rect3.height = Mathf.Max(rect3.height, Text.CalcHeight(text, rect3.width));
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.min);
			float num2 = rect2.x + rect2.width * Mathf.InverseLerp(min, max, range.max);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					float num5 = (Event.current.mousePosition.x - rect2.x) / rect2.width * (max - min) + min;
					num5 = Mathf.Clamp(num5, min, max);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = num5;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000CDD68 File Offset: 0x000CBF68
		public static void IntRange(Rect rect, int id, ref IntRange range, int min = 0, int max = 100, string labelKey = null, int minWidth = 0)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string text = range.min.ToStringCached() + " - " + range.max.ToStringCached();
			if (labelKey != null)
			{
				text = labelKey.Translate(text);
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, text);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			float num = rect2.x + rect2.width * (float)(range.min - min) / (float)(max - min);
			float num2 = rect2.x + rect2.width * (float)(range.max - min) / (float)(max - min);
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.rawType == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || Widgets.draggingId == id)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt(Mathf.Clamp((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(max - min) + (float)min, (float)min, (float)max));
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (num5 != range.min)
						{
							range.min = num5;
							if (range.min > max - minWidth)
							{
								range.min = max - minWidth;
							}
							int num6 = Mathf.Max(min, range.min + minWidth);
							if (range.max < num6)
							{
								range.max = num6;
							}
							Widgets.CheckPlayDragSliderSound();
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && num5 != range.max)
					{
						range.max = num5;
						if (range.max < min + minWidth)
						{
							range.max = min + minWidth;
						}
						int num7 = Mathf.Min(max, range.max - minWidth);
						if (range.min > num7)
						{
							range.min = num7;
						}
						Widgets.CheckPlayDragSliderSound();
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000CE124 File Offset: 0x000CC324
		private static void CheckPlayDragSliderSound()
		{
			if (Time.realtimeSinceStartup > Widgets.lastDragSliderSoundTime + 0.075f)
			{
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				Widgets.lastDragSliderSoundTime = Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060020F7 RID: 8439 RVA: 0x000CE150 File Offset: 0x000CC350
		public static void QualityRange(Rect rect, int id, ref QualityRange range)
		{
			Rect rect2 = rect;
			rect2.xMin += 8f;
			rect2.xMax -= 8f;
			GUI.color = Widgets.RangeControlTextColor;
			string label;
			if (range == RimWorld.QualityRange.All)
			{
				label = "AnyQuality".Translate();
			}
			else if (range.max == range.min)
			{
				label = "OnlyQuality".Translate(range.min.GetLabel());
			}
			else
			{
				label = range.min.GetLabel() + " - " + range.max.GetLabel();
			}
			GameFont font = Text.Font;
			Text.Font = GameFont.Tiny;
			Text.Anchor = TextAnchor.UpperCenter;
			Rect rect3 = rect2;
			rect3.yMin -= 2f;
			Widgets.Label(rect3, label);
			Text.Anchor = TextAnchor.UpperLeft;
			Rect position = new Rect(rect2.x, rect2.yMax - 8f - 1f, rect2.width, 2f);
			GUI.DrawTexture(position, BaseContent.WhiteTex);
			GUI.color = Color.white;
			int length = Enum.GetValues(typeof(QualityCategory)).Length;
			float num = rect2.x + rect2.width / (float)(length - 1) * (float)range.min;
			float num2 = rect2.x + rect2.width / (float)(length - 1) * (float)range.max;
			Rect position2 = new Rect(num - 16f, position.center.y - 8f, 16f, 16f);
			GUI.DrawTexture(position2, Widgets.FloatRangeSliderTex);
			Rect position3 = new Rect(num2 + 16f, position.center.y - 8f, -16f, 16f);
			GUI.DrawTexture(position3, Widgets.FloatRangeSliderTex);
			if (Widgets.curDragEnd != Widgets.RangeEnd.None && (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseDown))
			{
				Widgets.draggingId = 0;
				Widgets.curDragEnd = Widgets.RangeEnd.None;
				SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
			}
			bool flag = false;
			if (Mouse.IsOver(rect) || id == Widgets.draggingId)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && id != Widgets.draggingId)
				{
					Widgets.draggingId = id;
					float x = Event.current.mousePosition.x;
					if (x < position2.xMax)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Min;
					}
					else if (x > position3.xMin)
					{
						Widgets.curDragEnd = Widgets.RangeEnd.Max;
					}
					else
					{
						float num3 = Mathf.Abs(x - position2.xMax);
						float num4 = Mathf.Abs(x - (position3.x - 16f));
						Widgets.curDragEnd = ((num3 < num4) ? Widgets.RangeEnd.Min : Widgets.RangeEnd.Max);
					}
					flag = true;
					Event.current.Use();
					SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
				}
				if (flag || (Widgets.curDragEnd != Widgets.RangeEnd.None && Event.current.type == EventType.MouseDrag))
				{
					int num5 = Mathf.RoundToInt((Event.current.mousePosition.x - rect2.x) / rect2.width * (float)(length - 1));
					num5 = Mathf.Clamp(num5, 0, length - 1);
					if (Widgets.curDragEnd == Widgets.RangeEnd.Min)
					{
						if (range.min != (QualityCategory)num5)
						{
							range.min = (QualityCategory)num5;
							if (range.max < range.min)
							{
								range.max = range.min;
							}
							SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
						}
					}
					else if (Widgets.curDragEnd == Widgets.RangeEnd.Max && range.max != (QualityCategory)num5)
					{
						range.max = (QualityCategory)num5;
						if (range.min > range.max)
						{
							range.min = range.max;
						}
						SoundDefOf.DragSlider.PlayOneShotOnCamera(null);
					}
					Event.current.Use();
				}
			}
			Text.Font = font;
		}

		// Token: 0x060020F8 RID: 8440 RVA: 0x000CE528 File Offset: 0x000CC728
		public static void FloatRangeWithTypeIn(Rect rect, int id, ref FloatRange fRange, float sliderMin = 0f, float sliderMax = 1f, ToStringStyle valueStyle = ToStringStyle.FloatTwo, string labelKey = null)
		{
			Rect rect2 = new Rect(rect);
			rect2.width = rect.width / 4f;
			Rect rect3 = new Rect(rect);
			rect3.width = rect.width / 2f;
			rect3.x = rect.x + rect.width / 4f;
			rect3.height = rect.height / 2f;
			rect3.width -= rect.height;
			Rect butRect = new Rect(rect3);
			butRect.x = rect3.xMax;
			butRect.height = rect.height;
			butRect.width = rect.height;
			Rect rect4 = new Rect(rect);
			rect4.x = rect.x + rect.width * 0.75f;
			rect4.width = rect.width / 4f;
			rect3.y += 4f;
			rect3.height += 4f;
			Widgets.FloatRange(rect3, id, ref fRange, sliderMin, sliderMax, labelKey, valueStyle);
			if (Widgets.ButtonImage(butRect, TexButton.RangeMatch, true))
			{
				fRange.max = fRange.min;
			}
			float.TryParse(Widgets.TextField(rect2, fRange.min.ToString()), out fRange.min);
			float.TryParse(Widgets.TextField(rect4, fRange.max.ToString()), out fRange.max);
		}

		// Token: 0x060020F9 RID: 8441 RVA: 0x000CE6A5 File Offset: 0x000CC8A5
		public static Rect FillableBar(Rect rect, float fillPercent)
		{
			return Widgets.FillableBar(rect, fillPercent, Widgets.BarFullTexHor);
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000CE6B4 File Offset: 0x000CC8B4
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex)
		{
			bool doBorder = rect.height > 15f && rect.width > 20f;
			return Widgets.FillableBar(rect, fillPercent, fillTex, Widgets.DefaultBarBgTex, doBorder);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000CE6F0 File Offset: 0x000CC8F0
		public static Rect FillableBar(Rect rect, float fillPercent, Texture2D fillTex, Texture2D bgTex, bool doBorder)
		{
			if (doBorder)
			{
				GUI.DrawTexture(rect, BaseContent.BlackTex);
				rect = rect.ContractedBy(3f);
			}
			if (bgTex != null)
			{
				GUI.DrawTexture(rect, bgTex);
			}
			Rect result = rect;
			rect.width *= fillPercent;
			GUI.DrawTexture(rect, fillTex);
			return result;
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000CE740 File Offset: 0x000CC940
		public static void FillableBarLabeled(Rect rect, float fillPercent, int labelWidth, string label)
		{
			if (fillPercent < 0f)
			{
				fillPercent = 0f;
			}
			if (fillPercent > 1f)
			{
				fillPercent = 1f;
			}
			Rect rect2 = rect;
			rect2.width = (float)labelWidth;
			Widgets.Label(rect2, label);
			Rect rect3 = rect;
			rect3.x += (float)labelWidth;
			rect3.width -= (float)labelWidth;
			Widgets.FillableBar(rect3, fillPercent);
		}

		// Token: 0x060020FD RID: 8445 RVA: 0x000CE7A8 File Offset: 0x000CC9A8
		public static void FillableBarChangeArrows(Rect barRect, float changeRate)
		{
			int changeRate2 = (int)(changeRate * Widgets.FillableBarChangeRateDisplayRatio);
			Widgets.FillableBarChangeArrows(barRect, changeRate2);
		}

		// Token: 0x060020FE RID: 8446 RVA: 0x000CE7C8 File Offset: 0x000CC9C8
		public static void FillableBarChangeArrows(Rect barRect, int changeRate)
		{
			if (changeRate == 0)
			{
				return;
			}
			if (changeRate > Widgets.MaxFillableBarChangeRate)
			{
				changeRate = Widgets.MaxFillableBarChangeRate;
			}
			if (changeRate < -Widgets.MaxFillableBarChangeRate)
			{
				changeRate = -Widgets.MaxFillableBarChangeRate;
			}
			float num = barRect.height;
			if (num > 16f)
			{
				num = 16f;
			}
			int num2 = Mathf.Abs(changeRate);
			float y = barRect.y + barRect.height / 2f - num / 2f;
			float num3;
			float num4;
			Texture2D image;
			if (changeRate > 0)
			{
				num3 = barRect.x + barRect.width + 2f;
				num4 = 8f;
				image = Widgets.FillArrowTexRight;
			}
			else
			{
				num3 = barRect.x - 8f - 2f;
				num4 = -8f;
				image = Widgets.FillArrowTexLeft;
			}
			for (int i = 0; i < num2; i++)
			{
				GUI.DrawTexture(new Rect(num3, y, 8f, num), image);
				num3 += num4;
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000CE8A9 File Offset: 0x000CCAA9
		public static void DrawWindowBackground(Rect rect)
		{
			GUI.color = Widgets.WindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.WindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000CE8DC File Offset: 0x000CCADC
		public static void DrawMenuSection(Rect rect)
		{
			GUI.color = Widgets.MenuSectionBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.MenuSectionBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000CE90F File Offset: 0x000CCB0F
		public static void DrawWindowBackgroundTutor(Rect rect)
		{
			GUI.color = Widgets.TutorWindowBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.TutorWindowBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000CE942 File Offset: 0x000CCB42
		public static void DrawOptionUnselected(Rect rect)
		{
			GUI.color = Widgets.OptionUnselectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionUnselectedBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000CE975 File Offset: 0x000CCB75
		public static void DrawOptionSelected(Rect rect)
		{
			GUI.color = Widgets.OptionSelectedBGFillColor;
			GUI.DrawTexture(rect, BaseContent.WhiteTex);
			GUI.color = Widgets.OptionSelectedBGBorderColor;
			Widgets.DrawBox(rect, 1, null);
			GUI.color = Color.white;
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000CE9A8 File Offset: 0x000CCBA8
		public static void DrawOptionBackground(Rect rect, bool selected)
		{
			if (selected)
			{
				Widgets.DrawOptionSelected(rect);
			}
			else
			{
				Widgets.DrawOptionUnselected(rect);
			}
			Widgets.DrawHighlightIfMouseover(rect);
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000CE9C4 File Offset: 0x000CCBC4
		public static void DrawShadowAround(Rect rect)
		{
			Rect rect2 = rect.ContractedBy(-9f);
			rect2.x += 2f;
			rect2.y += 2f;
			Widgets.DrawAtlas(rect2, Widgets.ShadowAtlas);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000CEA0E File Offset: 0x000CCC0E
		public static void DrawAtlas(Rect rect, Texture2D atlas)
		{
			Widgets.DrawAtlas(rect, atlas, true);
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000CEA18 File Offset: 0x000CCC18
		private static Rect AdjustRectToUIScaling(Rect rect)
		{
			Rect result = rect;
			result.xMin = Widgets.AdjustCoordToUIScalingFloor(rect.xMin);
			result.yMin = Widgets.AdjustCoordToUIScalingFloor(rect.yMin);
			result.xMax = Widgets.AdjustCoordToUIScalingCeil(rect.xMax);
			result.yMax = Widgets.AdjustCoordToUIScalingCeil(rect.yMax);
			return result;
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000CEA74 File Offset: 0x000CCC74
		public static float AdjustCoordToUIScalingFloor(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Floor(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000CEA9C File Offset: 0x000CCC9C
		public static float AdjustCoordToUIScalingCeil(float coord)
		{
			double num = (double)(Prefs.UIScale * coord);
			float num2 = (float)(num - Math.Ceiling(num)) / Prefs.UIScale;
			return coord - num2;
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000CEAC4 File Offset: 0x000CCCC4
		public static void DrawAtlas(Rect rect, Texture2D atlas, bool drawTop)
		{
			rect.x = Mathf.Round(rect.x);
			rect.y = Mathf.Round(rect.y);
			rect.width = Mathf.Round(rect.width);
			rect.height = Mathf.Round(rect.height);
			rect = Widgets.AdjustRectToUIScaling(rect);
			float num = (float)atlas.width * 0.25f;
			num = Widgets.AdjustCoordToUIScalingCeil(GenMath.Min(num, rect.height / 2f, rect.width / 2f));
			GUI.BeginGroup(rect);
			Rect drawRect;
			Rect uvRect;
			if (drawTop)
			{
				drawRect = new Rect(0f, 0f, num, num);
				uvRect = new Rect(0f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
				drawRect = new Rect(rect.width - num, 0f, num, num);
				uvRect = new Rect(0.75f, 0f, 0.25f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(0f, rect.height - num, num, num);
			uvRect = new Rect(0f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, rect.height - num, num, num);
			uvRect = new Rect(0.75f, 0.75f, 0.25f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(num, num, rect.width - num * 2f, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.25f, 0.25f, 0.5f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			if (drawTop)
			{
				drawRect = new Rect(num, 0f, rect.width - num * 2f, num);
				uvRect = new Rect(0.25f, 0f, 0.5f, 0.25f);
				Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			}
			drawRect = new Rect(num, rect.height - num, rect.width - num * 2f, num);
			uvRect = new Rect(0.25f, 0.75f, 0.5f, 0.25f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(0f, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			drawRect = new Rect(rect.width - num, num, num, rect.height - num * 2f);
			if (!drawTop)
			{
				drawRect.height += num;
				drawRect.y -= num;
			}
			uvRect = new Rect(0.75f, 0.25f, 0.25f, 0.5f);
			Widgets.DrawTexturePart(drawRect, uvRect, atlas);
			GUI.EndGroup();
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000CEE07 File Offset: 0x000CD007
		public static Rect ToUVRect(this Rect r, Vector2 texSize)
		{
			return new Rect(r.x / texSize.x, r.y / texSize.y, r.width / texSize.x, r.height / texSize.y);
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000CEE46 File Offset: 0x000CD046
		public static void DrawTexturePart(Rect drawRect, Rect uvRect, Texture2D tex)
		{
			uvRect.y = 1f - uvRect.y - uvRect.height;
			GUI.DrawTextureWithTexCoords(drawRect, tex, uvRect);
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000CEE6C File Offset: 0x000CD06C
		public static void ScrollHorizontal(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, float ScrollWheelSpeed = 20f)
		{
			if (Event.current.type == EventType.ScrollWheel && Mouse.IsOver(outRect))
			{
				scrollPosition.x += Event.current.delta.y * ScrollWheelSpeed;
				float num = 0f;
				float num2 = viewRect.width - outRect.width + 16f;
				if (scrollPosition.x < num)
				{
					scrollPosition.x = num;
				}
				if (scrollPosition.x > num2)
				{
					scrollPosition.x = num2;
				}
				Event.current.Use();
			}
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000CEEF0 File Offset: 0x000CD0F0
		public static void BeginScrollView(Rect outRect, ref Vector2 scrollPosition, Rect viewRect, bool showScrollbars = true)
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Widgets.mouseOverScrollViewStack.Push(Widgets.mouseOverScrollViewStack.Peek() && outRect.Contains(Event.current.mousePosition));
			}
			else
			{
				Widgets.mouseOverScrollViewStack.Push(outRect.Contains(Event.current.mousePosition));
			}
			if (showScrollbars)
			{
				scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect);
				return;
			}
			scrollPosition = GUI.BeginScrollView(outRect, scrollPosition, viewRect, GUIStyle.none, GUIStyle.none);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000CEF85 File Offset: 0x000CD185
		public static void EndScrollView()
		{
			Widgets.mouseOverScrollViewStack.Pop();
			GUI.EndScrollView();
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000CEF97 File Offset: 0x000CD197
		public static void EnsureMousePositionStackEmpty()
		{
			if (Widgets.mouseOverScrollViewStack.Count > 0)
			{
				Log.Error("Mouse position stack is not empty. There were more calls to BeginScrollView than EndScrollView. Fixing.");
				Widgets.mouseOverScrollViewStack.Clear();
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000CEFBC File Offset: 0x000CD1BC
		public static bool ColorSelector(Rect rect, ref Color color, List<Color> colors, Texture icon = null, int colorSize = 22, int colorPadding = 2)
		{
			bool result = false;
			int num = colorSize + colorPadding * 2;
			float num2 = (float)(colorSize * 4) + 10f;
			int num3 = Mathf.FloorToInt((rect.width - num2) / (float)(num + colorPadding));
			int num4 = Mathf.CeilToInt((float)colors.Count / (float)num3);
			GUI.BeginGroup(rect);
			if (icon != null)
			{
				GUI.color = color;
				GUI.DrawTexture(new Rect(5f, 5f, (float)(colorSize * 4), (float)(colorSize * 4)), icon);
				GUI.color = Color.white;
			}
			int num5 = 0;
			foreach (Color color2 in colors)
			{
				int num6 = num5 / num3;
				int num7 = num5 % num3;
				float num8 = (num2 - (float)(num * num4) - (float)colorPadding) / 2f;
				Rect rect2 = new Rect(num2 + (float)(num7 * num) + (float)(num7 * colorPadding), num8 + (float)(num6 * num) + (float)(num6 * colorPadding), (float)num, (float)num);
				Widgets.DrawLightHighlight(rect2);
				Widgets.DrawHighlightIfMouseover(rect2);
				if (Widgets.<ColorSelector>g__Approximately|179_0(color.r, color2.r) && Widgets.<ColorSelector>g__Approximately|179_0(color.g, color2.g) && Widgets.<ColorSelector>g__Approximately|179_0(color.b, color2.b) && Widgets.<ColorSelector>g__Approximately|179_0(color.a, color2.a))
				{
					Widgets.DrawBox(rect2, 1, null);
				}
				Widgets.DrawBoxSolid(new Rect(rect2.x + (float)colorPadding, rect2.y + (float)colorPadding, (float)colorSize, (float)colorSize), color2);
				if (Widgets.ButtonInvisible(rect2, true))
				{
					result = true;
					color = color2;
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
				num5++;
			}
			GUI.EndGroup();
			return result;
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000CF19C File Offset: 0x000CD39C
		public static void DrawHighlightSelected(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightSelectedTex);
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000CF1A9 File Offset: 0x000CD3A9
		public static void DrawHighlightIfMouseover(Rect rect)
		{
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
			}
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000CF1B9 File Offset: 0x000CD3B9
		public static void DrawHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.HighlightTex);
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000CF1C6 File Offset: 0x000CD3C6
		public static void DrawLightHighlight(Rect rect)
		{
			GUI.DrawTexture(rect, Widgets.LightHighlight);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000CF1D3 File Offset: 0x000CD3D3
		public static void DrawTitleBG(Rect rect)
		{
			GUI.DrawTexture(rect, TexUI.TitleBGTex);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000CF1E0 File Offset: 0x000CD3E0
		public static bool InfoCardButton(float x, float y, Thing thing)
		{
			IConstructible constructible = thing as IConstructible;
			if (constructible != null)
			{
				ThingDef thingDef = thing.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					return Widgets.InfoCardButton(x, y, thingDef, constructible.EntityToBuildStuff());
				}
				return Widgets.InfoCardButton(x, y, thing.def.entityDefToBuild);
			}
			else
			{
				if (Widgets.InfoCardButtonWorker(x, y))
				{
					Find.WindowStack.Add(new Dialog_InfoCard(thing, null));
					return true;
				}
				return false;
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000CF24A File Offset: 0x000CD44A
		public static bool InfoCardButton(float x, float y, Def def)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000CF269 File Offset: 0x000CD469
		public static bool InfoCardButton(Rect rect, Def def)
		{
			if (Widgets.InfoCardButtonWorker(rect))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, null));
				return true;
			}
			return false;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000CF287 File Offset: 0x000CD487
		public static bool InfoCardButton(float x, float y, Def def, Precept_ThingStyle precept)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(def, precept));
				return true;
			}
			return false;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000CF2A6 File Offset: 0x000CD4A6
		public static bool InfoCardButton(float x, float y, ThingDef thingDef, ThingDef stuffDef)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(thingDef, stuffDef, null));
				return true;
			}
			return false;
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000CF2C6 File Offset: 0x000CD4C6
		public static bool InfoCardButton(float x, float y, WorldObject worldObject)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(worldObject));
				return true;
			}
			return false;
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000CF2E4 File Offset: 0x000CD4E4
		public static bool InfoCardButtonCentered(Rect rect, Thing thing)
		{
			return Widgets.InfoCardButton(rect.center.x - 12f, rect.center.y - 12f, thing);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000CF310 File Offset: 0x000CD510
		public static bool InfoCardButton(float x, float y, Faction faction)
		{
			if (Widgets.InfoCardButtonWorker(x, y))
			{
				Find.WindowStack.Add(new Dialog_InfoCard(faction));
				return true;
			}
			return false;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000CF32E File Offset: 0x000CD52E
		private static bool InfoCardButtonWorker(float x, float y)
		{
			return Widgets.InfoCardButtonWorker(new Rect(x, y, 24f, 24f));
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000CF346 File Offset: 0x000CD546
		private static bool InfoCardButtonWorker(Rect rect)
		{
			MouseoverSounds.DoRegion(rect);
			TooltipHandler.TipRegionByKey(rect, "DefInfoTip");
			bool result = Widgets.ButtonImage(rect, TexButton.Info, GUI.color, true);
			UIHighlighter.HighlightOpportunity(rect, "InfoCard");
			return result;
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000CF375 File Offset: 0x000CD575
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale)
		{
			Widgets.DrawTextureFitted(outerRect, tex, scale, new Vector2((float)tex.width, (float)tex.height), new Rect(0f, 0f, 1f, 1f), 0f, null);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000CF3B4 File Offset: 0x000CD5B4
		public static void DrawTextureFitted(Rect outerRect, Texture tex, float scale, Vector2 texProportions, Rect texCoords, float angle = 0f, Material mat = null)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			Rect rect = new Rect(0f, 0f, texProportions.x, texProportions.y);
			float num;
			if (rect.width / rect.height < outerRect.width / outerRect.height)
			{
				num = outerRect.height / rect.height;
			}
			else
			{
				num = outerRect.width / rect.width;
			}
			num *= scale;
			rect.width *= num;
			rect.height *= num;
			rect.x = outerRect.x + outerRect.width / 2f - rect.width / 2f;
			rect.y = outerRect.y + outerRect.height / 2f - rect.height / 2f;
			Matrix4x4 matrix = Matrix4x4.identity;
			if (angle != 0f)
			{
				matrix = GUI.matrix;
				UI.RotateAroundPivot(angle, rect.center);
			}
			GenUI.DrawTextureWithMaterial(rect, tex, mat, texCoords);
			if (angle != 0f)
			{
				GUI.matrix = matrix;
			}
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000CF4E4 File Offset: 0x000CD6E4
		public static void DrawTextureRotated(Vector2 center, Texture tex, float angle, float scale = 1f)
		{
			float num = (float)tex.width * scale;
			float num2 = (float)tex.height * scale;
			Widgets.DrawTextureRotated(new Rect(center.x - num / 2f, center.y - num2 / 2f, num, num2), tex, angle);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000CF52F File Offset: 0x000CD72F
		public static void DrawTextureRotated(Rect rect, Texture tex, float angle)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (angle == 0f)
			{
				GUI.DrawTexture(rect, tex);
				return;
			}
			Matrix4x4 matrix = GUI.matrix;
			UI.RotateAroundPivot(angle, rect.center);
			GUI.DrawTexture(rect, tex);
			GUI.matrix = matrix;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000CF56D File Offset: 0x000CD76D
		public static void NoneLabel(float y, float width, string customLabel = null)
		{
			Widgets.NoneLabel(ref y, width, customLabel);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000CF578 File Offset: 0x000CD778
		public static void NoneLabel(ref float curY, float width, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.UpperCenter;
			Widgets.Label(new Rect(0f, curY, width, 30f), customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			curY += 25f;
			GUI.color = Color.white;
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000CF5DA File Offset: 0x000CD7DA
		public static void NoneLabelCenteredVertically(Rect rect, string customLabel = null)
		{
			GUI.color = Color.gray;
			Text.Anchor = TextAnchor.MiddleCenter;
			Widgets.Label(rect, customLabel ?? "NoneBrackets".Translate());
			Text.Anchor = TextAnchor.UpperLeft;
			GUI.color = Color.white;
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000CF618 File Offset: 0x000CD818
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			Widgets.Dropdown<Target, Payload>(rect, target, Color.white, getPayload, menuGenerator, buttonLabel, buttonIcon, dragLabel, dragIcon, dropdownOpened, paintable);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x000CF640 File Offset: 0x000CD840
		public static void Dropdown<Target, Payload>(Rect rect, Target target, Color iconColor, Func<Target, Payload> getPayload, Func<Target, IEnumerable<Widgets.DropdownMenuElement<Payload>>> menuGenerator, string buttonLabel = null, Texture2D buttonIcon = null, string dragLabel = null, Texture2D dragIcon = null, Action dropdownOpened = null, bool paintable = false)
		{
			MouseoverSounds.DoRegion(rect);
			Widgets.DraggableResult draggableResult;
			if (buttonIcon != null)
			{
				Widgets.DrawHighlightIfMouseover(rect);
				GUI.color = iconColor;
				Widgets.DrawTextureFitted(rect, buttonIcon, 1f);
				GUI.color = Color.white;
				draggableResult = Widgets.ButtonInvisibleDraggable(rect, false);
			}
			else
			{
				draggableResult = Widgets.ButtonTextDraggable(rect, buttonLabel, true, false, true);
			}
			if (draggableResult == Widgets.DraggableResult.Pressed)
			{
				List<FloatMenuOption> options = (from opt in menuGenerator(target)
				select opt.option).ToList<FloatMenuOption>();
				Find.WindowStack.Add(new FloatMenu(options));
				if (dropdownOpened != null)
				{
					dropdownOpened();
					return;
				}
			}
			else
			{
				if (paintable && draggableResult == Widgets.DraggableResult.Dragged)
				{
					Widgets.dropdownPainting = true;
					Widgets.dropdownPainting_Payload = getPayload(target);
					Widgets.dropdownPainting_Type = typeof(Payload);
					Widgets.dropdownPainting_Text = ((dragLabel != null) ? dragLabel : buttonLabel);
					Widgets.dropdownPainting_Icon = ((dragIcon != null) ? dragIcon : buttonIcon);
					return;
				}
				if (paintable && Widgets.dropdownPainting && Mouse.IsOver(rect) && Widgets.dropdownPainting_Type == typeof(Payload))
				{
					FloatMenuOption floatMenuOption = (from opt in menuGenerator(target)
					where object.Equals(opt.payload, Widgets.dropdownPainting_Payload)
					select opt.option).FirstOrDefault<FloatMenuOption>();
					if (floatMenuOption != null && !floatMenuOption.Disabled)
					{
						Payload x = getPayload(target);
						floatMenuOption.action();
						Payload y = getPayload(target);
						if (!EqualityComparer<Payload>.Default.Equals(x, y))
						{
							SoundDefOf.Click.PlayOneShotOnCamera(null);
						}
					}
				}
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000CF80C File Offset: 0x000CDA0C
		public static void MouseAttachedLabel(string label, float xOffset = 0f, float yOffset = 0f)
		{
			Vector2 mousePosition = Event.current.mousePosition;
			Rect rect = new Rect(mousePosition.x + 8f + xOffset, mousePosition.y + 8f + yOffset, 32f, 32f);
			GUI.color = Color.white;
			Text.Font = GameFont.Small;
			Rect rect2 = new Rect(rect.xMax, rect.y, 9999f, 100f);
			Vector2 vector = Text.CalcSize(label);
			rect2.height = Mathf.Max(rect2.height, vector.y);
			GUI.DrawTexture(new Rect(rect2.x - vector.x * 0.1f, rect2.y, vector.x * 1.2f, vector.y), TexUI.GrayTextBG);
			Widgets.Label(rect2, label);
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000CF8E4 File Offset: 0x000CDAE4
		public static void WidgetsOnGUI()
		{
			if (Event.current.rawType == EventType.MouseUp || Input.GetMouseButtonUp(0))
			{
				Widgets.checkboxPainting = false;
				Widgets.dropdownPainting = false;
			}
			if (Widgets.checkboxPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.checkboxPaintingState ? Widgets.CheckboxOnTex : Widgets.CheckboxOffTex);
			}
			if (Widgets.dropdownPainting)
			{
				GenUI.DrawMouseAttachment(Widgets.dropdownPainting_Icon, Widgets.dropdownPainting_Text, 0f, default(Vector2), null, false, default(Color));
			}
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x000CF968 File Offset: 0x000CDB68
		[CompilerGenerated]
		internal static bool <ColorSelector>g__Approximately|179_0(float f1, float f2)
		{
			return Mathf.Abs(f1 - f2) < 0.01f;
		}

		// Token: 0x0400144C RID: 5196
		public static Stack<bool> mouseOverScrollViewStack = new Stack<bool>();

		// Token: 0x0400144D RID: 5197
		public static readonly GUIStyle EmptyStyle = new GUIStyle();

		// Token: 0x0400144E RID: 5198
		[TweakValue("Input", 0f, 100f)]
		private static float DragStartDistanceSquared = 20f;

		// Token: 0x0400144F RID: 5199
		private static readonly Color InactiveColor = new Color(0.37f, 0.37f, 0.37f, 0.8f);

		// Token: 0x04001450 RID: 5200
		private static readonly Texture2D DefaultBarBgTex = BaseContent.BlackTex;

		// Token: 0x04001451 RID: 5201
		private static readonly Texture2D BarFullTexHor = SolidColorMaterials.NewSolidColorTexture(new Color(0.2f, 0.8f, 0.85f));

		// Token: 0x04001452 RID: 5202
		public static readonly Texture2D CheckboxOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOn", true);

		// Token: 0x04001453 RID: 5203
		public static readonly Texture2D CheckboxOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckOff", true);

		// Token: 0x04001454 RID: 5204
		public static readonly Texture2D CheckboxPartialTex = ContentFinder<Texture2D>.Get("UI/Widgets/CheckPartial", true);

		// Token: 0x04001455 RID: 5205
		public const float CheckboxSize = 24f;

		// Token: 0x04001456 RID: 5206
		public const float RadioButtonSize = 24f;

		// Token: 0x04001457 RID: 5207
		public static readonly Texture2D RadioButOnTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOn", true);

		// Token: 0x04001458 RID: 5208
		private static readonly Texture2D RadioButOffTex = ContentFinder<Texture2D>.Get("UI/Widgets/RadioButOff", true);

		// Token: 0x04001459 RID: 5209
		private static readonly Texture2D FillArrowTexRight = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowRight", true);

		// Token: 0x0400145A RID: 5210
		private static readonly Texture2D FillArrowTexLeft = ContentFinder<Texture2D>.Get("UI/Widgets/FillChangeArrowLeft", true);

		// Token: 0x0400145B RID: 5211
		public static readonly Texture2D PlaceholderIconTex = ContentFinder<Texture2D>.Get("UI/Icons/MenuOptionNoIcon", true);

		// Token: 0x0400145C RID: 5212
		private const int FillableBarBorderWidth = 3;

		// Token: 0x0400145D RID: 5213
		private const int MaxFillChangeArrowHeight = 16;

		// Token: 0x0400145E RID: 5214
		private const int FillChangeArrowWidth = 8;

		// Token: 0x0400145F RID: 5215
		public const float CloseButtonSize = 18f;

		// Token: 0x04001460 RID: 5216
		public const float CloseButtonMargin = 4f;

		// Token: 0x04001461 RID: 5217
		public const float BackButtonWidth = 120f;

		// Token: 0x04001462 RID: 5218
		public const float BackButtonHeight = 40f;

		// Token: 0x04001463 RID: 5219
		public const float BackButtonMargin = 16f;

		// Token: 0x04001464 RID: 5220
		private static readonly Texture2D ShadowAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/DropShadow", true);

		// Token: 0x04001465 RID: 5221
		public static readonly Texture2D ButtonBGAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBG", true);

		// Token: 0x04001466 RID: 5222
		private static readonly Texture2D ButtonBGAtlasMouseover = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGMouseover", true);

		// Token: 0x04001467 RID: 5223
		public static readonly Texture2D ButtonBGAtlasClick = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonBGClick", true);

		// Token: 0x04001468 RID: 5224
		private static readonly Texture2D FloatRangeSliderTex = ContentFinder<Texture2D>.Get("UI/Widgets/RangeSlider", true);

		// Token: 0x04001469 RID: 5225
		public static readonly Texture2D LightHighlight = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.04f));

		// Token: 0x0400146A RID: 5226
		private static readonly Rect DefaultTexCoords = new Rect(0f, 0f, 1f, 1f);

		// Token: 0x0400146B RID: 5227
		private static readonly Rect LinkedTexCoords = new Rect(0f, 0.5f, 0.25f, 0.25f);

		// Token: 0x0400146C RID: 5228
		[TweakValue("Input", 0f, 100f)]
		private static int IntEntryButtonWidth = 40;

		// Token: 0x0400146D RID: 5229
		private static Texture2D LineTexAA = null;

		// Token: 0x0400146E RID: 5230
		private static readonly Texture2D AltTexture = SolidColorMaterials.NewSolidColorTexture(new Color(1f, 1f, 1f, 0.05f));

		// Token: 0x0400146F RID: 5231
		public static readonly Color NormalOptionColor = new Color(0.8f, 0.85f, 1f);

		// Token: 0x04001470 RID: 5232
		public static readonly Color MouseoverOptionColor = Color.yellow;

		// Token: 0x04001471 RID: 5233
		private static Dictionary<string, float> LabelCache = new Dictionary<string, float>();

		// Token: 0x04001472 RID: 5234
		private const float TileSize = 64f;

		// Token: 0x04001473 RID: 5235
		public static readonly Color SeparatorLabelColor = new Color(0.8f, 0.8f, 0.8f, 1f);

		// Token: 0x04001474 RID: 5236
		public static readonly Color SeparatorLineColor = new Color(0.3f, 0.3f, 0.3f, 1f);

		// Token: 0x04001475 RID: 5237
		private const float SeparatorLabelHeight = 20f;

		// Token: 0x04001476 RID: 5238
		public const float ListSeparatorHeight = 25f;

		// Token: 0x04001477 RID: 5239
		private static bool checkboxPainting = false;

		// Token: 0x04001478 RID: 5240
		private static bool checkboxPaintingState = false;

		// Token: 0x04001479 RID: 5241
		public static readonly Texture2D ButtonSubtleAtlas = ContentFinder<Texture2D>.Get("UI/Widgets/ButtonSubtleAtlas", true);

		// Token: 0x0400147A RID: 5242
		private static readonly Texture2D ButtonBarTex = SolidColorMaterials.NewSolidColorTexture(new ColorInt(78, 109, 129, 130).ToColor);

		// Token: 0x0400147B RID: 5243
		public const float ButtonSubtleDefaultMarginPct = 0.15f;

		// Token: 0x0400147C RID: 5244
		private static int buttonInvisibleDraggable_activeControl = 0;

		// Token: 0x0400147D RID: 5245
		private static bool buttonInvisibleDraggable_dragged = false;

		// Token: 0x0400147E RID: 5246
		private static Vector3 buttonInvisibleDraggable_mouseStart = Vector3.zero;

		// Token: 0x0400147F RID: 5247
		public const float RangeControlIdealHeight = 31f;

		// Token: 0x04001480 RID: 5248
		public const float RangeControlCompactHeight = 28f;

		// Token: 0x04001481 RID: 5249
		private const float RangeSliderSize = 16f;

		// Token: 0x04001482 RID: 5250
		private static readonly Color RangeControlTextColor = new Color(0.6f, 0.6f, 0.6f);

		// Token: 0x04001483 RID: 5251
		private static int draggingId = 0;

		// Token: 0x04001484 RID: 5252
		private static Widgets.RangeEnd curDragEnd = Widgets.RangeEnd.None;

		// Token: 0x04001485 RID: 5253
		private static float lastDragSliderSoundTime = -1f;

		// Token: 0x04001486 RID: 5254
		private static float FillableBarChangeRateDisplayRatio = 100000000f;

		// Token: 0x04001487 RID: 5255
		public static int MaxFillableBarChangeRate = 3;

		// Token: 0x04001488 RID: 5256
		private static readonly Color WindowBGBorderColor = new ColorInt(97, 108, 122).ToColor;

		// Token: 0x04001489 RID: 5257
		public static readonly Color WindowBGFillColor = new ColorInt(21, 25, 29).ToColor;

		// Token: 0x0400148A RID: 5258
		public static readonly Color MenuSectionBGFillColor = new ColorInt(42, 43, 44).ToColor;

		// Token: 0x0400148B RID: 5259
		private static readonly Color MenuSectionBGBorderColor = new ColorInt(135, 135, 135).ToColor;

		// Token: 0x0400148C RID: 5260
		private static readonly Color TutorWindowBGFillColor = new ColorInt(133, 85, 44).ToColor;

		// Token: 0x0400148D RID: 5261
		private static readonly Color TutorWindowBGBorderColor = new ColorInt(176, 139, 61).ToColor;

		// Token: 0x0400148E RID: 5262
		private static readonly Color OptionUnselectedBGFillColor = new Color(0.21f, 0.21f, 0.21f);

		// Token: 0x0400148F RID: 5263
		private static readonly Color OptionUnselectedBGBorderColor = Widgets.OptionUnselectedBGFillColor * 1.8f;

		// Token: 0x04001490 RID: 5264
		private static readonly Color OptionSelectedBGFillColor = new Color(0.32f, 0.28f, 0.21f);

		// Token: 0x04001491 RID: 5265
		private static readonly Color OptionSelectedBGBorderColor = Widgets.OptionSelectedBGFillColor * 1.8f;

		// Token: 0x04001492 RID: 5266
		public const float InfoCardButtonSize = 24f;

		// Token: 0x04001493 RID: 5267
		private static bool dropdownPainting = false;

		// Token: 0x04001494 RID: 5268
		private static object dropdownPainting_Payload = null;

		// Token: 0x04001495 RID: 5269
		private static Type dropdownPainting_Type = null;

		// Token: 0x04001496 RID: 5270
		private static string dropdownPainting_Text = "";

		// Token: 0x04001497 RID: 5271
		private static Texture2D dropdownPainting_Icon = null;

		// Token: 0x02001C6D RID: 7277
		public enum DraggableResult
		{
			// Token: 0x04006DD2 RID: 28114
			Idle,
			// Token: 0x04006DD3 RID: 28115
			Pressed,
			// Token: 0x04006DD4 RID: 28116
			Dragged,
			// Token: 0x04006DD5 RID: 28117
			DraggedThenPressed
		}

		// Token: 0x02001C6E RID: 7278
		private enum RangeEnd : byte
		{
			// Token: 0x04006DD7 RID: 28119
			None,
			// Token: 0x04006DD8 RID: 28120
			Min,
			// Token: 0x04006DD9 RID: 28121
			Max
		}

		// Token: 0x02001C6F RID: 7279
		public struct DropdownMenuElement<Payload>
		{
			// Token: 0x04006DDA RID: 28122
			public FloatMenuOption option;

			// Token: 0x04006DDB RID: 28123
			public Payload payload;
		}
	}
}
