using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200022E RID: 558
	public abstract class Zone : IExposable, ISelectable, ILoadReferenceable
	{
		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x00059CF7 File Offset: 0x00057EF7
		public Map Map
		{
			get
			{
				return this.zoneManager.map;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x00059D04 File Offset: 0x00057F04
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

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000FCA RID: 4042 RVA: 0x00059D25 File Offset: 0x00057F25
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

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000FCB RID: 4043 RVA: 0x00059D5D File Offset: 0x00057F5D
		public string BaseLabel
		{
			get
			{
				return this.baseLabel;
			}
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x00059D65 File Offset: 0x00057F65
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

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000FCD RID: 4045 RVA: 0x00059D74 File Offset: 0x00057F74
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

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x00059D96 File Offset: 0x00057F96
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

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000FCF RID: 4047 RVA: 0x00059DA8 File Offset: 0x00057FA8
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

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x0001276E File Offset: 0x0001096E
		public virtual bool IsMultiselectable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000FD1 RID: 4049
		protected abstract Color NextZoneColor { get; }

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00059E12 File Offset: 0x00058012
		public Zone()
		{
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x00059E44 File Offset: 0x00058044
		public Zone(string baseName, ZoneManager zoneManager)
		{
			this.baseLabel = baseName;
			this.label = zoneManager.NewZoneName(baseName);
			this.zoneManager = zoneManager;
			this.ID = Find.UniqueIDsManager.GetNextZoneID();
			this.color = this.NextZoneColor;
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00059EB8 File Offset: 0x000580B8
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

		// Token: 0x06000FD5 RID: 4053 RVA: 0x00059F54 File Offset: 0x00058154
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
				}));
				return;
			}
			List<Thing> list = this.Map.thingGrid.ThingsListAt(c);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (!thing.def.CanOverlapZones)
				{
					Log.Error("Added zone over zone-incompatible thing " + thing);
					return;
				}
			}
			this.cells.Add(c);
			this.zoneManager.AddZoneGridCell(this, c);
			this.Map.mapDrawer.MapMeshDirty(c, MapMeshFlag.Zone);
			AutoHomeAreaMaker.Notify_ZoneCellAdded(c, this);
			this.cellsShuffled = false;
		}

		// Token: 0x06000FD6 RID: 4054 RVA: 0x0005A024 File Offset: 0x00058224
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
				}));
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

		// Token: 0x06000FD7 RID: 4055 RVA: 0x0005A0B8 File Offset: 0x000582B8
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

		// Token: 0x06000FD8 RID: 4056 RVA: 0x0005A121 File Offset: 0x00058321
		public void Deregister()
		{
			this.zoneManager.DeregisterZone(this);
		}

		// Token: 0x06000FD9 RID: 4057 RVA: 0x0005A12F File Offset: 0x0005832F
		public virtual void PostRegister()
		{
			this.CheckAddHaulDestination();
		}

		// Token: 0x06000FDA RID: 4058 RVA: 0x0005A138 File Offset: 0x00058338
		public virtual void PostDeregister()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.RemoveHaulDestination(haulDestination);
			}
		}

		// Token: 0x06000FDB RID: 4059 RVA: 0x0005A160 File Offset: 0x00058360
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

		// Token: 0x06000FDC RID: 4060 RVA: 0x00014F75 File Offset: 0x00013175
		public virtual string GetInspectString()
		{
			return "";
		}

		// Token: 0x06000FDD RID: 4061 RVA: 0x00002688 File Offset: 0x00000888
		public virtual IEnumerable<InspectTabBase> GetInspectTabs()
		{
			return null;
		}

		// Token: 0x06000FDE RID: 4062 RVA: 0x0005A19A File Offset: 0x0005839A
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

		// Token: 0x06000FDF RID: 4063 RVA: 0x0005A1AA File Offset: 0x000583AA
		public virtual IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield break;
		}

		// Token: 0x06000FE0 RID: 4064 RVA: 0x0005A1B4 File Offset: 0x000583B4
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

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0005A328 File Offset: 0x00058528
		private void CheckAddHaulDestination()
		{
			IHaulDestination haulDestination = this as IHaulDestination;
			if (haulDestination != null)
			{
				this.Map.haulDestinationManager.AddHaulDestination(haulDestination);
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0005A350 File Offset: 0x00058550
		public override string ToString()
		{
			return this.label;
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0005A358 File Offset: 0x00058558
		public string GetUniqueLoadID()
		{
			return "Zone_" + this.ID;
		}

		// Token: 0x04000C66 RID: 3174
		public ZoneManager zoneManager;

		// Token: 0x04000C67 RID: 3175
		public int ID = -1;

		// Token: 0x04000C68 RID: 3176
		public string label;

		// Token: 0x04000C69 RID: 3177
		private string baseLabel;

		// Token: 0x04000C6A RID: 3178
		public List<IntVec3> cells = new List<IntVec3>();

		// Token: 0x04000C6B RID: 3179
		private bool cellsShuffled;

		// Token: 0x04000C6C RID: 3180
		public Color color = Color.white;

		// Token: 0x04000C6D RID: 3181
		private Material materialInt;

		// Token: 0x04000C6E RID: 3182
		public bool hidden;

		// Token: 0x04000C6F RID: 3183
		private int lastStaticFireCheckTick = -9999;

		// Token: 0x04000C70 RID: 3184
		private bool lastStaticFireCheckResult;

		// Token: 0x04000C71 RID: 3185
		private const int StaticFireCheckInterval = 1000;

		// Token: 0x04000C72 RID: 3186
		private static BoolGrid extantGrid;

		// Token: 0x04000C73 RID: 3187
		private static BoolGrid foundGrid;
	}
}
