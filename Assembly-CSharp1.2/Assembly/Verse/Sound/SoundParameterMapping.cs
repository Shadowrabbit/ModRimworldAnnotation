using System;

namespace Verse.Sound
{
	// Token: 0x02000920 RID: 2336
	public class SoundParameterMapping
	{
		// Token: 0x060039C1 RID: 14785 RVA: 0x001681BC File Offset: 0x001663BC
		public SoundParameterMapping()
		{
			this.curve = new SimpleCurve();
			this.curve.Add(new CurvePoint(0f, 0f), true);
			this.curve.Add(new CurvePoint(1f, 1f), true);
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x00168210 File Offset: 0x00166410
		public void DoEditWidgets(WidgetRow widgetRow)
		{
			string title = ((this.inParam != null) ? this.inParam.Label : "null") + " -> " + ((this.outParam != null) ? this.outParam.Label : "null");
			if (widgetRow.ButtonText("Edit curve", "Edit the curve mapping the in parameter to the out parameter.", true, true))
			{
				Find.WindowStack.Add(new EditWindow_CurveEditor(this.curve, title));
			}
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x00168288 File Offset: 0x00166488
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

		// Token: 0x04002805 RID: 10245
		[Description("The independent parameter that the game will change to drive this relationship.\n\nOn the graph, this is the X axis.")]
		public SoundParamSource inParam;

		// Token: 0x04002806 RID: 10246
		[Description("The dependent parameter that will respond to changes to the in-parameter.\n\nThis must match something the game can change about this sound.\n\nOn the graph, this is the y-axis.")]
		public SoundParamTarget outParam;

		// Token: 0x04002807 RID: 10247
		[Description("Determines when sound parameters should be applies to samples.\n\nConstant means the parameters are updated every frame and can change continuously.\n\nOncePerSample means that the parameters are applied exactly once to each sample that plays.")]
		public SoundParamUpdateMode paramUpdateMode;

		// Token: 0x04002808 RID: 10248
		[EditorHidden]
		public SimpleCurve curve;
	}
}
