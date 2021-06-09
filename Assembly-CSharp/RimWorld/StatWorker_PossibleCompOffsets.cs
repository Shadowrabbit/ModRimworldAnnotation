using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D69 RID: 7529
	public class StatWorker_PossibleCompOffsets : StatWorker
	{
		// Token: 0x0600A3B1 RID: 41905 RVA: 0x002FA900 File Offset: 0x002F8B00
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num = base.GetValueUnfinalized(req, applyPostProcess);
			if (req.HasThing)
			{
				CompStatOffsetBase compStatOffsetBase = req.Thing.TryGetComp<CompStatOffsetBase>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == this.stat)
				{
					num += compStatOffsetBase.GetStatOffset(req.Pawn);
				}
			}
			return num;
		}

		// Token: 0x0600A3B2 RID: 41906 RVA: 0x002FA954 File Offset: 0x002F8B54
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			string explanationUnfinalized = base.GetExplanationUnfinalized(req, numberSense);
			StringBuilder stringBuilder = new StringBuilder();
			ThingDef thingDef;
			if (req.Thing != null)
			{
				Thing thing = req.Thing;
				CompStatOffsetBase compStatOffsetBase = thing.TryGetComp<CompStatOffsetBase>();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == this.stat)
				{
					stringBuilder.AppendLine();
					for (int i = 0; i < compStatOffsetBase.Props.offsets.Count; i++)
					{
						FocusStrengthOffset focusStrengthOffset = compStatOffsetBase.Props.offsets[i];
						if (focusStrengthOffset.CanApply(thing, null))
						{
							list.Add(focusStrengthOffset.GetExplanation(thing));
						}
						else
						{
							list2.Add(focusStrengthOffset.GetExplanationAbstract(thing.def));
						}
					}
					if (list.Count > 0)
					{
						stringBuilder.AppendLine(list.ToLineList(null));
					}
					if (list2.Count > 0)
					{
						if (list.Count > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine("StatReport_PossibleOffsets".Translate() + ":");
						stringBuilder.AppendLine(list2.ToLineList("  - "));
					}
				}
			}
			else if ((thingDef = (req.Def as ThingDef)) != null)
			{
				CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
				if (compProperties != null && compProperties.offsets.Count > 0 && compProperties.statDef == this.stat)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatReport_PossibleOffsets".Translate() + ":");
					stringBuilder.AppendLine(compProperties.GetExplanationAbstract(thingDef).ToLineList("  - ", false));
				}
			}
			return explanationUnfinalized + stringBuilder;
		}

		// Token: 0x0600A3B3 RID: 41907 RVA: 0x002FAB10 File Offset: 0x002F8D10
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			float num = 0f;
			float num2 = 0f;
			bool flag = false;
			if (optionalReq.Thing != null && optionalReq.Thing.Spawned)
			{
				num2 = (num = optionalReq.Thing.def.GetStatValueAbstract(stat, null));
				Thing thing = optionalReq.Thing;
				CompStatOffsetBase compStatOffsetBase = thing.TryGetComp<CompStatOffsetBase>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == stat)
				{
					for (int i = 0; i < compStatOffsetBase.Props.offsets.Count; i++)
					{
						FocusStrengthOffset focusStrengthOffset = compStatOffsetBase.Props.offsets[i];
						if (!focusStrengthOffset.DependsOnPawn)
						{
							if (focusStrengthOffset.CanApply(thing, null))
							{
								float offset = focusStrengthOffset.GetOffset(thing, null);
								num += offset;
								num2 += offset;
							}
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			else if (optionalReq.Def is ThingDef)
			{
				ValueTuple<float, float> valueTuple = this.AbstractValueRange(optionalReq, numberSense);
				num2 = valueTuple.Item1;
				num = valueTuple.Item2;
			}
			string str = flag ? " (+)" : "";
			return this.RangeToString(num2, num, numberSense, finalized) + str;
		}

		// Token: 0x0600A3B4 RID: 41908 RVA: 0x002FAC34 File Offset: 0x002F8E34
		private ValueTuple<float, float> AbstractValueRange(StatRequest req, ToStringNumberSense numberSense)
		{
			ThingDef thingDef = (ThingDef)req.Def;
			float num2;
			float num = num2 = thingDef.GetStatValueAbstract(this.stat, null);
			CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
			if (compProperties != null && compProperties.statDef == this.stat)
			{
				for (int i = 0; i < compProperties.offsets.Count; i++)
				{
					FocusStrengthOffset focusStrengthOffset = compProperties.offsets[i];
					if (!focusStrengthOffset.NeedsToBeSpawned && req.Thing != null)
					{
						num += focusStrengthOffset.GetOffset(req.Thing, null);
					}
					else
					{
						float num3 = focusStrengthOffset.MinOffset(null);
						float num4 = focusStrengthOffset.MaxOffset(null);
						if (num4 > 0f)
						{
							num2 += num4;
						}
						if (num4 < 0f)
						{
							num += num4;
						}
						num += num3;
					}
				}
			}
			return new ValueTuple<float, float>(num, num2);
		}

		// Token: 0x0600A3B5 RID: 41909 RVA: 0x002FAD00 File Offset: 0x002F8F00
		private string RangeToString(float min, float max, ToStringNumberSense numberSense, bool finalized)
		{
			if (max - min >= 1E-45f)
			{
				string str = min.ToStringByStyle(this.stat.toStringStyle, numberSense);
				string str2 = this.stat.ValueToString(max, numberSense, finalized);
				return str + " - " + str2;
			}
			return this.stat.ValueToString(max, numberSense, finalized);
		}

		// Token: 0x0600A3B6 RID: 41910 RVA: 0x002FAD54 File Offset: 0x002F8F54
		public override string GetExplanationFinalizePart(StatRequest req, ToStringNumberSense numberSense, float finalVal)
		{
			if (!req.HasThing || !req.Thing.Spawned)
			{
				ValueTuple<float, float> valueTuple = this.AbstractValueRange(req, numberSense);
				float item = valueTuple.Item1;
				float item2 = valueTuple.Item2;
				return "StatsReport_FinalValue".Translate() + ": " + this.RangeToString(item, item2, numberSense, true);
			}
			return base.GetExplanationFinalizePart(req, numberSense, finalVal);
		}
	}
}
