using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.SketchGen
{
	// Token: 0x02001E13 RID: 7699
	public class SketchResolver_AssignRandomStuff : SketchResolver
	{
		// Token: 0x0600A6A5 RID: 42661 RVA: 0x00305CEC File Offset: 0x00303EEC
		protected override void ResolveInt(ResolveParams parms)
		{
			ThingDef assignRandomStuffTo = parms.assignRandomStuffTo;
			bool flag = parms.connectedGroupsSameStuff ?? false;
			bool allowWood = parms.allowWood ?? true;
			bool allowFlammableWalls = parms.allowFlammableWalls ?? true;
			this.thingsAt.Clear();
			foreach (SketchThing sketchThing in parms.sketch.Things)
			{
				if (assignRandomStuffTo == null || sketchThing.def == assignRandomStuffTo)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						List<SketchThing> list;
						if (!this.thingsAt.TryGetValue(key, out list))
						{
							list = new List<SketchThing>();
							this.thingsAt.Add(key, list);
						}
						list.Add(sketchThing);
					}
				}
			}
			this.visited.Clear();
			using (List<SketchThing>.Enumerator enumerator = parms.sketch.Things.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SketchThing thing = enumerator.Current;
					if ((assignRandomStuffTo == null || thing.def == assignRandomStuffTo) && !this.visited.Contains(thing))
					{
						ThingDef stuff = GenStuff.RandomStuffInexpensiveFor(thing.def, null, (ThingDef x) => SketchGenUtility.IsStuffAllowed(x, allowWood, parms.useOnlyStonesAvailableOnMap, allowFlammableWalls, thing.def));
						thing.stuff = stuff;
						this.visited.Add(thing);
						if (flag)
						{
							this.stack.Clear();
							this.stack.Push(thing);
							while (this.stack.Count != 0)
							{
								SketchThing sketchThing2 = this.stack.Pop();
								sketchThing2.stuff = stuff;
								foreach (IntVec3 key2 in sketchThing2.OccupiedRect.ExpandedBy(1))
								{
									List<SketchThing> list2;
									if (this.thingsAt.TryGetValue(key2, out list2))
									{
										for (int i = 0; i < list2.Count; i++)
										{
											if (list2[i].def == thing.def && !this.visited.Contains(list2[i]))
											{
												this.visited.Add(list2[i]);
												this.stack.Push(list2[i]);
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600A6A6 RID: 42662 RVA: 0x0000A2A7 File Offset: 0x000084A7
		protected override bool CanResolveInt(ResolveParams parms)
		{
			return true;
		}

		// Token: 0x04007104 RID: 28932
		private Dictionary<IntVec3, List<SketchThing>> thingsAt = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x04007105 RID: 28933
		private HashSet<SketchThing> visited = new HashSet<SketchThing>();

		// Token: 0x04007106 RID: 28934
		private Stack<SketchThing> stack = new Stack<SketchThing>();
	}
}
