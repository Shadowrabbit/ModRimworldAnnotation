using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016E6 RID: 5862
	public class Blight : Thing
	{
		// Token: 0x17001402 RID: 5122
		// (get) Token: 0x060080BC RID: 32956 RVA: 0x00056739 File Offset: 0x00054939
		// (set) Token: 0x060080BD RID: 32957 RVA: 0x00056741 File Offset: 0x00054941
		public float Severity
		{
			get
			{
				return this.severity;
			}
			set
			{
				this.severity = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001403 RID: 5123
		// (get) Token: 0x060080BE RID: 32958 RVA: 0x0005674F File Offset: 0x0005494F
		public Plant Plant
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return BlightUtility.GetFirstBlightableEverPlant(base.Position, base.Map);
			}
		}

		// Token: 0x17001404 RID: 5124
		// (get) Token: 0x060080BF RID: 32959 RVA: 0x0005676C File Offset: 0x0005496C
		protected float ReproduceMTBHours
		{
			get
			{
				if (this.severity < 0.28f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.28f, 1f, 16.8f, 2.1f, this.severity);
			}
		}

		// Token: 0x060080C0 RID: 32960 RVA: 0x000567A0 File Offset: 0x000549A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x060080C1 RID: 32961 RVA: 0x000567D0 File Offset: 0x000549D0
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x060080C2 RID: 32962 RVA: 0x002624F8 File Offset: 0x002606F8
		public override void TickLong()
		{
			this.CheckHarmPlant();
			if (this.DestroyIfNoPlantHere())
			{
				return;
			}
			this.Severity += 0.033333335f;
			float reproduceMTBHours = this.ReproduceMTBHours;
			if (reproduceMTBHours > 0f && Rand.MTBEventOccurs(reproduceMTBHours, 2500f, 2000f))
			{
				this.TryReproduceNow();
			}
			if (Mathf.Abs(this.Severity - this.lastMapMeshUpdateSeverity) >= 0.05f)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				this.lastMapMeshUpdateSeverity = this.Severity;
			}
		}

		// Token: 0x060080C3 RID: 32963 RVA: 0x000567F9 File Offset: 0x000549F9
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x060080C4 RID: 32964 RVA: 0x00056802 File Offset: 0x00054A02
		private bool DestroyIfNoPlantHere()
		{
			if (base.Destroyed)
			{
				return true;
			}
			if (this.Plant == null)
			{
				this.Destroy(DestroyMode.Vanish);
				return true;
			}
			return false;
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x0026258C File Offset: 0x0026078C
		private void CheckHarmPlant()
		{
			int ticksGame = Find.TickManager.TicksGame;
			if (ticksGame - this.lastPlantHarmTick >= 60000)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Plant plant = thingList[i] as Plant;
					if (plant != null)
					{
						this.HarmPlant(plant);
					}
				}
				this.lastPlantHarmTick = ticksGame;
			}
		}

		// Token: 0x060080C6 RID: 32966 RVA: 0x002625F4 File Offset: 0x002607F4
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label, plant).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060080C7 RID: 32967 RVA: 0x00056820 File Offset: 0x00054A20
		public void TryReproduceNow()
		{
			GenRadial.ProcessEquidistantCells(base.Position, 4f, delegate(List<IntVec3> cells)
			{
				IntVec3 c;
				if ((from x in cells
				where BlightUtility.GetFirstBlightableNowPlant(x, base.Map) != null
				select x).TryRandomElement(out c))
				{
					BlightUtility.GetFirstBlightableNowPlant(c, base.Map).CropBlighted();
					return true;
				}
				return false;
			}, base.Map);
		}

		// Token: 0x060080C8 RID: 32968 RVA: 0x002626A8 File Offset: 0x002608A8
		public override void Print(SectionLayer layer)
		{
			Plant plant = this.Plant;
			if (plant != null)
			{
				PlantUtility.SetWindExposureColors(Blight.workingColors, plant);
			}
			else
			{
				Blight.workingColors[0].a = (Blight.workingColors[1].a = (Blight.workingColors[2].a = (Blight.workingColors[3].a = 0)));
			}
			float num = Blight.SizeRange.LerpThroughRange(this.severity);
			if (plant != null)
			{
				float a = plant.Graphic.drawSize.x * plant.def.plant.visualSizeRange.LerpThroughRange(plant.Growth);
				num *= Mathf.Min(a, 1f);
			}
			num = Mathf.Clamp(num, 0.5f, 0.9f);
			Printer_Plane.PrintPlane(layer, this.TrueCenter(), this.def.graphic.drawSize * num, this.Graphic.MatAt(base.Rotation, this), 0f, false, null, Blight.workingColors, 0.1f, 0f);
		}

		// Token: 0x04005361 RID: 21345
		private float severity = 0.2f;

		// Token: 0x04005362 RID: 21346
		private int lastPlantHarmTick;

		// Token: 0x04005363 RID: 21347
		private float lastMapMeshUpdateSeverity;

		// Token: 0x04005364 RID: 21348
		private const float InitialSeverity = 0.2f;

		// Token: 0x04005365 RID: 21349
		private const float SeverityPerDay = 1f;

		// Token: 0x04005366 RID: 21350
		private const int DamagePerDay = 5;

		// Token: 0x04005367 RID: 21351
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x04005368 RID: 21352
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x04005369 RID: 21353
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x0400536A RID: 21354
		private const float ReproductionRadius = 4f;

		// Token: 0x0400536B RID: 21355
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x0400536C RID: 21356
		private static Color32[] workingColors = new Color32[4];
	}
}
