using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010E4 RID: 4324
	public class QuestPart_LendColonistsToFaction : QuestPartActivable
	{
		// Token: 0x17000EA8 RID: 3752
		// (get) Token: 0x06005E5D RID: 24157 RVA: 0x000415F1 File Offset: 0x0003F7F1
		public List<Thing> LentColonistsListForReading
		{
			get
			{
				return this.lentColonists;
			}
		}

		// Token: 0x06005E5E RID: 24158 RVA: 0x001DF158 File Offset: 0x001DD358
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

		// Token: 0x17000EA9 RID: 3753
		// (get) Token: 0x06005E5F RID: 24159 RVA: 0x001DF1EC File Offset: 0x001DD3EC
		public override string DescriptionPart
		{
			get
			{
				if (base.State == QuestPartState.Disabled || this.lentColonists.Count == 0)
				{
					return null;
				}
				return "PawnsLent".Translate((from t in this.lentColonists
				select t.LabelShort).ToCommaList(true), Mathf.Max(this.returnColonistsOnTick - GenTicks.TicksGame, 0).ToStringTicksToDays("0.0"));
			}
		}

		// Token: 0x06005E60 RID: 24160 RVA: 0x000415F9 File Offset: 0x0003F7F9
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.returnLentColonistsInTicks)
			{
				base.Complete();
			}
		}

		// Token: 0x06005E61 RID: 24161 RVA: 0x001DF278 File Offset: 0x001DD478
		protected override void Complete(SignalArgs signalArgs)
		{
			Map map = (this.returnMap == null) ? Find.AnyPlayerHomeMap : this.returnMap.Map;
			if (map == null)
			{
				return;
			}
			base.Complete(new SignalArgs(new LookTargets(this.lentColonists).Named("SUBJECT"), (from c in this.lentColonists
			select c.LabelShort).ToCommaList(true).Named("PAWNS")));
			if (this.lendColonistsToFaction == Faction.Empire)
			{
				SkyfallerUtility.MakeDropoffShuttle(map, this.lentColonists, Faction.Empire);
				return;
			}
			DropPodUtility.DropThingsNear(DropCellFinder.TradeDropSpot(map), map, this.lentColonists, 110, false, false, false, false);
		}

		// Token: 0x06005E62 RID: 24162 RVA: 0x001DF338 File Offset: 0x001DD538
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
			}
		}

		// Token: 0x06005E63 RID: 24163 RVA: 0x001DF3A8 File Offset: 0x001DD5A8
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

		// Token: 0x06005E64 RID: 24164 RVA: 0x001DF40C File Offset: 0x001DD60C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<Faction>(ref this.lendColonistsToFaction, "lendColonistsToFaction", false);
			Scribe_Values.Look<int>(ref this.returnLentColonistsInTicks, "returnLentColonistsInTicks", 0, false);
			Scribe_Values.Look<int>(ref this.returnColonistsOnTick, "colonistsReturnOnTick", 0, false);
			Scribe_Collections.Look<Thing>(ref this.lentColonists, "lentPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.returnMap, "returnMap", false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.lentColonists.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x04003F12 RID: 16146
		public Thing shuttle;

		// Token: 0x04003F13 RID: 16147
		public Faction lendColonistsToFaction;

		// Token: 0x04003F14 RID: 16148
		public int returnLentColonistsInTicks = -1;

		// Token: 0x04003F15 RID: 16149
		public MapParent returnMap;

		// Token: 0x04003F16 RID: 16150
		private int returnColonistsOnTick;

		// Token: 0x04003F17 RID: 16151
		private List<Thing> lentColonists = new List<Thing>();
	}
}
