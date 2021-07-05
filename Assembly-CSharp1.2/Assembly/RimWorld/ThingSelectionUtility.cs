using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B23 RID: 6947
	public static class ThingSelectionUtility
	{
		// Token: 0x060098CA RID: 39114 RVA: 0x002CE944 File Offset: 0x002CCB44
		public static bool SelectableByMapClick(Thing t)
		{
			if (!t.def.selectable)
			{
				return false;
			}
			if (!t.Spawned)
			{
				return false;
			}
			if (t.def.size.x == 1 && t.def.size.z == 1)
			{
				return !t.Position.Fogged(t.Map);
			}
			using (CellRect.Enumerator enumerator = t.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Fogged(t.Map))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060098CB RID: 39115 RVA: 0x00065DC6 File Offset: 0x00063FC6
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x060098CC RID: 39116 RVA: 0x00065DDD File Offset: 0x00063FDD
		public static IEnumerable<Thing> MultiSelectableThingsInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedThings.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						List<Thing> cellThings = Find.CurrentMap.thingGrid.ThingsListAt(c);
						if (cellThings != null)
						{
							int num;
							for (int i = 0; i < cellThings.Count; i = num + 1)
							{
								Thing t = cellThings[i];
								if (ThingSelectionUtility.SelectableByMapClick(t) && !t.def.neverMultiSelect && !ThingSelectionUtility.yieldedThings.Contains(t))
								{
									yield return t;
									ThingSelectionUtility.yieldedThings.Add(t);
								}
								t = null;
								num = i;
							}
						}
						cellThings = null;
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedThings.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x060098CD RID: 39117 RVA: 0x00065DED File Offset: 0x00063FED
		public static IEnumerable<Zone> MultiSelectableZonesInScreenRectDistinct(Rect rect)
		{
			CellRect mapRect = ThingSelectionUtility.GetMapRect(rect);
			ThingSelectionUtility.yieldedZones.Clear();
			try
			{
				foreach (IntVec3 c in mapRect)
				{
					if (c.InBounds(Find.CurrentMap))
					{
						Zone zone = c.GetZone(Find.CurrentMap);
						if (zone != null && zone.IsMultiselectable)
						{
							if (!ThingSelectionUtility.yieldedZones.Contains(zone))
							{
								yield return zone;
								ThingSelectionUtility.yieldedZones.Add(zone);
							}
							zone = null;
						}
					}
				}
			}
			finally
			{
				ThingSelectionUtility.yieldedZones.Clear();
			}
			yield break;
			yield break;
		}

		// Token: 0x060098CE RID: 39118 RVA: 0x002CE9FC File Offset: 0x002CCBFC
		private static CellRect GetMapRect(Rect rect)
		{
			Vector2 screenLoc = new Vector2(rect.x, (float)UI.screenHeight - rect.y);
			Vector2 screenLoc2 = new Vector2(rect.x + rect.width, (float)UI.screenHeight - (rect.y + rect.height));
			Vector3 vector = UI.UIToMapPosition(screenLoc);
			Vector3 vector2 = UI.UIToMapPosition(screenLoc2);
			return new CellRect
			{
				minX = Mathf.FloorToInt(vector.x),
				minZ = Mathf.FloorToInt(vector2.z),
				maxX = Mathf.FloorToInt(vector2.x),
				maxZ = Mathf.FloorToInt(vector.z)
			};
		}

		// Token: 0x060098CF RID: 39119 RVA: 0x002CEAB0 File Offset: 0x002CCCB0
		public static void SelectNextColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			int num = -1;
			for (int i = ThingSelectionUtility.tmpColonists.Count - 1; i >= 0; i--)
			{
				if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[0]);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[(num + 1) % ThingSelectionUtility.tmpColonists.Count]);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x060098D0 RID: 39120 RVA: 0x002CEBB0 File Offset: 0x002CCDB0
		public static void SelectPreviousColonist()
		{
			ThingSelectionUtility.tmpColonists.Clear();
			ThingSelectionUtility.tmpColonists.AddRange(Find.ColonistBar.GetColonistsInOrder().Where(new Func<Pawn, bool>(ThingSelectionUtility.SelectableByHotkey)));
			if (ThingSelectionUtility.tmpColonists.Count == 0)
			{
				return;
			}
			bool worldRenderedNow = WorldRendererUtility.WorldRenderedNow;
			int num = -1;
			for (int i = 0; i < ThingSelectionUtility.tmpColonists.Count; i++)
			{
				if ((!worldRenderedNow && Find.Selector.IsSelected(ThingSelectionUtility.tmpColonists[i])) || (worldRenderedNow && ThingSelectionUtility.tmpColonists[i].IsCaravanMember() && Find.WorldSelector.IsSelected(ThingSelectionUtility.tmpColonists[i].GetCaravan())))
				{
					num = i;
					break;
				}
			}
			if (num == -1)
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[ThingSelectionUtility.tmpColonists.Count - 1]);
			}
			else
			{
				CameraJumper.TryJumpAndSelect(ThingSelectionUtility.tmpColonists[GenMath.PositiveMod(num - 1, ThingSelectionUtility.tmpColonists.Count)]);
			}
			ThingSelectionUtility.tmpColonists.Clear();
		}

		// Token: 0x040061AD RID: 25005
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x040061AE RID: 25006
		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x040061AF RID: 25007
		private static List<Pawn> tmpColonists = new List<Pawn>();
	}
}
