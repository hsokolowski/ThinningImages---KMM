using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThinningImages___KMM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap bmp;

        private void loadImgBTN_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                progressBar1.Value=0;
                bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
                progressBar1.PerformStep();
            }
            
        }

        private void binary_Click(object sender, EventArgs e)
        {
            Color color, new_color;
            int r, g, b,a;
            Bitmap t = new Bitmap(pictureBox1.Image);
            Bitmap tmp = new Bitmap(t.Width, t.Height);

            int manual = Convert.ToInt32(textBox1.Text);
            if (manual > 255)
            {
                manual = 255;
            }
            else if (manual < 0)
            {
                manual = 0;
            }

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    new_color = bmp.GetPixel(i, j);
                    r = new_color.R;
                    g = new_color.G;
                    b = new_color.B;
                    a = (r + g + b) / 3;

                    tmp.SetPixel(i, j, a> manual ? Color.White : Color.Black);

                }
            }
            pictureBox2.Image = tmp;
            pictureBox2.Refresh();
        }


        private void first_Click(object sender, EventArgs e)
        {
            Color color, black,white;
            bool czyBok;
            int r, g, b, a;
            Bitmap t = new Bitmap(pictureBox2.Image); //zbinaryzowany
            Bitmap tmp = new Bitmap(t.Width, t.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    color = t.GetPixel(i,j);
                    if (color.R != 255)
                    {
                        tmp.SetPixel(i, j, kontur(i, j, t) ? Color.Red : Color.Black); //red (2) styka sie bokiem z tlem
                        if(!kontur(i, j, t))
                        {
                            tmp.SetPixel(i, j, rog(i, j, t) ? Color.Green : Color.Black); //green stykaja się rogiem
                        }
                       
                    }
                    else tmp.SetPixel(i, j, Color.White);
                    

                }
            }
            pictureBox3.Image = tmp;
            pictureBox3.Refresh();
        }
        private bool kontur(int x, int y,Bitmap tmp)
        {
            if (tmp.GetPixel(x - 1, y).R == 255) return true;
            if (tmp.GetPixel(x , y-1).R == 255) return true;
            if (tmp.GetPixel(x + 1, y).R == 255) return true;
            if (tmp.GetPixel(x , y+1).R == 255) return true;

            return false;
        }
        private bool rog(int x, int y, Bitmap tmp)
        {
            if (tmp.GetPixel(x - 1, y-1).R == 255) return true;
            if (tmp.GetPixel(x-1, y + 1).R == 255) return true;
            if (tmp.GetPixel(x + 1, y+1).R == 255) return true;
            if (tmp.GetPixel(x+1, y - 1).R == 255) return true;

            return false;
        }

        private void set_Click(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            bmp = new Bitmap("C:/Users/Hubert/source/repos/ThinningImages - KMM/ThinningImages - KMM/img/szymon1.png");
            pictureBox1.Image = bmp;
        }

        private void target_Click(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            binaryzajca();
            progressBar1.PerformStep();
            first_Click(sender, e);

        }
        private void binaryzajca()
        {
            Color color, new_color;
            int r, g, b, a;
            Bitmap t = new Bitmap(pictureBox1.Image);
            Bitmap tmp = new Bitmap(t.Width, t.Height);

            int manual = 128;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    new_color = bmp.GetPixel(i, j);
                    r = new_color.R;
                    g = new_color.G;
                    b = new_color.B;
                    a = (r + g + b) / 3;

                    tmp.SetPixel(i, j, a > manual ? Color.White : Color.Black);

                }
            }
            pictureBox2.Image = tmp;
            pictureBox2.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            progressBar1.Maximum = 3;
            progressBar1.Step = 1;
        }
    }
}
