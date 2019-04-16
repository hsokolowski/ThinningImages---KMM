using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
            Color new_color;
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
            Color color;
            int sumaSasiadow;
           
            Bitmap t = new Bitmap(pictureBox2.Image); //zbinaryzowany
            Bitmap tmp = new Bitmap(t.Width, t.Height);
            for (int i = 1; i < bmp.Width-1; i++)
            {
                for (int j = 1; j < bmp.Height-1; j++)
                {
                    color = t.GetPixel(i,j);
                    if (color.R != 255)
                    {
                        tmp.SetPixel(i, j, kontur(i, j, t) ? Color.Red : Color.Black); //red (2) styka sie bokiem z tlem
                        
                        if (!kontur(i, j, t))
                        {
                            tmp.SetPixel(i, j, rog(i, j, t) ? Color.Green : Color.Black); //green stykaja się rogiem
                           
                        }
                        else
                        {
                            sumaSasiadow = kontur2(i, j, tmp);
                            tmp.SetPixel(i, j, sprawdzSume(sumaSasiadow) ? Color.Blue : Color.Red); //blu (2) to kontur
                        }
                       
                    }
                    else tmp.SetPixel(i, j, Color.White);
                    

                }
            }
            pictureBox3.Image = tmp;
            pictureBox3.Refresh();
        }
        int[]  tabNaRogi ={3, 6, 12, 24, 48, 96, 192, 129, 7, 14, 28, 56, 112, 224, 193, 131, 15, 30, 60, 120, 240, 225, 195, 135};
        int[] tabUsuniec = {
        3,5,7,12,13,14,15,20,
        21,22,23,28,29,30,31,48,
        52,53,54,55,56,60,61,62,
        63,65,67,69,71,77,79,80,
        81,83,84,85,86,87,88,89,
        91,92,93,94,95,97,99,101,
        103,109,111,112,113,115,116,117,
        118,119,120,121,123,124,125,126,
        127,131,133,135,141,143,149,151,
        157,159,181,183,189,191,192,193,
        195,197,199,205,207,208,209,211,
        212,213,214,215,216,217,219,220,
        221,222,223,224,225,227,229,231,
        237,239,240,241,243,244,245,246,
        247,248,249,251,252,253,254,255
        };
        private bool sprawdzSume(int s)
        {
            for(int i=0; i<24;i++)
            {
                if (s == tabNaRogi[i]) return true;
            }
            return false;
            
        }
        private int kontur2(int x, int y, Bitmap tmp)
        {
            int suma=0;
            if (tmp.GetPixel(x-1 , y).R != 255) suma+=1;
            if (tmp.GetPixel(x-1, y + 1).R != 255) suma += 2;
            if (tmp.GetPixel(x , y+1).R != 255) suma += 4;
            if (tmp.GetPixel(x+1, y + 1).R != 255) suma += 8;
            if (tmp.GetPixel(x +1, y).R != 255) suma += 16;
            if (tmp.GetPixel(x+1, y - 1).R != 255) suma += 32;
            if (tmp.GetPixel(x , y-1).R != 255) suma += 64;
            if (tmp.GetPixel(x-1, y - 1).R != 255) suma += 128;

            return suma;
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
            bmp = new Bitmap("C:/Users/smaly/source/repos/ThinningImages---KMM/ThinningImages - KMM/img/szymon1.png");
            pictureBox1.Image = bmp;
        }

        private void target_Click(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            binaryzajca();
            progressBar1.PerformStep();
            first_Click(sender, e);
            Bitmap doZapisu = new Bitmap(pictureBox3.Image);
            doZapisu.Save("C:/Image/po4etapach.png", ImageFormat.Png);

        }
        private void binaryzajca()
        {
            Color  new_color;
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

        private void cornerButton_Click(object sender, EventArgs e)
        {
            Color color;


            Bitmap t = new Bitmap(pictureBox2.Image); //zbinaryzowany
            Bitmap tmp = new Bitmap(t.Width, t.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    color = t.GetPixel(i, j);
                    if (color.R != 255)
                    {
                        tmp.SetPixel(i, j, kontur(i, j, t) ? Color.Red : Color.Black); //red (2) styka sie bokiem z tlem
                        if (!kontur(i, j, t))
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            Bitmap doZapisu = new Bitmap(pictureBox3.Image);
            doZapisu.Save("C:/Image/po4etapach.png", ImageFormat.Png);

        }
    }
}
