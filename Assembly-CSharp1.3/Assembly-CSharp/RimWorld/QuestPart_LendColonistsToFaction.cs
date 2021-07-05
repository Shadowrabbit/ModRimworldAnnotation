using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B84 RID: 2948
	public class QuestPart_LendColonistsToFaction : QuestPartActivable
	{
		// Token: 0x17000C12 RID: 3090
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x0016D1A4 File Offset: 0x0016B3A4
		public List<Thing> LentColonistsListForReading
		{
			get
			{
				return this.lentColonists;
			}
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0016D1AC File Offset: 0x0016B3AC
		protected override void Enable(SignalArgs receivedArgs)
		{
			base.Enable(receivedArgs);
			CompTransporter compTransporter = this.shuttle.TryGetComp<CompTransporter>();
			if (this.lendColonistsToFaction != null && compTransporter != null)
			{
				using (IEnumerator<Thing> enumerator = ((IEnumerable<Thing>)compTransporter.innerContainer).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn;
						if ((pawn = (enumerator.Current as Pawn)) != null && pawn.IsFreeColonist)
						{
							this.lentColonists.Add(pawn);
						}
					}
				}
				this.returnColonistsOnTick = GenTicks.TicksGame + this.returnLentColonistsInTicks;
			}
		}

		// Token: 0x17000C13 RID: 3091
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x0016D240 File Offset: 0x0016B440
		public override string DescriptionPart
		{
			get
			{
				if (base.State == QuestPartState.Disabled || this.lentColonists.Count == 0)
				{
					return null;
				}
				return "PawnsLent".Translate((from t in this.lentColonists
				select t.LabelShort).ToCommaList(true, false), Mathf.Max(this.returnColonistsOnTick - GenTicks.TicksGame, 0).ToStringTicksToDays("0.0"));
			}
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x0016D2CB File Offset: 0x0016B4CB
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.returnLentColonistsInTicks)
			{
				base.Complete();
			}
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x0016D2F4 File Offset: 0x0016B4F4
		protected override void Complete(SignalArgs signalArgs)
		{
			Map map = (this.returnMap == null) ? Find.AnyPlayerHomeMap : this.returnMap.Map;
			if (map == null)
			{
				return;
			}
			base.Complete(new SignalArgs(new LookTargets(this.lentColonists).Named("SUBJECT"), (from c in this.lentColonists
			select c.LabelShort).ToCommaList(true, false).Named("PAWNS")));
			if (this.lendColonistsToFaction != null && this.lendColonistsToFaction == Faction.OfEmpire)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
				thing.SetFaction(Faction.OfEmpire, null);
				TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, this.lentColonists, thing);
				transportShip.ArriveAt(DropCellFinder.GetBestShuttleLandingSpot(map, Faction.OfEmpire), map.Parent);
				transportShip.AddJobs(new ShipJobDef[]
				{
					ShipJobDefOf.Unload,
					ShipJobDefOf.FlyAway
				});
				return;
			}
			DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map), map, this.lentColonists, 110, false, false, false, false);
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0016D404 File Offset: 0x0016B604
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			if (this.lentColonists.Contains(pawn))
			{
				Building_Grave assignedGrave = null;
				if (pawn.ownership != null)
				{
					assignedGrave = pawn.ownership.AssignedGrave;
				}
				Corpse val = pawn.MakeCorpse(assignedGrave, false, 0f);
				this.lentColonists.Remove(pawn);
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				if (anyPlayerHomeMap != null)
				{
					DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(anyPlayerHomeMap), anyPlayerHomeMap, Gen.YieldSingle<Corpse>(val), 110, false, false, false, false);
				}
				if (!this.outSignalColonistsDied.NullOrEmpty() && this.lentColonists.Count == 0)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalColonistsDied));
				}
			}
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x0016D4A4 File Offset: 0x0016B6A4
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true))
			{
				base.Complete();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0016D508 File Offset: 0x0016B708
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.lendColonistsToFaction, "lendColonistsToFaction", false);
			Scribe_Values.Look<int>(ref this.returnLentColonistsInTicks, "returnLentColonistsInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.returnColonistsOnTick, "colonistsReturnOnTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.lentColonists, "lentPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.returnMap, "returnMap", false);
			Scribe_Values.Look<string>(ref this.outSignalColonistsDied, "outSignalColonistsDied", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lentColonists.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x040029CF RID: 10703
		public Thing shuttle;

		// Token: 0x040029D0 RID: 10704
		public Faction lendColonistsToFaction;

		// Token: 0x040029D1 RID: 10705
		public int returnLentColonistsInTicks = -1;

		// Token: 0x040029D2 RID: 10706
		public MapParent returnMap;

		// Token: 0x040029D3 RID: 10707
		public string outSignalColonistsDied;

		// Token: 0x040029D4 RID: 10708
		private int returnColonistsOnTick;

		// Token: 0x040029D5 RID: 10709
		private List<Thing> lentColonists = new List<Thing>();
	}
}
