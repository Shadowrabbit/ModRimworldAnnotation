using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183C RID: 6204
	public class CompShipLandingBeacon : ThingComp
	{
		// Token: 0x17001593 RID: 5523
		// (get) Token: 0x0600898B RID: 35211 RVA: 0x0005C606 File Offset: 0x0005A806
		public CompProperties_ShipLandingBeacon Props
		{
			get
			{
				return (CompProperties_ShipLandingBeacon)this.props;
			}
		}

		// Token: 0x17001594 RID: 5524
		// (get) Token: 0x0600898C RID: 35212 RVA: 0x0005C613 File Offset: 0x0005A813
		public List<ShipLandingArea> LandingAreas
		{
			get
			{
				return this.landingAreas;
			}
		}

		// Token: 0x17001595 RID: 5525
		// (get) Token: 0x0600898D RID: 35213 RVA: 0x00282754 File Offset: 0x00280954
		public bool Active
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp == null || comp.PowerOn;
			}
		}

		// Token: 0x0600898E RID: 35214 RVA: 0x0005C61B File Offset: 0x0005A81B
		private bool CanLinkTo(CompShipLandingBeacon other)
		{
			return other != this && ShipLandingBeaconUtility.CanLinkTo(this.parent.Position, other);
		}

		// Token: 0x0600898F RID: 35215 RVA: 0x00282778 File Offset: 0x00280978
		public void EstablishConnections()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			List<CompShipLandingBeacon> list = new List<CompShipLandingBeacon>();
			List<CompShipLandingBeacon> list2 = new List<CompShipLandingBeacon>();
			List<Thing> list3 = this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon);
			foreach (Thing thing in list3)
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null && this.CanLinkTo(compShipLandingBeacon))
				{
					if (this.parent.Position.x == compShipLandingBeacon.parent.Position.x)
					{
						list2.Add(compShipLandingBeacon);
					}
					else if (this.parent.Position.z == compShipLandingBeacon.parent.Position.z)
					{
						list.Add(compShipLandingBeacon);
					}
				}
			}
			using (List<CompShipLandingBeacon>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CompShipLandingBeacon h = enumerator2.Current;
					using (List<CompShipLandingBeacon>.Enumerator enumerator3 = list2.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							CompShipLandingBeacon v = enumerator3.Current;
							Thing thing2 = list3.FirstOrDefault((Thing x) => x.Position.x == h.parent.Position.x && x.Position.z == v.parent.Position.z);
							if (thing2 != null)
							{
								this.TryAddArea(new ShipLandingArea(CellRect.FromLimits(thing2.Position, this.parent.Position).ContractedBy(1), this.parent.Map)
								{
									beacons = new List<CompShipLandingBeacon>
									{
										this,
										thing2.TryGetComp<CompShipLandingBeacon>(),
										v,
										h
									}
								});
							}
						}
					}
				}
			}
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				using (List<CompShipLandingBeacon>.Enumerator enumerator2 = this.landingAreas[i].beacons.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (!enumerator2.Current.TryAddArea(this.landingAreas[i]))
						{
							this.RemoveArea(this.landingAreas[i]);
							break;
						}
					}
				}
			}
		}

		// Token: 0x06008990 RID: 35216 RVA: 0x00282A50 File Offset: 0x00280C50
		private void RemoveArea(ShipLandingArea area)
		{
			foreach (CompShipLandingBeacon compShipLandingBeacon in area.beacons)
			{
				if (compShipLandingBeacon.landingAreas.Contains(area))
				{
					compShipLandingBeacon.landingAreas.Remove(area);
				}
			}
			this.landingAreas.Remove(area);
		}

		// Token: 0x06008991 RID: 35217 RVA: 0x00282AC4 File Offset: 0x00280CC4
		public bool TryAddArea(ShipLandingArea newArea)
		{
			if (!this.landingAreas.Contains(newArea))
			{
				for (int i = this.landingAreas.Count - 1; i >= 0; i--)
				{
					if (this.landingAreas[i].MyRect.Overlaps(newArea.MyRect) && this.landingAreas[i].MyRect != newArea.MyRect)
					{
						if (this.landingAreas[i].MyRect.Area <= newArea.MyRect.Area)
						{
							return false;
						}
						this.RemoveArea(this.landingAreas[i]);
					}
				}
				this.landingAreas.Add(newArea);
			}
			return true;
		}

		// Token: 0x06008992 RID: 35218 RVA: 0x00282B8C File Offset: 0x00280D8C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			CompGlower compGlower = this.parent.TryGetComp<CompGlower>();
			if (compGlower != null)
			{
				this.fieldColor = compGlower.Props.glowColor.ToColor.ToOpaque();
			}
			this.EstablishConnections();
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x06008993 RID: 35219 RVA: 0x00282C0C File Offset: 0x00280E0C
		public override void PostDeSpawn(Map map)
		{
			for (int i = this.landingAreas.Count - 1; i >= 0; i--)
			{
				this.RemoveArea(this.landingAreas[i]);
			}
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				CompShipLandingBeacon compShipLandingBeacon = thing.TryGetComp<CompShipLandingBeacon>();
				if (compShipLandingBeacon != null)
				{
					compShipLandingBeacon.EstablishConnections();
				}
			}
		}

		// Token: 0x06008994 RID: 35220 RVA: 0x00282C9C File Offset: 0x00280E9C
		public override void CompTickRare()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				shipLandingArea.RecalculateBlockingThing();
			}
		}

		// Token: 0x06008995 RID: 35221 RVA: 0x00282CEC File Offset: 0x00280EEC
		public override void PostDrawExtraSelectionOverlays()
		{
			foreach (ShipLandingArea shipLandingArea in this.landingAreas)
			{
				if (shipLandingArea.Active)
				{
					Color color = shipLandingArea.Clear ? this.fieldColor : Color.red;
					color.a = Pulser.PulseBrightness(1f, 0.6f);
					GenDraw.DrawFieldEdges(shipLandingArea.MyRect.ToList<IntVec3>(), color);
				}
				foreach (CompShipLandingBeacon compShipLandingBeacon in shipLandingArea.beacons)
				{
					if (this.CanLinkTo(compShipLandingBeacon))
					{
						GenDraw.DrawLineBetween(this.parent.TrueCenter(), compShipLandingBeacon.parent.TrueCenter(), SimpleColor.White);
					}
				}
			}
		}

		// Token: 0x06008996 RID: 35222 RVA: 0x00282DEC File Offset: 0x00280FEC
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			string text = "";
			if (!this.Active)
			{
				text += "NotUsable".Translate() + ": " + "Unpowered".Translate().CapitalizeFirst();
			}
			int i = 0;
			while (i < this.landingAreas.Count)
			{
				if (!this.landingAreas[i].Clear)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": ";
					if (this.landingAreas[i].BlockedByRoof)
					{
						text += "BlockedByRoof".Translate().CapitalizeFirst();
						break;
					}
					text += "BlockedBy".Translate(this.landingAreas[i].FirstBlockingThing).CapitalizeFirst();
					break;
				}
				else
				{
					i++;
				}
			}
			foreach (Thing thing in this.parent.Map.listerThings.ThingsOfDef(ThingDefOf.ShipLandingBeacon))
			{
				if (thing != this.parent && ShipLandingBeaconUtility.AlignedDistanceTooShort(this.parent.Position, thing.Position, this.Props.edgeLengthRange.min - 1f))
				{
					if (!text.NullOrEmpty())
					{
						text += "\n";
					}
					text += "NotUsable".Translate() + ": " + "TooCloseToOtherBeacon".Translate().CapitalizeFirst();
					break;
				}
			}
			return text;
		}

		// Token: 0x0400582B RID: 22571
		private List<ShipLandingArea> landingAreas = new List<ShipLandingArea>();

		// Token: 0x0400582C RID: 22572
		private Color fieldColor = Color.white;
	}
}
