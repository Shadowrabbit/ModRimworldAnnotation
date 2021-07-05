using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001045 RID: 4165
	[StaticConstructorOnStartup]
	public class Frame : Building, IThingHolder, IConstructible
	{
		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06006271 RID: 25201 RVA: 0x00215CDC File Offset: 0x00213EDC
		public float WorkToBuild
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, base.Stuff);
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06006272 RID: 25202 RVA: 0x00215CF9 File Offset: 0x00213EF9
		public float WorkLeft
		{
			get
			{
				return this.WorkToBuild - this.workDone;
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06006273 RID: 25203 RVA: 0x00215D08 File Offset: 0x00213F08
		public float PercentComplete
		{
			get
			{
				return this.workDone / this.WorkToBuild;
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06006274 RID: 25204 RVA: 0x00215D17 File Offset: 0x00213F17
		public override string Label
		{
			get
			{
				return this.LabelEntityToBuild + "FrameLabelExtra".Translate();
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06006275 RID: 25205 RVA: 0x00215D34 File Offset: 0x00213F34
		public string LabelEntityToBuild
		{
			get
			{
				string text = this.def.entityDefToBuild.label;
				if (base.StyleSourcePrecept != null)
				{
					text = base.StyleSourcePrecept.TransformThingLabel(text);
				}
				if (base.Stuff != null)
				{
					return "ThingMadeOfStuffLabel".Translate(base.Stuff.LabelAsStuff, text);
				}
				return text;
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06006276 RID: 25206 RVA: 0x00215D98 File Offset: 0x00213F98
		public override Color DrawColor
		{
			get
			{
				if (!this.def.MadeFromStuff)
				{
					List<ThingDefCountClass> costList = this.def.entityDefToBuild.CostList;
					if (costList != null)
					{
						for (int i = 0; i < costList.Count; i++)
						{
							ThingDef thingDef = costList[i].thingDef;
							if (thingDef.IsStuff && thingDef.stuffProps.color != Color.white)
							{
								return this.def.GetColorForStuff(thingDef);
							}
						}
					}
					return new Color(0.6f, 0.6f, 0.6f);
				}
				return base.DrawColor;
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06006277 RID: 25207 RVA: 0x00215E2C File Offset: 0x0021402C
		public EffecterDef ConstructionEffect
		{
			get
			{
				if (base.Stuff != null && base.Stuff.stuffProps.constructEffect != null)
				{
					return base.Stuff.stuffProps.constructEffect;
				}
				if (this.def.entityDefToBuild.constructEffect != null)
				{
					return this.def.entityDefToBuild.constructEffect;
				}
				return EffecterDefOf.ConstructMetal;
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06006278 RID: 25208 RVA: 0x00215E8C File Offset: 0x0021408C
		private Material CornerMat
		{
			get
			{
				if (this.cachedCornerMat == null)
				{
					this.cachedCornerMat = MaterialPool.MatFrom(Frame.CornerTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedCornerMat;
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x06006279 RID: 25209 RVA: 0x00215EBD File Offset: 0x002140BD
		private Material TileMat
		{
			get
			{
				if (this.cachedTileMat == null)
				{
					this.cachedTileMat = MaterialPool.MatFrom(Frame.TileTex, ShaderDatabase.Cutout, this.DrawColor);
				}
				return this.cachedTileMat;
			}
		}

		// Token: 0x0600627A RID: 25210 RVA: 0x00215EEE File Offset: 0x002140EE
		public Frame()
		{
			this.resourceContainer = new ThingOwner<Thing>(this, false, LookMode.Deep);
		}

		// Token: 0x0600627B RID: 25211 RVA: 0x00215F0F File Offset: 0x0021410F
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.resourceContainer;
		}

		// Token: 0x0600627C RID: 25212 RVA: 0x00215F17 File Offset: 0x00214117
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600627D RID: 25213 RVA: 0x00215F25 File Offset: 0x00214125
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.workDone, "workDone", 0f, false);
			Scribe_Deep.Look<ThingOwner>(ref this.resourceContainer, "resourceContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600627E RID: 25214 RVA: 0x00215F60 File Offset: 0x00214160
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (spawned)
			{
				ThingUtility.CheckAutoRebuildOnDestroyed(this, mode, map, this.def.entityDefToBuild);
			}
		}

		// Token: 0x0600627F RID: 25215 RVA: 0x00215F96 File Offset: 0x00214196
		public ThingDef EntityToBuildStuff()
		{
			return base.Stuff;
		}

		// Token: 0x06006280 RID: 25216 RVA: 0x00215FA0 File Offset: 0x002141A0
		public List<ThingDefCountClass> MaterialsNeeded()
		{
			this.cachedMaterialsNeeded.Clear();
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				int num = this.resourceContainer.TotalStackCountOfDef(thingDefCountClass.thingDef);
				int num2 = thingDefCountClass.count - num;
				if (num2 > 0)
				{
					this.cachedMaterialsNeeded.Add(new ThingDefCountClass(thingDefCountClass.thingDef, num2));
				}
			}
			return this.cachedMaterialsNeeded;
		}

		// Token: 0x06006281 RID: 25217 RVA: 0x00216028 File Offset: 0x00214228
		public void CompleteConstruction(Pawn worker)
		{
			if (worker.Faction != null)
			{
				QuestUtility.SendQuestTargetSignals(worker.Faction.questTags, "BuiltBuilding", this.Named("SUBJECT"));
			}
			this.resourceContainer.ClearAndDestroyContents(DestroyMode.Vanish);
			Map map = base.Map;
			this.Destroy(DestroyMode.Vanish);
			if (this.GetStatValue(StatDefOf.WorkToBuild, true) > 150f && this.def.entityDefToBuild is ThingDef && ((ThingDef)this.def.entityDefToBuild).category == ThingCategory.Building)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
			}
			ThingDef thingDef = this.def.entityDefToBuild as ThingDef;
			Thing thing = null;
			if (thingDef != null)
			{
				thing = ThingMaker.MakeThing(thingDef, base.Stuff);
				thing.SetFactionDirect(base.Faction);
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					QualityCategory q = QualityUtility.GenerateQualityCreatedByPawn(worker, SkillDefOf.Construction);
					compQuality.SetQuality(q, ArtGenerationContext.Colony);
					QualityUtility.SendCraftNotification(thing, worker);
				}
				CompArt compArt = thing.TryGetComp<CompArt>();
				if (compArt != null)
				{
					if (compQuality == null)
					{
						compArt.InitializeArt(ArtGenerationContext.Colony);
					}
					compArt.JustCreatedBy(worker);
				}
				if (worker.Ideo != null)
				{
					thing.StyleDef = worker.Ideo.GetStyleFor(thingDef);
				}
				thing.HitPoints = Mathf.CeilToInt((float)this.HitPoints / (float)base.MaxHitPoints * (float)thing.MaxHitPoints);
				GenSpawn.Spawn(thing, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
				Building building;
				if ((building = (thing as Building)) != null)
				{
					Lord lord = worker.GetLord();
					if (lord != null)
					{
						lord.AddBuilding(building);
					}
					building.StyleSourcePrecept = base.StyleSourcePrecept;
				}
				if (thingDef != null)
				{
					Color? ideoColorForBuilding = IdeoUtility.GetIdeoColorForBuilding(thingDef, base.Faction);
					if (ideoColorForBuilding != null)
					{
						thing.SetColor(ideoColorForBuilding.Value, true);
					}
				}
			}
			else
			{
				map.terrainGrid.SetTerrain(base.Position, (TerrainDef)this.def.entityDefToBuild);
				FilthMaker.RemoveAllFilth(base.Position, map);
			}
			worker.records.Increment(RecordDefOf.ThingsConstructed);
			if (thing != null && thing.GetStatValue(StatDefOf.WorkToBuild, true) >= 9500f)
			{
				TaleRecorder.RecordTale(TaleDefOf.CompletedLongConstructionProject, new object[]
				{
					worker,
					thing.def
				});
			}
		}

		// Token: 0x06006282 RID: 25218 RVA: 0x00216260 File Offset: 0x00214460
		public void FailConstruction(Pawn worker)
		{
			Map map = base.Map;
			this.Destroy(DestroyMode.FailConstruction);
			Blueprint_Build blueprint_Build = null;
			if (this.def.entityDefToBuild.blueprintDef != null)
			{
				blueprint_Build = (Blueprint_Build)ThingMaker.MakeThing(this.def.entityDefToBuild.blueprintDef, null);
				blueprint_Build.stuffToUse = base.Stuff;
				blueprint_Build.SetFactionDirect(base.Faction);
				blueprint_Build.StyleSourcePrecept = base.StyleSourcePrecept;
				GenSpawn.Spawn(blueprint_Build, base.Position, map, base.Rotation, WipeMode.FullRefund, false);
			}
			Lord lord = worker.GetLord();
			if (lord != null)
			{
				lord.Notify_ConstructionFailed(worker, this, blueprint_Build);
			}
			MoteMaker.ThrowText(this.DrawPos, map, "TextMote_ConstructionFail".Translate(), 6f);
			if (base.Faction == Faction.OfPlayer && this.WorkToBuild > 1400f)
			{
				Messages.Message("MessageConstructionFailed".Translate(this.LabelEntityToBuild, worker.LabelShort, worker.Named("WORKER")), new TargetInfo(base.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06006283 RID: 25219 RVA: 0x00216380 File Offset: 0x00214580
		public override void Draw()
		{
			Vector2 vector = new Vector2((float)this.def.size.x, (float)this.def.size.z);
			vector.x *= 1.15f;
			vector.y *= 1.15f;
			Vector3 s = new Vector3(vector.x, 1f, vector.y);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(this.DrawPos, base.Rotation.AsQuat, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, Frame.UnderfieldMat, 0);
			int num = 4;
			for (int i = 0; i < num; i++)
			{
				float num2 = (float)Mathf.Min(base.RotatedSize.x, base.RotatedSize.z) * 0.38f;
				IntVec3 intVec = default(IntVec3);
				if (i == 0)
				{
					intVec = new IntVec3(-1, 0, -1);
				}
				else if (i == 1)
				{
					intVec = new IntVec3(-1, 0, 1);
				}
				else if (i == 2)
				{
					intVec = new IntVec3(1, 0, 1);
				}
				else if (i == 3)
				{
					intVec = new IntVec3(1, 0, -1);
				}
				Vector3 b = default(Vector3);
				b.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num2 / 2f);
				b.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num2 / 2f);
				Vector3 s2 = new Vector3(num2, 1f, num2);
				Matrix4x4 matrix2 = default(Matrix4x4);
				matrix2.SetTRS(this.DrawPos + Vector3.up * 0.03f + b, new Rot4(i).AsQuat, s2);
				Graphics.DrawMesh(MeshPool.plane10, matrix2, this.CornerMat, 0);
			}
			int num3 = Mathf.CeilToInt((this.PercentComplete - 0f) / 1f * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
			IntVec2 intVec2 = base.RotatedSize * 2;
			for (int j = 0; j < num3; j++)
			{
				IntVec2 intVec3 = default(IntVec2);
				intVec3.z = j / intVec2.x;
				intVec3.x = j - intVec3.z * intVec2.x;
				Vector3 a = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + this.DrawPos;
				a.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
				a.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
				Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
				Matrix4x4 matrix3 = default(Matrix4x4);
				matrix3.SetTRS(a + Vector3.up * 0.02f, Quaternion.identity, s3);
				Graphics.DrawMesh(MeshPool.plane10, matrix3, this.TileMat, 0);
			}
			base.Comps_PostDraw();
		}

		// Token: 0x06006284 RID: 25220 RVA: 0x002166CD File Offset: 0x002148CD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, base.Stuff);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006285 RID: 25221 RVA: 0x002166E0 File Offset: 0x002148E0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			List<ThingDefCountClass> list = this.def.entityDefToBuild.CostListAdjusted(base.Stuff, true);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass need = list[i];
				int num = need.count;
				IEnumerable<ThingDefCountClass> source = this.MaterialsNeeded();
				Func<ThingDefCountClass, bool> predicate;
				Func<ThingDefCountClass, bool> <>9__0;
				if ((predicate = <>9__0) == null)
				{
					predicate = (<>9__0 = ((ThingDefCountClass needed) => needed.thingDef == need.thingDef));
				}
				foreach (ThingDefCountClass thingDefCountClass in source.Where(predicate))
				{
					num -= thingDefCountClass.count;
				}
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					need.thingDef.LabelCap + ": ",
					num,
					" / ",
					need.count
				}));
			}
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkLeft.ToStringWorkAmount());
			return stringBuilder.ToString();
		}

		// Token: 0x06006286 RID: 25222 RVA: 0x00216878 File Offset: 0x00214A78
		public override ushort PathFindCostFor(Pawn p)
		{
			if (base.Faction == null)
			{
				return 0;
			}
			if (this.def.entityDefToBuild is TerrainDef)
			{
				return 0;
			}
			if (p.Faction == base.Faction || p.HostFaction == base.Faction)
			{
				return Frame.AvoidUnderConstructionPathFindCost;
			}
			return 0;
		}

		// Token: 0x040037E1 RID: 14305
		public ThingOwner resourceContainer;

		// Token: 0x040037E2 RID: 14306
		public float workDone;

		// Token: 0x040037E3 RID: 14307
		private Material cachedCornerMat;

		// Token: 0x040037E4 RID: 14308
		private Material cachedTileMat;

		// Token: 0x040037E5 RID: 14309
		protected const float UnderfieldOverdrawFactor = 1.15f;

		// Token: 0x040037E6 RID: 14310
		protected const float CenterOverdrawFactor = 0.5f;

		// Token: 0x040037E7 RID: 14311
		private const int LongConstructionProjectThreshold = 9500;

		// Token: 0x040037E8 RID: 14312
		private static readonly Material UnderfieldMat = MaterialPool.MatFrom("Things/Building/BuildingFrame/Underfield", ShaderDatabase.Transparent);

		// Token: 0x040037E9 RID: 14313
		private static readonly Texture2D CornerTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Corner", true);

		// Token: 0x040037EA RID: 14314
		private static readonly Texture2D TileTex = ContentFinder<Texture2D>.Get("Things/Building/BuildingFrame/Tile", true);

		// Token: 0x040037EB RID: 14315
		[TweakValue("Pathfinding", 0f, 1000f)]
		public static ushort AvoidUnderConstructionPathFindCost = 800;

		// Token: 0x040037EC RID: 14316
		private List<ThingDefCountClass> cachedMaterialsNeeded = new List<ThingDefCountClass>();
	}
}
