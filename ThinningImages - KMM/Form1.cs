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
        Bitmap poprzednia;

        private void loadImgBTN_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                progressBar1.Value = 0;
                bmp = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = bmp;
                progressBar1.PerformStep();
            }

        }

        private void binary_Click(object sender, EventArgs e)
        {
            Color new_color;
            int r, g, b, a;
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

                    tmp.SetPixel(i, j, a > manual ? Color.White : Color.Black);

                }
            }
            pictureBox2.Image = tmp;
            pictureBox2.Refresh();
        }


        private void first_Click(object sender, EventArgs e)
        {
            Color color;
            int sumaSasiadow;

            Bitmap t; //zbinaryzowany
            if (licznikDoPrzycisku == 1)t = new Bitmap(pictureBox2.Image);
            else t = new Bitmap(pictureBox4.Image);
            Bitmap tmp = new Bitmap(t.Width, t.Height);
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    color = t.GetPixel(i, j);
                    
                    if (color.R != 255)
                    {
                        tmp.SetPixel(i, j, kontur(i, j, t) ? Color.Red : Color.Black); //red (2) styka sie bokiem z tlem

                        if (!kontur(i, j, t))
                        {
                            tmp.SetPixel(i, j, rog(i, j, t) ? Color.Green : Color.Black); //green stykaja się rogiem

                        }
                        else
                        {
                            sumaSasiadow = policzSume(i, j, t);
                            tmp.SetPixel(i, j, sprawdzSume(sumaSasiadow) ? Color.Blue : Color.Red); //blu (4) to kontur
                        }

                    }
                    else tmp.SetPixel(i, j, Color.White);

                    if(i==4 && j ==5)
                    {
                        int a = 1;
                    }

                }
            }
            pictureBox3.Image = tmp;
            pictureBox3.Refresh();
        }
        int[] tabNaRogi = { 3, 6, 12, 24, 48, 96, 192, 129, 7, 14, 28, 56, 112, 224, 193, 131, 15, 30, 60, 120, 240, 225, 195, 135 };
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
            for (int i = 0; i < 24; i++)
            {
                if (s == tabNaRogi[i]) return true;
            }
            return false;

        }
        private int policzSume(int x, int y, Bitmap tmp)
        {
            int suma = 0;

            if (tmp.GetPixel(x - 1, y).R != 255 || tmp.GetPixel(x - 1, y).B != 255 || tmp.GetPixel(x - 1, y).G != 255) suma += 1;
            if (tmp.GetPixel(x - 1, y + 1).R != 255 || tmp.GetPixel(x - 1, y + 1).B != 255 || tmp.GetPixel(x - 1, y + 1).G != 255) suma += 2;
            if (tmp.GetPixel(x , y+1).R != 255 || tmp.GetPixel(x, y + 1).B != 255 || tmp.GetPixel(x, y + 1).G != 255) suma += 4;
            if (tmp.GetPixel(x+1, y + 1).R != 255 || tmp.GetPixel(x + 1, y + 1).B != 255 || tmp.GetPixel(x + 1, y + 1).G != 255) suma += 8;
            if (tmp.GetPixel(x +1, y).R != 255 || tmp.GetPixel(x + 1, y).B != 255 || tmp.GetPixel(x + 1, y).G != 255) suma += 16;
            if (tmp.GetPixel(x+1, y - 1).R != 255 || tmp.GetPixel(x + 1, y - 1).B != 255 || tmp.GetPixel(x + 1, y - 1).G != 255) suma += 32;
            if (tmp.GetPixel(x , y-1).R != 255  || tmp.GetPixel(x, y - 1).B != 255 || tmp.GetPixel(x, y - 1).G != 255) suma += 64;
            if (tmp.GetPixel(x-1, y - 1).R != 255 || tmp.GetPixel(x-1, y - 1).B != 255 || tmp.GetPixel(x-1, y - 1).G != 255) suma += 128;

            return suma;
        }
        private int policzSumeDla2(int x, int y, Bitmap tmp)
        {
            int suma = 0;
            if (tmp.GetPixel(x - 1, y).B==0 ) suma += 1;
            if (tmp.GetPixel(x - 1, y + 1).B == 0) suma += 2;
            if (tmp.GetPixel(x, y + 1).B == 0) suma += 4;
            if (tmp.GetPixel(x + 1, y + 1).B == 0) suma += 8;
            if (tmp.GetPixel(x + 1, y).B == 0) suma += 16;
            if (tmp.GetPixel(x + 1, y - 1).B == 0) suma += 32;
            if (tmp.GetPixel(x, y - 1).B == 0) suma += 64;
            if (tmp.GetPixel(x - 1, y - 1).B == 0) suma += 128;

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
            //bmp = new Bitmap("C:/Users/smaly/source/repos/ThinningImages---KMM/ThinningImages - KMM/img/szymon1.png");
            bmp = new Bitmap("C:/Users/Hubert/source/repos/ThinningImages - KMM/ThinningImages - KMM/img/szymon1.png");
            pictureBox1.Image = bmp;
        }
        int licznikDoPrzycisku = 0;
        int licznikZmian = 1;

        private void target_Click(object sender, EventArgs e)
        {
            progressBar1.PerformStep();
            binaryzajca();
            licznikDoPrzycisku++;
            progressBar1.PerformStep();
            first_Click(sender, e);
            Bitmap doZapisu = new Bitmap(pictureBox3.Image);
            doZapisu.Save("C:/Image/po4etapach.png", ImageFormat.Png);
            cornerButton_Click(sender, e);
          
            

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
            progressBar1.Maximum = 10;
            progressBar1.Step = 1;
        }

        private void cornerButton_Click(object sender, EventArgs e)
        {
            Color color;
            

            Bitmap t = new Bitmap(pictureBox3.Image); //skolorowany
            Bitmap tmp = new Bitmap(t.Width, t.Height);
            for (int i = 1; i < bmp.Width - 1; i++)
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    color = t.GetPixel(i, j);
                    if(color.B==255 && color.R!=255)
                    {
                        color = Color.White;
                        licznikZmian++;
                    }
                    
                      // usuwanmy niebieskie
                    
                    tmp.SetPixel(i, j, color);

                }
            }
            for (int i = 1; i < bmp.Width - 1; i++) // usuwamy 2
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {

                    color = tmp.GetPixel(i, j);
                    if(color.R == 255 && color.B != 255 && sprawdzCzyUsunac(i, j, tmp))
                    {
                        color = Color.White;
                        licznikZmian++;
                    }
                    //color = (color.R==255 && color.B!=255 && sprawdzCzyUsunac(i,j,tmp)) ? Color.White : color; // 2 do suniecia

                    if (color.R == 255 && color.B == 0) color = Color.FromArgb(0, 0, 0); // 2 ktroych nie zmienilismy zmieniamy na czarne
                    tmp.SetPixel(i, j, color);

                }
            }
            for (int i = 1; i < bmp.Width - 1; i++) // usuwamy 3
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {

                    
                    color = tmp.GetPixel(i, j);
                    if(color.G == 128 && color.B != 255 && sprawdzCzyUsunac(i, j, tmp))
                    {
                        color = Color.White;
                        licznikZmian++;
                    }
                   // color = (color.G == 128 && color.B != 255 && sprawdzCzyUsunac(i, j, tmp)) ? Color.White : color;

                    if (color.G == 128 && color.B == 0) color = Color.FromArgb(0, 0, 0); // zielone na czarne 
                    tmp.SetPixel(i, j, color);

                }
            }
            pictureBox4.Image = tmp;
            

            
            
            pictureBox4.Refresh();
            
            //tmp.Save("C:/Image/poUsunieciu2.png", ImageFormat.Png);
        }
        private bool sprawdzCzyUsunac(int i, int j, Bitmap tmp)
        {
            int suma = policzSumeDla2(i, j, tmp);
            for (int x=0; x<120;x++)
            {
                if (suma == tabUsuniec[x]) return true;
            }
            return false;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Bitmap doZapisu = new Bitmap(pictureBox3.Image);
            doZapisu.Save("C:/Image/po4etapach.png", ImageFormat.Png);

        }

        private void crossingNumber(object sender, EventArgs e)
        {
            Bitmap t = new Bitmap(pictureBox4.Image);
            int[,] tab = new int[t.Width, t.Height];
            int[,] minuncje = new int[t.Width, t.Height];
            Bitmap tmp = new Bitmap(t.Width, t.Height);

            Color color;
            for (int i = 1; i < bmp.Width - 1; i++) // zwykłe uzupełnienie tabliicy 0 i 1 do liczenia minuncji
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    color= t.GetPixel(i, j);
                    tmp.SetPixel(i, j, color);
                    tab[i, j] = 0;
                    if(color.R!=255)
                    {
                        tab[i, j] = 1;
                    }
                    //Console.Write(tab[i,j]);

                }
                //Console.WriteLine();
            }
           
            for (int i = 1; i < bmp.Width - 1; i++)   //liczenie minuncji i zaznaczenie
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    minuncje[i, j] = 0;
                    if(tab[i,j]==1) //jezeli to czarny pixel
                    {
                        if (minuncja(i, j, tab)==1) // zakonczenia
                        {
                            minuncje[i, j] = 1;
                            for (int k = i - 3; k < i + 4; k++)
                            {
                                for (int l = j - 3; l < j + 4; l++)
                                {
                                    if (k == i-3 || l == j-3 || k==i+3 || l==j+3)
                                    {
                                        tmp.SetPixel(k, l, Color.Red);
                                    }
                                }
                            }
                            //tmp.SetPixel(i, j, Color.Red);
                        }
                        if (minuncja(i, j, tab) == 2) //rozwidlenia
                        {
                            minuncje[i, j] = 2;
                            for (int k = i - 3; k < i + 4; k++)
                            {
                                for (int l = j - 3; l < j + 4; l++)
                                {
                                    if (k == i - 3 || l == j - 3 || k == i + 3 || l == j + 3)
                                    {
                                        tmp.SetPixel(k, l, Color.Blue);
                                    }
                                }
                            }
                            //tmp.SetPixel(i, j, Color.Red);
                        }
                    }
                   

                }
            }
            tmp.Save("C:/Temp/minuncje.png", ImageFormat.Jpeg);
            pictureBox2.Image = tmp;
            pictureBox2.Refresh();

            int[,] minuncje_tmp = minuncje;

            bool czyRowne = AreTablesTheSame(minuncje_tmp, minuncje,bmp);


            for (int i = 1; i < bmp.Width - 1; i++)   
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                    if(minuncje[i,j]==2|| minuncje[i,j]==1)
                    {
                        //Console.WriteLine("X:"+i+"Y:"+j+"Przed "+minuncje_tmp[i,j]);
                        minuncje_tmp[i,j] = filtracja_minucji(i, j, minuncje_tmp);
                        //Console.WriteLine("PO "+minuncje_tmp[i, j]);
                    }
                }
            }
            bool czyRown = AreTablesTheSame(minuncje_tmp, minuncje, bmp);
            Bitmap filtered = t;

            for (int i = 1; i < bmp.Width - 1; i++)   //liczenie minuncji i zaznaczenie
            {
                for (int j = 1; j < bmp.Height - 1; j++)
                {
                   if(minuncje_tmp[i,j]==1)
                    {
                        for (int k = i - 3; k < i + 4; k++)
                        {
                            for (int l = j - 3; l < j + 4; l++)
                            {
                                if (k == i - 3 || l == j - 3 || k == i + 3 || l == j + 3)
                                {
                                    filtered.SetPixel(k, l, Color.Red);
                                }
                            }
                        }
                    }
                    if (minuncje_tmp[i, j] == 2)
                    {
                        for (int k = i - 3; k < i + 4; k++)
                        {
                            for (int l = j - 3; l < j + 4; l++)
                            {
                                if (k == i - 3 || l == j - 3 || k == i + 3 || l == j + 3)
                                {
                                    filtered.SetPixel(k, l, Color.Blue);
                                }
                            }
                        }
                    }


                }
            }
            filtered.Save("C:/Temp/poFiltracji.png", ImageFormat.Jpeg);
            pictureBox3.Image = filtered;
            pictureBox3.Refresh();
        }
        public static bool AreTablesTheSame(int[,] tbl1, int[,] tbl2 , Bitmap bmp)
        {
            int a, b;
            for (int i = 0; i <bmp.Width; i++)
            {
                for (int c = 0; c < bmp.Height; c++)
                {
                    a = tbl1[i, c];
                    b = tbl2[i, c];
                    if (a!=b )
                        return false;
                }
            }
            return true;
        }
        private int filtracja_minucji(int i, int j,int[,] tab)
        {
            int licznik = 0;
            for (int k = i - 7; k < i + 8; k++) //domyślnie 6x6 
            {
                for (int l = j - 7; l < j + 8; l++)
                {
                    if (k != i && l != j)
                    {
                        if (tab[k,l]==1||tab[k,l]==2)
                        {
                            licznik++;
                        }
                    }
                }
            }
            if(licznik>0) return 0;
            return tab[i,j];
        }
        int[] minuncje = {1,3 };
        private int minuncja(int x,int y,int[,] t)
        {
            int suma = 0;
            suma += Math.Abs(t[x, y + 1]-   t[x-1, y + 1]);
            suma += Math.Abs(t[x-1, y + 1]- t[x-1, y]);
            suma += Math.Abs(t[x-1, y]-     t[x-1, y - 1]);
            suma += Math.Abs(t[x-1, y - 1]- t[x, y - 1]);
            suma += Math.Abs(t[x, y - 1]-   t[x+1, y - 1]);
            suma += Math.Abs(t[x+1, y - 1]- t[x+1, y ]);
            suma += Math.Abs(t[x+1, y ]-    t[x+1, y + 1]);
            suma += Math.Abs(t[x+1, y + 1]- t[x, y + 1]);

            suma /= 2;
            //for(int i=0;i<minuncje.Length;i++)
            //{
            //    if (suma == minuncje[i]) return true;
            //}
            if (suma == minuncje[0]) return 1;
            if (suma == minuncje[1]) return 2;
            return 0;
            
        }


        private void fullButtnon_Click(object sender, EventArgs e)
        {
           
            binaryzajca();
            while (licznikZmian!=0)
            {
                licznikZmian = 0;
                licznikDoPrzycisku++;
                first_Click(sender, e);
                Bitmap doZapisu = new Bitmap(pictureBox3.Image);
                //doZapisu.Save("C:/Image/po4etapach.png", ImageFormat.Png);
                cornerButton_Click(sender, e);
                progressBar1.PerformStep();
            }
            progressBar1.PerformStep();
            progressBar1.Value=10;
            

        }
    }
}
