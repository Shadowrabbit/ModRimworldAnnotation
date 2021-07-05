using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020011C8 RID: 4552
	[StaticConstructorOnStartup]
	public class CompTreeConnection : ThingComp
	{
		// Token: 0x1700130B RID: 4875
		// (get) Token: 0x06006DB8 RID: 28088 RVA: 0x0024C165 File Offset: 0x0024A365
		public CompProperties_TreeConnection Props
		{
			get
			{
				return (CompProperties_TreeConnection)this.props;
			}
		}

		// Token: 0x1700130C RID: 4876
		// (get) Token: 0x06006DB9 RID: 28089 RVA: 0x0024C172 File Offset: 0x0024A372
		public Pawn ConnectedPawn
		{
			get
			{
				return this.connectedPawn;
			}
		}

		// Token: 0x1700130D RID: 4877
		// (get) Token: 0x06006DBA RID: 28090 RVA: 0x0024C17A File Offset: 0x0024A37A
		public bool Connected
		{
			get
			{
				return this.ConnectedPawn != null;
			}
		}

		// Token: 0x1700130E RID: 4878
		// (get) Token: 0x06006DBB RID: 28091 RVA: 0x0024C185 File Offset: 0x0024A385
		public bool ConnectionTorn
		{
			get
			{
				return this.nextUntornTick >= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x06006DBC RID: 28092 RVA: 0x0024C19C File Offset: 0x0024A39C
		public bool HasProductionMode
		{
			get
			{
				return this.desiredMode != null;
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x06006DBD RID: 28093 RVA: 0x0024C1A7 File Offset: 0x0024A3A7
		public int UntornInDurationTicks
		{
			get
			{
				return this.nextUntornTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x06006DBE RID: 28094 RVA: 0x0024C1BA File Offset: 0x0024A3BA
		public GauranlenTreeModeDef Mode
		{
			get
			{
				return this.currentMode;
			}
		}

		// Token: 0x17001312 RID: 4882
		// (get) Token: 0x06006DBF RID: 28095 RVA: 0x0024C1C2 File Offset: 0x0024A3C2
		public PawnKindDef DryadKind
		{
			get
			{
				GauranlenTreeModeDef mode = this.Mode;
				return ((mode != null) ? mode.pawnKindDef : null) ?? PawnKindDefOf.Dryad_Basic;
			}
		}

		// Token: 0x17001313 RID: 4883
		// (get) Token: 0x06006DC0 RID: 28096 RVA: 0x0024C1DF File Offset: 0x0024A3DF
		public int MaxDryads
		{
			get
			{
				if (!this.Connected)
				{
					return this.Props.maxDryadsWild;
				}
				return (int)this.Props.maxDryadsPerConnectionStrengthCurve.Evaluate(this.ConnectionStrength);
			}
		}

		// Token: 0x17001314 RID: 4884
		// (get) Token: 0x06006DC1 RID: 28097 RVA: 0x0024C20C File Offset: 0x0024A40C
		private int SpawningDurationTicks
		{
			get
			{
				return (int)(60000f * this.Props.spawnDays);
			}
		}

		// Token: 0x17001315 RID: 4885
		// (get) Token: 0x06006DC2 RID: 28098 RVA: 0x0024C220 File Offset: 0x0024A420
		// (set) Token: 0x06006DC3 RID: 28099 RVA: 0x0024C228 File Offset: 0x0024A428
		public float DesiredConnectionStrength
		{
			get
			{
				return this.desiredConnectionStrength;
			}
			set
			{
				this.desiredConnectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001316 RID: 4886
		// (get) Token: 0x06006DC4 RID: 28100 RVA: 0x0024C236 File Offset: 0x0024A436
		// (set) Token: 0x06006DC5 RID: 28101 RVA: 0x0024C23E File Offset: 0x0024A43E
		public float ConnectionStrength
		{
			get
			{
				return this.connectionStrength;
			}
			set
			{
				this.connectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17001317 RID: 4887
		// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x0024C24C File Offset: 0x0024A44C
		private Material PodMat
		{
			get
			{
				if (this.cachedPodMat == null)
				{
					this.cachedPodMat = MaterialPool.MatFrom("Things/Building/Misc/DryadFormingPod/DryadFormingPod", ShaderDatabase.Cutout);
				}
				return this.cachedPodMat;
			}
		}

		// Token: 0x17001318 RID: 4888
		// (get) Token: 0x06006DC7 RID: 28103 RVA: 0x0024C277 File Offset: 0x0024A477
		private List<Thing> BuildingsReducingConnectionStrength
		{
			get
			{
				return this.parent.Map.listerArtificialBuildingsForMeditation.GetForCell(this.parent.Position, this.Props.radiusToBuildingForConnectionStrengthLoss);
			}
		}

		// Token: 0x17001319 RID: 4889
		// (get) Token: 0x06006DC8 RID: 28104 RVA: 0x0024C2A4 File Offset: 0x0024A4A4
		private float ConnectionStrengthLossPerDay
		{
			get
			{
				float num = this.Props.connectionLossPerLevelCurve.Evaluate(this.ConnectionStrength);
				List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
				if (this.parent.Spawned && buildingsReducingConnectionStrength.Any<Thing>())
				{
					num += this.Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(this.ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
				}
				return num;
			}
		}

		// Token: 0x1700131A RID: 4890
		// (get) Token: 0x06006DC9 RID: 28105 RVA: 0x0024C300 File Offset: 0x0024A500
		public float ConnectionStrengthGainPerHourOfPruning
		{
			get
			{
				float num = this.Props.connectionStrengthGainPerHourPruningBase;
				if (this.Props.connectionStrengthGainPerPlantSkill != null)
				{
					num *= this.Props.connectionStrengthGainPerPlantSkill.Evaluate((float)this.ConnectedPawn.skills.GetSkill(SkillDefOf.Plants).Level);
				}
				return num;
			}
		}

		// Token: 0x1700131B RID: 4891
		// (get) Token: 0x06006DCA RID: 28106 RVA: 0x0024C358 File Offset: 0x0024A558
		private float MinConnectionStrengthForSingleDryad
		{
			get
			{
				foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve.Points)
				{
					if (curvePoint.y > 0f)
					{
						return curvePoint.x;
					}
				}
				return 0f;
			}
		}

		// Token: 0x06006DCB RID: 28107 RVA: 0x0024C3D0 File Offset: 0x0024A5D0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!respawningAfterLoad)
			{
				this.lastPrunedTick = Find.TickManager.TicksGame;
				this.spawnTick = Find.TickManager.TicksGame + this.SpawningDurationTicks;
			}
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x0024C420 File Offset: 0x0024A620
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.leafEffecter = null;
			Pawn pawn = this.ConnectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.Notify_ConnectedThingDestroyed(this.parent);
				}
			}
			for (int i = 0; i < this.dryads.Count; i++)
			{
				Pawn_ConnectionsTracker connections2 = this.dryads[i].connections;
				if (connections2 != null)
				{
					connections2.Notify_ConnectedThingDestroyed(this.parent);
				}
				this.dryads[i].forceNoDeathNotification = true;
				this.dryads[i].Kill(null, null);
				this.dryads[i].forceNoDeathNotification = false;
			}
			if (this.Connected && this.ConnectedPawn.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelConnectedTreeDestroyed".Translate(this.parent.Named("TREE")), "LetterTextConnectedTreeDestroyed".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("CONNECTEDPAWN")), LetterDefOf.NegativeEvent, this.ConnectedPawn, null, null, null, null);
			}
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x0024C55C File Offset: 0x0024A75C
		public override void CompTick()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (Find.TickManager.TicksGame >= this.spawnTick)
			{
				this.SpawnDryad();
			}
			if (this.leafEffecter == null)
			{
				this.leafEffecter = EffecterDefOf.GauranlenLeavesBatch.Spawn();
				this.leafEffecter.Trigger(this.parent, this.parent);
			}
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.parent, this.parent);
			}
			if (this.Connected && Find.TickManager.TicksGame - this.lastPrunedTick > 1)
			{
				this.ConnectionStrength -= this.ConnectionStrengthLossPerDay / 60000f;
			}
			if (this.parent.IsHashIntervalTick(300))
			{
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.dryads.Count >= 3)
				{
					IntVec3 loc;
					if (this.gaumakerPod == null && CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 3, delegate(IntVec3 x)
					{
						Thing thing;
						return ThingDefOf.Plant_PodGauranlen.CanEverPlantAt(x, this.parent.Map, out thing, false).Accepted;
					}, out loc, -1))
					{
						this.gaumakerPod = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GaumakerCocoon, null), loc, this.parent.Map, WipeMode.Vanish);
						return;
					}
				}
				else if (this.gaumakerPod != null && !this.gaumakerPod.Destroyed)
				{
					this.gaumakerPod.Destroy(DestroyMode.Vanish);
					this.gaumakerPod = null;
				}
			}
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x0024C6D0 File Offset: 0x0024A8D0
		public void ConnectToPawn(Pawn pawn)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				return;
			}
			if (this.ConnectionTorn)
			{
				return;
			}
			this.connectedPawn = pawn;
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.ConnectionStrength = this.Props.initialConnectionStrength;
			this.lastPrunedTick = 0;
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
				MentalState mentalState = this.dryads[i].MentalState;
				if (mentalState != null)
				{
					mentalState.RecoverFromState();
				}
			}
		}

		// Token: 0x06006DCF RID: 28111 RVA: 0x0024C770 File Offset: 0x0024A970
		public void FinalizeMode()
		{
			this.currentMode = this.desiredMode;
			if (this.Connected)
			{
				MoteMaker.MakeStaticMote((this.ConnectedPawn.Position.ToVector3Shifted() + this.parent.Position.ToVector3Shifted()) / 2f, this.parent.Map, ThingDefOf.Mote_GauranlenCasteChanged, 1f);
			}
		}

		// Token: 0x06006DD0 RID: 28112 RVA: 0x0024C7E4 File Offset: 0x0024A9E4
		public void Notify_PawnDied(Pawn p)
		{
			if (this.connectedPawn == p)
			{
				this.TearConnection();
				return;
			}
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (p == this.dryads[i])
				{
					if (this.Connected)
					{
						Pawn_NeedsTracker needs = this.ConnectedPawn.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									thoughts.memories.TryGainMemory(ThoughtDefOf.DryadDied, null, null);
								}
							}
						}
						this.ConnectionStrength -= this.Props.connectionStrengthLossPerDryadDeath;
					}
					this.dryads.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06006DD1 RID: 28113 RVA: 0x0024C88E File Offset: 0x0024AA8E
		public void RemoveDryad(Pawn oldDryad)
		{
			this.dryads.Remove(oldDryad);
		}

		// Token: 0x06006DD2 RID: 28114 RVA: 0x0024C8A0 File Offset: 0x0024AAA0
		public bool ShouldReturnToTree(Pawn dryad)
		{
			if (this.dryads.NullOrEmpty<Pawn>() || !this.dryads.Contains(dryad))
			{
				return false;
			}
			int num = this.dryads.Count - this.MaxDryads;
			if (num <= 0)
			{
				return false;
			}
			this.tmpDryads.Clear();
			this.tmpDryads.AddRange(this.dryads);
			this.tmpDryads.SortBy((Pawn x) => x.ageTracker.AgeChronologicalTicks);
			for (int i = 0; i < num; i++)
			{
				if (this.tmpDryads[i] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x06006DD3 RID: 28115 RVA: 0x0024C960 File Offset: 0x0024AB60
		public bool ShouldEnterGaumakerPod(Pawn dryad)
		{
			if (this.gaumakerPod == null || this.gaumakerPod.Destroyed)
			{
				return false;
			}
			if (this.dryads.NullOrEmpty<Pawn>() || this.dryads.Count < 3 || !this.dryads.Contains(dryad))
			{
				return false;
			}
			this.tmpDryads.Clear();
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (this.dryads[i].kindDef == PawnKindDefOf.Dryad_Gaumaker)
				{
					this.tmpDryads.Add(this.dryads[i]);
				}
			}
			if (this.tmpDryads.Count < 3)
			{
				this.tmpDryads.Clear();
				return false;
			}
			this.tmpDryads.SortBy((Pawn x) => -x.ageTracker.AgeChronologicalTicks);
			for (int j = 0; j < 3; j++)
			{
				if (this.tmpDryads[j] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x06006DD4 RID: 28116 RVA: 0x0024CA78 File Offset: 0x0024AC78
		private void TearConnection()
		{
			Messages.Message("MessageConnectedPawnDied".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("PAWN"), 1800000.ToStringTicksToDays("F1").Named("DURATION")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
			}
			SoundDefOf.GauranlenConnectionTorn.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			this.nextUntornTick = Find.TickManager.TicksGame + 1800000;
			this.connectedPawn = null;
			this.currentMode = null;
		}

		// Token: 0x06006DD5 RID: 28117 RVA: 0x0024CB4C File Offset: 0x0024AD4C
		private void SpawnDryad()
		{
			this.spawnTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.spawnDays);
			Pawn pawn = this.GenerateNewDryad(this.Props.pawnKind);
			GenSpawn.Spawn(pawn, this.parent.Position, this.parent.Map, WipeMode.Vanish).Rotation = Rot4.South;
			EffecterDefOf.DryadSpawn.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(SoundInfo.InMap(pawn, MaintenanceType.None));
		}

		// Token: 0x06006DD6 RID: 28118 RVA: 0x0024CBF8 File Offset: 0x0024ADF8
		public Pawn GenerateNewDryad(PawnKindDef dryadCaste)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest
			{
				KindDef = dryadCaste,
				Newborn = true,
				FixedGender = new Gender?(Gender.Male)
			});
			this.ResetDryad(pawn);
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.dryads.Add(pawn);
			return pawn;
		}

		// Token: 0x06006DD7 RID: 28119 RVA: 0x0024CC5C File Offset: 0x0024AE5C
		private void ResetDryad(Pawn dryad)
		{
			if (this.Connected)
			{
				Faction faction = dryad.Faction;
				Pawn pawn = this.ConnectedPawn;
				if (faction != ((pawn != null) ? pawn.Faction : null))
				{
					Pawn pawn2 = this.ConnectedPawn;
					dryad.SetFaction((pawn2 != null) ? pawn2.Faction : null, null);
				}
			}
			if (dryad.training != null)
			{
				foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefs)
				{
					if (dryad.training.CanAssignToTrain(trainableDef).Accepted)
					{
						dryad.training.SetWantedRecursive(trainableDef, true);
						dryad.training.Train(trainableDef, this.ConnectedPawn, true);
						if (trainableDef == TrainableDefOf.Release)
						{
							dryad.playerSettings.followDrafted = true;
						}
					}
				}
			}
		}

		// Token: 0x06006DD8 RID: 28120 RVA: 0x0024CD30 File Offset: 0x0024AF30
		public void Prune()
		{
			this.lastPrunedTick = Find.TickManager.TicksGame;
			this.ConnectionStrength += this.ConnectionStrengthGainPerHourOfPruning / 2500f;
		}

		// Token: 0x06006DD9 RID: 28121 RVA: 0x0024CD5C File Offset: 0x0024AF5C
		public bool ShouldBePrunedNow(bool forced)
		{
			if (this.ConnectionStrength >= this.desiredConnectionStrength)
			{
				return false;
			}
			if (!forced)
			{
				if (this.ConnectionStrength >= this.desiredConnectionStrength - 0.03f)
				{
					return false;
				}
				if ((float)Find.TickManager.TicksGame < (float)this.lastPrunedTick + this.TimeBetweenAutoPruning)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06006DDA RID: 28122 RVA: 0x0024CDB0 File Offset: 0x0024AFB0
		private float ClosestDistanceToBlockingBuilding(List<Thing> buildings)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < buildings.Count; i++)
			{
				float num2 = buildings[i].Position.DistanceTo(this.parent.Position);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06006DDB RID: 28123 RVA: 0x0024CDF8 File Offset: 0x0024AFF8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Connected)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "ChangeMode".Translate();
				command_Action.defaultDesc = "ChangeModeDesc".Translate(this.parent.Named("TREE"));
				command_Action.icon = ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeDryads", true);
				command_Action.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_ChangeDryadCaste(this.parent));
				};
				if (!this.ConnectedPawn.Spawned || this.ConnectedPawn.Map != this.parent.Map)
				{
					command_Action.Disable("ConnectedPawnAway".Translate(this.ConnectedPawn.Named("PAWN")));
				}
				yield return command_Action;
				if (this.pruningGizmo == null)
				{
					this.pruningGizmo = new Gizmo_PruningConfig(this);
				}
				yield return this.pruningGizmo;
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn dryad",
					action = delegate()
					{
						this.SpawnDryad();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Connection strength -10%",
					action = delegate()
					{
						this.ConnectionStrength -= 0.1f;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Connection strength +10%",
					action = delegate()
					{
						this.ConnectionStrength += 0.1f;
					}
				};
			}
			yield break;
		}

		// Token: 0x06006DDC RID: 28124 RVA: 0x0024CE08 File Offset: 0x0024B008
		public override void PostDraw()
		{
			if (this.dryads.Count < this.MaxDryads)
			{
				Matrix4x4 matrix = default(Matrix4x4);
				Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor()) + this.Props.spawningPodOffset;
				float num = this.Props.spawningPodSizeRange.LerpThroughRange(1f - (float)this.spawnTick - (float)Find.TickManager.TicksGame / (float)this.SpawningDurationTicks);
				matrix.SetTRS(pos, Quaternion.identity, new Vector3(num, 1f, num));
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.PodMat, 0);
			}
		}

		// Token: 0x06006DDD RID: 28125 RVA: 0x0024CEBC File Offset: 0x0024B0BC
		public override void PostDrawExtraSelectionOverlays()
		{
			Pawn pawn = this.connectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.DrawConnectionLine(this.parent);
				}
			}
			if (this.parent.Spawned && this.Connected)
			{
				int num = 0;
				foreach (Thing t in this.BuildingsReducingConnectionStrength)
				{
					if (num++ > 10)
					{
						break;
					}
					GenDraw.DrawLineBetween(this.parent.TrueCenter(), t.TrueCenter(), SimpleColor.Red, 0.2f);
				}
			}
		}

		// Token: 0x06006DDE RID: 28126 RVA: 0x0024CF68 File Offset: 0x0024B168
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			if (this.ConnectionTorn)
			{
				text += "ConnectionTorn".Translate(this.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true)).Resolve();
			}
			else
			{
				text = text + "ConnectedPawn".Translate().Resolve() + ": " + (this.Connected ? this.connectedPawn.NameFullColored : "Nobody".Translate().CapitalizeFirst()).Resolve();
				if (this.Connected)
				{
					text = text + " (" + "ConnectionStrengthDisplay".Translate(this.ConnectionStrength.ToStringPercent()) + ")";
				}
			}
			if (this.Connected)
			{
				text += "\n";
				if (this.lastPrunedTick < 0 || Find.TickManager.TicksGame - this.lastPrunedTick > 60)
				{
					text = text + "ConnectionStrengthFall".Translate() + ": " + "PerDay".Translate(this.ConnectionStrengthLossPerDay.ToStringPercent()).Resolve();
					if (this.BuildingsReducingConnectionStrength.Any<Thing>())
					{
						text = text + " (" + "TooCloseToBuildings".Translate().Colorize(ColorLibrary.RedReadable) + ")";
					}
				}
				else
				{
					text = text + "PruningConnectionStrength".Translate() + ": " + "PerHour".Translate(this.ConnectionStrengthGainPerHourOfPruning.ToStringPercent()).Resolve();
				}
				if (this.Mode != null)
				{
					text += "\n" + "GauranlenTreeMode".Translate() + ": " + this.Mode.LabelCap;
				}
				if (this.HasProductionMode && this.Mode != this.desiredMode)
				{
					text = text + "\n" + "WaitingForConnectorToChangeCaste".Translate(this.ConnectedPawn.Named("CONNECTEDPAWN")).Resolve();
				}
				else if (this.dryads.Count < this.MaxDryads)
				{
					text = text + "\n" + "SpawningDryadIn".Translate(this.Props.pawnKind.Named("DRYAD"), (this.spawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true).Named("TIME")).Resolve();
				}
				if (this.MaxDryads > 0)
				{
					text = text + "\n" + "DryadPlural".Translate() + string.Format(" ({0}/{1})", this.dryads.Count, this.MaxDryads);
					if (this.dryads.Count > 0)
					{
						text = text + ": " + (from x in this.dryads
						select x.NameShortColored.Resolve()).ToCommaList(false, false).CapitalizeFirst();
					}
				}
				else
				{
					text = text + "\n" + "NotEnoughConnectionStrengthForSingleDryad".Translate(this.MinConnectionStrengthForSingleDryad.ToStringPercent()).Colorize(ColorLibrary.RedReadable);
				}
				if (!this.HasProductionMode)
				{
					text = text + "\n" + "AlertGauranlenTreeWithoutDryadTypeLabel".Translate().Colorize(ColorLibrary.RedReadable);
				}
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.MaxDryads < 3)
				{
					text = text + "\n" + "ConnectionStrengthTooWeakForGaumakerPod".Translate().Colorize(ColorLibrary.RedReadable);
				}
			}
			return text;
		}

		// Token: 0x06006DDF RID: 28127 RVA: 0x0024D351 File Offset: 0x0024B551
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectedPawn".Translate(), (this.ConnectedPawn != null) ? this.ConnectedPawn.NameFullColored : "Nobody".Translate(), "ConnectedPawnDesc".Translate(1800000.ToStringTicksToPeriod(true, false, true, true).Named("DURATION"), this.parent.Named("TREE")), 6010, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.ConnectedPawn, -1)), false);
			if (this.Connected)
			{
				if (this.Mode != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "GauranlenTreeMode".Translate(), this.currentMode.LabelCap, "GauranlenTreeModeDesc".Translate() + "\n\n" + this.currentMode.LabelCap + ": " + this.currentMode.Description, 5990, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.currentMode.pawnKindDef.race, -1)), false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectionStrength".Translate(), this.ConnectionStrength.ToStringPercent(), "ConnectionStrengthDesc".Translate(), 6000, null, null, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "MaximumDryads".Translate(), this.MaxDryads.ToString(), "MaximumDryadsDesc".Translate() + "\n\n" + this.ConnectionStrengthToMaxDryadsDesc(), 5980, null, null, false);
			yield break;
		}

		// Token: 0x06006DE0 RID: 28128 RVA: 0x0024D364 File Offset: 0x0024B564
		private string ConnectionStrengthToMaxDryadsDesc()
		{
			string text = "MaxDryadsBasedOnConnectionStrength".Translate() + ":\n -  " + "Unconnected".Translate() + ": " + this.Props.maxDryadsWild;
			foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve)
			{
				text = text + ("\n -  " + "ConnectionStrengthDisplay".Translate(curvePoint.x.ToStringPercent()) + ": ") + curvePoint.y;
			}
			return text;
		}

		// Token: 0x06006DE1 RID: 28129 RVA: 0x0024D440 File Offset: 0x0024B640
		public override void PostExposeData()
		{
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.currentMode, "currentMode");
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.desiredMode, "desiredMode");
			Scribe_Values.Look<int>(ref this.nextUntornTick, "nextUntornTick", -1, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastPrunedTick, "lastPrunedTick", 0, false);
			Scribe_Values.Look<float>(ref this.desiredConnectionStrength, "desiredConnectionStrength", 0.5f, false);
			Scribe_Values.Look<float>(ref this.connectionStrength, "connectionStrength", 0f, false);
			Scribe_References.Look<Pawn>(ref this.connectedPawn, "connectedPawn", false);
			Scribe_References.Look<Thing>(ref this.gaumakerPod, "gaumakerPod", false);
			Scribe_Collections.Look<Pawn>(ref this.dryads, "dryads", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.dryads.RemoveAll((Pawn x) => x == null || x.Dead);
			}
		}

		// Token: 0x04003CF4 RID: 15604
		private Pawn connectedPawn;

		// Token: 0x04003CF5 RID: 15605
		private int nextUntornTick = -1;

		// Token: 0x04003CF6 RID: 15606
		private int spawnTick = -1;

		// Token: 0x04003CF7 RID: 15607
		private float connectionStrength;

		// Token: 0x04003CF8 RID: 15608
		private List<Pawn> dryads = new List<Pawn>();

		// Token: 0x04003CF9 RID: 15609
		private int lastPrunedTick;

		// Token: 0x04003CFA RID: 15610
		private float desiredConnectionStrength = 0.5f;

		// Token: 0x04003CFB RID: 15611
		private GauranlenTreeModeDef currentMode;

		// Token: 0x04003CFC RID: 15612
		public Thing gaumakerPod;

		// Token: 0x04003CFD RID: 15613
		public GauranlenTreeModeDef desiredMode;

		// Token: 0x04003CFE RID: 15614
		private Material cachedPodMat;

		// Token: 0x04003CFF RID: 15615
		private Effecter leafEffecter;

		// Token: 0x04003D00 RID: 15616
		private Gizmo_PruningConfig pruningGizmo;

		// Token: 0x04003D01 RID: 15617
		private const int ConnectionTornDurationTicks = 1800000;

		// Token: 0x04003D02 RID: 15618
		private const int CheckPodSpawnInterval = 300;

		// Token: 0x04003D03 RID: 15619
		public const int DryadsToCreatePod = 3;

		// Token: 0x04003D04 RID: 15620
		private float TimeBetweenAutoPruning = 10000f;

		// Token: 0x04003D05 RID: 15621
		private const float PruningConnectionStrengthDithering = 0.03f;

		// Token: 0x04003D06 RID: 15622
		private List<Pawn> tmpDryads = new List<Pawn>();
	}
}
