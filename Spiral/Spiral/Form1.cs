using System;
using System.Drawing;
using System.Windows.Forms;

namespace Spiral {
    public partial class Form1 : Form {

        static bool isVis = false;
        static bool spIsVis = false;

        PictureBox spiral = new PictureBox(), grids = new PictureBox();
        Graphics g, g2;
        Bitmap btm, btm1;

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            this.Controls.Add(spiral);
            this.Controls.Add(grids);
            spiral.Controls.Add(grids);
            spiral.Size = this.ClientSize;
            grids.Size = this.ClientSize;

            btm = new Bitmap(1000, 1000);
            g = Graphics.FromImage(btm);
            spiral.BackgroundImage = btm;
            spiral.BackColor = Color.Transparent;
            spiral.BackgroundImageLayout = ImageLayout.Zoom;

            btm1 = new Bitmap(1000, 1000);
            g2 = Graphics.FromImage(btm1);
            grids.BackgroundImage = btm1;
            grids.BackColor = Color.Transparent;
            grids.BackgroundImageLayout = ImageLayout.Zoom;

            drawButton_Click(sender, e);
        }

        private void lines()
        {
            g2.Clear(Color.Transparent);
            Pen pen2 = new Pen(Color.Black, 3);

            g2.DrawLine(pen2, 499, 0, 499, 1000);
            g2.DrawLine(pen2, 0, 499, 1000, 499);
            for (int i = 19; i > 0; i--)
            {
                g2.DrawLine(pen2, 505, (i * 50), 493, (i * 50));
                g2.DrawLine(pen2, (i * 50), 505, (i * 50), 493);
            }
            pen2.Dispose();
        }

        private void button2_Click(object sender, EventArgs e) {
            if (!isVis) grids.Show();
            else grids.Hide();
            isVis = !isVis;
        }

         void get255(ref double a, ref double b, ref double c, double n) {
            c += n;
            b += c / 255;
            a += b / 255;
            c = c % 256;
            b = b % 256;
            a = a % 256;
                
        }

        private void spiralDraw() { 
            g.Clear(Color.Transparent);
            double c1 = 0, c2 = 0, c3 = 0;

            Pen pen = new Pen(Color.FromArgb(255, Int32.Parse(textBox4.Text), Int32.Parse(textBox5.Text), Int32.Parse(textBox6.Text)), (float)numericUpDown1.Value);
            double angle = double.Parse(textBox3.Text) * Math.PI/2;
            double a = angle;
            double r = 1;
            int n = Int32.Parse(textBox1.Text);
            double rad = Int32.Parse(textBox2.Text) * 50;
            double f = 1.0 * rad / n;
            double r2 = r;
            double a2 = a;
            int mult255 = 255 * 255 * 255;
            while (a <= n * Math.PI * 2 + angle)
            {

                if (RB.Checked)
                {
                    pen.Color = Color.FromArgb(255, (int)c1, (int)c2, (int)c3);
                    get255(ref c1, ref c2, ref c3, (1.0 / r * mult255) / n * Math.PI * 2);
                }
                g.DrawEllipse(pen, (float)(499 + (r * Math.Cos(a + angle))), (float)(499 + -r * (Math.Sin(a + angle))), 1, 1);
                a += 1.0 / r;
                r += 1.0 / ((2 * Math.PI * (r)) / (f));
            }
            spiral.BackgroundImage = btm;
            pen.Dispose();
        }

        private void Form1_ResizeEnd_1(object sender, EventArgs e) {
            Control control = (Control)sender;
            spiral.Size = this.ClientSize;
            grids.Size = this.ClientSize;
            grids.Refresh();
            spiral.Refresh();
        }

        private void textBox1_TextChanged(object sender, EventArgs e) {
            int g;
            if (!Int32.TryParse(textBox1.Text, out g)) {
                textBox1.Text = "1";
            };
        }

        private void textBox2_TextChanged(object sender, EventArgs e) {
            double g;
            if (!Double.TryParse(textBox2.Text, out g)) {
                textBox2.Text = "1";
            };
        }

        private void textBox3_TextChanged(object sender, EventArgs e) {
            double g;
            if (!Double.TryParse(textBox3.Text, out g)) {
                textBox3.Text = "0";
            };
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            int g;
            if (Int32.Parse(textBox6.Text) > 255 && Int32.Parse(textBox6.Text) < 0 && !Int32.TryParse(textBox6.Text, out g))
            {
                textBox3.Text = "0";
            };
        }


        private void drawButton_Click(object sender, EventArgs e)
        {
            lines();
            spiralDraw();
            grids.Refresh();
            spiral.Refresh();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) grids.Show();
            else grids.Hide();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value < 0) numericUpDown1.Value = 1;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            int g;
            if (Int32.Parse(textBox6.Text) > 255 && Int32.Parse(textBox6.Text) < 0 && !Int32.TryParse(textBox5.Text, out g))
            {
                textBox3.Text = "0";
            };
        }

        
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            int g;
            if (Int32.Parse(textBox6.Text) > 255 && Int32.Parse(textBox6.Text) < 0 && !Int32.TryParse(textBox4.Text, out g))
            {
                textBox3.Text = "0";
            };
        }
    }
}
