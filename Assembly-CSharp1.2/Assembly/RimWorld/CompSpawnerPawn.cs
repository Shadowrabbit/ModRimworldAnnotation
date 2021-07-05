using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200185D RID: 6237
	public class CompSpawnerPawn : ThingComp
	{
		// Token: 0x170015B9 RID: 5561
		// (get) Token: 0x06008A59 RID: 35417 RVA: 0x0005CD24 File Offset: 0x0005AF24
		private CompProperties_SpawnerPawn Props
		{
			get
			{
				return (CompProperties_SpawnerPawn)this.props;
			}
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x06008A5A RID: 35418 RVA: 0x0005CD31 File Offset: 0x0005AF31
		public Lord Lord
		{
			get
			{
				return CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, null);
			}
		}

		// Token: 0x170015BB RID: 5563
		// (get) Token: 0x06008A5B RID: 35419 RVA: 0x002867CC File Offset: 0x002849CC
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

		// Token: 0x170015BC RID: 5564
		// (get) Token: 0x06008A5C RID: 35420 RVA: 0x0005CD55 File Offset: 0x0005AF55
		public bool Active
		{
			get
			{
				return this.pawnsLeftToSpawn != 0 && !this.Dormant;
			}
		}

		// Token: 0x170015BD RID: 5565
		// (get) Token: 0x06008A5D RID: 35421 RVA: 0x00286818 File Offset: 0x00284A18
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

		// Token: 0x170015BE RID: 5566
		// (get) Token: 0x06008A5E RID: 35422 RVA: 0x0005CD6A File Offset: 0x0005AF6A
		public bool Dormant
		{
			get
			{
				return this.DormancyComp != null && !this.DormancyComp.Awake;
			}
		}

		// Token: 0x06008A5F RID: 35423 RVA: 0x00286844 File Offset: 0x00284A44
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

		// Token: 0x06008A60 RID: 35424 RVA: 0x0028689C File Offset: 0x00284A9C
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

		// Token: 0x06008A61 RID: 35425 RVA: 0x002869B4 File Offset: 0x00284BB4
		public static Lord CreateNewLord(Thing byThing, bool aggressive, float defendRadius, Type lordJobType)
		{
			IntVec3 invalid;
			if (!CellFinder.TryFindRandomCellNear(byThing.Position, byThing.Map, 5, (IntVec3 c) => c.Standable(byThing.Map) && byThing.Map.reachability.CanReach(c, byThing, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)), out invalid, -1))
			{
				Log.Error("Found no place for mechanoids to defend " + byThing, false);
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

		// Token: 0x06008A62 RID: 35426 RVA: 0x00286A70 File Offset: 0x00284C70
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

		// Token: 0x06008A63 RID: 35427 RVA: 0x00286AB4 File Offset: 0x00284CB4
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.", false);
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

		// Token: 0x06008A64 RID: 35428 RVA: 0x0005CD84 File Offset: 0x0005AF84
		private void CalculateNextPawnSpawnTick()
		{
			this.CalculateNextPawnSpawnTick(this.Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
		}

		// Token: 0x06008A65 RID: 35429 RVA: 0x00286AF8 File Offset: 0x00284CF8
		public void CalculateNextPawnSpawnTick(float delayTicks)
		{
			float num = GenMath.LerpDouble(0f, 5f, 1f, 0.5f, (float)this.spawnedPawns.Count);
			if (Find.Storyteller.difficultyValues.enemyReproductionRateFactor > 0f)
			{
				this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(delayTicks / (num * Find.Storyteller.difficultyValues.enemyReproductionRateFactor));
				return;
			}
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)delayTicks;
		}

		// Token: 0x06008A66 RID: 35430 RVA: 0x00286B7C File Offset: 0x00284D7C
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

		// Token: 0x06008A67 RID: 35431 RVA: 0x00286BC0 File Offset: 0x00284DC0
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

		// Token: 0x06008A68 RID: 35432 RVA: 0x00286C24 File Offset: 0x00284E24
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
			pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(this.chosenKind, this.parent.Faction, PawnGenerationContext.NonPlayer, -1, false, false, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, new float?(this.chosenKind.race.race.lifeStageAges[index].minAge), null, null, null, null, null, null));
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

		// Token: 0x06008A69 RID: 35433 RVA: 0x0005CDA2 File Offset: 0x0005AFA2
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.Active && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
		}

		// Token: 0x06008A6A RID: 35434 RVA: 0x00286DCC File Offset: 0x00284FCC
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
					if ((this.Props.maxSpawnedPawnsPoints < 0f || this.SpawnedPawnsPoints < this.Props.maxSpawnedPawnsPoints) && Find.Storyteller.difficultyValues.enemyReproductionRateFactor > 0f && this.TrySpawnPawn(out pawn) && pawn.caller != null)
					{
						pawn.caller.DoCall();
					}
					this.CalculateNextPawnSpawnTick();
				}
			}
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x00286E88 File Offset: 0x00285088
		public void SendMessage()
		{
			if (!this.Props.spawnMessageKey.NullOrEmpty() && MessagesRepeatAvoider.MessageShowAllowed(this.Props.spawnMessageKey, 0.1f))
			{
				Messages.Message(this.Props.spawnMessageKey.Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x0005CDC5 File Offset: 0x0005AFC5
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

		// Token: 0x06008A6D RID: 35437 RVA: 0x00286EEC File Offset: 0x002850EC
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

		// Token: 0x06008A6E RID: 35438 RVA: 0x0028703C File Offset: 0x0028523C
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

		// Token: 0x040058CB RID: 22731
		public int nextPawnSpawnTick = -1;

		// Token: 0x040058CC RID: 22732
		public int pawnsLeftToSpawn = -1;

		// Token: 0x040058CD RID: 22733
		public List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x040058CE RID: 22734
		public bool aggressive = true;

		// Token: 0x040058CF RID: 22735
		public bool canSpawnPawns = true;

		// Token: 0x040058D0 RID: 22736
		private PawnKindDef chosenKind;

		// Token: 0x040058D1 RID: 22737
		private CompCanBeDormant dormancyCompCached;
	}
}
