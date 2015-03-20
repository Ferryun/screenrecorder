namespace Atf.ScreenRecorder.Screen {
   using System;
   using System.ComponentModel;
   [Serializable]
   [TypeConverter(typeof(MarginsConverter))]
   public struct Margins {
      private int bottom;
      private int left;
      private int right;
      private int top;

      public Margins(int all) {
         this.bottom = all;
         this.left = all;
         this.right = all;
         this.top = all;
      }
      public Margins(int left, int top, int right, int bottom) {
         this.bottom = bottom;
         this.left = left;
         this.right = right;
         this.top = top;
      }
      public int Bottom {
         get {
            return this.bottom;
         }
      }
      public int Horizontal {
         get {
            return this.left + this.right;
         }
      }
      public int Left {
         get {
            return this.left;
         }
      }
      public int Right {
         get {
            return this.right;
         }
      }
      public int Top {
         get {
            return this.top;
         }
      }
      public int Vertical {
         get {
            return this.bottom + this.top;
         }
      }
      public override bool Equals(object obj) {
         if (!(obj is Margins)) {
            return false;
         }
         Margins margins = (Margins)obj;
         return margins.bottom.Equals(this.bottom) &&
                margins.left.Equals(this.left) &&
                margins.right.Equals(this.right) &&
                margins.top.Equals(this.top);
      }
      public override int GetHashCode() {
         return this.bottom.GetHashCode() ^
                this.left.GetHashCode() ^
                this.right.GetHashCode() ^
                this.top.GetHashCode();
      }
   }
}
