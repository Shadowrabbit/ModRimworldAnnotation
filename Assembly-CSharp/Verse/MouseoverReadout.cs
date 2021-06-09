using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000754 RID: 1876
	public class MouseoverReadout
	{
		// Token: 0x06002F4A RID: 12106 RVA: 0x000250DB File Offset: 0x000232DB
		public MouseoverReadout()
		{
			this.MakePermaCache();
		}

		// Token: 0x06002F4B RID: 12107 RVA: 0x0013A958 File Offset: 0x00138B58
		private void MakePermaCache()
		{
			this.glowStrings = new string[101];
			for (int i = 0; i <= 100; i++)
			{
				this.glowStrings[i] = GlowGrid.PsychGlowAtGlow((float)i / 100f).GetLabel() + " (" + ((float)i / 100f).ToStringPercent() + ")";
			}
		}

		// Token: 0x06002F4C RID: 12108 RVA: 0x0013A9B8 File Offset: 0x00138BB8
		public void MouseoverReadoutOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Find.MainTabsRoot.OpenTab != null)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, (float)(UI.screenHeight - 256), -256f, 256f));
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
			IntVec3 c = UI.MouseCell();
			if (!c.InBounds(Find.CurrentMap))
			{
				return;
			}
			float num = 0f;
			if (c.Fogged(Find.CurrentMap))
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), "Undiscovered".Translate());
				GUI.color = Color.white;
				return;
			}
			Rect rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
			int num2 = Mathf.RoundToInt(Find.CurrentMap.glowGrid.GameGlowAt(c, false) * 100f);
			Widgets.Label(rect, this.glowStrings[num2]);
			num += 19f;
			Rect rect2 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
			TerrainDef terrain = c.GetTerrain(Find.CurrentMap);
			if (terrain != this.cachedTerrain)
			{
				string t = ((double)terrain.fertility > 0.0001) ? (" " + "FertShort".TranslateSimple() + " " + terrain.fertility.ToStringPercent()) : "";
				this.cachedTerrainString = terrain.LabelCap + ((terrain.passability != Traversability.Impassable) ? (" (" + "WalkSpeed".Translate(this.SpeedPercentString((float)terrain.pathCost)) + t + ")") : null);
				this.cachedTerrain = terrain;
			}
			Widgets.Label(rect2, this.cachedTerrainString);
			num += 19f;
			Zone zone = c.GetZone(Find.CurrentMap);
			if (zone != null)
			{
				Rect rect3 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				string label = zone.label;
				Widgets.Label(rect3, label);
				num += 19f;
			}
			float depth = Find.CurrentMap.snowGrid.GetDepth(c);
			if (depth > 0.03f)
			{
				Rect rect4 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				SnowCategory snowCategory = SnowUtility.GetSnowCategory(depth);
				string label2 = SnowUtility.GetDescription(snowCategory) + " (" + "WalkSpeed".Translate(this.SpeedPercentString((float)SnowUtility.MovementTicksAddOn(snowCategory))) + ")";
				Widgets.Label(rect4, label2);
				num += 19f;
			}
			List<Thing> thingList = c.GetThingList(Find.CurrentMap);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				if (thing.def.category != ThingCategory.Mote)
				{
					Rect rect5 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
					string labelMouseover = thing.LabelMouseover;
					Widgets.Label(rect5, labelMouseover);
					num += 19f;
				}
			}
			RoofDef roof = c.GetRoof(Find.CurrentMap);
			if (roof != null)
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), roof.LabelCap);
				num += 19f;
			}
			GUI.color = Color.white;
		}

		// Token: 0x06002F4D RID: 12109 RVA: 0x000250E9 File Offset: 0x000232E9
		private string SpeedPercentString(float extraPathTicks)
		{
			return (13f / (extraPathTicks + 13f)).ToStringPercent();
		}

		// Token: 0x04002014 RID: 8212
		private TerrainDef cachedTerrain;

		// Token: 0x04002015 RID: 8213
		private string cachedTerrainString;

		// Token: 0x04002016 RID: 8214
		private string[] glowStrings;

		// Token: 0x04002017 RID: 8215
		private const float YInterval = 19f;

		// Token: 0x04002018 RID: 8216
		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);
	}
}
