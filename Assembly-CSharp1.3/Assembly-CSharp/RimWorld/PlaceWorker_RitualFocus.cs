using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001562 RID: 5474
	[StaticConstructorOnStartup]
	public class PlaceWorker_RitualFocus : PlaceWorker
	{
		// Token: 0x170015F1 RID: 5617
		// (get) Token: 0x060081AB RID: 33195 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool UseArrow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060081AC RID: 33196 RVA: 0x002DD754 File Offset: 0x002DB954
		public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			CellRect cellRect;
			PlaceWorker_SpectatorPreview.DrawSpectatorPreview(def, center, rot, ghostCol, this.UseArrow, out cellRect, thing);
			List<Thing> forCell = Find.CurrentMap.listerBuldingOfDefInProximity.GetForCell(center, 35f, ThingDefOf.Lectern, null);
			try
			{
				foreach (Thing thing2 in forCell)
				{
					PlaceWorker_RitualPosition placeWorker_RitualPosition = (PlaceWorker_RitualPosition)thing2.def.PlaceWorkers.FirstOrDefault((PlaceWorker w) => w is PlaceWorker_RitualPosition);
					if (placeWorker_RitualPosition != null)
					{
						placeWorker_RitualPosition.DrawGhost(thing2.def, thing2.Position, thing2.Rotation, ghostCol, thing2);
						if (cellRect.ClosestCellTo(thing2.Position).InHorDistOf(thing2.Position, 4.9f))
						{
							PlaceWorker_RitualFocus.tmpConnectedRitualSeats.AddRange(PlaceWorker_RitualPosition.RitualSeatsInRange(thing2.def, thing2.Position, thing2.Rotation, thing2));
							GenDraw.DrawLineBetween(GenThing.TrueCenter(center, Rot4.North, def.size, def.Altitude), thing2.TrueCenter(), SimpleColor.Green, 0.2f);
						}
					}
				}
				PlaceWorker_RitualPosition.DrawRitualSeatConnections(def, center, rot, thing, PlaceWorker_RitualFocus.tmpConnectedRitualSeats);
			}
			finally
			{
				PlaceWorker_RitualFocus.tmpConnectedRitualSeats.Clear();
			}
		}

		// Token: 0x040050B5 RID: 20661
		private static List<Thing> tmpConnectedRitualSeats = new List<Thing>();
	}
}
