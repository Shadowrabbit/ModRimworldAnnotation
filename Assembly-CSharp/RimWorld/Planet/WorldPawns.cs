using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021BB RID: 8635
	public class WorldPawns : IExposable
	{
		// Token: 0x17001B6C RID: 7020
		// (get) Token: 0x0600B8C0 RID: 47296 RVA: 0x00077B85 File Offset: 0x00075D85
		public List<Pawn> AllPawnsAliveOrDead
		{
			get
			{
				this.allPawnsAliveOrDeadResult.Clear();
				this.allPawnsAliveOrDeadResult.AddRange(this.AllPawnsAlive);
				this.allPawnsAliveOrDeadResult.AddRange(this.AllPawnsDead);
				return this.allPawnsAliveOrDeadResult;
			}
		}

		// Token: 0x17001B6D RID: 7021
		// (get) Token: 0x0600B8C1 RID: 47297 RVA: 0x00077BBA File Offset: 0x00075DBA
		public List<Pawn> AllPawnsAlive
		{
			get
			{
				this.allPawnsAliveResult.Clear();
				this.allPawnsAliveResult.AddRange(this.pawnsAlive);
				this.allPawnsAliveResult.AddRange(this.pawnsMothballed);
				return this.allPawnsAliveResult;
			}
		}

		// Token: 0x17001B6E RID: 7022
		// (get) Token: 0x0600B8C2 RID: 47298 RVA: 0x00077BEF File Offset: 0x00075DEF
		public HashSet<Pawn> AllPawnsDead
		{
			get
			{
				return this.pawnsDead;
			}
		}

		// Token: 0x17001B6F RID: 7023
		// (get) Token: 0x0600B8C3 RID: 47299 RVA: 0x00077BF7 File Offset: 0x00075DF7
		public HashSet<Pawn> ForcefullyKeptPawns
		{
			get
			{
				return this.pawnsForcefullyKeptAsWorldPawns;
			}
		}

		// Token: 0x0600B8C4 RID: 47300 RVA: 0x003518CC File Offset: 0x0034FACC
		public void WorldPawnsTick()
		{
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				try
				{
					WorldPawns.tmpPawnsToTick[i].Tick();
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception ticking world pawn ",
						WorldPawns.tmpPawnsToTick[i].ToStringSafe<Pawn>(),
						". Suppressing further errors. ",
						ex
					}), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1148571423, false);
				}
				try
				{
					if (this.ShouldAutoTendTo(WorldPawns.tmpPawnsToTick[i]))
					{
						TendUtility.DoTend(null, WorldPawns.tmpPawnsToTick[i], null);
					}
				}
				catch (Exception ex2)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception tending to a world pawn ",
						WorldPawns.tmpPawnsToTick[i].ToStringSafe<Pawn>(),
						". Suppressing further errors. ",
						ex2
					}), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 8765780, false);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
			if (Find.TickManager.TicksGame % 15000 == 0)
			{
				this.DoMothballProcessing();
			}
			WorldPawns.tmpPawnsToRemove.Clear();
			foreach (Pawn pawn in this.pawnsDead)
			{
				if (pawn == null)
				{
					Log.ErrorOnce("Dead null world pawn detected, discarding.", 94424128, false);
					WorldPawns.tmpPawnsToRemove.Add(pawn);
				}
				else if (pawn.Discarded)
				{
					Log.Error("World pawn " + pawn + " has been discarded while still being a world pawn. This should never happen, because discard destroy mode means that the pawn is no longer managed by anything. Pawn should have been removed from the world first.", false);
					WorldPawns.tmpPawnsToRemove.Add(pawn);
				}
			}
			for (int j = 0; j < WorldPawns.tmpPawnsToRemove.Count; j++)
			{
				this.pawnsDead.Remove(WorldPawns.tmpPawnsToRemove[j]);
			}
			WorldPawns.tmpPawnsToRemove.Clear();
			try
			{
				this.gc.WorldPawnGCTick();
			}
			catch (Exception arg)
			{
				Log.Error("Error in WorldPawnGCTick(): " + arg, false);
			}
		}

		// Token: 0x0600B8C5 RID: 47301 RVA: 0x00351B28 File Offset: 0x0034FD28
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcefullyKeptAsWorldPawns, true, "pawnsForcefullyKeptAsWorldPawns", LookMode.Reference);
			Scribe_Collections.Look<Pawn>(ref this.pawnsAlive, "pawnsAlive", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsMothballed, "pawnsMothballed", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsDead, true, "pawnsDead", LookMode.Deep);
			Scribe_Deep.Look<WorldPawnGC>(ref this.gc, "gc", Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.pawnsMothballed == null)
				{
					this.pawnsMothballed = new HashSet<Pawn>();
				}
				if (this.pawnsForcefullyKeptAsWorldPawns.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsForcefullyKeptAsWorldPawns were null after loading.", false);
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsAlive were null after loading.", false);
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsMothballed were null after loading.", false);
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsDead were null after loading.", false);
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsAlive had null def after loading.", false);
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsMothballed had null def after loading.", false);
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsDead had null def after loading.", false);
				}
			}
		}

		// Token: 0x0600B8C6 RID: 47302 RVA: 0x00077BFF File Offset: 0x00075DFF
		public bool Contains(Pawn p)
		{
			return this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p) || this.pawnsDead.Contains(p);
		}

		// Token: 0x0600B8C7 RID: 47303 RVA: 0x00351D38 File Offset: 0x0034FF38
		public void PassToWorld(Pawn pawn, PawnDiscardDecideMode discardMode = PawnDiscardDecideMode.Decide)
		{
			if (pawn.Spawned)
			{
				Log.Error("Tried to call PassToWorld with spawned pawn: " + pawn + ". Despawn him first.", false);
				return;
			}
			if (this.Contains(pawn))
			{
				Log.Error("Tried to pass pawn " + pawn + " to world, but it's already here.", false);
				return;
			}
			if (discardMode == PawnDiscardDecideMode.KeepForever && pawn.Discarded)
			{
				Log.Error("Tried to pass a discarded pawn " + pawn + " to world with discardMode=Keep. Discarded pawns should never be stored in WorldPawns.", false);
				discardMode = PawnDiscardDecideMode.Decide;
			}
			if (PawnComponentsUtility.HasSpawnedComponents(pawn))
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
			}
			switch (discardMode)
			{
			case PawnDiscardDecideMode.Decide:
				this.AddPawn(pawn);
				return;
			case PawnDiscardDecideMode.KeepForever:
				this.pawnsForcefullyKeptAsWorldPawns.Add(pawn);
				this.AddPawn(pawn);
				return;
			case PawnDiscardDecideMode.Discard:
				this.DiscardPawn(pawn, false);
				return;
			default:
				return;
			}
		}

		// Token: 0x0600B8C8 RID: 47304 RVA: 0x00351DF0 File Offset: 0x0034FFF0
		public void RemovePawn(Pawn p)
		{
			if (!this.Contains(p))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove pawn ",
					p,
					" from ",
					base.GetType(),
					", but it's not here."
				}), false);
			}
			this.gc.CancelGCPass();
			if (this.pawnsMothballed.Contains(p) && Find.TickManager.TicksGame % 15000 != 0)
			{
				try
				{
					p.TickMothballed(Find.TickManager.TicksGame % 15000);
				}
				catch (Exception arg)
				{
					Log.Error("Exception ticking mothballed world pawn (just before removing): " + arg, false);
				}
			}
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Remove(p);
			this.pawnsForcefullyKeptAsWorldPawns.Remove(p);
			p.becameWorldPawnTickAbs = -1;
		}

		// Token: 0x0600B8C9 RID: 47305 RVA: 0x00077C2B File Offset: 0x00075E2B
		public void RemoveAndDiscardPawnViaGC(Pawn p)
		{
			this.RemovePawn(p);
			this.DiscardPawn(p, true);
		}

		// Token: 0x0600B8CA RID: 47306 RVA: 0x00351EE0 File Offset: 0x003500E0
		public WorldPawnSituation GetSituation(Pawn p)
		{
			if (!this.Contains(p))
			{
				return WorldPawnSituation.None;
			}
			if (p.Dead || p.Destroyed)
			{
				return WorldPawnSituation.Dead;
			}
			if (PawnUtility.IsFactionLeader(p))
			{
				return WorldPawnSituation.FactionLeader;
			}
			if (PawnUtility.IsKidnappedPawn(p))
			{
				return WorldPawnSituation.Kidnapped;
			}
			if (p.IsBorrowedByAnyFaction())
			{
				return WorldPawnSituation.Borrowed;
			}
			if (p.IsCaravanMember())
			{
				return WorldPawnSituation.CaravanMember;
			}
			if (PawnUtility.IsTravelingInTransportPodWorldObject(p))
			{
				return WorldPawnSituation.InTravelingTransportPod;
			}
			if (PawnUtility.ForSaleBySettlement(p))
			{
				return WorldPawnSituation.ForSaleBySettlement;
			}
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(p))
			{
				return WorldPawnSituation.ReservedByQuest;
			}
			if (p.teleporting)
			{
				return WorldPawnSituation.Teleporting;
			}
			return WorldPawnSituation.Free;
		}

		// Token: 0x0600B8CB RID: 47307 RVA: 0x00351F60 File Offset: 0x00350160
		public IEnumerable<Pawn> GetPawnsBySituation(WorldPawnSituation situation)
		{
			return from x in this.AllPawnsAliveOrDead
			where this.GetSituation(x) == situation
			select x;
		}

		// Token: 0x0600B8CC RID: 47308 RVA: 0x00351F98 File Offset: 0x00350198
		public int GetPawnsBySituationCount(WorldPawnSituation situation)
		{
			int num = 0;
			foreach (Pawn p in this.pawnsAlive)
			{
				if (this.GetSituation(p) == situation)
				{
					num++;
				}
			}
			foreach (Pawn p2 in this.pawnsDead)
			{
				if (this.GetSituation(p2) == situation)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600B8CD RID: 47309 RVA: 0x00077C3C File Offset: 0x00075E3C
		private bool ShouldAutoTendTo(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Destroyed && pawn.IsHashIntervalTick(7500) && !pawn.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(pawn);
		}

		// Token: 0x0600B8CE RID: 47310 RVA: 0x00077C6E File Offset: 0x00075E6E
		public bool IsBeingDiscarded(Pawn p)
		{
			return this.pawnsBeingDiscarded.Contains(p);
		}

		// Token: 0x0600B8CF RID: 47311 RVA: 0x00352040 File Offset: 0x00350240
		public void Notify_PawnDestroyed(Pawn p)
		{
			if (this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p))
			{
				this.pawnsAlive.Remove(p);
				this.pawnsMothballed.Remove(p);
				this.pawnsDead.Add(p);
			}
		}

		// Token: 0x0600B8D0 RID: 47312 RVA: 0x00077C7C File Offset: 0x00075E7C
		private bool ShouldMothball(Pawn p)
		{
			return this.DefPreventingMothball(p) == null && !p.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(p);
		}

		// Token: 0x0600B8D1 RID: 47313 RVA: 0x00352090 File Offset: 0x00350290
		private HediffDef DefPreventingMothball(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (!hediffs[i].def.AlwaysAllowMothball && !hediffs[i].IsPermanent())
				{
					return hediffs[i].def;
				}
			}
			return null;
		}

		// Token: 0x0600B8D2 RID: 47314 RVA: 0x003520F0 File Offset: 0x003502F0
		private void AddPawn(Pawn p)
		{
			this.gc.CancelGCPass();
			if (p.Dead || p.Destroyed)
			{
				this.pawnsDead.Add(p);
			}
			else
			{
				try
				{
					int num = 0;
					while (this.ShouldAutoTendTo(p) && num < 30)
					{
						TendUtility.DoTend(null, p, null);
						num++;
					}
				}
				catch (Exception ex)
				{
					Log.ErrorOnce(string.Concat(new object[]
					{
						"Exception tending to a world pawn ",
						p.ToStringSafe<Pawn>(),
						". Suppressing further errors. ",
						ex
					}), p.thingIDNumber ^ 8765780, false);
				}
				this.pawnsAlive.Add(p);
			}
			p.Notify_PassedToWorld();
		}

		// Token: 0x0600B8D3 RID: 47315 RVA: 0x003521A8 File Offset: 0x003503A8
		private void DiscardPawn(Pawn p, bool silentlyRemoveReferences = false)
		{
			this.pawnsBeingDiscarded.Push(p);
			try
			{
				if (!p.Destroyed)
				{
					p.Destroy(DestroyMode.Vanish);
				}
				if (!p.Discarded)
				{
					p.Discard(silentlyRemoveReferences);
				}
			}
			finally
			{
				this.pawnsBeingDiscarded.Pop();
			}
		}

		// Token: 0x0600B8D4 RID: 47316 RVA: 0x00352200 File Offset: 0x00350400
		private void DoMothballProcessing()
		{
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsMothballed);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				try
				{
					WorldPawns.tmpPawnsToTick[i].TickMothballed(15000);
				}
				catch (Exception arg)
				{
					Log.ErrorOnce("Exception ticking mothballed world pawn. Suppressing further errors. " + arg, WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1535437893, false);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int j = 0; j < WorldPawns.tmpPawnsToTick.Count; j++)
			{
				Pawn pawn = WorldPawns.tmpPawnsToTick[j];
				if (this.ShouldMothball(pawn))
				{
					this.pawnsAlive.Remove(pawn);
					this.pawnsMothballed.Add(pawn);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
		}

		// Token: 0x0600B8D5 RID: 47317 RVA: 0x00077C9A File Offset: 0x00075E9A
		public void DebugRunMothballProcessing()
		{
			this.DoMothballProcessing();
			Log.Message(string.Format("World pawn mothball run complete", Array.Empty<object>()), false);
		}

		// Token: 0x0600B8D6 RID: 47318 RVA: 0x00077CB7 File Offset: 0x00075EB7
		public void UnpinAllForcefullyKeptPawns()
		{
			this.pawnsForcefullyKeptAsWorldPawns.Clear();
		}

		// Token: 0x0600B8D7 RID: 47319 RVA: 0x003522EC File Offset: 0x003504EC
		public void LogWorldPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns =======");
			stringBuilder.AppendLine("Count: " + this.AllPawnsAliveOrDead.Count<Pawn>());
			stringBuilder.AppendLine(string.Format("(Live: {0} - Mothballed: {1} - Dead: {2}; {3} forcefully kept)", new object[]
			{
				this.pawnsAlive.Count,
				this.pawnsMothballed.Count,
				this.pawnsDead.Count,
				this.pawnsForcefullyKeptAsWorldPawns.Count
			}));
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("== " + worldPawnSituation + " ==");
					foreach (Pawn pawn in this.GetPawnsBySituation(worldPawnSituation).OrderBy(delegate(Pawn x)
					{
						if (x.Faction != null)
						{
							return x.Faction.loadID;
						}
						return -1;
					}))
					{
						string text = (pawn.Name != null) ? pawn.Name.ToStringFull : pawn.LabelCap;
						text = text + ", " + pawn.KindLabel;
						if (pawn.royalty != null && pawn.royalty.AllTitlesForReading.Count > 0)
						{
							foreach (RoyalTitle royalTitle in pawn.royalty.AllTitlesForReading)
							{
								text = text + ", " + royalTitle.def.GetLabelFor(pawn);
							}
						}
						text = text + ", " + pawn.Faction;
						stringBuilder.AppendLine(text);
					}
				}
			}
			stringBuilder.AppendLine("===========================");
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600B8D8 RID: 47320 RVA: 0x00352538 File Offset: 0x00350738
		public void LogWorldPawnMothballPrevention()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns Mothball Prevention =======");
			stringBuilder.AppendLine(string.Format("Count: {0}", this.pawnsAlive.Count<Pawn>()));
			int num = 0;
			Dictionary<HediffDef, int> dictionary = new Dictionary<HediffDef, int>();
			foreach (Pawn p in this.pawnsAlive)
			{
				HediffDef hediffDef = this.DefPreventingMothball(p);
				if (hediffDef == null)
				{
					num++;
				}
				else
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = 0;
					}
					Dictionary<HediffDef, int> dictionary2 = dictionary;
					HediffDef key = hediffDef;
					int value = dictionary2[key] + 1;
					dictionary2[key] = value;
				}
			}
			stringBuilder.AppendLine(string.Format("Will be mothballed: {0}", num));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Reasons to avoid mothballing:");
			foreach (KeyValuePair<HediffDef, int> keyValuePair in from kvp in dictionary
			orderby kvp.Value descending
			select kvp)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Value, keyValuePair.Key));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04007E22 RID: 32290
		private HashSet<Pawn> pawnsAlive = new HashSet<Pawn>();

		// Token: 0x04007E23 RID: 32291
		private HashSet<Pawn> pawnsMothballed = new HashSet<Pawn>();

		// Token: 0x04007E24 RID: 32292
		private HashSet<Pawn> pawnsDead = new HashSet<Pawn>();

		// Token: 0x04007E25 RID: 32293
		private HashSet<Pawn> pawnsForcefullyKeptAsWorldPawns = new HashSet<Pawn>();

		// Token: 0x04007E26 RID: 32294
		public WorldPawnGC gc = new WorldPawnGC();

		// Token: 0x04007E27 RID: 32295
		private Stack<Pawn> pawnsBeingDiscarded = new Stack<Pawn>();

		// Token: 0x04007E28 RID: 32296
		private const int TendIntervalTicks = 7500;

		// Token: 0x04007E29 RID: 32297
		private const int MothballUpdateInterval = 15000;

		// Token: 0x04007E2A RID: 32298
		private List<Pawn> allPawnsAliveOrDeadResult = new List<Pawn>();

		// Token: 0x04007E2B RID: 32299
		private List<Pawn> allPawnsAliveResult = new List<Pawn>();

		// Token: 0x04007E2C RID: 32300
		private static List<Pawn> tmpPawnsToTick = new List<Pawn>();

		// Token: 0x04007E2D RID: 32301
		private static List<Pawn> tmpPawnsToRemove = new List<Pawn>();
	}
}
