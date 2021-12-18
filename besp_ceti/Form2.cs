using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.IO;

namespace besp_ceti
{
    public partial class Form2 : Form
    {
        int n = 0;
        double num = 20;
        double[] pi = new double[1000];
        double[] pi_gr = new double[100];
        float[] prevkoord = new float[2];
        Pen pen = new Pen(Color.Black);
        Pen pen1 = new Pen(Color.Red);
        Pen pen2 = new Pen(Color.Green);
        Font font = new Font("Arial", 8);
        SolidBrush brush = new SolidBrush(Color.Black);
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 f)
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = this.Owner as Form1;
            Graphics g = pictureBox1.CreateGraphics();
            pen.Color = Color.Black;
            g.Clear(Color.White);
            g.DrawLine(pen, 20, pictureBox1.Height - 20, pictureBox1.Width - 20, pictureBox1.Height - 20);
            g.DrawLine(pen, 20, pictureBox1.Height - 20, 20, 20);
            g.DrawString("0", font, brush, 20, pictureBox1.Height - 20);
            g.DrawString("Pe", font, brush, 20, 20);
            g.DrawString("P(i)", font, brush, pictureBox1.Width - 20, pictureBox1.Height - 20);
            int number_gr = 1;
            if (checkBox2.Checked)
            {
                number_gr = 3;
  
            }
            for (int cicl = 0; cicl < number_gr; cicl++)
            {
                if (cicl == 0)
                {
                    pen1.Color = Color.Red;
                    n = Convert.ToInt32(textBox2.Text);
                    g.DrawString($"-> при L = {n}", font, brush, 50,  25);
                    g.DrawLine(pen1, 40, 30, 50, 30 );
                }
                if (cicl == 1)
                {
                    pen1.Color = Color.DarkBlue;
                    n = Convert.ToInt32(textBox6.Text);
                    g.DrawString($"-> при L = {n}", font, brush, 140, 25);
                    g.DrawLine(pen1, 130, 30, 140, 30);
                }
                if (cicl == 2)
                {
                    pen1.Color = Color.Green;
                    n = Convert.ToInt32(textBox7.Text);
                    g.DrawString($"-> при L = {n}", font, brush, 230,  25);
                    g.DrawLine(pen1, 220, 30, 230, 30);
                }
                try
                {
                    Progression();
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Некорректные значения");
                }

                prevkoord[0] = 20;
                prevkoord[1] = pictureBox1.Height - 20;
                for (int i = 0; i <= num; i++)
                {


                    g.DrawLine(pen2, prevkoord[0] + (pictureBox1.Width - 40) / (float)num, pictureBox1.Height - 20 - ((pictureBox1.Height - 40) * (float)pi[i]), prevkoord[0] + (pictureBox1.Width - 40) / (float)num, pictureBox1.Height - 20);
                    g.DrawLine(pen1, prevkoord[0], prevkoord[1], prevkoord[0] + (pictureBox1.Width - 40) / (float)num, pictureBox1.Height - 20 - ((pictureBox1.Height - 40) * (float)pi[i]));
                    g.DrawLine(pen, prevkoord[0], pictureBox1.Height - 25, prevkoord[0], pictureBox1.Height - 15);

                    if (i > 0)
                    {
                        prevkoord[0] += (pictureBox1.Width - 40) / (float)num;
                        prevkoord[1] = pictureBox1.Height - 20 - ((pictureBox1.Height - 40) * (float)pi[i]);
                        g.DrawString($"{pi_gr[i]}", font, brush, prevkoord[0] - 5, pictureBox1.Height - 15);
                    }
                    g.DrawString($"{pi[i]}", font, brush, prevkoord[0], prevkoord[1] - 10);


                }
               
            }
            g.Dispose();
        }

        private void Progression()
        {
            Form1 f = this.Owner as Form1;
            
            pen.Width = 0;
            num = Convert.ToDouble(textBox4.Text);
            int g = 0;
            
            g = Convert.ToInt32(textBox3.Text);
         
            double p = 0;
            double e = Convert.ToDouble(textBox5.Text);
            double pd = 0.9;
            ulong N = (ulong)(0.25 / ((1 - pd) * e * e));
            label6.Text = $"Количество повторений N = {N}";
            string g2 = Convert.ToString(g);
            int[] g_mas1 = new int[g2.Length];
            
            for (int j1 = 0; g > 0; j1++)
            {
                g_mas1[j1] = (int)g % 10;
                g = g / 10;
            }
            int d_g = f.degree(g_mas1);
            n = n + d_g;
            

            int[] m_mas = new int[n];
            int[] mx_mas = new int[n];
            int[] e_mas = new int[n];
            int[] b_mas = new int[n];
            int[] s_mas = new int[n];
            int[] a_mas = new int[n];
            

            int[] g_mas = new int[n];
            for (int k1 = 0; k1 < g_mas1.Length; k1++)
            {
                g_mas[k1] = g_mas1[k1];
            }
            if (!checkBox1.Checked)
            {
                int m = Convert.ToInt32(textBox1.Text);
                m_mas = f.dec_bin(m,n);
                mx_mas = f.sdvig(m_mas, d_g);
                a_mas = f.dig(mx_mas, g_mas, false);
            }
            ulong j = 0;
            int p_c = 0;
            double step = 1/num;
            for (p = 0; p <= 1.01; p += step)
            {
                j = 0;
                pi_gr[p_c] = p;
                for (ulong i = N; i > 0; i--)
                {

                    int  s_dec;

                    if (checkBox1.Checked) {
                        Array.Clear(mx_mas, 0, n);
                        Array.Clear(m_mas, 0, n);
                        Array.Clear(a_mas, 0, n);
                        m_mas = f.gen_m(m_mas, n - d_g);
                        mx_mas = f.sdvig(m_mas, d_g);
                        a_mas = f.dig(mx_mas, g_mas, false);
                    }
                    Array.Clear(b_mas, 0, n);
                    Array.Clear(s_mas, 0, n);
                    Array.Clear(e_mas, 0, n);

                  
                    int err = 0;

                    e_mas = f.gen_e(e_mas, n, p);
                    if (f.bin_dec(e_mas) == 0 ) err++;
                    b_mas = f.xor_f(a_mas, e_mas);
                    s_mas = f.dig(b_mas, g_mas, true);
                    s_dec = f.bin_dec(s_mas);
                    /*if (f.checkBox2.Checked)
                    {
                        f.textBox7.Text += $"\r\nm  = {f.str(m_mas)}";
                        f.textBox7.Text += $"\r\nmx = {f.str(mx_mas)}";
                        f.textBox7.Text += $"\r\na  = {f.str(a_mas)}";
                        f.textBox7.Text += $"\r\ne  = {f.str(e_mas)}";
                        f.textBox7.Text += $"\r\nb  = {f.str(b_mas)}";
                        f.textBox7.Text += $"\r\ns  = {f.str(s_mas)}";

                    }
                    
                    */
                    if (s_dec == 0)
                    {
                        if (err == 0) j++;
                        //if (f.checkBox2.Checked) f.textBox7.Text += "\r\nОшибка не обнаружена";
                    }
                    else
                    {

                        //if (f.checkBox2.Checked) f.textBox7.Text += "\r\nОшибка обнаружена";
                    }
                }
                pi[p_c] = (double)((double)j / (double)N);
                f.textBox7.Text += $"\r\np({p}) = {pi[p_c]}";
                p_c++;
            }
            
        }
    }
}
