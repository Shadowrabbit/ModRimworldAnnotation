using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020021B4 RID: 8628
	public class WorldPawnGC : IExposable
	{
		// Token: 0x0600B897 RID: 47255 RVA: 0x00350D60 File Offset: 0x0034EF60
		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate), false);
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
							Log.Message("World pawn GC complete", false);
						}
					}
				}
			}
		}

		// Token: 0x0600B898 RID: 47256 RVA: 0x00077A1B File Offset: 0x00075C1B
		public void CancelGCPass()
		{
			if (this.activeGCProcess != null)
			{
				this.activeGCProcess = null;
				this.currentGCRate = Mathf.Min(this.currentGCRate * 2, 16777216);
				if (DebugViewSettings.logWorldPawnGC)
				{
					Log.Message("World pawn GC cancelled", false);
				}
			}
		}

		// Token: 0x0600B899 RID: 47257 RVA: 0x00077A56 File Offset: 0x00075C56
		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			foreach (Pawn pawn4 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				string criticalPawnReason = this.GetCriticalPawnReason(pawn4);
				if (!criticalPawnReason.NullOrEmpty())
				{
					keptPawns[pawn4] = criticalPawnReason;
					if (this.logDotgraph != null)
					{
						this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", new object[]
						{
							WorldPawnGC.DotgraphIdentifier(pawn4),
							criticalPawnReason,
							(pawn4.relations != null && pawn4.relations.everSeenByPlayer) ? "black" : "grey",
							pawn4.RaceProps.Humanlike ? "oval" : "box"
						}));
					}
				}
				else if (this.logDotgraph != null)
				{
					this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(pawn4), (pawn4.relations != null && pawn4.relations.everSeenByPlayer) ? "black" : "grey", pawn4.RaceProps.Humanlike ? "oval" : "box"));
				}
			}
			IEnumerable<Pawn> allMapsWorldAndTemporary_Alive = PawnsFinder.AllMapsWorldAndTemporary_Alive;
			Func<Pawn, bool> <>9__0;
			Func<Pawn, bool> predicate;
			if ((predicate = <>9__0) == null)
			{
				predicate = (<>9__0 = ((Pawn pawn) => this.AllowedAsStoryPawn(pawn) && !keptPawns.ContainsKey(pawn)));
			}
			foreach (Pawn key in (from pawn in allMapsWorldAndTemporary_Alive.Where(predicate)
			orderby pawn.records.StoryRelevance descending
			select pawn).Take(20))
			{
				keptPawns[key] = "StoryRelevant";
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

		// Token: 0x0600B89A RID: 47258 RVA: 0x00350E2C File Offset: 0x0034F02C
		private Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			this.AccumulatePawnGCData(dictionary).ExecuteEnumerable();
			return dictionary;
		}

		// Token: 0x0600B89B RID: 47259 RVA: 0x00350E4C File Offset: 0x0034F04C
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

		// Token: 0x0600B89C RID: 47260 RVA: 0x00077A6D File Offset: 0x00075C6D
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

		// Token: 0x0600B89D RID: 47261 RVA: 0x00350F40 File Offset: 0x0034F140
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

		// Token: 0x0600B89E RID: 47262 RVA: 0x00077A7D File Offset: 0x00075C7D
		private bool AllowedAsStoryPawn(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike;
		}

		// Token: 0x0600B89F RID: 47263 RVA: 0x00351064 File Offset: 0x0034F264
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

		// Token: 0x0600B8A0 RID: 47264 RVA: 0x00351128 File Offset: 0x0034F328
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

		// Token: 0x0600B8A1 RID: 47265 RVA: 0x00077A8F File Offset: 0x00075C8F
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSuccessfulGCTick, "lastSuccessfulGCTick", 0, false);
			Scribe_Values.Look<int>(ref this.currentGCRate, "nextGCRate", 1, false);
		}

		// Token: 0x0600B8A2 RID: 47266 RVA: 0x00077AB5 File Offset: 0x00075CB5
		public void LogGC()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= GC =======");
			stringBuilder.AppendLine(this.PawnGCDebugResults());
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600B8A3 RID: 47267 RVA: 0x00351248 File Offset: 0x0034F448
		public void RunGC()
		{
			this.CancelGCPass();
			PerfLogger.Reset();
			foreach (object obj in this.PawnGCPass())
			{
			}
			float num = PerfLogger.Duration() * 1000f;
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num), false);
		}

		// Token: 0x0600B8A4 RID: 47268 RVA: 0x003512C8 File Offset: 0x0034F4C8
		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard", false);
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		// Token: 0x0600B8A5 RID: 47269 RVA: 0x00351340 File Offset: 0x0034F540
		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.LabelShort
			where char.IsLetter(ch)
			select ch).ToArray<char>()) + "_" + pawn.thingIDNumber.ToString();
		}

		// Token: 0x04007DF2 RID: 32242
		private int lastSuccessfulGCTick;

		// Token: 0x04007DF3 RID: 32243
		private int currentGCRate = 1;

		// Token: 0x04007DF4 RID: 32244
		private const int AdditionalStoryRelevantPawns = 20;

		// Token: 0x04007DF5 RID: 32245
		private const int GCUpdateInterval = 15000;

		// Token: 0x04007DF6 RID: 32246
		private IEnumerator activeGCProcess;

		// Token: 0x04007DF7 RID: 32247
		private StringBuilder logDotgraph;

		// Token: 0x04007DF8 RID: 32248
		private HashSet<string> logDotgraphUniqueLinks;
	}
}
