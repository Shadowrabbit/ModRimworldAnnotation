using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020010A5 RID: 4261
	[StaticConstructorOnStartup]
	public class TunnelHiveSpawner : ThingWithComps
	{
		// Token: 0x06006593 RID: 26003 RVA: 0x00224F94 File Offset: 0x00223194
		public static void ResetStaticData()
		{
			TunnelHiveSpawner.filthTypes.Clear();
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_Dirt);
			TunnelHiveSpawner.filthTypes.Add(ThingDefOf.Filth_RubbleRock);
		}

		// Token: 0x06006594 RID: 26004 RVA: 0x00224FE8 File Offset: 0x002231E8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.secondarySpawnTick, "secondarySpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.spawnHive, "spawnHive", true, false);
			Scribe_Values.Look<float>(ref this.insectsPoints, "insectsPoints", 0f, false);
			Scribe_Values.Look<bool>(ref this.spawnedByInfestationThingComp, "spawnedByInfestationThingComp", false, false);
		}

		// Token: 0x06006595 RID: 26005 RVA: 0x00225048 File Offset: 0x00223248
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.secondarySpawnTick = Find.TickManager.TicksGame + this.ResultSpawnDelay.RandomInRange.SecondsToTicks();
			}
			this.CreateSustainer();
		}

		// Token: 0x06006596 RID: 26006 RVA: 0x0022508C File Offset: 0x0022328C
		public override void Tick()
		{
			if (base.Spawned)
			{
				this.sustainer.Maintain();
				Vector3 vector = base.Position.ToVector3Shifted();
				IntVec3 c;
				if (Rand.MTBEventOccurs(TunnelHiveSpawner.FilthSpawnMTB, 1f, 1.TicksToSeconds()) && CellFinder.TryFindRandomReachableCellNear(base.Position, base.Map, TunnelHiveSpawner.FilthSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false), null, null, out c, 999999))
				{
					FilthMaker.TryMakeFilth(c, base.Map, TunnelHiveSpawner.filthTypes.RandomElement<ThingDef>(), 1, FilthSourceFlags.None);
				}
				if (Rand.MTBEventOccurs(TunnelHiveSpawner.DustMoteSpawnMTB, 1f, 1.TicksToSeconds()))
				{
					FleckMaker.ThrowDustPuffThick(new Vector3(vector.x, 0f, vector.z)
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
					this.Spawn(map, position);
				}
			}
		}

		// Token: 0x06006597 RID: 26007 RVA: 0x002251C8 File Offset: 0x002233C8
		protected virtual void Spawn(Map map, IntVec3 loc)
		{
			if (this.spawnHive)
			{
				Hive hive = (Hive)GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Hive, null), loc, map, WipeMode.Vanish);
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
						Log.Error("Too many iterations.");
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
					GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(loc, map, 2, null), map, WipeMode.Vanish);
					pawn.mindState.spawnedByInfestationThingComp = this.spawnedByInfestationThingComp;
					list.Add(pawn);
					pointsLeft -= pawnKindDef.combatPower;
				}
				if (list.Any<Pawn>())
				{
					LordMaker.MakeNewLord(Faction.OfInsects, new LordJob_AssaultColony(Faction.OfInsects, true, false, false, false, true, false, false), map, list);
				}
			}
		}

		// Token: 0x06006598 RID: 26008 RVA: 0x002253A4 File Offset: 0x002235A4
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

		// Token: 0x06006599 RID: 26009 RVA: 0x00225414 File Offset: 0x00223614
		private void DrawDustPart(float initialAngle, float speedMultiplier, float scale)
		{
			float num = (Find.TickManager.TicksGame - this.secondarySpawnTick).TicksToSeconds();
			Vector3 pos = base.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.Filth);
			pos.y += 0.04054054f * Rand.Range(0f, 1f);
			Color value = new Color(0.47058824f, 0.38431373f, 0.3254902f, 0.7f);
			TunnelHiveSpawner.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, initialAngle + speedMultiplier * num, 0f), Vector3.one * scale);
			Graphics.DrawMesh(MeshPool.plane10, matrix, TunnelHiveSpawner.TunnelMaterial, 0, null, 0, TunnelHiveSpawner.matPropertyBlock);
		}

		// Token: 0x0600659A RID: 26010 RVA: 0x002254D2 File Offset: 0x002236D2
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tunnel = SoundDefOf.Tunnel;
				this.sustainer = tunnel.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			});
		}

		// Token: 0x0400394F RID: 14671
		private int secondarySpawnTick;

		// Token: 0x04003950 RID: 14672
		public bool spawnHive = true;

		// Token: 0x04003951 RID: 14673
		public float insectsPoints;

		// Token: 0x04003952 RID: 14674
		public bool spawnedByInfestationThingComp;

		// Token: 0x04003953 RID: 14675
		private Sustainer sustainer;

		// Token: 0x04003954 RID: 14676
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x04003955 RID: 14677
		private readonly FloatRange ResultSpawnDelay = new FloatRange(26f, 30f);

		// Token: 0x04003956 RID: 14678
		[TweakValue("Gameplay", 0f, 1f)]
		private static float DustMoteSpawnMTB = 0.2f;

		// Token: 0x04003957 RID: 14679
		[TweakValue("Gameplay", 0f, 1f)]
		private static float FilthSpawnMTB = 0.3f;

		// Token: 0x04003958 RID: 14680
		[TweakValue("Gameplay", 0f, 10f)]
		private static float FilthSpawnRadius = 3f;

		// Token: 0x04003959 RID: 14681
		private static readonly Material TunnelMaterial = MaterialPool.MatFrom("Things/Filth/Grainy/GrainyA", ShaderDatabase.Transparent);

		// Token: 0x0400395A RID: 14682
		private static List<ThingDef> filthTypes = new List<ThingDef>();
	}
}
