using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020008BD RID: 2237
	public class LordToil_Siege : LordToil
	{
		// Token: 0x17000A9B RID: 2715
		// (get) Token: 0x06003B03 RID: 15107 RVA: 0x0014997D File Offset: 0x00147B7D
		public override IntVec3 FlagLoc
		{
			get
			{
				return this.Data.siegeCenter;
			}
		}

		// Token: 0x17000A9C RID: 2716
		// (get) Token: 0x06003B04 RID: 15108 RVA: 0x0014998A File Offset: 0x00147B8A
		private LordToilData_Siege Data
		{
			get
			{
				return (LordToilData_Siege)this.data;
			}
		}

		// Token: 0x17000A9D RID: 2717
		// (get) Token: 0x06003B05 RID: 15109 RVA: 0x00149997 File Offset: 0x00147B97
		private IEnumerable<Frame> Frames
		{
			get
			{
				LordToilData_Siege data = this.Data;
				float radSquared = (data.baseRadius + 10f) * (data.baseRadius + 10f);
				List<Thing> framesList = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingFrame);
				if (framesList.Count == 0)
				{
					yield break;
				}
				int num;
				for (int i = 0; i < framesList.Count; i = num + 1)
				{
					Frame frame = (Frame)framesList[i];
					if (frame.Faction == this.lord.faction && (float)(frame.Position - data.siegeCenter).LengthHorizontalSquared < radSquared)
					{
						yield return frame;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17000A9E RID: 2718
		// (get) Token: 0x06003B06 RID: 15110 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B07 RID: 15111 RVA: 0x001499A7 File Offset: 0x00147BA7
		public LordToil_Siege(IntVec3 siegeCenter, float blueprintPoints)
		{
			this.data = new LordToilData_Siege();
			this.Data.siegeCenter = siegeCenter;
			this.Data.blueprintPoints = blueprintPoints;
		}

		// Token: 0x06003B08 RID: 15112 RVA: 0x001499E0 File Offset: 0x00147BE0
		public override void Init()
		{
			base.Init();
			LordToilData_Siege data = this.Data;
			data.baseRadius = Mathf.InverseLerp(14f, 25f, (float)this.lord.ownedPawns.Count / 50f);
			data.baseRadius = Mathf.Clamp(data.baseRadius, 14f, 25f);
			List<Thing> list = new List<Thing>();
			foreach (Blueprint_Build blueprint_Build in SiegeBlueprintPlacer.PlaceBlueprints(data.siegeCenter, base.Map, this.lord.faction, data.blueprintPoints))
			{
				data.blueprints.Add(blueprint_Build);
				using (List<ThingDefCountClass>.Enumerator enumerator2 = blueprint_Build.MaterialsNeeded().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ThingDefCountClass cost = enumerator2.Current;
						Thing thing = list.FirstOrDefault((Thing t) => t.def == cost.thingDef);
						if (thing != null)
						{
							thing.stackCount += cost.count;
						}
						else
						{
							Thing thing2 = ThingMaker.MakeThing(cost.thingDef, null);
							thing2.stackCount = cost.count;
							list.Add(thing2);
						}
					}
				}
				ThingDef thingDef = blueprint_Build.def.entityDefToBuild as ThingDef;
				if (thingDef != null)
				{
					ThingDef thingDef2 = TurretGunUtility.TryFindRandomShellDef(thingDef, false, true, this.lord.faction.def.techLevel, false, 250f);
					if (thingDef2 != null)
					{
						Thing thing3 = ThingMaker.MakeThing(thingDef2, null);
						thing3.stackCount = 5;
						list.Add(thing3);
					}
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].stackCount = Mathf.CeilToInt((float)list[i].stackCount * Rand.Range(1f, 1.2f));
			}
			List<List<Thing>> list2 = new List<List<Thing>>();
			for (int j = 0; j < list.Count; j++)
			{
				while (list[j].stackCount > list[j].def.stackLimit)
				{
					int num = Mathf.CeilToInt((float)list[j].def.stackLimit * Rand.Range(0.9f, 0.999f));
					Thing thing4 = ThingMaker.MakeThing(list[j].def, null);
					thing4.stackCount = num;
					list[j].stackCount -= num;
					list.Add(thing4);
				}
			}
			List<Thing> list3 = new List<Thing>();
			for (int k = 0; k < list.Count; k++)
			{
				list3.Add(list[k]);
				if (k % 2 == 1 || k == list.Count - 1)
				{
					list2.Add(list3);
					list3 = new List<Thing>();
				}
			}
			List<Thing> list4 = new List<Thing>();
			int num2 = Mathf.RoundToInt(LordToil_Siege.MealCountRangePerRaider.RandomInRange * (float)this.lord.ownedPawns.Count);
			for (int l = 0; l < num2; l++)
			{
				Thing item = ThingMaker.MakeThing(ThingDefOf.MealSurvivalPack, null);
				list4.Add(item);
			}
			list2.Add(list4);
			DropPodUtility.DropThingGroupsNear(data.siegeCenter, base.Map, list2, 110, false, false, true, true, true, false);
			data.desiredBuilderFraction = LordToil_Siege.BuilderCountFraction.RandomInRange;
		}

		// Token: 0x06003B09 RID: 15113 RVA: 0x00149D9C File Offset: 0x00147F9C
		public override void UpdateAllDuties()
		{
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil < 450)
			{
				for (int i = 0; i < this.lord.ownedPawns.Count; i++)
				{
					this.SetAsDefender(this.lord.ownedPawns[i]);
				}
				return;
			}
			this.rememberedDuties.Clear();
			int num = Mathf.RoundToInt((float)this.lord.ownedPawns.Count * data.desiredBuilderFraction);
			if (num <= 0)
			{
				num = 1;
			}
			int num2 = (from b in base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial)
			where b.def.hasInteractionCell && b.Faction == this.lord.faction && b.Position.InHorDistOf(this.FlagLoc, data.baseRadius)
			select b).Count<Thing>();
			if (num < num2)
			{
				num = num2;
			}
			int num3 = 0;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = this.lord.ownedPawns[j];
				if (pawn.mindState.duty.def == DutyDefOf.Build)
				{
					this.rememberedDuties.Add(pawn, DutyDefOf.Build);
					this.SetAsBuilder(pawn);
					num3++;
				}
			}
			int num4 = num - num3;
			Func<Pawn, bool> <>9__1;
			for (int k = 0; k < num4; k++)
			{
				IEnumerable<Pawn> ownedPawns = this.lord.ownedPawns;
				Func<Pawn, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((Pawn pa) => !this.rememberedDuties.ContainsKey(pa) && this.CanBeBuilder(pa)));
				}
				Pawn pawn2;
				if (ownedPawns.Where(predicate).TryRandomElement(out pawn2))
				{
					this.rememberedDuties.Add(pawn2, DutyDefOf.Build);
					this.SetAsBuilder(pawn2);
					num3++;
				}
			}
			for (int l = 0; l < this.lord.ownedPawns.Count; l++)
			{
				Pawn pawn3 = this.lord.ownedPawns[l];
				if (!this.rememberedDuties.ContainsKey(pawn3))
				{
					this.SetAsDefender(pawn3);
					this.rememberedDuties.Add(pawn3, DutyDefOf.Defend);
				}
			}
			if (num3 == 0)
			{
				this.lord.ReceiveMemo("NoBuilders");
				return;
			}
		}

		// Token: 0x06003B0A RID: 15114 RVA: 0x00149FBA File Offset: 0x001481BA
		public override void Notify_PawnLost(Pawn victim, PawnLostCondition cond)
		{
			this.UpdateAllDuties();
			base.Notify_PawnLost(victim, cond);
		}

		// Token: 0x06003B0B RID: 15115 RVA: 0x00149FCA File Offset: 0x001481CA
		public override void Notify_ConstructionFailed(Pawn pawn, Frame frame, Blueprint_Build newBlueprint)
		{
			base.Notify_ConstructionFailed(pawn, frame, newBlueprint);
			if (frame.Faction == this.lord.faction && newBlueprint != null)
			{
				this.Data.blueprints.Add(newBlueprint);
			}
		}

		// Token: 0x06003B0C RID: 15116 RVA: 0x00149FFC File Offset: 0x001481FC
		private bool CanBeBuilder(Pawn p)
		{
			return !p.WorkTypeIsDisabled(WorkTypeDefOf.Construction) && !p.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter);
		}

		// Token: 0x06003B0D RID: 15117 RVA: 0x0014A01C File Offset: 0x0014821C
		private void SetAsBuilder(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Build, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
			int minLevel = Mathf.Max(ThingDefOf.Sandbags.constructionSkillPrerequisite, ThingDefOf.Turret_Mortar.constructionSkillPrerequisite);
			p.skills.GetSkill(SkillDefOf.Construction).EnsureMinLevelWithMargin(minLevel);
			p.workSettings.EnableAndInitialize();
			List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				WorkTypeDef workTypeDef = allDefsListForReading[i];
				if (workTypeDef == WorkTypeDefOf.Construction)
				{
					p.workSettings.SetPriority(workTypeDef, 1);
				}
				else
				{
					p.workSettings.Disable(workTypeDef);
				}
			}
		}

		// Token: 0x06003B0E RID: 15118 RVA: 0x0014A0EC File Offset: 0x001482EC
		private void SetAsDefender(Pawn p)
		{
			LordToilData_Siege data = this.Data;
			p.mindState.duty = new PawnDuty(DutyDefOf.Defend, data.siegeCenter, -1f);
			p.mindState.duty.radius = data.baseRadius;
		}

		// Token: 0x06003B0F RID: 15119 RVA: 0x0014A13C File Offset: 0x0014833C
		public override void LordToilTick()
		{
			base.LordToilTick();
			LordToilData_Siege data = this.Data;
			if (this.lord.ticksInToil == 450)
			{
				this.lord.CurLordToil.UpdateAllDuties();
			}
			if (this.lord.ticksInToil > 450 && this.lord.ticksInToil % 500 == 0)
			{
				this.UpdateAllDuties();
			}
			if (Find.TickManager.TicksGame % 500 == 0)
			{
				if (!(from frame in this.Frames
				where !frame.Destroyed
				select frame).Any<Frame>())
				{
					if (!(from blue in data.blueprints
					where !blue.Destroyed
					select blue).Any<Blueprint>() && !base.Map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial).Any((Thing b) => b.Faction == this.lord.faction && b.def.building.buildingTags.Contains("Artillery")))
					{
						this.lord.ReceiveMemo("NoArtillery");
						return;
					}
				}
				int num = GenRadial.NumCellsInRadius(20f);
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = data.siegeCenter + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j].def.IsShell)
							{
								num2 += thingList[j].stackCount;
							}
							if (thingList[j].def == ThingDefOf.MealSurvivalPack)
							{
								num3 += thingList[j].stackCount;
							}
							if (thingList[j].def == ThingDefOf.ReinforcedBarrel)
							{
								num5 += thingList[j].stackCount;
							}
							if (Find.Storyteller.difficulty.classicMortars && thingList[j].def.building != null && thingList[j].def.building.IsMortar && thingList[j].Faction == this.lord.faction)
							{
								CompRefuelable compRefuelable = thingList[j].TryGetComp<CompRefuelable>();
								if (compRefuelable != null)
								{
									num4++;
									if (compRefuelable.Props.fuelFilter.Allows(ThingDefOf.ReinforcedBarrel) && compRefuelable.HasFuel)
									{
										num5++;
									}
								}
							}
						}
					}
				}
				if (num2 < 4)
				{
					ThingDef thingDef = TurretGunUtility.TryFindRandomShellDef(ThingDefOf.Turret_Mortar, false, true, this.lord.faction.def.techLevel, false, 250f);
					if (thingDef != null)
					{
						this.DropSupplies(thingDef, 6);
					}
				}
				if (num3 < 5)
				{
					this.DropSupplies(ThingDefOf.MealSurvivalPack, 12);
				}
				if (Find.Storyteller.difficulty.classicMortars && num5 < num4)
				{
					this.DropSupplies(ThingDefOf.ReinforcedBarrel, num4 - num5);
				}
			}
		}

		// Token: 0x06003B10 RID: 15120 RVA: 0x0014A458 File Offset: 0x00148658
		private void DropSupplies(ThingDef thingDef, int count)
		{
			List<Thing> list = new List<Thing>();
			Thing thing = ThingMaker.MakeThing(thingDef, null);
			thing.stackCount = count;
			list.Add(thing);
			DropPodUtility.DropThingsNear(this.Data.siegeCenter, base.Map, list, 110, false, false, true, true);
		}

		// Token: 0x06003B11 RID: 15121 RVA: 0x0014A4A0 File Offset: 0x001486A0
		public override void Cleanup()
		{
			LordToilData_Siege data = this.Data;
			data.blueprints.RemoveAll((Blueprint blue) => blue.Destroyed);
			for (int i = 0; i < data.blueprints.Count; i++)
			{
				data.blueprints[i].Destroy(DestroyMode.Cancel);
			}
			foreach (Frame frame in this.Frames.ToList<Frame>())
			{
				frame.Destroy(DestroyMode.Cancel);
			}
			foreach (Building building in this.lord.ownedBuildings)
			{
				building.SetFaction(null, null);
			}
		}

		// Token: 0x04002025 RID: 8229
		public Dictionary<Pawn, DutyDef> rememberedDuties = new Dictionary<Pawn, DutyDef>();

		// Token: 0x04002026 RID: 8230
		private const float BaseRadiusMin = 14f;

		// Token: 0x04002027 RID: 8231
		private const float BaseRadiusMax = 25f;

		// Token: 0x04002028 RID: 8232
		private static readonly FloatRange MealCountRangePerRaider = new FloatRange(1f, 3f);

		// Token: 0x04002029 RID: 8233
		private const int StartBuildingDelay = 450;

		// Token: 0x0400202A RID: 8234
		private static readonly FloatRange BuilderCountFraction = new FloatRange(0.25f, 0.4f);

		// Token: 0x0400202B RID: 8235
		private const float FractionLossesToAssault = 0.4f;

		// Token: 0x0400202C RID: 8236
		private const int InitalShellsPerCannon = 5;

		// Token: 0x0400202D RID: 8237
		private const int ReplenishAtShells = 4;

		// Token: 0x0400202E RID: 8238
		private const int ShellReplenishCount = 6;

		// Token: 0x0400202F RID: 8239
		private const int ReplenishAtMeals = 5;

		// Token: 0x04002030 RID: 8240
		private const int MealReplenishCount = 12;
	}
}
