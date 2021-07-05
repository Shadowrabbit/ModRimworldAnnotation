using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000224 RID: 548
	public sealed class TooltipGiverList
	{
		// Token: 0x06000F90 RID: 3984 RVA: 0x0005866A File Offset: 0x0005686A
		public void Notify_ThingSpawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Add(t);
			}
		}

		// Token: 0x06000F91 RID: 3985 RVA: 0x0005868E File Offset: 0x0005688E
		public void Notify_ThingDespawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Remove(t);
			}
		}

		// Token: 0x06000F92 RID: 3986 RVA: 0x000586B4 File Offset: 0x000568B4
		public void DispenseAllThingTooltips()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (Find.WindowStack.FloatMenu != null)
			{
				return;
			}
			if (Find.Targeter.IsTargeting && Find.Targeter.targetingSource != null && Find.Targeter.targetingSource.HidePawnTooltips)
			{
				return;
			}
			CellRect currentViewRect = Find.CameraDriver.CurrentViewRect;
			float cellSizePixels = Find.CameraDriver.CellSizePixels;
			Vector2 vector = new Vector2(cellSizePixels, cellSizePixels);
			Rect rect = new Rect(0f, 0f, vector.x, vector.y);
			int num = 0;
			for (int i = 0; i < this.givers.Count; i++)
			{
				Thing thing = this.givers[i];
				if (currentViewRect.Contains(thing.Position) && !thing.Position.Fogged(thing.Map))
				{
					Vector2 vector2 = thing.DrawPos.MapToUIPosition();
					rect.x = vector2.x - vector.x / 2f;
					rect.y = vector2.y - vector.y / 2f;
					if (rect.Contains(Event.current.mousePosition))
					{
						string text = this.ShouldShowShotReport(thing) ? TooltipUtility.ShotCalculationTipString(thing) : null;
						if (thing.def.hasTooltip || !text.NullOrEmpty())
						{
							TipSignal tooltip = thing.GetTooltip();
							if (!text.NullOrEmpty())
							{
								tooltip.text = tooltip.text + "\n\n" + text;
							}
							TooltipHandler.TipRegion(rect, tooltip);
						}
					}
					num++;
				}
			}
		}

		// Token: 0x06000F93 RID: 3987 RVA: 0x00058859 File Offset: 0x00056A59
		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}

		// Token: 0x04000C44 RID: 3140
		private List<Thing> givers = new List<Thing>();
	}
}
