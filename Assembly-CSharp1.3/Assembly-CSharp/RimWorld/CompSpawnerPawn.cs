using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011A8 RID: 4520
	public class CompSpawnerPawn : ThingComp
	{
		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x06006CD1 RID: 27857 RVA: 0x0024869C File Offset: 0x0024689C
		private CompProperties_SpawnerPawn Props
		{
			get
			{
				return (CompProperties_SpawnerPawn)this.props;
			}
		}

		// Token: 0x170012D9 RID: 4825
		// (get) Token: 0x06006CD2 RID: 27858 RVA: 0x002486A9 File Offset: 0x002468A9
		public Lord Lord
		{
			get
			{
				return CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, null);
			}
		}

		// Token: 0x170012DA RID: 4826
		// (get) Token: 0x06006CD3 RID: 27859 RVA: 0x002486D0 File Offset: 0x002468D0
		private float SpawnedPawnsPoints
		{
			get
			{
				this.FilterOutUnspawnedPawns();
				float num = 0f;
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					num += this.spawnedPawns[i].kindDef.combatPower;
				}
				return num;
			}
		}

		// Token: 0x170012DB RID: 4827
		// (get) Token: 0x06006CD4 RID: 27860 RVA: 0x00248719 File Offset: 0x00246919
		public bool Active
		{
			get
			{
				return this.pawnsLeftToSpawn != 0 && !this.Dormant;
			}
		}

		// Token: 0x170012DC RID: 4828
		// (get) Token: 0x06006CD5 RID: 27861 RVA: 0x00248730 File Offset: 0x00246930
		public CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06006CD6 RID: 27862 RVA: 0x0024875B File Offset: 0x0024695B
		public bool Dormant
		{
			get
			{
				return this.DormancyComp != null && !this.DormancyComp.Awake;
			}
		}

		// Token: 0x06006CD7 RID: 27863 RVA: 0x00248778 File Offset: 0x00246978
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.chosenKind == null)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.Props.maxPawnsToSpawn != IntRange.zero)
			{
				this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
			}
		}

		// Token: 0x06006CD8 RID: 27864 RVA: 0x002487D0 File Offset: 0x002469D0
		public static Lord FindLordToJoin(Thing spawner, Type lordJobType, bool shouldTryJoinParentLord, Func<Thing, List<Pawn>> spawnedPawnSelector = null)
		{
			if (spawner.Spawned)
			{
				if (shouldTryJoinParentLord)
				{
					Building building = spawner as Building;
					Lord lord = (building != null) ? building.GetLord() : null;
					if (lord != null)
					{
						return lord;
					}
				}
				if (spawnedPawnSelector == null)
				{
					spawnedPawnSelector = delegate(Thing s)
					{
						CompSpawnerPawn compSpawnerPawn = s.TryGetComp<CompSpawnerPawn>();
						if (compSpawnerPawn != null)
						{
							return compSpawnerPawn.spawnedPawns;
						}
						return null;
					};
				}
				Predicate<Pawn> hasJob = delegate(Pawn x)
				{
					Lord lord2 = x.GetLord();
					return lord2 != null && lord2.LordJob.GetType() == lordJobType;
				};
				Pawn foundPawn = null;
				RegionTraverser.BreadthFirstTraverse(spawner.GetRegion(RegionType.Set_Passable), (Region from, Region to) => true, delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsOfDef(spawner.def);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Faction == spawner.Faction)
						{
							List<Pawn> list2 = spawnedPawnSelector(list[i]);
							if (list2 != null)
							{
								foundPawn = list2.Find(hasJob);
							}
							if (foundPawn != null)
							{
								return true;
							}
						}
					}
					return false;
				}, 40, RegionType.Set_Passable);
				if (foundPawn != null)
				{
					return foundPawn.GetLord();
				}
			}
			return null;
		}

		// Token: 0x06006CD9 RID: 27865 RVA: 0x002488EC File Offset: 0x00246AEC
		public static Lord CreateNewLord(Thing byThing, bool aggressive, float defendRadius, Type lordJobType)
		{
			IntVec3 invalid;
			if (!CellFinder.TryFindRandomCellNear(byThing.Position, byThing.Map, 5, (IntVec3 c) => c.Standable(byThing.Map) && byThing.Map.reachability.CanReach(c, byThing, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false)), out invalid, -1))
			{
				Log.Error("Found no place for mechanoids to defend " + byThing);
				invalid = IntVec3.Invalid;
			}
			return LordMaker.MakeNewLord(byThing.Faction, Activator.CreateInstance(lordJobType, new object[]
			{
				new SpawnedPawnParams
				{
					aggressive = aggressive,
					defendRadius = defendRadius,
					defSpot = invalid,
					spawnerThing = byThing
				}
			}) as LordJob, byThing.Map, null);
		}

		// Token: 0x06006CDA RID: 27866 RVA: 0x002489A4 File Offset: 0x00246BA4
		private void SpawnInitialPawns()
		{
			int num = 0;
			Pawn pawn;
			while (num < this.Props.initialPawnsCount && this.TrySpawnPawn(out pawn))
			{
				num++;
			}
			this.SpawnPawnsUntilPoints(this.Props.initialPawnsPoints);
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06006CDB RID: 27867 RVA: 0x002489E8 File Offset: 0x00246BE8
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.");
					break;
				}
				Pawn pawn;
				if (!this.TrySpawnPawn(out pawn))
				{
					break;
				}
			}
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06006CDC RID: 27868 RVA: 0x00248A2A File Offset: 0x00246C2A
		private void CalculateNextPawnSpawnTick()
		{
			this.CalculateNextPawnSpawnTick(this.Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x00248A48 File Offset: 0x00246C48
		public void CalculateNextPawnSpawnTick(float delayTicks)
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
			{
				this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(delayTicks / (num * Find.Storyteller.difficulty.enemyReproductionRateFactor));
				return;
			}
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)delayTicks;
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x00248ACC File Offset: 0x00246CCC
		private void FilterOutUnspawnedPawns()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x06006CDF RID: 27871 RVA: 0x00248B10 File Offset: 0x00246D10
		private PawnKindDef RandomPawnKindDef()
		{
			float curPoints = this.SpawnedPawnsPoints;
			IEnumerable<PawnKindDef> source = this.Props.spawnablePawnKinds;
			if (this.Props.maxSpawnedPawnsPoints > -1f)
			{
				source = from x in source
				where curPoints + x.combatPower <= this.Props.maxSpawnedPawnsPoints
				select x;
			}
			PawnKindDef result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06006CE0 RID: 27872 RVA: 0x00248B74 File Offset: 0x00246D74
		private bool TrySpawnPawn(out Pawn pawn)
		{
			if (!this.canSpawnPawns)
			{
				pawn = null;
				return false;
			}
			if (!this.Props.chooseSingleTypeToSpawn)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.chosenKind == null)
			{
				pawn = null;
				return false;
			}
			int index = this.chosenKind.lifeStages.Count - 1;
			pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.chosenKind, this.parent.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, new float?(this.chosenKind.race.race.lifeStageAges[index].minAge), null, null, null, null, null, null, null, false, false));
			this.spawnedPawns.Add(pawn);
			GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.parent.Position, this.parent.Map, this.Props.pawnSpawnRadius, null), this.parent.Map, WipeMode.Vanish);
			Lord lord = this.Lord;
			if (lord == null)
			{
				lord = CompSpawnerPawn.CreateNewLord(this.parent, this.aggressive, this.Props.defendRadius, this.Props.lordJob);
			}
			lord.AddPawn(pawn);
			if (this.Props.spawnSound != null)
			{
				this.Props.spawnSound.PlayOneShot(this.parent);
			}
			if (this.pawnsLeftToSpawn > 0)
			{
				this.pawnsLeftToSpawn--;
			}
			this.SendMessage();
			return true;
		}

		// Token: 0x06006CE1 RID: 27873 RVA: 0x00248D24 File Offset: 0x00246F24
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.Active && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x00248D48 File Offset: 0x00246F48
		public override void CompTick()
		{
			if (this.Active && this.parent.Spawned && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
			if (this.parent.Spawned)
			{
				this.FilterOutUnspawnedPawns();
				if (this.Active && Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
				{
					Pawn pawn;
					if ((this.Props.maxSpawnedPawnsPoints < 0f || this.SpawnedPawnsPoints < this.Props.maxSpawnedPawnsPoints) && Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f && this.TrySpawnPawn(out pawn) && pawn.caller != null)
					{
						pawn.caller.DoCall();
					}
					this.CalculateNextPawnSpawnTick();
				}
			}
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x00248E04 File Offset: 0x00247004
		public void SendMessage()
		{
			if (!this.Props.spawnMessageKey.NullOrEmpty() && MessagesRepeatAvoider.MessageShowAllowed(this.Props.spawnMessageKey, 0.1f))
			{
				Messages.Message(this.Props.spawnMessageKey.Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06006CE4 RID: 27876 RVA: 0x00248E65 File Offset: 0x00247065
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Spawn pawn",
					icon = TexCommand.ReleaseAnimals,
					action = delegate()
					{
						Pawn pawn;
						this.TrySpawnPawn(out pawn);
					}
				};
			}
			yield break;
		}

		// Token: 0x06006CE5 RID: 27877 RVA: 0x00248E78 File Offset: 0x00247078
		public override string CompInspectStringExtra()
		{
			if (!this.Props.showNextSpawnInInspect || this.nextPawnSpawnTick <= 0 || this.chosenKind == null)
			{
				return null;
			}
			if (this.pawnsLeftToSpawn == 0 && !this.Props.noPawnsLeftToSpawnKey.NullOrEmpty())
			{
				return this.Props.noPawnsLeftToSpawnKey.Translate();
			}
			string text;
			if (!this.Dormant)
			{
				text = (this.Props.nextSpawnInspectStringKey ?? "SpawningNextPawnIn").Translate(this.chosenKind.LabelCap, (this.nextPawnSpawnTick - Find.TickManager.TicksGame).ToStringTicksToDays("F1"));
			}
			else
			{
				if (this.Props.nextSpawnInspectStringKeyDormant == null)
				{
					return null;
				}
				text = this.Props.nextSpawnInspectStringKeyDormant.Translate() + ": " + this.chosenKind.LabelCap;
			}
			if (this.pawnsLeftToSpawn > 0 && !this.Props.pawnsLeftToSpawnKey.NullOrEmpty())
			{
				text = text + ("\n" + this.Props.pawnsLeftToSpawnKey.Translate() + ": ") + this.pawnsLeftToSpawn;
			}
			return text;
		}

		// Token: 0x06006CE6 RID: 27878 RVA: 0x00248FC8 File Offset: 0x002471C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.pawnsLeftToSpawn, "pawnsLeftToSpawn", -1, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.chosenKind, "chosenKind");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
				if (this.pawnsLeftToSpawn == -1 && this.Props.maxPawnsToSpawn != IntRange.zero)
				{
					this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
				}
			}
		}

		// Token: 0x04003C93 RID: 15507
		public int nextPawnSpawnTick = -1;

		// Token: 0x04003C94 RID: 15508
		public int pawnsLeftToSpawn = -1;

		// Token: 0x04003C95 RID: 15509
		public List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x04003C96 RID: 15510
		public bool aggressive = true;

		// Token: 0x04003C97 RID: 15511
		public bool canSpawnPawns = true;

		// Token: 0x04003C98 RID: 15512
		private PawnKindDef chosenKind;

		// Token: 0x04003C99 RID: 15513
		private CompCanBeDormant dormancyCompCached;
	}
}
