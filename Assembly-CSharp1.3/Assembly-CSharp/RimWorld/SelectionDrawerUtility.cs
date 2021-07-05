﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135B RID: 4955
	[StaticConstructorOnStartup]
	public static class SelectionDrawerUtility
	{
		// Token: 0x0600780F RID: 30735 RVA: 0x002A501C File Offset: 0x002A321C
		public static void CalculateSelectionBracketPositionsUI<T>(Vector2[] bracketLocs, T obj, Rect rect, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (rect.width - textureSize.x) + num3;
			float num5 = 0.5f * (rect.height - textureSize.y) + num3;
			bracketLocs[0] = new Vector2(rect.center.x - num4, rect.center.y - num5);
			bracketLocs[1] = new Vector2(rect.center.x + num4, rect.center.y - num5);
			bracketLocs[2] = new Vector2(rect.center.x + num4, rect.center.y + num5);
			bracketLocs[3] = new Vector2(rect.center.x - num4, rect.center.y + num5);
		}

		// Token: 0x06007810 RID: 30736 RVA: 0x002A513C File Offset: 0x002A333C
		public static void CalculateSelectionBracketPositionsWorld<T>(Vector3[] bracketLocs, T obj, Vector3 worldPos, Vector2 worldSize, Dictionary<T, float> selectTimes, Vector2 textureSize, float jumpDistanceFactor = 1f)
		{
			float num;
			float num2;
			if (!selectTimes.TryGetValue(obj, out num))
			{
				num2 = 1f;
			}
			else
			{
				num2 = Mathf.Max(0f, 1f - (Time.realtimeSinceStartup - num) / 0.07f);
			}
			float num3 = num2 * 0.2f * jumpDistanceFactor;
			float num4 = 0.5f * (worldSize.x - textureSize.x) + num3;
			float num5 = 0.5f * (worldSize.y - textureSize.y) + num3;
			float y = AltitudeLayer.MetaOverlays.AltitudeFor();
			bracketLocs[0] = new Vector3(worldPos.x - num4, y, worldPos.z - num5);
			bracketLocs[1] = new Vector3(worldPos.x + num4, y, worldPos.z - num5);
			bracketLocs[2] = new Vector3(worldPos.x + num4, y, worldPos.z + num5);
			bracketLocs[3] = new Vector3(worldPos.x - num4, y, worldPos.z + num5);
		}

		// Token: 0x040042C9 RID: 17097
		private const float SelJumpDuration = 0.07f;

		// Token: 0x040042CA RID: 17098
		private const float SelJumpDistance = 0.2f;

		// Token: 0x040042CB RID: 17099
		public static readonly Texture2D SelectedTexGUI = ContentFinder<Texture2D>.Get("UI/Overlays/SelectionBracketGUI", true);
	}
}
