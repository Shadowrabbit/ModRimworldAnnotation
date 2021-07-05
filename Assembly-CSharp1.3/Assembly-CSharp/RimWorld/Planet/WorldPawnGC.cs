using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001801 RID: 6145
	public class WorldPawnGC : IExposable
	{
		// Token: 0x06008FA2 RID: 36770 RVA: 0x00336580 File Offset: 0x00334780
		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate));
					}
				}
				if (this.activeGCProcess != null)
				{
					bool flag = false;
					int num = 0;
					while (num < this.currentGCRate && !flag)
					{
						flag = !this.activeGCProcess.MoveNext();
						num++;
					}
					if (flag)
					{
						this.lastSuccessfulGCTick = Find.TickManager.TicksGame;
						this.currentGCRate = 1;
						this.activeGCProcess = null;
						if (DebugViewSettings.logWorldPawnGC)
						{
							Log.Message("World pawn GC complete");
						}
					}
				}
			}
		}

		// Token: 0x06008FA3 RID: 36771 RVA: 0x00336647 File Offset: 0x00334847
		public void CancelGCPass()
		{
			if (this.activeGCProcess != null)
			{
				this.activeGCProcess = null;
				this.currentGCRate = Mathf.Min(this.currentGCRate * 2, 16777216);
				if (DebugViewSettings.logWorldPawnGC)
				{
					Log.Message("World pawn GC cancelled");
				}
			}
		}

		// Token: 0x06008FA4 RID: 36772 RVA: 0x00336681 File Offset: 0x00334881
		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			foreach (Pawn pawn in Find.WorldPawns.AllPawnsAliveOrDead)
			{
				string criticalPawnReason = this.GetCriticalPawnReason(pawn);
				if (!criticalPawnReason.NullOrEmpty())
				{
					keptPawns[pawn] = criticalPawnReason;
					if (this.logDotgraph != null)
					{
						this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", new object[]
						{
							WorldPawnGC.DotgraphIdentifier(pawn),
							criticalPawnReason,
							(pawn.relations != null && pawn.relations.everSeenByPlayer) ? "black" : "grey",
							pawn.RaceProps.Humanlike ? "oval" : "box"
						}));
					}
				}
				else if (this.logDotgraph != null)
				{
					this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(pawn), (pawn.relations != null && pawn.relations.everSeenByPlayer) ? "black" : "grey", pawn.RaceProps.Humanlike ? "oval" : "box"));
				}
			}
			IEnumerable<Pawn> allPawnsAlive = Find.WorldPawns.AllPawnsAlive;
			Func<Pawn, bool> <>9__0;
			Func<Pawn, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Pawn p) => p.RaceProps.Humanlike && !keptPawns.ContainsKey(p) && Rand.ChanceSeeded(0.25f, p.thingIDNumber ^ 980675837)));
			}
			foreach (Pawn key in allPawnsAlive.Where(predicate).Take(10))
			{
				keptPawns[key] = "RandomlyKept";
			}
			Pawn[] criticalPawns = keptPawns.Keys.ToArray<Pawn>();
			foreach (Pawn pawn2 in criticalPawns)
			{
				this.AddAllRelationships(pawn2, keptPawns);
				yield return null;
			}
			Pawn[] array = null;
			foreach (Pawn pawn3 in criticalPawns)
			{
				this.AddAllMemories(pawn3, keptPawns);
			}
			yield break;
		}

		// Token: 0x06008FA5 RID: 36773 RVA: 0x00336698 File Offset: 0x00334898
		private Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			this.AccumulatePawnGCData(dictionary).ExecuteEnumerable();
			return dictionary;
		}

		// Token: 0x06008FA6 RID: 36774 RVA: 0x003366B8 File Offset: 0x003348B8
		public string PawnGCDebugResults()
		{
			Dictionary<Pawn, string> dictionary = this.AccumulatePawnGCDataImmediate();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (Pawn key in Find.WorldPawns.AllPawnsAlive)
			{
				string text = "Discarded";
				if (dictionary.ContainsKey(key))
				{
					text = dictionary[key];
				}
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2[text] = 0;
				}
				Dictionary<string, int> dictionary3 = dictionary2;
				string key2 = text;
				int value = dictionary3[key2] + 1;
				dictionary3[key2] = value;
			}
			return (from kvp in dictionary2
			orderby kvp.Value descending
			select string.Format("{0}: {1}", kvp.Value, kvp.Key)).ToLineList(null, false);
		}

		// Token: 0x06008FA7 RID: 36775 RVA: 0x003367AC File Offset: 0x003349AC
		public IEnumerable PawnGCPass()
		{
			Dictionary<Pawn, string> keptPawns = new Dictionary<Pawn, string>();
			Pawn[] worldPawnsSnapshot = Find.WorldPawns.AllPawnsAliveOrDead.ToArray();
			foreach (object obj in this.AccumulatePawnGCData(keptPawns))
			{
				yield return null;
			}
			IEnumerator enumerator = null;
			foreach (Pawn pawn in worldPawnsSnapshot)
			{
				if (pawn.IsWorldPawn() && !keptPawns.ContainsKey(pawn))
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn);
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06008FA8 RID: 36776 RVA: 0x003367BC File Offset: 0x003349BC
		private string GetCriticalPawnReason(Pawn pawn)
		{
			if (pawn.Discarded)
			{
				return null;
			}
			if (PawnUtility.EverBeenColonistOrTameAnimal(pawn) && pawn.RaceProps.Humanlike)
			{
				return "Colonist";
			}
			if (PawnGenerator.IsBeingGenerated(pawn))
			{
				return "Generating";
			}
			if (PawnUtility.IsFactionLeader(pawn))
			{
				return "FactionLeader";
			}
			if (PawnUtility.IsKidnappedPawn(pawn))
			{
				return "Kidnapped";
			}
			if (pawn.IsCaravanMember())
			{
				return "CaravanMember";
			}
			if (PawnUtility.IsTravelingInTransportPodWorldObject(pawn))
			{
				return "TransportPod";
			}
			if (PawnUtility.ForSaleBySettlement(pawn))
			{
				return "ForSale";
			}
			if (Find.WorldPawns.ForcefullyKeptPawns.Contains(pawn))
			{
				return "ForceKept";
			}
			if (pawn.SpawnedOrAnyParentSpawned)
			{
				return "Spawned";
			}
			if (!pawn.Corpse.DestroyedOrNull())
			{
				return "CorpseExists";
			}
			if (pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing)
			{
				if (Find.PlayLog.AnyEntryConcerns(pawn))
				{
					return "InPlayLog";
				}
				if (Find.BattleLog.AnyEntryConcerns(pawn))
				{
					return "InBattleLog";
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.TaleManager.AnyActiveTaleConcerns(pawn))
			{
				return "InActiveTale";
			}
			if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(pawn))
			{
				return "ReservedByQuest";
			}
			return null;
		}

		// Token: 0x06008FA9 RID: 36777 RVA: 0x003368E0 File Offset: 0x00334AE0
		public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.relations == null)
			{
				return;
			}
			foreach (Pawn pawn2 in pawn.relations.RelatedPawns)
			{
				if (this.logDotgraph != null)
				{
					string text = string.Format("{0}->{1} [label=<{2}> color=\"purple\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(pawn2), pawn.GetRelations(pawn2).FirstOrDefault<PawnRelationDef>().ToString());
					if (!this.logDotgraphUniqueLinks.Contains(text))
					{
						this.logDotgraphUniqueLinks.Add(text);
						this.logDotgraph.AppendLine(text);
					}
				}
				if (!keptPawns.ContainsKey(pawn2))
				{
					keptPawns[pawn2] = "Relationship";
				}
			}
		}

		// Token: 0x06008FAA RID: 36778 RVA: 0x003369A4 File Offset: 0x00334BA4
		public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.needs == null || pawn.needs.mood == null || pawn.needs.mood.thoughts == null || pawn.needs.mood.thoughts.memories == null)
			{
				return;
			}
			foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
			{
				if (thought_Memory.otherPawn != null)
				{
					if (this.logDotgraph != null)
					{
						string text = string.Format("{0}->{1} [label=<{2}> color=\"orange\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(thought_Memory.otherPawn), thought_Memory.def);
						if (!this.logDotgraphUniqueLinks.Contains(text))
						{
							this.logDotgraphUniqueLinks.Add(text);
							this.logDotgraph.AppendLine(text);
						}
					}
					if (!keptPawns.ContainsKey(thought_Memory.otherPawn))
					{
						keptPawns[thought_Memory.otherPawn] = "Memory";
					}
				}
			}
		}

		// Token: 0x06008FAB RID: 36779 RVA: 0x00336AC4 File Offset: 0x00334CC4
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSuccessfulGCTick, "lastSuccessfulGCTick", 0, false);
			Scribe_Values.Look<int>(ref this.currentGCRate, "nextGCRate", 1, false);
		}

		// Token: 0x06008FAC RID: 36780 RVA: 0x00336AEA File Offset: 0x00334CEA
		public void LogGC()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= GC =======");
			stringBuilder.AppendLine(this.PawnGCDebugResults());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008FAD RID: 36781 RVA: 0x00336B14 File Offset: 0x00334D14
		public void RunGC()
		{
			this.CancelGCPass();
			PerfLogger.Reset();
			foreach (object obj in this.PawnGCPass())
			{
			}
			float num = PerfLogger.Duration() * 1000f;
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num));
		}

		// Token: 0x06008FAE RID: 36782 RVA: 0x00336B94 File Offset: 0x00334D94
		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard");
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		// Token: 0x06008FAF RID: 36783 RVA: 0x00336C08 File Offset: 0x00334E08
		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.LabelShort
			where char.IsLetter(ch)
			select ch).ToArray<char>()) + "_" + pawn.thingIDNumber.ToString();
		}

		// Token: 0x04005A31 RID: 23089
		private int lastSuccessfulGCTick;

		// Token: 0x04005A32 RID: 23090
		private int currentGCRate = 1;

		// Token: 0x04005A33 RID: 23091
		private const int AdditionalRandomHumanlikes = 10;

		// Token: 0x04005A34 RID: 23092
		private const float AdditionalRandomHumanlikeKeepChance = 0.25f;

		// Token: 0x04005A35 RID: 23093
		private const int GCUpdateInterval = 15000;

		// Token: 0x04005A36 RID: 23094
		private IEnumerator activeGCProcess;

		// Token: 0x04005A37 RID: 23095
		private StringBuilder logDotgraph;

		// Token: 0x04005A38 RID: 23096
		private HashSet<string> logDotgraphUniqueLinks;
	}
}
