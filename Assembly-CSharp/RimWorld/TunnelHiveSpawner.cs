using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016FB RID: 5883
	[StaticConstructorOnStartup]
	public class TunnelHiveSpawner : ThingWithComps
	{
		// Token: 0x06008152 RID: 33106 RVA: 0x00266004 File Offset: 0x00264204
		public static void ResetStaticData()
		{
			TunnelHiveSpawner.filthTypes.Clear();
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_RubbleRock);
		}

		// Token: 0x06008153 RID: 33107 RVA: 0x00266058 File Offset: 0x00264258
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.secondarySpawnTick, "secondarySpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnHive, "spawnHive", true, false);
			Scribe_Values.Look<float>(ref this.insectsPoints, "insectsPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
		}

		// Token: 0x06008154 RID: 33108 RVA: 0x002660B8 File Offset: 0x002642B8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.ResultSpawnDelay.RandomInRange.SecondsToTicks();
			}
			this.CreateSustainer();
		}

		// Token: 0x06008155 RID: 33109 RVA: 0x002660FC File Offset: 0x002642FC
		public override void Tick()
		{
			if (base.Spawned)
			{
				this.sustainer.Maintain();
				Vector3 vector = base.Position.ToVector3Shifted();
				IntVec3 c;
				if (Rand.MTBEventOccurs(TunnelHiveSpawner.FilthSpawnMTB, 1f, 1.TicksToSeconds()) && CellFinder.TryFindRandomReachableCellNear(base.Position, base.Map, TunnelHiveSpawner.FilthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c, 999999))
				{
					FilthMaker.TryMakeFilth(c, base.Map, TunnelHiveSpawner.filthTypes.RandomElement<ThingDef>(), 1, FilthSourceFlags.None);
				}
				if (Rand.MTBEventOccurs(TunnelHiveSpawner.DustMoteSpawnMTB, 1f, 1.TicksToSeconds()))
				{
					MoteMaker.ThrowDustPuffThick(new Vector3(vector.x, 0f, vector.z)
					{
						y = AltitudeLayer.MoteOverhead.AltitudeFor()
					}, base.Map, Rand.Range(1.5f, 3f), new Color(1f, 1f, 1f, 2.5f));
				}
				if (this.secondarySpawnTick <= Find.TickManager.TicksGame)
				{
					this.sustainer.End();
					Map map = base.Map;
					IntVec3 position = base.Position;
					this.Destroy(DestroyMode.Vanish);
					if (this.spawnHive)
					{
						Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), position, map, WipeMode.Vanish);
						hive.SetFaction(Faction.OfInsects, null);
						hive.questTags = this.questTags;
						foreach (CompSpawner compSpawner in hive.GetComps<CompSpawner>())
						{
							if (compSpawner.PropsSpawner.thingToSpawn == ThingDefOf.InsectJelly)
							{
								compSpawner.TryDoSpawn();
								break;
							}
						}
					}
					if (this.insectsPoints > 0f)
					{
						this.insectsPoints = Mathf.Max(this.insectsPoints, Hive.spawnablePawnKinds.Min((PawnKindDef x) => x.combatPower));
						float pointsLeft = this.insectsPoints;
						List<Pawn> list = new List<Pawn>();
						int num = 0;
						Func<PawnKindDef, bool> <>9__1;
						while (pointsLeft > 0f)
						{
							num++;
							if (num > 1000)
							{
								Log.Error("Too many iterations.", false);
								break;
							}
							IEnumerable<PawnKindDef> spawnablePawnKinds = Hive.spawnablePawnKinds;
							Func<PawnKindDef, bool> predicate;
							if ((predicate = <>9__1) == null)
							{
								predicate = (<>9__1 = ((PawnKindDef x) => x.combatPower <= pointsLeft));
							}
							PawnKindDef pawnKindDef;
							if (!spawnablePawnKinds.Where(predicate).TryRandomElement(out pawnKindDef))
							{
								break;
							}
							Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, Faction.OfInsects);
							GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(position, map, 2, null), map, WipeMode.Vanish);
							pawn.mindState.spawnedByInfestationThingComp = this.spawnedByInfestationThingComp;
							list.Add(pawn);
							pointsLeft -= pawnKindDef.combatPower;
						}
						if (list.Any<Pawn>())
						{
							LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(Faction.OfInsects, true, false, false, false, true), map, list);
						}
					}
				}
			}
		}

		// Token: 0x06008156 RID: 33110 RVA: 0x00266414 File Offset: 0x00264614
		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 6; i++)
			{
				this.DrawDustPart(Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f) * (float)Rand.Sign * 4f, Rand.Range(1f, 1.5f));
			}
			Rand.PopState();
		}

		// Token: 0x06008157 RID: 33111 RVA: 0x00266484 File Offset: 0x00264684
		private void DrawDustPart(float initialAngle, float speedMultiplier, float scale)
		{
			float num = (Find.TickManager.TicksGame - this.secondarySpawnTick).TicksToSeconds();
			Vector3 pos = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Filth);
			pos.y += 0.042857144f * Rand.Range(0f, 1f);
			Color value = new Color(0.47058824f, 0.38431373f, 0.3254902f, 0.7f);
			TunnelHiveSpawner.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, initialAngle + speedMultiplier * num, 0f), Vector3.one * scale);
			Graphics.DrawMesh(MeshPool.plane10, matrix, TunnelHiveSpawner.TunnelMaterial, 0, null, 0, TunnelHiveSpawner.matPropertyBlock);
		}

		// Token: 0x06008158 RID: 33112 RVA: 0x00056D45 File Offset: 0x00054F45
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tunnel = SoundDefOf.Tunnel;
				this.sustainer = tunnel.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			});
		}

		// Token: 0x040053E4 RID: 21476
		private int secondarySpawnTick;

		// Token: 0x040053E5 RID: 21477
		public bool spawnHive = true;

		// Token: 0x040053E6 RID: 21478
		public float insectsPoints;

		// Token: 0x040053E7 RID: 21479
		public bool spawnedByInfestationThingComp;

		// Token: 0x040053E8 RID: 21480
		private Sustainer sustainer;

		// Token: 0x040053E9 RID: 21481
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x040053EA RID: 21482
		private readonly FloatRange ResultSpawnDelay = new FloatRange(26f, 30f);

		// Token: 0x040053EB RID: 21483
		[TweakValue("Gameplay", 0f, 1f)]
		private static float DustMoteSpawnMTB = 0.2f;

		// Token: 0x040053EC RID: 21484
		[TweakValue("Gameplay", 0f, 1f)]
		private static float FilthSpawnMTB = 0.3f;

		// Token: 0x040053ED RID: 21485
		[TweakValue("Gameplay", 0f, 10f)]
		private static float FilthSpawnRadius = 3f;

		// Token: 0x040053EE RID: 21486
		private static readonly Material TunnelMaterial = MaterialPool.MatFrom("Things/Filth/Grainy/GrainyA", ShaderDatabase.Transparent);

		// Token: 0x040053EF RID: 21487
		private static List<ThingDef> filthTypes = new List<ThingDef>();
	}
}
