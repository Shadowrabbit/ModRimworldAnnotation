using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000315 RID: 789
	public sealed class TooltipGiverList
	{
		// Token: 0x06001416 RID: 5142 RVA: 0x00014702 File Offset: 0x00012902
		public void Notify_ThingSpawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Add(t);
			}
		}

		// Token: 0x06001417 RID: 5143 RVA: 0x00014726 File Offset: 0x00012926
		public void Notify_ThingDespawned(Thing t)
		{
			if (t.def.hasTooltip || this.ShouldShowShotReport(t))
			{
				this.givers.Remove(t);
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x000CD344 File Offset: 0x000CB544
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

		// Token: 0x06001419 RID: 5145 RVA: 0x0001474B File Offset: 0x0001294B
		private bool ShouldShowShotReport(Thing t)
		{
			return t.def.hasTooltip || t is Hive || t is IAttackTarget;
		}

		// Token: 0x04000FCF RID: 4047
		private List<Thing> givers = new List<Thing>();
	}
}
