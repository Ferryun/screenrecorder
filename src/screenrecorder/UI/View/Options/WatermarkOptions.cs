/*
 * Copyright (c) 2015 Mehrzad Chehraz (mehrzady@gmail.com)
 * Released under the MIT License
 * http://chehraz.ir/mit_license
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
namespace Atf.ScreenRecorder.UI.View {
   using Atf.ScreenRecorder.Screen;
   using System;
   using System.Collections.Generic;
   using System.ComponentModel;
   using System.Drawing;
   using System.Windows.Forms;
   partial class WatermarkOptions : UserControl {
      private const ContentAlignment defaultAlignment = ContentAlignment.TopLeft;
      private const string defaultColorString = "Lime";
      private const string defaultFontString = "DefaultFont";
      private const string defaultMarginString = "3, 3, 3, 3";
      private const bool defaultOutline = true;
      private const string defaultOutlineColorString = "Green";
      private const bool defaultRightToLeft = false;
      private static readonly int defaultMissingMargin = 3;
      private static readonly string preview = "(Preview)";

      private ContentAlignment alignment = (ContentAlignment)int.MaxValue;
      private Color color;
      private Font font;
      private Margins margin;
      private bool outline;
      private Color outlineColor;
      private bool rightToLeft;
      private string text;
      private WaterMark watermark;
      private bool updateMargin = true;

      public WatermarkOptions() {
         InitializeComponent();
         // Watermark
         this.watermark = new WaterMark();
         // Alignment items
         var alignmentItems = ContentAlignmentItem.All();
         foreach (var item in alignmentItems) {
            this.cmbAlignment.Items.Add(item);
         }
         this.cmbRightToLeft.SelectedItem = alignmentItems[0];
         // Right to left items
         var rtlItems = RightToLeftItem.All();
         foreach (var item in rtlItems) {
            this.cmbRightToLeft.Items.Add(item);
         }
         this.cmbRightToLeft.SelectedItem = rtlItems[0];

         this.WatermarkAlignment = defaultAlignment;
         this.WatermarkColor = Color.FromName(defaultColorString);
         this.WatermarkFont = (Font)new FontConverter().ConvertFromString(defaultFontString);
         this.WatermarkMargin = (Margins)new MarginsConverter().ConvertFromString(defaultMarginString);
         this.WatermarkRightToLeft = defaultRightToLeft;
         this.WatermarkText = "This is a text!";
      }
      #region Properties
      [Browsable(true)]
      [DefaultValue(defaultAlignment)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public ContentAlignment WatermarkAlignment {
         get {
            return this.alignment;
         }
         set {
            if (this.alignment != value) {
               this.alignment = value;
               this.OnWatermarkAlignmentChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(typeof(Color), defaultColorString)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Color WatermarkColor {
         get {
            return this.color;
         }
         set {
            if (this.color != value) {
               this.color = value;
               this.OnWatermarkColorChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(typeof(Font), defaultFontString)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Font WatermarkFont {
         get {
            return this.font;
         }
         set {
            if (this.font != value) {
               this.font = value;
               this.OnWatermarkFontChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(typeof(Margins), defaultMarginString)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Margins WatermarkMargin {
         get {
            return this.margin;
         }
         set {
            if (!this.margin.Equals(value)) {
               this.margin = value;
               this.OnWatermarkMarginChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(defaultOutline)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public bool WatermarkOutline {
         get {
            return this.outline;
         }
         set {
            if (this.outline != value) {
               this.outline = value;
               this.OnWatermarkOutlineChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(typeof(Color), defaultOutlineColorString)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public Color WatermarkOutlineColor {
         get {
            return this.outlineColor;
         }
         set {
            if (this.outlineColor != value) {
               this.outlineColor = value;
               this.OnWatermarkOutlineColorChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(defaultRightToLeft)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public bool WatermarkRightToLeft {
         get {
            return this.rightToLeft;
         }
         set {
            if (this.rightToLeft != value) {
               this.rightToLeft = value;
               this.OnWatermarkRightToLeftChanged(EventArgs.Empty);
            }
         }
      }
      [Browsable(true)]
      [DefaultValue(null)]
      [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
      [EditorBrowsable(EditorBrowsableState.Always)]
      public string WatermarkText {
         get {
            return this.text;
         }
         set {
            if (this.text != value) {
               this.text = value;
               this.OnWatermarkTextChanged(EventArgs.Empty);
            }
         }
      }
      #endregion
      #region Methods
      private void btnColor_Click(object sender, EventArgs e) {
         using (ColorDialog colorDialog = new ColorDialog()) {
            colorDialog.Color = this.color;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
               this.WatermarkColor = colorDialog.Color;
            }
         }
      }
      private void btnFont_Click(object sender, EventArgs e) {
         using (FontDialog fontDialog = new FontDialog()) {
            fontDialog.Font = this.font;
            if (fontDialog.ShowDialog() == DialogResult.OK) {
               this.WatermarkFont = fontDialog.Font;
            }
         }
      }      
      private void chkOutline_CheckedChanged(object sender, EventArgs e) {
         this.WatermarkOutline = this.chkOutline.Checked;
      }
      private void btnOutlineColor_Click(object sender, EventArgs e) {
         using (ColorDialog colorDialog = new ColorDialog()) {
            colorDialog.Color = this.outlineColor;
            if (colorDialog.ShowDialog() == DialogResult.OK) {
               this.WatermarkOutlineColor = colorDialog.Color;
            }
         }
      }
      private void cmbAlignment_SelectedIndexChanged(object sender, EventArgs e) {
         ContentAlignmentItem item = cmbAlignment.SelectedItem as ContentAlignmentItem;
         if (item != null) {
            this.WatermarkAlignment = item.Value;
         }
      }
      private void cmbRightToLeft_SelectedIndexChanged(object sender, EventArgs e) {
         RightToLeftItem item = cmbRightToLeft.SelectedItem as RightToLeftItem;
         if (item != null) {
            this.WatermarkRightToLeft = item.Value;
         }
      }
      private void OnWatermarkAlignmentChanged(EventArgs eventArgs) {
         this.cmbAlignment.SelectedItem = new ContentAlignmentItem(this.alignment);
         picPreview.Invalidate();
         this.UpdateMargin();
      }
      private void OnWatermarkColorChanged(EventArgs eventArgs) {
         // this.btnColor.BackColor = this.color;
         this.lblColor.Text = ColorTranslator.ToHtml(this.color);
         picPreview.Invalidate();
      }
      private void OnWatermarkFontChanged(EventArgs eventArgs) {
         this.lblFont.Text = new FontConverter().ConvertToString(this.font);
         picPreview.Invalidate();
      }
      private void OnWatermarkMarginChanged(EventArgs eventArgs) {
         this.UpdateMarginControls();
         picPreview.Invalidate();
      }
      private void OnWatermarkOutlineChanged(EventArgs eventArgs) {
         this.chkOutline.Checked = this.outline;
         this.btnOutlineColor.Enabled = this.outline;
         this.picPreview.Invalidate();
      }
      private void OnWatermarkOutlineColorChanged(EventArgs eventArgs) {
         this.lblOutlineColor.Text = ColorTranslator.ToHtml(this.outlineColor);
         this.picPreview.Invalidate();
      }
      private void OnWatermarkRightToLeftChanged(EventArgs eventArgs) {
         this.cmbRightToLeft.SelectedItem = new RightToLeftItem(this.rightToLeft);
         picPreview.Invalidate();
      }
      private void OnWatermarkTextChanged(EventArgs eventArgs) {
         picPreview.Invalidate();
      }
      private void nudHorMargin_ValueChanged(object sender, EventArgs e) {
         if (updateMargin) {
            this.UpdateMargin();
         }
      }
      private void nudMargin_Enter(object sender, EventArgs e) {
         NumericUpDown nud = (NumericUpDown)sender;
         nud.Select(0, nud.Value.ToString().Length);
      }
      private void nudVerMargin_ValueChanged(object sender, EventArgs e) {
         if (updateMargin) {
            this.UpdateMargin();
         }
      }
      private void picPreview_Paint(object sender, PaintEventArgs e) {
         Graphics g = e.Graphics;
         g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
         g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
         TextRenderer.DrawText(e.Graphics, preview, this.Font, this.picPreview.ClientRectangle, SystemColors.WindowText,
                               Color.Transparent, TextFormatFlags.VerticalCenter|TextFormatFlags.HorizontalCenter);
         this.watermark.Alignment = this.alignment;
         this.watermark.Color = this.color;
         this.watermark.Font = this.font;
         this.watermark.Margin = this.margin;
         this.watermark.Outline = this.outline;
         this.watermark.OutlineColor = this.outlineColor;
         this.watermark.RightToLeft = this.rightToLeft;
         this.watermark.Text = this.text;
         this.watermark.RenderSample(e.Graphics, this.picPreview.ClientSize);
      }
      private void UpdateMargin() {
         int hor = (int)nudHorMargin.Value;
         int ver = (int)nudVerMargin.Value;
         int left = defaultMissingMargin;
         int top = defaultMissingMargin;
         int right = defaultMissingMargin;
         int bottom = defaultMissingMargin;
         switch (this.alignment) {
            case ContentAlignment.BottomCenter:
               bottom = ver;
               left = hor;
               break;
            case ContentAlignment.BottomLeft:
               bottom = ver;
               left = hor;
               break;
            case ContentAlignment.BottomRight:
               bottom = ver;
               right = hor;
               break;
            case ContentAlignment.MiddleCenter:
               top = ver;
               left = hor;
               break;
            case ContentAlignment.MiddleLeft:
               top = ver;
               left = hor;
               break;
            case ContentAlignment.MiddleRight:
               top = ver;
               right = hor;
               break;
            case ContentAlignment.TopCenter:
               top = ver;
               left = hor;
               break;
            case ContentAlignment.TopLeft:
               top = ver;
               left = hor;
               break;
            case ContentAlignment.TopRight:
               top = ver;
               right = hor;
               break;
         }
         this.WatermarkMargin = new Margins(left, top, right, bottom);
      }
      private void UpdateMarginControls() {
         int hor = 0;
         int ver = 0;
         switch (this.alignment) {
            case ContentAlignment.BottomCenter:
               ver = this.margin.Bottom;
               hor = this.margin.Left;
               break;
            case ContentAlignment.BottomLeft:
               ver = this.margin.Bottom;
               hor = this.margin.Left;               
               break;
            case ContentAlignment.BottomRight:
               ver = this.margin.Bottom;
               hor = this.margin.Right;
               break;
            case ContentAlignment.MiddleCenter:
               ver = this.margin.Top;
               hor = this.margin.Left;
               break;
            case ContentAlignment.MiddleLeft:
               ver = this.margin.Top;
               hor = this.margin.Left;
               break;
            case ContentAlignment.MiddleRight:
               ver = this.margin.Top;
               hor = this.margin.Right;
               break;
            case ContentAlignment.TopCenter:
               ver = this.margin.Top;
               hor = this.margin.Left;
               break;
            case ContentAlignment.TopLeft:
               ver = this.margin.Top;
               hor = this.margin.Left;
               break;
            case ContentAlignment.TopRight:
               ver = this.margin.Top;
               hor = this.margin.Right;
               break;
         }
         this.updateMargin = false;
         this.nudHorMargin.Value = hor;
         this.nudVerMargin.Value = ver;
         this.updateMargin = true;
         this.UpdateMargin();
      }
      #endregion
      class EnumItem<TValue> {
         private TValue value;
         public EnumItem()
            : this(default(TValue)) {
         }
         public EnumItem(TValue value) {
            if (!(value is Enum)) {
               throw new ArgumentException("T", "T must be enum.");
            }
            this.value = value;
         }
         public TValue Value {
            get {
               return this.value;
            }
         }
         public static TItem[] All<TItem>() where TItem : EnumItem<TValue> {
            Array values = Enum.GetValues(typeof(TValue));
            int count = values.Length;
            List<TItem> items = new List<TItem>(count);
            foreach (object value in values) {
               TItem item = (TItem)Activator.CreateInstance(typeof(TItem), value);
               items.Add(item);
            }
            return items.ToArray();
         }
         public override bool Equals(object obj) {
            EnumItem<TValue> item = obj as EnumItem<TValue>;
            if (item == null) {
               return false;
            }
            return item.value.Equals(this.value);
         }
         public override int GetHashCode() {
            return this.value.GetHashCode();
         }
         public override string ToString() {
            return value.ToString();
         }
      }
      class ContentAlignmentItem : EnumItem<ContentAlignment> {
         public ContentAlignmentItem(ContentAlignment value)
            : base(value) {
         }
         public static ContentAlignmentItem[] All() {
            return EnumItem<ContentAlignment>.All<ContentAlignmentItem>();
         }
         public override string ToString() {
            switch (this.Value) {
               case ContentAlignment.BottomCenter:
                  return "Bottom-Center";
               case ContentAlignment.BottomLeft:
                  return "Bottom-Left";
               case ContentAlignment.BottomRight:
                  return "Bottom-Right";
               case ContentAlignment.MiddleCenter:
                  return "Middle-Center";
               case ContentAlignment.MiddleLeft:
                  return "Middle-Left";
               case ContentAlignment.MiddleRight:
                  return "Middle-Right";
               case ContentAlignment.TopCenter:
                  return "Top-Center";
               case ContentAlignment.TopLeft:
                  return "Top-Left";
               case ContentAlignment.TopRight:
                  return "Top-Right";
               default:
                  throw new InvalidOperationException();
            }
         }
      }
      class RightToLeftItem {
         private bool value;
         public RightToLeftItem(bool value) {
            this.value = value;
         }
         public static RightToLeftItem[] All() {
             return new RightToLeftItem[] {
               new RightToLeftItem(false),
               new RightToLeftItem(true),
            };
         }
         public override bool Equals(object obj) {
            RightToLeftItem rightToLeftItem = obj as RightToLeftItem;
            if (rightToLeftItem == null) {
               return false;
            }
            return this.value.Equals(rightToLeftItem.value);
         }
         public override int GetHashCode() {
            return this.value.GetHashCode();
         }
         public override string ToString() {
            switch (this.Value) {
               case false:
                  return "Left to right";
               case true:
                  return "Right to left";
               default:
                  throw new InvalidOperationException();
            }
         }
         public bool Value {
            get {
               return this.value;
            }
         }
      }
   }
}
