using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016FA RID: 5882
	[StaticConstructorOnStartup]
	public class Tornado : ThingWithComps
	{
		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x0600813F RID: 33087 RVA: 0x002654EC File Offset: 0x002636EC
		private float FadeInOutFactor
		{
			get
			{
				float a = Mathf.Clamp01((float)(Find.TickManager.TicksGame - this.spawnTick) / 120f);
				float b = (this.leftFadeOutTicks < 0) ? 1f : Mathf.Min((float)this.leftFadeOutTicks / 120f, 1f);
				return Mathf.Min(a, b);
			}
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x00265544 File Offset: 0x00263744
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Vector2>(ref this.realPosition, "realPosition", default(Vector2), false);
			Scribe_Values.Look<float>(ref this.direction, "direction", 0f, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.leftFadeOutTicks, "leftFadeOutTicks", 0, false);
			Scribe_Values.Look<int>(ref this.ticksLeftToDisappear, "ticksLeftToDisappear", 0, false);
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x002655C0 File Offset: 0x002637C0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				Vector3 vector = base.Position.ToVector3Shifted();
				this.realPosition = new Vector2(vector.x, vector.z);
				this.direction = Rand.Range(0f, 360f);
				this.spawnTick = Find.TickManager.TicksGame;
				this.leftFadeOutTicks = -1;
				this.ticksLeftToDisappear = Tornado.DurationTicks.RandomInRange;
			}
			this.CreateSustainer();
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x00265644 File Offset: 0x00263844
		public override void Tick()
		{
			if (base.Spawned)
			{
				if (this.sustainer == null)
				{
					Log.Error("Tornado sustainer is null.", false);
					this.CreateSustainer();
				}
				this.sustainer.Maintain();
				this.UpdateSustainerVolume();
				base.GetComp<CompWindSource>().wind = 5f * this.FadeInOutFactor;
				if (this.leftFadeOutTicks > 0)
				{
					this.leftFadeOutTicks--;
					if (this.leftFadeOutTicks == 0)
					{
						this.Destroy(DestroyMode.Vanish);
						return;
					}
				}
				else
				{
					if (Tornado.directionNoise == null)
					{
						Tornado.directionNoise = new Perlin(0.0020000000949949026, 2.0, 0.5, 4, 1948573612, QualityMode.Medium);
					}
					this.direction += (float)Tornado.directionNoise.GetValue((double)Find.TickManager.TicksAbs, (double)((float)(this.thingIDNumber % 500) * 1000f), 0.0) * 0.78f;
					this.realPosition = this.realPosition.Moved(this.direction, 0.028333334f);
					IntVec3 intVec = new Vector3(this.realPosition.x, 0f, this.realPosition.y).ToIntVec3();
					if (intVec.InBounds(base.Map))
					{
						base.Position = intVec;
						if (this.IsHashIntervalTick(15))
						{
							this.DamageCloseThings();
						}
						if (Rand.MTBEventOccurs(15f, 1f, 1f))
						{
							this.DamageFarThings();
						}
						if (this.IsHashIntervalTick(20))
						{
							this.DestroyRoofs();
						}
						if (this.ticksLeftToDisappear > 0)
						{
							this.ticksLeftToDisappear--;
							if (this.ticksLeftToDisappear == 0)
							{
								this.leftFadeOutTicks = 120;
								Messages.Message("MessageTornadoDissipated".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
							}
						}
						if (this.IsHashIntervalTick(4) && !this.CellImmuneToDamage(base.Position))
						{
							float num = Rand.Range(0.6f, 1f);
							MoteMaker.ThrowTornadoDustPuff(new Vector3(this.realPosition.x, 0f, this.realPosition.y)
							{
								y = AltitudeLayer.MoteOverhead.AltitudeFor()
							} + Vector3Utility.RandomHorizontalOffset(1.5f), base.Map, Rand.Range(1.5f, 3f), new Color(num, num, num));
							return;
						}
					}
					else
					{
						this.leftFadeOutTicks = 120;
						Messages.Message("MessageTornadoLeftMap".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.PositiveEvent, true);
					}
				}
			}
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x002658F8 File Offset: 0x00263AF8
		public override void Draw()
		{
			Rand.PushState();
			Rand.Seed = this.thingIDNumber;
			for (int i = 0; i < 180; i++)
			{
				this.DrawTornadoPart(Tornado.PartsDistanceFromCenter.RandomInRange, Rand.Range(0f, 360f), Rand.Range(0.9f, 1.1f), Rand.Range(0.52f, 0.88f));
			}
			Rand.PopState();
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x0026596C File Offset: 0x00263B6C
		private void DrawTornadoPart(float distanceFromCenter, float initialAngle, float speedMultiplier, float colorMultiplier)
		{
			int ticksGame = Find.TickManager.TicksGame;
			float num = 1f / distanceFromCenter;
			float num2 = 25f * speedMultiplier * num;
			float num3 = (initialAngle + (float)ticksGame * num2) % 360f;
			Vector2 vector = this.realPosition.Moved(num3, this.AdjustedDistanceFromCenter(distanceFromCenter));
			vector.y += distanceFromCenter * 4f;
			vector.y += Tornado.ZOffsetBias;
			Vector3 a = new Vector3(vector.x, AltitudeLayer.Weather.AltitudeFor() + 0.042857144f * Rand.Range(0f, 1f), vector.y);
			float num4 = distanceFromCenter * 3f;
			float num5 = 1f;
			if (num3 > 270f)
			{
				num5 = GenMath.LerpDouble(270f, 360f, 0f, 1f, num3);
			}
			else if (num3 > 180f)
			{
				num5 = GenMath.LerpDouble(180f, 270f, 1f, 0f, num3);
			}
			float num6 = Mathf.Min(distanceFromCenter / (Tornado.PartsDistanceFromCenter.max + 2f), 1f);
			float d = Mathf.InverseLerp(0.18f, 0.4f, num6);
			Vector3 a2 = new Vector3(Mathf.Sin((float)ticksGame / 1000f + (float)(this.thingIDNumber * 10)) * 2f, 0f, 0f);
			Vector3 pos = a + a2 * d;
			float a3 = Mathf.Max(1f - num6, 0f) * num5 * this.FadeInOutFactor;
			Color value = new Color(colorMultiplier, colorMultiplier, colorMultiplier, a3);
			Tornado.matPropertyBlock.SetColor(ShaderPropertyIDs.Color, value);
			Matrix4x4 matrix = Matrix4x4.TRS(pos, Quaternion.Euler(0f, num3, 0f), new Vector3(num4, 1f, num4));
			Graphics.DrawMesh(MeshPool.plane10, matrix, Tornado.TornadoMaterial, 0, null, 0, Tornado.matPropertyBlock);
		}

		// Token: 0x06008145 RID: 33093 RVA: 0x00265B50 File Offset: 0x00263D50
		private float AdjustedDistanceFromCenter(float distanceFromCenter)
		{
			float num = Mathf.Min(distanceFromCenter / 8f, 1f);
			num *= num;
			return distanceFromCenter * num;
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x00056CF9 File Offset: 0x00054EF9
		private void UpdateSustainerVolume()
		{
			this.sustainer.info.volumeFactor = this.FadeInOutFactor;
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x00056D11 File Offset: 0x00054F11
		private void CreateSustainer()
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				SoundDef tornado = SoundDefOf.Tornado;
				this.sustainer = tornado.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				this.UpdateSustainerVolume();
			});
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x00265B78 File Offset: 0x00263D78
		private void DamageCloseThings()
		{
			int num = GenRadial.NumCellsInRadius(4.2f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(base.Map) && !this.CellImmuneToDamage(intVec))
				{
					Pawn firstPawn = intVec.GetFirstPawn(base.Map);
					if (firstPawn == null || !firstPawn.Downed || !Rand.Bool)
					{
						float damageFactor = GenMath.LerpDouble(0f, 4.2f, 1f, 0.2f, intVec.DistanceTo(base.Position));
						this.DoDamage(intVec, damageFactor);
					}
				}
			}
		}

		// Token: 0x06008149 RID: 33097 RVA: 0x00265C20 File Offset: 0x00263E20
		private void DamageFarThings()
		{
			IntVec3 c = (from x in GenRadial.RadialCellsAround(base.Position, 10f, true)
			where x.InBounds(base.Map)
			select x).RandomElement<IntVec3>();
			if (this.CellImmuneToDamage(c))
			{
				return;
			}
			this.DoDamage(c, 0.5f);
		}

		// Token: 0x0600814A RID: 33098 RVA: 0x00265C6C File Offset: 0x00263E6C
		private void DestroyRoofs()
		{
			this.removedRoofsTmp.Clear();
			foreach (IntVec3 intVec in from x in GenRadial.RadialCellsAround(base.Position, 4.2f, true)
			where x.InBounds(base.Map)
			select x)
			{
				if (!this.CellImmuneToDamage(intVec) && intVec.Roofed(base.Map))
				{
					RoofDef roof = intVec.GetRoof(base.Map);
					if (!roof.isThickRoof && !roof.isNatural)
					{
						RoofCollapserImmediate.DropRoofInCells(intVec, base.Map, null);
						this.removedRoofsTmp.Add(intVec);
					}
				}
			}
			if (this.removedRoofsTmp.Count > 0)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(this.removedRoofsTmp, base.Map, true, false);
			}
		}

		// Token: 0x0600814B RID: 33099 RVA: 0x00265D48 File Offset: 0x00263F48
		private bool CellImmuneToDamage(IntVec3 c)
		{
			if (c.Roofed(base.Map) && c.GetRoof(base.Map).isThickRoof)
			{
				return true;
			}
			Building edifice = c.GetEdifice(base.Map);
			return edifice != null && edifice.def.category == ThingCategory.Building && (edifice.def.building.isNaturalRock || (edifice.def == ThingDefOf.Wall && edifice.Faction == null));
		}

		// Token: 0x0600814C RID: 33100 RVA: 0x00265DC0 File Offset: 0x00263FC0
		private void DoDamage(IntVec3 c, float damageFactor)
		{
			Tornado.tmpThings.Clear();
			Tornado.tmpThings.AddRange(c.GetThingList(base.Map));
			Vector3 vector = c.ToVector3Shifted();
			Vector2 b = new Vector2(vector.x, vector.z);
			float angle = -this.realPosition.AngleTo(b) + 180f;
			for (int i = 0; i < Tornado.tmpThings.Count; i++)
			{
				BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = null;
				switch (Tornado.tmpThings[i].def.category)
				{
				case ThingCategory.Pawn:
				{
					Pawn pawn = (Pawn)Tornado.tmpThings[i];
					battleLogEntry_DamageTaken = new BattleLogEntry_DamageTaken(pawn, RulePackDefOf.DamageEvent_Tornado, null);
					Find.BattleLog.Add(battleLogEntry_DamageTaken);
					if (pawn.RaceProps.baseHealthScale < 1f)
					{
						damageFactor *= pawn.RaceProps.baseHealthScale;
					}
					if (pawn.RaceProps.Animal)
					{
						damageFactor *= 0.75f;
					}
					if (pawn.Downed)
					{
						damageFactor *= 0.2f;
					}
					break;
				}
				case ThingCategory.Item:
					damageFactor *= 0.68f;
					break;
				case ThingCategory.Building:
					damageFactor *= 0.8f;
					break;
				case ThingCategory.Plant:
					damageFactor *= 1.7f;
					break;
				}
				int num = Mathf.Max(GenMath.RoundRandom(30f * damageFactor), 1);
				Tornado.tmpThings[i].TakeDamage(new DamageInfo(DamageDefOf.TornadoScratch, (float)num, 0f, angle, this, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null)).AssociateWithLog(battleLogEntry_DamageTaken);
			}
			Tornado.tmpThings.Clear();
		}

		// Token: 0x040053C1 RID: 21441
		private Vector2 realPosition;

		// Token: 0x040053C2 RID: 21442
		private float direction;

		// Token: 0x040053C3 RID: 21443
		private int spawnTick;

		// Token: 0x040053C4 RID: 21444
		private int leftFadeOutTicks = -1;

		// Token: 0x040053C5 RID: 21445
		private int ticksLeftToDisappear = -1;

		// Token: 0x040053C6 RID: 21446
		private Sustainer sustainer;

		// Token: 0x040053C7 RID: 21447
		private static MaterialPropertyBlock matPropertyBlock = new MaterialPropertyBlock();

		// Token: 0x040053C8 RID: 21448
		private static ModuleBase directionNoise;

		// Token: 0x040053C9 RID: 21449
		private const float Wind = 5f;

		// Token: 0x040053CA RID: 21450
		private const int CloseDamageIntervalTicks = 15;

		// Token: 0x040053CB RID: 21451
		private const int RoofDestructionIntervalTicks = 20;

		// Token: 0x040053CC RID: 21452
		private const float FarDamageMTBTicks = 15f;

		// Token: 0x040053CD RID: 21453
		private const float CloseDamageRadius = 4.2f;

		// Token: 0x040053CE RID: 21454
		private const float FarDamageRadius = 10f;

		// Token: 0x040053CF RID: 21455
		private const float BaseDamage = 30f;

		// Token: 0x040053D0 RID: 21456
		private const int SpawnMoteEveryTicks = 4;

		// Token: 0x040053D1 RID: 21457
		private static readonly IntRange DurationTicks = new IntRange(2700, 10080);

		// Token: 0x040053D2 RID: 21458
		private const float DownedPawnDamageFactor = 0.2f;

		// Token: 0x040053D3 RID: 21459
		private const float AnimalPawnDamageFactor = 0.75f;

		// Token: 0x040053D4 RID: 21460
		private const float BuildingDamageFactor = 0.8f;

		// Token: 0x040053D5 RID: 21461
		private const float PlantDamageFactor = 1.7f;

		// Token: 0x040053D6 RID: 21462
		private const float ItemDamageFactor = 0.68f;

		// Token: 0x040053D7 RID: 21463
		private const float CellsPerSecond = 1.7f;

		// Token: 0x040053D8 RID: 21464
		private const float DirectionChangeSpeed = 0.78f;

		// Token: 0x040053D9 RID: 21465
		private const float DirectionNoiseFrequency = 0.002f;

		// Token: 0x040053DA RID: 21466
		private const float TornadoAnimationSpeed = 25f;

		// Token: 0x040053DB RID: 21467
		private const float ThreeDimensionalEffectStrength = 4f;

		// Token: 0x040053DC RID: 21468
		private const int FadeInTicks = 120;

		// Token: 0x040053DD RID: 21469
		private const int FadeOutTicks = 120;

		// Token: 0x040053DE RID: 21470
		private const float MaxMidOffset = 2f;

		// Token: 0x040053DF RID: 21471
		private static readonly Material TornadoMaterial = MaterialPool.MatFrom("Things/Ethereal/Tornado", ShaderDatabase.Transparent, MapMaterialRenderQueues.Tornado);

		// Token: 0x040053E0 RID: 21472
		private static readonly FloatRange PartsDistanceFromCenter = new FloatRange(1f, 10f);

		// Token: 0x040053E1 RID: 21473
		private static readonly float ZOffsetBias = -4f * Tornado.PartsDistanceFromCenter.min;

		// Token: 0x040053E2 RID: 21474
		private List<IntVec3> removedRoofsTmp = new List<IntVec3>();

		// Token: 0x040053E3 RID: 21475
		private static List<Thing> tmpThings = new List<Thing>();
	}
}
