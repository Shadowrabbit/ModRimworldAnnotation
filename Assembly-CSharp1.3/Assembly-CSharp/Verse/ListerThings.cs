using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020001BC RID: 444
	public sealed class ListerThings
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x00043785 File Offset: 0x00041985
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x00043790 File Offset: 0x00041990
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.stateHashByGroup = new int[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x000437EB File Offset: 0x000419EB
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x000437FC File Offset: 0x000419FC
		public int StateHashOfGroup(ThingRequestGroup group)
		{
			if (this.use == ListerThingsUse.Region && !group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get state hash of group " + group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968738832);
				return -1;
			}
			return Gen.HashCombineInt(85693994, this.stateHashByGroup[(int)group]);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0004384D File Offset: 0x00041A4D
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0004385C File Offset: 0x00041A5C
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				List<Thing> result;
				if (!this.listsByDef.TryGetValue(req.singleDef, out result))
				{
					return ListerThings.EmptyList;
				}
				return result;
			}
			else
			{
				if (req.group == ThingRequestGroup.Undefined)
				{
					throw new InvalidOperationException("Invalid ThingRequest " + req);
				}
				if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
				{
					Log.ErrorOnce("Tried to get things in group " + req.group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132);
					return ListerThings.EmptyList;
				}
				return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
			}
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x00043902 File Offset: 0x00041B02
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00043910 File Offset: 0x00041B10
		public void Add(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (!this.listsByDef.TryGetValue(t.def, out list))
			{
				list = new List<Thing>();
				this.listsByDef.Add(t.def, list);
			}
			list.Add(t);
			foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
			{
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
					if (list2 == null)
					{
						list2 = new List<Thing>();
						this.listsByGroup[(int)thingRequestGroup] = list2;
						this.stateHashByGroup[(int)thingRequestGroup] = 0;
					}
					list2.Add(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x000439E0 File Offset: 0x00041BE0
		public void Remove(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			this.listsByDef[t.def].Remove(t);
			ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
			for (int i = 0; i < allGroups.Length; i++)
			{
				ThingRequestGroup thingRequestGroup = allGroups[i];
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					this.listsByGroup[i].Remove(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00043A70 File Offset: 0x00041C70
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x00043A9C File Offset: 0x00041C9C
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
				this.stateHashByGroup[i] = 0;
			}
		}

		// Token: 0x04000A1D RID: 2589
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04000A1E RID: 2590
		private List<Thing>[] listsByGroup;

		// Token: 0x04000A1F RID: 2591
		private int[] stateHashByGroup;

		// Token: 0x04000A20 RID: 2592
		public ListerThingsUse use;

		// Token: 0x04000A21 RID: 2593
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}
