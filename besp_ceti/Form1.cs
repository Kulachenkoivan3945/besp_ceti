using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace besp_ceti
{
    // каждый эксперимент новое рандомное м для графиков и вывод сразу несколких графиков - для l<k;l=k;l>k;
    //
    //
    //
    //
    //
    public partial class Form1 : Form
    {
        public static int n = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            textBox6.Text = "";
            textBox7.Text = "";
            try
            {
                long g = Convert.ToInt32(textBox2.Text);

                n = Convert.ToInt32(textBox3.Text);
                double pi = Convert.ToDouble(textBox4.Text);
                double eps = Convert.ToDouble(textBox5.Text);
                int j = 0;
                double pd = 0.9;
                int k = 4, m = 0;
                ulong N = (ulong)(0.25 / ((1 - pd) * eps * eps));
                if (!checkBox1.Checked) m = Convert.ToInt32(textBox1.Text);
                string g2 = Convert.ToString(g);

                int[] g_mas1 = new int[g2.Length];
                textBox6.Text += $"\r\nколичесвто экспериментов N = {N}\r\nТочность обнаружения е = {eps}\r\ng = {g}\r\nВероятность ошибки на бит Р = {pi}";
                for (int j1 = 0; g > 0; j1++)
                {
                    g_mas1[j1] = (int)g % 10;
                    g = g / 10;
                }
                int d_g = degree(g_mas1);
                n = n + d_g;
                int[] m_mas = new int[n];

                if (!checkBox1.Checked)
                {
                    textBox6.Text += $"\r\nсообщение m = {m}";
                    m_mas = dec_bin(m,n);
                    textBox6.Text += $"\r\nm = {str(m_mas)}";

                }



                int[] g_mas = new int[n];
                for (int k1 = 0; k1 < g_mas1.Length; k1++)
                {
                    g_mas[k1] = g_mas1[k1];
                }
                int[] mx_mas = new int[n];
                int[] e_mas = new int[n];
                int[] b_mas = new int[n];

                int[] s_mas = new int[n];
                int[] a_mas = new int[n];
                if (checkBox1.Checked)
                {
                    m_mas = gen_m(m_mas, n - d_g);
                    textBox6.Text += $"\r\nm = {str(m_mas)}";
                }
                mx_mas = sdvig(m_mas, d_g);
                a_mas = dig(mx_mas, g_mas, false);
                for (ulong i = N; i > 0; i--)
                {

                    int  s_dec;
                    Array.Clear(b_mas, 0, n);
                    Array.Clear(s_mas, 0, n);
                    Array.Clear(e_mas, 0, n);
                    if (checkBox1.Checked)
                    {
                        Array.Clear(m_mas, 0, n);
                        Array.Clear(mx_mas, 0, n);
                        Array.Clear(a_mas, 0, n);
                        m_mas = gen_m(m_mas, n - d_g);
                        mx_mas = sdvig(m_mas, d_g);
                        a_mas = dig(mx_mas, g_mas, false);
                        //textBox6.Text += $"\r\nm = {str(m_mas)}";
                    }

                    int err = 0;


                    e_mas = gen_e(e_mas, n, pi);
                    if (bin_dec(e_mas) == 0) err++;
                    b_mas = xor_f(a_mas, e_mas);
                    s_mas = dig(b_mas, g_mas, true);
                    s_dec = bin_dec(s_mas);
                    if (checkBox2.Checked)
                    {
                        textBox7.Text += $"\r\nm  = {str(m_mas)}";
                        textBox7.Text += $"\r\nmx = {str(mx_mas)}";
                        textBox7.Text += $"\r\na  = {str(a_mas)}";
                        textBox7.Text += $"\r\ne  = {str(e_mas)}";
                        textBox7.Text += $"\r\nb  = {str(b_mas)}";
                        textBox7.Text += $"\r\ns  = {str(s_mas)}";

                    }


                    if (s_dec == 0)
                    {
                        if(err ==0)j++;
                        if (checkBox2.Checked) textBox7.Text += "\r\nОшибка не обнаружена";
                        
                    }
                    else
                    {

                        if (checkBox2.Checked) textBox7.Text += "\r\nОшибка обнаружена";
                    }
                }
                textBox6.Text += "\r\nОшибка не обнаружена  " + j + "  раз";
                textBox6.Text += $"\r\nОшибка обнаружена { N - (ulong)j}  раз";
                textBox6.Text += $"\r\nВероятность ошибки декодирования =  { (double)((double)j / (double)N)} ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Проверьте заполнены ли входные данные,если заполнены, то корректно ли");
            }
        }

    


       public  int bin_dec(int[] m)
        {
            int m1 = 0;

            for (int i = 0; i < m.Length; i++)
            {
                m1 += m[i] << i;

            }
            return m1;
        }

        public  int[] sdvig(int[] mas, int r)
        {
            int[] mas1 = new int[mas.Length];
            for (int i = 0; i < mas.Length - r; i++)
            {
                mas1[i + r] = mas[i];
            }
            return mas1;
        }

        public  int[] xor_f(int[] mas, int[] e)
        {
            int[] mas1 = new int[mas.Length];
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] == e[i]) mas1[i] = 0;
                else mas1[i] = 1;
            }
            return mas1;
        }

        public  int[] gen_e(int[] e_mas, int t, double p)
        {

            double m1 = 0;
            int a = 0;
            Random rnd = new Random();
           
                for (int i = 0; i < t; i++)
                {
                    m1 = rnd.NextDouble();
                    if (m1 >= p) e_mas[i] = 0;
                    else
                    {
                        e_mas[i] = 1;
                        a++;
                    }
              
            }
            return e_mas;
        }
        public  int[] gen_m(int[] m_mas, int t)
        {
            double m1 = 0;
            int a = 0;
            Random rnd = new Random();
            while (a == 0)
            {
                for (int i = 0; i < t; i++)
                {
                    m1 = rnd.NextDouble();
                    if (m1 >= 0.5) m_mas[i] = 0;
                    else
                    {
                        m_mas[i] = 1;
                        a++;
                    }

                }
            }
            return m_mas;
        }
        public  int[] dec_bin(int m, int n)
        {
            int m1 = m;
            int[] m_mas = new int[n];
            for (int i = 0; m1 > 0; i++)
            {
                m_mas[i] = m1 % 2;
                m1 /= 2;

            }
            return m_mas;
        }

        public  int[] dig(int[] m, int[] g, bool s)
        {
            int  d_m, d_g, raz;
            int[] prom_rez_bin = m;
            int[] m_mas = new int[n];
            int[] g1 = g;
            d_m = degree(m);
            d_g = degree(g);
            raz = d_m - d_g;
            if (raz < 0)
            {
                m_mas = m;
                return m_mas;
            }
            while (raz >= 0)
            {
                g1 = g;
                g1 = sdvig(g, raz);
                prom_rez_bin = xor_f(prom_rez_bin, g1);
                d_m = degree(prom_rez_bin);
                raz = d_m - d_g;
            }


            if (s == false) m_mas = xor_f(m, prom_rez_bin);
            else m_mas = prom_rez_bin;
            return m_mas;
        }
        public int degree(int[] m)
        {
            int i1 = 0;
            for (int i = 0; i < m.Length; i++)
            {
                if (m[i] > 0) i1 = i;

            }
            return i1;
        }
        public string str(int[] mas)
        {
            string str = "";
            for (int i = mas.Length - 1; i >= 0; i--)
            {
                 str += $"{mas[i]}";
                
            }
            return str;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 newform = new Form2(this);
            newform.Owner = this;
            newform.Show();
        }
    }
}
