using System;

namespace Verse.Sound
{
	// Token: 0x02000564 RID: 1380
	public class SoundParameterMapping
	{
		// Token: 0x060028C2 RID: 10434 RVA: 0x000F73D8 File Offset: 0x000F55D8
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x000F742C File Offset: 0x000F562C
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			string title = ((this.inParam != null) ? this.inParam.Label : "null") + " -> " + ((this.outParam != null) ? this.outParam.Label : "null");
			if (widgetRow.ButtonText("Edit curve", "Edit the curve mapping the in parameter to the out parameter.", true, true, true, null))
			{
				Find.WindowStack.Add(new EditWindow_CurveEditor(this.curve, title));
			}
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000F74AC File Offset: 0x000F56AC
		public void Apply(Sample samp)
		{
			if (this.inParam == null || this.outParam == null)
			{
				return;
			}
			float x = this.inParam.ValueFor(samp);
			float value = this.curve.Evaluate(x);
			this.outParam.SetOn(samp, value);
		}

		// Token: 0x04001930 RID: 6448
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam;

		// Token: 0x04001931 RID: 6449
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam;

		// Token: 0x04001932 RID: 6450
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode;

		// Token: 0x04001933 RID: 6451
		[EditorHidden]
		public SimpleCurve curve;
	}
}
