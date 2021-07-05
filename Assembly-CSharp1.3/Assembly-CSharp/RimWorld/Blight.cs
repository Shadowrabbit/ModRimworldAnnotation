using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108F RID: 4239
	public class Blight : Thing
	{
		// Token: 0x1700114B RID: 4427
		// (get) Token: 0x060064EF RID: 25839 RVA: 0x002211BE File Offset: 0x0021F3BE
		// (set) Token: 0x060064F0 RID: 25840 RVA: 0x002211C6 File Offset: 0x0021F3C6
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

		// Token: 0x1700114C RID: 4428
		// (get) Token: 0x060064F1 RID: 25841 RVA: 0x002211D4 File Offset: 0x0021F3D4
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

		// Token: 0x1700114D RID: 4429
		// (get) Token: 0x060064F2 RID: 25842 RVA: 0x002211F1 File Offset: 0x0021F3F1
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

		// Token: 0x060064F3 RID: 25843 RVA: 0x00221225 File Offset: 0x0021F425
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.severity, "severity", 0f, false);
			Scribe_Values.Look<int>(ref this.lastPlantHarmTick, "lastPlantHarmTick", 0, false);
		}

		// Token: 0x060064F4 RID: 25844 RVA: 0x00221255 File Offset: 0x0021F455
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.lastPlantHarmTick = Find.TickManager.TicksGame;
			}
			this.lastMapMeshUpdateSeverity = this.Severity;
		}

		// Token: 0x060064F5 RID: 25845 RVA: 0x00221280 File Offset: 0x0021F480
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

		// Token: 0x060064F6 RID: 25846 RVA: 0x00221311 File Offset: 0x0021F511
		public void Notify_PlantDeSpawned()
		{
			this.DestroyIfNoPlantHere();
		}

		// Token: 0x060064F7 RID: 25847 RVA: 0x0022131A File Offset: 0x0021F51A
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

		// Token: 0x060064F8 RID: 25848 RVA: 0x00221338 File Offset: 0x0021F538
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

		// Token: 0x060064F9 RID: 25849 RVA: 0x002213A0 File Offset: 0x0021F5A0
		private void HarmPlant(Plant plant)
		{
			bool isCrop = plant.IsCrop;
			IntVec3 position = base.Position;
			Map map = base.Map;
			plant.TakeDamage(new DamageInfo(DamageDefOf.Rotting, 5f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
			if (plant.Destroyed && isCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfBlight-" + plant.def.defName, 240f))
			{
				Messages.Message("MessagePlantDiedOfBlight".Translate(plant.Label, plant).CapitalizeFirst(), new TargetInfo(position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x060064FA RID: 25850 RVA: 0x00221454 File Offset: 0x0021F654
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

		// Token: 0x060064FB RID: 25851 RVA: 0x00221478 File Offset: 0x0021F678
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

		// Token: 0x040038D1 RID: 14545
		private float severity = 0.2f;

		// Token: 0x040038D2 RID: 14546
		private int lastPlantHarmTick;

		// Token: 0x040038D3 RID: 14547
		private float lastMapMeshUpdateSeverity;

		// Token: 0x040038D4 RID: 14548
		private const float InitialSeverity = 0.2f;

		// Token: 0x040038D5 RID: 14549
		private const float SeverityPerDay = 1f;

		// Token: 0x040038D6 RID: 14550
		private const int DamagePerDay = 5;

		// Token: 0x040038D7 RID: 14551
		private const float MinSeverityToReproduce = 0.28f;

		// Token: 0x040038D8 RID: 14552
		private const float ReproduceMTBHoursAtMinSeverity = 16.8f;

		// Token: 0x040038D9 RID: 14553
		private const float ReproduceMTBHoursAtMaxSeverity = 2.1f;

		// Token: 0x040038DA RID: 14554
		private const float ReproductionRadius = 4f;

		// Token: 0x040038DB RID: 14555
		private static FloatRange SizeRange = new FloatRange(0.6f, 1f);

		// Token: 0x040038DC RID: 14556
		private static Color32[] workingColors = new Color32[4];
	}
}
