using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020016F0 RID: 5872
	public abstract class QuestNode_Root_AncientComplex : QuestNode
	{
		// Token: 0x17001615 RID: 5653
		// (get) Token: 0x06008790 RID: 34704 RVA: 0x00307C79 File Offset: 0x00305E79
		protected virtual SimpleCurve ComplexSizeOverPointsCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 30f),
						true
					},
					{
						new CurvePoint(10000f, 50f),
						true
					}
				};
			}
		}

		// Token: 0x17001616 RID: 5654
		// (get) Token: 0x06008791 RID: 34705 RVA: 0x00307CAC File Offset: 0x00305EAC
		protected virtual SimpleCurve TerminalsOverRoomCountCurve
		{
			get
			{
				return new SimpleCurve
				{
					{
						new CurvePoint(0f, 1f),
						true
					},
					{
						new CurvePoint(10f, 4f),
						true
					},
					{
						new CurvePoint(20f, 6f),
						true
					},
					{
						new CurvePoint(50f, 10f),
						true
					}
				};
			}
		}

		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x06008792 RID: 34706 RVA: 0x00307D16 File Offset: 0x00305F16
		protected virtual ComplexDef ComplexDef
		{
			get
			{
				return ComplexDefOf.AncientComplex;
			}
		}

		// Token: 0x06008793 RID: 34707 RVA: 0x00307D20 File Offset: 0x00305F20
		public virtual ComplexSketch GenerateSketch(float points, bool generateTerminals = true)
		{
			int num = (int)this.ComplexSizeOverPointsCurve.Evaluate(points);
			ComplexSketch complexSketch = this.ComplexDef.Worker.GenerateSketch(new IntVec2(num, num), null);
			if (generateTerminals)
			{
				int num2 = Mathf.FloorToInt(this.TerminalsOverRoomCountCurve.Evaluate((float)complexSketch.layout.Rooms.Count));
				for (int i = 0; i < num2; i++)
				{
					complexSketch.thingsToSpawn.Add(ThingMaker.MakeThing(ThingDefOf.AncientTerminal, null));
				}
			}
			return complexSketch;
		}

		// Token: 0x040055AF RID: 21935
		protected static readonly SimpleCurve ThreatPointsOverPointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(35f, 38.5f),
				true
			},
			{
				new CurvePoint(400f, 165f),
				true
			},
			{
				new CurvePoint(10000f, 4125f),
				true
			}
		};
	}
}
