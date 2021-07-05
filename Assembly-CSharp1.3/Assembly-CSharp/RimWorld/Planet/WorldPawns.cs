using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001804 RID: 6148
	public class WorldPawns : IExposable
	{
		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x06008FB1 RID: 36785 RVA: 0x00336C6D File Offset: 0x00334E6D
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

		// Token: 0x1700178D RID: 6029
		// (get) Token: 0x06008FB2 RID: 36786 RVA: 0x00336CA2 File Offset: 0x00334EA2
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

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06008FB3 RID: 36787 RVA: 0x00336CD7 File Offset: 0x00334ED7
		public HashSet<Pawn> AllPawnsDead
		{
			get
			{
				return this.pawnsDead;
			}
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x06008FB4 RID: 36788 RVA: 0x00336CDF File Offset: 0x00334EDF
		public HashSet<Pawn> ForcefullyKeptPawns
		{
			get
			{
				return this.pawnsForcefullyKeptAsWorldPawns;
			}
		}

		// Token: 0x06008FB5 RID: 36789 RVA: 0x00336CE8 File Offset: 0x00334EE8
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
					}), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1148571423);
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
					}), WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 8765780);
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
					Log.ErrorOnce("Dead null world pawn detected, discarding.", 94424128);
					WorldPawns.tmpPawnsToRemove.Add(pawn);
				}
				else if (pawn.Discarded)
				{
					Log.Error("World pawn " + pawn + " has been discarded while still being a world pawn. This should never happen, because discard destroy mode means that the pawn is no longer managed by anything. Pawn should have been removed from the world first.");
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
				Log.Error("Error in WorldPawnGCTick(): " + arg);
			}
		}

		// Token: 0x06008FB6 RID: 36790 RVA: 0x00336F40 File Offset: 0x00335140
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
					Log.Error("Some pawnsForcefullyKeptAsWorldPawns were null after loading.");
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsAlive were null after loading.");
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsMothballed were null after loading.");
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsDead were null after loading.");
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsAlive had null def after loading.");
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsMothballed had null def after loading.");
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x.def == null || x.kindDef == null) != 0)
				{
					Log.Error("Some pawnsDead had null def after loading.");
				}
			}
		}

		// Token: 0x06008FB7 RID: 36791 RVA: 0x00337146 File Offset: 0x00335346
		public bool Contains(Pawn p)
		{
			return this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p) || this.pawnsDead.Contains(p);
		}

		// Token: 0x06008FB8 RID: 36792 RVA: 0x00337174 File Offset: 0x00335374
		public void PassToWorld(Pawn pawn, PawnDiscardDecideMode discardMode = PawnDiscardDecideMode.Decide)
		{
			if (pawn.Spawned)
			{
				Log.Error("Tried to call PassToWorld with spawned pawn: " + pawn + ". Despawn him first.");
				return;
			}
			if (this.Contains(pawn))
			{
				Log.Error("Tried to pass pawn " + pawn + " to world, but it's already here.");
				return;
			}
			if (discardMode == PawnDiscardDecideMode.KeepForever && pawn.Discarded)
			{
				Log.Error("Tried to pass a discarded pawn " + pawn + " to world with discardMode=Keep. Discarded pawns should never be stored in WorldPawns.");
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

		// Token: 0x06008FB9 RID: 36793 RVA: 0x00337228 File Offset: 0x00335428
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
				}));
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
					Log.Error("Exception ticking mothballed world pawn (just before removing): " + arg);
				}
			}
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Remove(p);
			this.pawnsForcefullyKeptAsWorldPawns.Remove(p);
			p.becameWorldPawnTickAbs = -1;
		}

		// Token: 0x06008FBA RID: 36794 RVA: 0x00337314 File Offset: 0x00335514
		public void RemoveAndDiscardPawnViaGC(Pawn p)
		{
			this.RemovePawn(p);
			this.DiscardPawn(p, true);
		}

		// Token: 0x06008FBB RID: 36795 RVA: 0x00337328 File Offset: 0x00335528
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
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(p))
			{
				return WorldPawnSituation.ReservedByQuest;
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
			if (p.teleporting)
			{
				return WorldPawnSituation.Teleporting;
			}
			return WorldPawnSituation.Free;
		}

		// Token: 0x06008FBC RID: 36796 RVA: 0x003373A8 File Offset: 0x003355A8
		public IEnumerable<Pawn> GetPawnsBySituation(WorldPawnSituation situation)
		{
			return from x in this.AllPawnsAliveOrDead
			where this.GetSituation(x) == situation
			select x;
		}

		// Token: 0x06008FBD RID: 36797 RVA: 0x003373E0 File Offset: 0x003355E0
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

		// Token: 0x06008FBE RID: 36798 RVA: 0x00337488 File Offset: 0x00335688
		private bool ShouldAutoTendTo(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Destroyed && pawn.IsHashIntervalTick(7500) && !pawn.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(pawn);
		}

		// Token: 0x06008FBF RID: 36799 RVA: 0x003374BA File Offset: 0x003356BA
		public bool IsBeingDiscarded(Pawn p)
		{
			return this.pawnsBeingDiscarded.Contains(p);
		}

		// Token: 0x06008FC0 RID: 36800 RVA: 0x003374C8 File Offset: 0x003356C8
		public void Notify_PawnDestroyed(Pawn p)
		{
			if (this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p))
			{
				this.pawnsAlive.Remove(p);
				this.pawnsMothballed.Remove(p);
				this.pawnsDead.Add(p);
			}
		}

		// Token: 0x06008FC1 RID: 36801 RVA: 0x00337518 File Offset: 0x00335718
		private bool ShouldMothball(Pawn p)
		{
			return this.DefPreventingMothball(p) == null && !p.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(p);
		}

		// Token: 0x06008FC2 RID: 36802 RVA: 0x00337538 File Offset: 0x00335738
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

		// Token: 0x06008FC3 RID: 36803 RVA: 0x00337598 File Offset: 0x00335798
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
					}), p.thingIDNumber ^ 8765780);
				}
				this.pawnsAlive.Add(p);
			}
			p.Notify_PassedToWorld();
		}

		// Token: 0x06008FC4 RID: 36804 RVA: 0x00337650 File Offset: 0x00335850
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

		// Token: 0x06008FC5 RID: 36805 RVA: 0x003376A8 File Offset: 0x003358A8
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
					Log.ErrorOnce("Exception ticking mothballed world pawn. Suppressing further errors. " + arg, WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1535437893);
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

		// Token: 0x06008FC6 RID: 36806 RVA: 0x00337794 File Offset: 0x00335994
		public void DebugRunMothballProcessing()
		{
			this.DoMothballProcessing();
			Log.Message(string.Format("World pawn mothball run complete", Array.Empty<object>()));
		}

		// Token: 0x06008FC7 RID: 36807 RVA: 0x003377B0 File Offset: 0x003359B0
		public void UnpinAllForcefullyKeptPawns()
		{
			this.pawnsForcefullyKeptAsWorldPawns.Clear();
		}

		// Token: 0x06008FC8 RID: 36808 RVA: 0x003377C0 File Offset: 0x003359C0
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008FC9 RID: 36809 RVA: 0x00337A0C File Offset: 0x00335C0C
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
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04005A49 RID: 23113
		private HashSet<Pawn> pawnsAlive = new HashSet<Pawn>();

		// Token: 0x04005A4A RID: 23114
		private HashSet<Pawn> pawnsMothballed = new HashSet<Pawn>();

		// Token: 0x04005A4B RID: 23115
		private HashSet<Pawn> pawnsDead = new HashSet<Pawn>();

		// Token: 0x04005A4C RID: 23116
		private HashSet<Pawn> pawnsForcefullyKeptAsWorldPawns = new HashSet<Pawn>();

		// Token: 0x04005A4D RID: 23117
		public WorldPawnGC gc = new WorldPawnGC();

		// Token: 0x04005A4E RID: 23118
		private Stack<Pawn> pawnsBeingDiscarded = new Stack<Pawn>();

		// Token: 0x04005A4F RID: 23119
		private const int TendIntervalTicks = 7500;

		// Token: 0x04005A50 RID: 23120
		private const int MothballUpdateInterval = 15000;

		// Token: 0x04005A51 RID: 23121
		private List<Pawn> allPawnsAliveOrDeadResult = new List<Pawn>();

		// Token: 0x04005A52 RID: 23122
		private List<Pawn> allPawnsAliveResult = new List<Pawn>();

		// Token: 0x04005A53 RID: 23123
		private static List<Pawn> tmpPawnsToTick = new List<Pawn>();

		// Token: 0x04005A54 RID: 23124
		private static List<Pawn> tmpPawnsToRemove = new List<Pawn>();
	}
}
