using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000135 RID: 309
	public class GameInitData
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x0002791F File Offset: 0x00025B1F
		public bool QuickStarted
		{
			get
			{
				return this.gameToLoad.NullOrEmpty() && !this.startedFromEntry;
			}
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x00027939 File Offset: 0x00025B39
		public void ChooseRandomStartingTile()
		{
			this.startingTile = TileFinder.RandomStartingTile();
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x00027946 File Offset: 0x00025B46
		public void ResetWorldRelatedMapInitData()
		{
			Current.Game.World = null;
			this.startingAndOptionalPawns.Clear();
			this.playerFaction = null;
			this.startingTile = -1;
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0002796C File Offset: 0x00025B6C
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

		// Token: 0x06000878 RID: 2168 RVA: 0x000279AC File Offset: 0x00025BAC
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

		// Token: 0x040007E7 RID: 2023
		public int startingTile = -1;

		// Token: 0x040007E8 RID: 2024
		public int mapSize = 250;

		// Token: 0x040007E9 RID: 2025
		public List<Pawn> startingAndOptionalPawns = new List<Pawn>();

		// Token: 0x040007EA RID: 2026
		public int startingPawnCount = -1;

		// Token: 0x040007EB RID: 2027
		public Faction playerFaction;

		// Token: 0x040007EC RID: 2028
		public Season startingSeason;

		// Token: 0x040007ED RID: 2029
		public bool permadeathChosen;

		// Token: 0x040007EE RID: 2030
		public bool permadeath;

		// Token: 0x040007EF RID: 2031
		public bool startedFromEntry;

		// Token: 0x040007F0 RID: 2032
		public string gameToLoad;

		// Token: 0x040007F1 RID: 2033
		public const int DefaultMapSize = 250;
	}
}
