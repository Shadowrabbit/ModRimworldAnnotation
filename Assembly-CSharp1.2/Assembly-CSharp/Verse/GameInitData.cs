using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020001D5 RID: 469
	public class GameInitData
	{
		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000C20 RID: 3104 RVA: 0x0000F555 File Offset: 0x0000D755
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0000F56F File Offset: 0x0000D76F
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0000F57C File Offset: 0x0000D77C
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0000F5A2 File Offset: 0x0000D7A2
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"startedFromEntry: ",
				this.startedFromEntry.ToString(),
				"\nstartingAndOptionalPawns: ",
				this.startingAndOptionalPawns.Count
			});
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x000A365C File Offset: 0x000A185C
		public void PrepForMapGen()
		{
			while (this.startingAndOptionalPawns.Count > this.startingPawnCount)
			{
				PawnComponentsUtility.RemoveComponentsOnDespawned(this.startingAndOptionalPawns[this.startingPawnCount]);
				Find.WorldPawns.PassToWorld(this.startingAndOptionalPawns[this.startingPawnCount], PawnDiscardDecideMode.KeepForever);
				this.startingAndOptionalPawns.RemoveAt(this.startingPawnCount);
			}
			List<Pawn> list = this.startingAndOptionalPawns;
			foreach (Pawn pawn in list)
			{
				pawn.SetFactionDirect(Faction.OfPlayer);
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, false);
			}
			foreach (Pawn pawn2 in list)
			{
				pawn2.workSettings.DisableAll();
			}
			using (IEnumerator<WorkTypeDef> enumerator2 = DefDatabase<WorkTypeDef>.AllDefs.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					WorkTypeDef w = enumerator2.Current;
					if (w.alwaysStartActive)
					{
						IEnumerable<Pawn> source = list;
						Func<Pawn, bool> predicate;
						Func<Pawn, bool> <>9__0;
						if ((predicate = <>9__0) == null)
						{
							predicate = (<>9__0 = ((Pawn col) => !col.WorkTypeIsDisabled(w)));
						}
						using (IEnumerator<Pawn> enumerator3 = source.Where(predicate).GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Pawn pawn3 = enumerator3.Current;
								pawn3.workSettings.SetPriority(w, 3);
							}
							continue;
						}
					}
					bool flag = false;
					foreach (Pawn pawn4 in list)
					{
						if (!pawn4.WorkTypeIsDisabled(w) && pawn4.skills.AverageOfRelevantSkillsFor(w) >= 6f)
						{
							pawn4.workSettings.SetPriority(w, 3);
							flag = true;
						}
					}
					if (!flag)
					{
						IEnumerable<Pawn> source2 = from col in list
						where !col.WorkTypeIsDisabled(w)
						select col;
						if (source2.Any<Pawn>())
						{
							source2.InRandomOrder(null).MaxBy((Pawn c) => c.skills.AverageOfRelevantSkillsFor(w)).workSettings.SetPriority(w, 3);
						}
					}
				}
			}
		}

		// Token: 0x04000A8F RID: 2703
		public int startingTile = -1;

		// Token: 0x04000A90 RID: 2704
		public int mapSize = 250;

		// Token: 0x04000A91 RID: 2705
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x04000A92 RID: 2706
		public int startingPawnCount = -1;

		// Token: 0x04000A93 RID: 2707
		public Faction playerFaction;

		// Token: 0x04000A94 RID: 2708
		public Season startingSeason;

		// Token: 0x04000A95 RID: 2709
		public bool permadeathChosen;

		// Token: 0x04000A96 RID: 2710
		public bool permadeath;

		// Token: 0x04000A97 RID: 2711
		public bool startedFromEntry;

		// Token: 0x04000A98 RID: 2712
		public string gameToLoad;

		// Token: 0x04000A99 RID: 2713
		public const int DefaultMapSize = 250;
	}
}
