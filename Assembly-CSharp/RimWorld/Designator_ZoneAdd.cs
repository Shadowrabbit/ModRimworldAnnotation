using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C9 RID: 6601
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x1700172E RID: 5934
		// (get) Token: 0x060091FE RID: 37374 RVA: 0x00061C86 File Offset: 0x0005FE86
		// (set) Token: 0x060091FF RID: 37375 RVA: 0x00061C92 File Offset: 0x0005FE92
		private Zone SelectedZone
		{
			get
			{
				return Find.Selector.SelectedZone;
			}
			set
			{
				Find.Selector.ClearSelection();
				if (value != null)
				{
					Find.Selector.Select(value, false, false);
				}
			}
		}

		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x06009200 RID: 37376
		protected abstract string NewZoneLabel { get; }

		// Token: 0x06009201 RID: 37377
		protected abstract Zone MakeNewZone();

		// Token: 0x06009202 RID: 37378 RVA: 0x00061CAE File Offset: 0x0005FEAE
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x06009203 RID: 37379 RVA: 0x0029F0E0 File Offset: 0x0029D2E0
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x06009204 RID: 37380 RVA: 0x0029F130 File Offset: 0x0029D330
		public override void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				string text = "";
				if (!Input.GetKey(KeyCode.Mouse0))
				{
					Zone selectedZone = Find.Selector.SelectedZone;
					if (selectedZone != null)
					{
						text = "ExpandOrCreateZone".Translate(selectedZone.label, this.NewZoneLabel);
					}
					else
					{
						text = "CreateNewZone".Translate(this.NewZoneLabel);
					}
				}
				GenUI.DrawMouseAttachment(this.icon, text, 0f, default(Vector2), null, false, default(Color));
			}
		}

		// Token: 0x06009205 RID: 37381 RVA: 0x0029F1DC File Offset: 0x0029D3DC
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			if (!c.InBounds(base.Map))
			{
				return false;
			}
			if (c.Fogged(base.Map))
			{
				return false;
			}
			if (c.InNoZoneEdgeArea(base.Map))
			{
				return "TooCloseToMapEdge".Translate();
			}
			Zone zone = base.Map.zoneManager.ZoneAt(c);
			if (zone != null && zone.GetType() != this.zoneTypeToPlace)
			{
				return false;
			}
			using (IEnumerator<Thing> enumerator = base.Map.thingGrid.ThingsAt(c).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.def.CanOverlapZones)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06009206 RID: 37382 RVA: 0x0029F2C0 File Offset: 0x0029D4C0
		public override void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			List<IntVec3> list = cells.ToList<IntVec3>();
			if (list.Count == 1)
			{
				Zone zone = base.Map.zoneManager.ZoneAt(list[0]);
				if (zone != null)
				{
					if (zone.GetType() == this.zoneTypeToPlace)
					{
						this.SelectedZone = zone;
					}
					return;
				}
			}
			if (this.SelectedZone == null)
			{
				Zone zone2 = null;
				foreach (IntVec3 c3 in cells)
				{
					Zone zone3 = base.Map.zoneManager.ZoneAt(c3);
					if (zone3 != null && zone3.GetType() == this.zoneTypeToPlace)
					{
						if (zone2 == null)
						{
							zone2 = zone3;
						}
						else if (zone3 != zone2)
						{
							zone2 = null;
							break;
						}
					}
				}
				this.SelectedZone = zone2;
			}
			list.RemoveAll((IntVec3 c) => base.Map.zoneManager.ZoneAt(c) != null);
			if (list.Count == 0)
			{
				return;
			}
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, list)))
			{
				return;
			}
			if (this.SelectedZone == null)
			{
				this.SelectedZone = this.MakeNewZone();
				base.Map.zoneManager.RegisterZone(this.SelectedZone);
				this.SelectedZone.AddCell(list[0]);
				list.RemoveAt(0);
			}
			bool somethingSucceeded;
			for (;;)
			{
				somethingSucceeded = true;
				int count = list.Count;
				for (int i = list.Count - 1; i >= 0; i--)
				{
					bool flag = false;
					for (int j = 0; j < 4; j++)
					{
						IntVec3 c2 = list[i] + GenAdj.CardinalDirections[j];
						if (c2.InBounds(base.Map) && base.Map.zoneManager.ZoneAt(c2) == this.SelectedZone)
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						this.SelectedZone.AddCell(list[i]);
						list.RemoveAt(i);
					}
				}
				if (list.Count == 0)
				{
					break;
				}
				if (list.Count == count)
				{
					this.SelectedZone = this.MakeNewZone();
					base.Map.zoneManager.RegisterZone(this.SelectedZone);
					this.SelectedZone.AddCell(list[0]);
					list.RemoveAt(0);
				}
			}
			this.SelectedZone.CheckContiguous();
			base.Finalize(somethingSucceeded);
			TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, list));
		}

		// Token: 0x04005C65 RID: 23653
		protected Type zoneTypeToPlace;
	}
}
