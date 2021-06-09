using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000277 RID: 631
	public sealed class ListerThings
	{
		// Token: 0x170002ED RID: 749
		// (get) Token: 0x0600104C RID: 4172 RVA: 0x000121EF File Offset: 0x000103EF
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x0600104D RID: 4173 RVA: 0x000121F9 File Offset: 0x000103F9
		public ListerThings(ListerThingsUse use)
		{
			this.use = use;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x0600104E RID: 4174 RVA: 0x00012237 File Offset: 0x00010437
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x0600104F RID: 4175 RVA: 0x00012245 File Offset: 0x00010445
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06001050 RID: 4176 RVA: 0x000B8F0C File Offset: 0x000B710C
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
					Log.ErrorOnce("Tried to get things in group " + req.group + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132, false);
					return ListerThings.EmptyList;
				}
				return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
			}
		}

		// Token: 0x06001051 RID: 4177 RVA: 0x00012253 File Offset: 0x00010453
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06001052 RID: 4178 RVA: 0x000B8FB4 File Offset: 0x000B71B4
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
					}
					list2.Add(t);
				}
			}
		}

		// Token: 0x06001053 RID: 4179 RVA: 0x000B9068 File Offset: 0x000B7268
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
				ThingRequestGroup group = allGroups[i];
				if ((this.use != ListerThingsUse.Region || group.StoreInRegion()) && group.Includes(t.def))
				{
					this.listsByGroup[i].Remove(t);
				}
			}
		}

		// Token: 0x06001054 RID: 4180 RVA: 0x00012261 File Offset: 0x00010461
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region);
		}

		// Token: 0x06001055 RID: 4181 RVA: 0x000B90E8 File Offset: 0x000B72E8
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
			}
		}

		// Token: 0x04000CF5 RID: 3317
		private Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04000CF6 RID: 3318
		private List<Thing>[] listsByGroup;

		// Token: 0x04000CF7 RID: 3319
		public ListerThingsUse use;

		// Token: 0x04000CF8 RID: 3320
		private static readonly List<Thing> EmptyList = new List<Thing>();
	}
}
