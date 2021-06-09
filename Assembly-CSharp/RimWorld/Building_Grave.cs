using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BC RID: 5820
	public class Building_Grave : Building_Casket, IStoreSettingsParent, IHaulDestination
	{
		// Token: 0x170013CD RID: 5069
		// (get) Token: 0x06007FA2 RID: 32674 RVA: 0x00055BF9 File Offset: 0x00053DF9
		public Pawn AssignedPawn
		{
			get
			{
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x170013CE RID: 5070
		// (get) Token: 0x06007FA3 RID: 32675 RVA: 0x00055C28 File Offset: 0x00053E28
		public CompAssignableToPawn_Grave CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Grave>();
			}
		}

		// Token: 0x170013CF RID: 5071
		// (get) Token: 0x06007FA4 RID: 32676 RVA: 0x0025DC04 File Offset: 0x0025BE04
		public override Graphic Graphic
		{
			get
			{
				if (!this.HasCorpse)
				{
					return base.Graphic;
				}
				if (this.def.building.fullGraveGraphicData == null)
				{
					return base.Graphic;
				}
				if (this.cachedGraphicFull == null)
				{
					this.cachedGraphicFull = this.def.building.fullGraveGraphicData.GraphicColoredFor(this);
				}
				return this.cachedGraphicFull;
			}
		}

		// Token: 0x170013D0 RID: 5072
		// (get) Token: 0x06007FA5 RID: 32677 RVA: 0x00055C30 File Offset: 0x00053E30
		public bool HasCorpse
		{
			get
			{
				return this.Corpse != null;
			}
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x06007FA6 RID: 32678 RVA: 0x0025DC64 File Offset: 0x0025BE64
		public Corpse Corpse
		{
			get
			{
				for (int i = 0; i < this.innerContainer.Count; i++)
				{
					Corpse corpse = this.innerContainer[i] as Corpse;
					if (corpse != null)
					{
						return corpse;
					}
				}
				return null;
			}
		}

		// Token: 0x170013D2 RID: 5074
		// (get) Token: 0x06007FA7 RID: 32679 RVA: 0x00055C3B File Offset: 0x00053E3B
		public bool StorageTabVisible
		{
			get
			{
				return this.AssignedPawn == null && !this.HasCorpse;
			}
		}

		// Token: 0x06007FA8 RID: 32680 RVA: 0x00055C50 File Offset: 0x00053E50
		public StorageSettings GetStoreSettings()
		{
			return this.storageSettings;
		}

		// Token: 0x06007FA9 RID: 32681 RVA: 0x00054D38 File Offset: 0x00052F38
		public StorageSettings GetParentStoreSettings()
		{
			return this.def.building.fixedStorageSettings;
		}

		// Token: 0x06007FAA RID: 32682 RVA: 0x0025DCA0 File Offset: 0x0025BEA0
		public override void PostMake()
		{
			base.PostMake();
			this.storageSettings = new StorageSettings(this);
			if (this.def.building.defaultStorageSettings != null)
			{
				this.storageSettings.CopyFrom(this.def.building.defaultStorageSettings);
			}
		}

		// Token: 0x06007FAB RID: 32683 RVA: 0x00055C58 File Offset: 0x00053E58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<StorageSettings>(ref this.storageSettings, "storageSettings", new object[]
			{
				this
			});
		}

		// Token: 0x06007FAC RID: 32684 RVA: 0x00055C7A File Offset: 0x00053E7A
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06007FAD RID: 32685 RVA: 0x0025DCEC File Offset: 0x0025BEEC
		public virtual void Notify_CorpseBuried(Pawn worker)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active)
			{
				comp.JustCreatedBy(worker);
				comp.InitializeArt(this.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
			worker.records.Increment(RecordDefOf.CorpsesBuried);
			TaleRecorder.RecordTale(TaleDefOf.BuriedCorpse, new object[]
			{
				worker,
				(this.Corpse != null) ? this.Corpse.InnerPawn : null
			});
		}

		// Token: 0x06007FAE RID: 32686 RVA: 0x0025DD78 File Offset: 0x0025BF78
		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (this.HasCorpse)
			{
				return false;
			}
			if (this.AssignedPawn != null)
			{
				Corpse corpse = thing as Corpse;
				if (corpse == null)
				{
					return false;
				}
				if (corpse.InnerPawn != this.AssignedPawn)
				{
					return false;
				}
			}
			else if (!this.storageSettings.AllowedToAccept(thing))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06007FAF RID: 32687 RVA: 0x0025DDD0 File Offset: 0x0025BFD0
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				Corpse corpse = thing as Corpse;
				if (corpse != null && corpse.InnerPawn.ownership != null && corpse.InnerPawn.ownership.AssignedGrave != this)
				{
					corpse.InnerPawn.ownership.UnclaimGrave();
				}
				if (base.Spawned)
				{
					base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x00055CA1 File Offset: 0x00053EA1
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.StorageTabVisible)
			{
				foreach (Gizmo gizmo2 in StorageSettingsClipboard.CopyPasteGizmosFor(this.storageSettings))
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007FB1 RID: 32689 RVA: 0x0025DE44 File Offset: 0x0025C044
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (this.HasCorpse)
			{
				if (base.Tile != -1)
				{
					string value = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
					stringBuilder.AppendLine();
					stringBuilder.Append("DiedOn".Translate(value));
				}
			}
			else if (this.AssignedPawn != null)
			{
				stringBuilder.AppendLine();
				stringBuilder.Append("AssignedColonist".Translate());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.AssignedPawn.LabelCap);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x040052D9 RID: 21209
		private StorageSettings storageSettings;

		// Token: 0x040052DA RID: 21210
		private Graphic cachedGraphicFull;
	}
}
