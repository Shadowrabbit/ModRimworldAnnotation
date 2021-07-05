using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107B RID: 4219
	public class Building_Grave : Building_CorpseCasket
	{
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x0600645B RID: 25691 RVA: 0x0021D7AF File Offset: 0x0021B9AF
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

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x0600645C RID: 25692 RVA: 0x0021D7DE File Offset: 0x0021B9DE
		public CompAssignableToPawn_Grave CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Grave>();
			}
		}

		// Token: 0x1700112E RID: 4398
		// (get) Token: 0x0600645D RID: 25693 RVA: 0x0021D7E8 File Offset: 0x0021B9E8
		public override Graphic Graphic
		{
			get
			{
				if (!base.HasCorpse)
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

		// Token: 0x1700112F RID: 4399
		// (get) Token: 0x0600645E RID: 25694 RVA: 0x0021D847 File Offset: 0x0021BA47
		public override bool StorageTabVisible
		{
			get
			{
				return base.StorageTabVisible && this.AssignedPawn == null;
			}
		}

		// Token: 0x0600645F RID: 25695 RVA: 0x0021D85C File Offset: 0x0021BA5C
		public override void EjectContents()
		{
			base.EjectContents();
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06006460 RID: 25696 RVA: 0x0021D884 File Offset: 0x0021BA84
		public virtual void Notify_CorpseBuried(Pawn worker)
		{
			CompArt comp = base.GetComp<CompArt>();
			if (comp != null && !comp.Active && worker.RaceProps.Humanlike)
			{
				comp.JustCreatedBy(worker);
				comp.InitializeArt(base.Corpse.InnerPawn);
			}
			base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things | MapMeshFlag.Buildings);
			worker.records.Increment(RecordDefOf.CorpsesBuried);
			TaleRecorder.RecordTale(TaleDefOf.BuriedCorpse, new object[]
			{
				worker,
				(base.Corpse != null) ? base.Corpse.InnerPawn : null
			});
		}

		// Token: 0x06006461 RID: 25697 RVA: 0x0021D920 File Offset: 0x0021BB20
		public override bool Accepts(Thing thing)
		{
			if (!base.Accepts(thing))
			{
				return false;
			}
			if (base.HasCorpse)
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

		// Token: 0x06006462 RID: 25698 RVA: 0x0021D978 File Offset: 0x0021BB78
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

		// Token: 0x06006463 RID: 25699 RVA: 0x0021D9EC File Offset: 0x0021BBEC
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

		// Token: 0x06006464 RID: 25700 RVA: 0x0021D9FC File Offset: 0x0021BBFC
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (base.HasCorpse)
			{
				if (base.Tile != -1)
				{
					string value = GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(base.Corpse.timeOfDeath), Find.WorldGrid.LongLatOf(base.Tile));
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

		// Token: 0x0400389F RID: 14495
		private Graphic cachedGraphicFull;
	}
}
