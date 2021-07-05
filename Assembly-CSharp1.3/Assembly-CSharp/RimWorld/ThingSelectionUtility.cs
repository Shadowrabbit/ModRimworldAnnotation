using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135C RID: 4956
	public static class ThingSelectionUtility
	{
		// Token: 0x06007812 RID: 30738 RVA: 0x002A524C File Offset: 0x002A344C
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

		// Token: 0x06007813 RID: 30739 RVA: 0x002A5304 File Offset: 0x002A3504
		public static bool SelectableByHotkey(Thing t)
		{
			return t.def.selectable && t.Spawned;
		}

		// Token: 0x06007814 RID: 30740 RVA: 0x002A531B File Offset: 0x002A351B
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

		// Token: 0x06007815 RID: 30741 RVA: 0x002A532B File Offset: 0x002A352B
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

		// Token: 0x06007816 RID: 30742 RVA: 0x002A533C File Offset: 0x002A353C
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

		// Token: 0x06007817 RID: 30743 RVA: 0x002A53F0 File Offset: 0x002A35F0
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

		// Token: 0x06007818 RID: 30744 RVA: 0x002A54F0 File Offset: 0x002A36F0
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

		// Token: 0x040042CC RID: 17100
		private static HashSet<Thing> yieldedThings = new HashSet<Thing>();

		// Token: 0x040042CD RID: 17101
		private static HashSet<Zone> yieldedZones = new HashSet<Zone>();

		// Token: 0x040042CE RID: 17102
		private static List<Pawn> tmpColonists = new List<Pawn>();
	}
}
