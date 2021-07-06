using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000320 RID: 800
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x000149DA File Offset: 0x00012BDA
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001450 RID: 5200 RVA: 0x000149E7 File Offset: 0x00012BE7
		public IntVec3 Position
		{
			get
			{
				if (this.cells.Count == 0)
				{
					return IntVec3.Invalid;
				}
				return this.cells[0];
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x00014A08 File Offset: 0x00012C08
		public Material Material
		{
			get
			{
				if (this.materialInt == null)
				{
					this.materialInt = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					this.materialInt.renderQueue = 3600;
				}
				return this.materialInt;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06001452 RID: 5202 RVA: 0x00014A40 File Offset: 0x00012C40
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x06001453 RID: 5203 RVA: 0x00014A48 File Offset: 0x00012C48
		public IEnumerator<IntVec3> GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.cells.Count; i = num + 1)
			{
				yield return this.cells[i];
				num = i;
			}
			yield break;
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x00014A57 File Offset: 0x00012C57
		public List<IntVec3> Cells
		{
			get
			{
				if (!this.cellsShuffled)
				{
					this.cells.Shuffle<IntVec3>();
					this.cellsShuffled = true;
				}
				return this.cells;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x00014A79 File Offset: 0x00012C79
		public IEnumerable<Thing> AllContainedThings
		{
			get
			{
				ThingGrid grids = this.Map.thingGrid;
				int num;
				for (int i = 0; i < this.cells.Count; i = num + 1)
				{
					List<Thing> thingList = grids.ThingsListAt(this.cells[i]);
					for (int j = 0; j < thingList.Count; j = num + 1)
					{
						yield return thingList[j];
						num = j;
					}
					thingList = null;
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x000CE6DC File Offset: 0x000CC8DC
		public bool ContainsStaticFire
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastStaticFireCheckTick + 1000)
				{
					this.lastStaticFireCheckResult = false;
					for (int i = 0; i < this.cells.Count; i++)
					{
						if (this.cells[i].ContainsStaticFire(this.Map))
						{
							this.lastStaticFireCheckResult = true;
							break;
						}
					}
				}
				return this.lastStaticFireCheckResult;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0000A2E4 File Offset: 0x000084E4
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x06001458 RID: 5208
		protected abstract Color NextZoneColor { get; }

		// Token: 0x06001459 RID: 5209 RVA: 0x00014A89 File Offset: 0x00012C89
		public Zone()
		{
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x000CE748 File Offset: 0x000CC948
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x000CE7BC File Offset: 0x000CC9BC
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", -1, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Values.Look<string>(ref this.baseLabel, "baseLabel", null, false);
			Scribe_Values.Look<Color>(ref this.color, "color", default(Color), false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.cells, "cells", LookMode.Undefined, Array.Empty<object>());
			BackCompatibility.PostExposeData(this);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CheckAddHaulDestination();
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x000CE858 File Offset: 0x000CCA58
		public virtual void AddCell(IntVec3 c)
		{
			if (this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Adding cell to zone which already has it. c=",
					c,
					", zone=",
					this
				}), false);
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					Log.Error("Added zone over zone-incompatible thing " + thing, false);
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x000CE92C File Offset: 0x000CCB2C
		public virtual void RemoveCell(IntVec3 c)
		{
			if (!this.cells.Contains(c))
			{
				Log.Error(string.Concat(new object[]
				{
					"Cannot remove cell from zone which doesn't have it. c=",
					c,
					", zone=",
					this
				}), false);
				return;
			}
			this.cells.Remove(c);
			this.zoneManager.ClearZoneGridCell(c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			this.cellsShuffled = false;
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000CE9C0 File Offset: 0x000CCBC0
		public virtual void Delete()
		{
			SoundDefOf.Designate_ZoneDelete.PlayOneShotOnCamera(this.Map);
			if (this.cells.Count == 0)
			{
				this.Deregister();
			}
			else
			{
				while (this.cells.Count > 0)
				{
					this.RemoveCell(this.cells[this.cells.Count - 1]);
				}
			}
			Find.Selector.Deselect(this);
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x00014AB9 File Offset: 0x00012CB9
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00014AC7 File Offset: 0x00012CC7
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x000CEA2C File Offset: 0x000CCC2C
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x000CEA54 File Offset: 0x000CCC54
		public bool ContainsCell(IntVec3 c)
		{
			for (int i = 0; i < this.cells.Count; i++)
			{
				if (this.cells[i] == c)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x0000A713 File Offset: 0x00008913
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x06001464 RID: 5220 RVA: 0x0000C32E File Offset: 0x0000A52E
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x00014ACF File Offset: 0x00012CCF
		public virtual IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Action
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/RenameZone", true),
				defaultLabel = "CommandRenameZoneLabel".Translate(),
				defaultDesc = "CommandRenameZoneDesc".Translate(),
				action = delegate()
				{
					Dialog_RenameZone dialog_RenameZone = new Dialog_RenameZone(this);
					if (KeyBindingDefOf.Misc1.IsDown)
					{
						dialog_RenameZone.WasOpenedByHotkey();
					}
					Find.WindowStack.Add(dialog_RenameZone);
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield return new Command_Toggle
			{
				icon = ContentFinder<Texture2D>.Get("UI/Commands/HideZone", true),
				defaultLabel = (this.hidden ? "CommandUnhideZoneLabel".Translate() : "CommandHideZoneLabel".Translate()),
				defaultDesc = "CommandHideZoneDesc".Translate(),
				isActive = (() => this.hidden),
				toggleAction = delegate()
				{
					this.hidden = !this.hidden;
					foreach (IntVec3 loc in this.Cells)
					{
						this.Map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.Zone);
					}
				},
				hotKey = KeyBindingDefOf.Misc2
			};
			foreach (Gizmo gizmo in this.GetZoneAddGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Designator designator = DesignatorUtility.FindAllowedDesignator<Designator_ZoneDelete_Shrink>();
			if (designator != null)
			{
				yield return designator;
			}
			yield return new Command_Action
			{
				icon = TexButton.DeleteX,
				defaultLabel = "CommandDeleteZoneLabel".Translate(),
				defaultDesc = "CommandDeleteZoneDesc".Translate(),
				action = new Action(this.Delete),
				hotKey = KeyBindingDefOf.Designator_Deconstruct
			};
			yield break;
			yield break;
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00014ADF File Offset: 0x00012CDF
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x000CEA90 File Offset: 0x000CCC90
		public void CheckContiguous()
		{
			if (this.cells.Count == 0)
			{
				return;
			}
			if (Zone.extantGrid == null)
			{
				Zone.extantGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.extantGrid.ClearAndResizeTo(this.Map);
			}
			if (Zone.foundGrid == null)
			{
				Zone.foundGrid = new BoolGrid(this.Map);
			}
			else
			{
				Zone.foundGrid.ClearAndResizeTo(this.Map);
			}
			for (int i = 0; i < this.cells.Count; i++)
			{
				Zone.extantGrid.Set(this.cells[i], true);
			}
			Predicate<IntVec3> passCheck = (IntVec3 c) => Zone.extantGrid[c] && !Zone.foundGrid[c];
			int numFound = 0;
			Action<IntVec3> processor = delegate(IntVec3 c)
			{
				Zone.foundGrid.Set(c, true);
				int numFound = numFound;
				numFound++;
			};
			this.Map.floodFiller.FloodFill(this.cells[0], passCheck, processor, int.MaxValue, false, null);
			if (numFound < this.cells.Count)
			{
				foreach (IntVec3 c2 in this.Map.AllCells)
				{
					if (Zone.extantGrid[c2] && !Zone.foundGrid[c2])
					{
						this.RemoveCell(c2);
					}
				}
			}
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000CEC04 File Offset: 0x000CCE04
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x00014AE8 File Offset: 0x00012CE8
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00014AF0 File Offset: 0x00012CF0
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID;
		}

		// Token: 0x04000FF3 RID: 4083
		public ZoneManager zoneManager;

		// Token: 0x04000FF4 RID: 4084
		public int ID = -1;

		// Token: 0x04000FF5 RID: 4085
		public string label;

		// Token: 0x04000FF6 RID: 4086
		private string baseLabel;

		// Token: 0x04000FF7 RID: 4087
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04000FF8 RID: 4088
		private bool cellsShuffled;

		// Token: 0x04000FF9 RID: 4089
		public Color color = Color.white;

		// Token: 0x04000FFA RID: 4090
		private Material materialInt;

		// Token: 0x04000FFB RID: 4091
		public bool hidden;

		// Token: 0x04000FFC RID: 4092
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x04000FFD RID: 4093
		private bool lastStaticFireCheckResult;

		// Token: 0x04000FFE RID: 4094
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x04000FFF RID: 4095
		private static BoolGrid extantGrid;

		// Token: 0x04001000 RID: 4096
		private static BoolGrid foundGrid;
	}
}
