using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C9 RID: 4809
	public abstract class Designator_ZoneAdd : Designator_Zone
	{
		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x0600730B RID: 29451 RVA: 0x00266976 File Offset: 0x00264B76
		// (set) Token: 0x0600730C RID: 29452 RVA: 0x00266982 File Offset: 0x00264B82
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

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x0600730D RID: 29453
		protected abstract string NewZoneLabel { get; }

		// Token: 0x0600730E RID: 29454
		protected abstract Zone MakeNewZone();

		// Token: 0x0600730F RID: 29455 RVA: 0x0026699E File Offset: 0x00264B9E
		public Designator_ZoneAdd()
		{
			this.soundDragSustain = SoundDefOf.Designate_DragAreaAdd;
			this.soundDragChanged = null;
			this.soundSucceeded = SoundDefOf.Designate_ZoneAdd;
			this.useMouseIcon = true;
			this.hotKey = KeyBindingDefOf.Misc6;
		}

		// Token: 0x06007310 RID: 29456 RVA: 0x002669D8 File Offset: 0x00264BD8
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			if (Find.Selector.SelectedZone != null && Find.Selector.SelectedZone.GetType() != this.zoneTypeToPlace)
			{
				Find.Selector.Deselect(Find.Selector.SelectedZone);
			}
		}

		// Token: 0x06007311 RID: 29457 RVA: 0x00266A28 File Offset: 0x00264C28
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

		// Token: 0x06007312 RID: 29458 RVA: 0x00266AD4 File Offset: 0x00264CD4
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

		// Token: 0x06007313 RID: 29459 RVA: 0x00266BB8 File Offset: 0x00264DB8
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
			Zone_Stockpile zone_Stockpile;
			if ((zone_Stockpile = (this.SelectedZone as Zone_Stockpile)) != null)
			{
				zone_Stockpile.slotGroup.RemoveHaulDesignationOnStoredThings();
			}
			TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, list));
		}

		// Token: 0x04003ED4 RID: 16084
		protected Type zoneTypeToPlace;
	}
}
