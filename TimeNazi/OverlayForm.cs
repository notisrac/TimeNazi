using NLog;
using nUtils.Gfx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeNazi
{
    public class OverlayForm : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Bitmap _backgroundImage = null;

        public OverlayForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            //_setBackground();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        public void Initialize()
        {
            _setBackground();
        }

        public void SetBackgroundToBlack()
        {
            try
            {
                logger.Debug("Setting the background to black");
                // set the background color to black
                Rectangle bounds = Screen.FromControl(this).Bounds;
                this.BackgroundImage = new Bitmap(bounds.Width, bounds.Height);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to set the background to black: {0}", ex.Message);
            }
        }

        private void _setBackground()
        {
            try
            {
                logger.Debug("Setting the background to the b/w screenshot");
                // set the background color to black
                this.BackColor = Color.Black;
                // create a screenshot of the background
                Screen sScreen = Screen.FromControl(this);
                Rectangle bounds = sScreen.Bounds;
                _backgroundImage = new Bitmap(bounds.Width, bounds.Height);
                using (Graphics g = Graphics.FromImage(_backgroundImage))
                {
                    g.CopyFromScreen(sScreen.Bounds.Location/*Point.Empty*/, Point.Empty, bounds.Size);
                }
                //_backgroundImage.Save(@"c:\Temp\" + DateTime.Now.Ticks + "_" + sScreen.DeviceName.Replace("\\", "_").Replace("/", "_").Replace(".", "_") + ".jpg", ImageFormat.Jpeg);
                // set it as the background, but make it grayscale first
                _backgroundImage = GfxUtils.MakeGrayscale3(_backgroundImage);
                //this.BackgroundImage = _backgroundImage;
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error while trying to set the background: {0}", ex.Message);
                SetBackgroundToBlack();
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.BackgroundImage = _backgroundImage;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        
        
        
        //protected override bool ShowWithoutActivation
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        //Bitmap _backgroundImage = null;

        //public OverlayForm()
        //{
        //    this.ShowInTaskbar = false;
        //}

        //public void Initialize()
        //{
        //    // set the background color to black
        //    this.BackColor = Color.Black;
        //    // create a screenshot of the background
        //    Rectangle bounds = Screen.GetBounds(Point.Empty);
        //    _backgroundImage = new Bitmap(bounds.Width, bounds.Height);
        //    using (Graphics g = Graphics.FromImage(_backgroundImage))
        //    {
        //        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
        //    }
        //    //bitmap.Save(@"c:\temp\test.jpg", ImageFormat.Jpeg);
        //    // set it as the background
        //    this.BackgroundImage = MakeGrayscale3(_backgroundImage);
        //}

        //protected override void OnShown(EventArgs e)
        //{
        //    base.OnShown(e);
        //    // make it fullscreen
        //    //this.TopMost = true;
        //    this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
        //    this.WindowState = FormWindowState.Maximized;
        //}

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        //make sure Top Most property on form is set to false
        //        //otherwise this doesn't work
        //        int WS_EX_TOPMOST = 0x8000000;
        //        int WS_CHILD = 0x40000000;
        //        CreateParams cp = base.CreateParams;
        //        //cp.Style |= WS_CHILD;
        //        cp.ExStyle |= WS_EX_TOPMOST;
        //        return cp;
        //    }
        //}


        //// http://tech.pro/tutorial/660/csharp-tutorial-convert-a-color-image-to-grayscale
        //public static Bitmap MakeGrayscale3(Bitmap original)
        //{
        //    //create a blank bitmap the same size as original
        //    Bitmap newBitmap = new Bitmap(original.Width, original.Height);

        //    //get a graphics object from the new image
        //    Graphics g = Graphics.FromImage(newBitmap);

        //    //create the grayscale ColorMatrix
        //    ColorMatrix colorMatrix = new ColorMatrix(
        //       new float[][] 
        //      {
        //         new float[] {.3f, .3f, .3f, 0, 0},
        //         new float[] {.59f, .59f, .59f, 0, 0},
        //         new float[] {.11f, .11f, .11f, 0, 0},
        //         //new float[] {0, 0, 0, 1, 0},
        //         new float[] {0, 0, 0, 0.3f/*alpha*/, 0},
        //         new float[] {0/*brightness*/, 0/*brightness*/, 0/*brightness*/, 0, 1}
        //         //new float[] {-0.4f, -0.4f, -0.4f, 0, 1}
        //      });

        //    //create some image attributes
        //    ImageAttributes attributes = new ImageAttributes();

        //    //set the color matrix attribute
        //    attributes.SetColorMatrix(colorMatrix);

        //    //draw the original image on the new image
        //    //using the grayscale color matrix
        //    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
        //       0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

        //    //dispose the Graphics object
        //    g.Dispose();
        //    return newBitmap;
        //}
    }
}
