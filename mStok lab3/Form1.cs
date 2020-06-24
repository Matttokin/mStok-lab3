using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mStok_lab3
{
    public partial class Form1 : Form
    {
        private int g;
        private int y;
        private int q;
        private int p;
        public Form1()
        {
            InitializeComponent();            
        }
        int modInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
        private int getHash(string str)
        {
            int var = 0;
            foreach(char ch in str)
            {
                var += (int)ch;
            }
            return var % 64;

        }
        private int getCountBite(int var)
        {
            int count = 0;
            while(var > 1)
            {
                var /= 2;
                count++;
            }
            return ++count;

        }
        private bool isSimple(int var)
        {
            bool b = true;
            for (int j = 2; j < var; j++)
            {
                if (var % j == 0 & var % 1 == 0)
                {
                    b = false;
                }
            }
            return b;
        }
        private int getP(int q)
        {
            int i = 1;
            while (true)
            {
                if (isSimple(q * i + 1))
                {
                    return q* i +1;
                }
                i++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Random rD = new Random();
            int  h = getHash(textBox1.Text.ToString());
            q = getMaxPrime(Convert.ToInt32("".PadLeft(getCountBite(h), '1'), 2));
            p = getP(q);

            int x = rD.Next(0, q);
            g = (int)BigInteger.ModPow(2, (p - 1) / q , p);
            y = (int)BigInteger.ModPow(g, x, p);

        genK:
            int k = rD.Next(0, q);
            int r = (int)BigInteger.ModPow(BigInteger.ModPow(g, k , p) ,1, q);
            if (r == 0) goto genK;

            int s = (int)BigInteger.ModPow(modInverse(k, q) * (h + x * r) ,1, q);
            if (s == 0) goto genK;            

            textBox2.Text = r.ToString();
            textBox3.Text = s.ToString();

            textBox8.Text = q.ToString();
            textBox9.Text = p.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int h = getHash(textBox4.Text.ToString());
            int r = Convert.ToInt32(textBox5.Text);
            int s = Convert.ToInt32(textBox6.Text);
            //проверка
            int w = (int)BigInteger.ModPow(modInverse(s, q),1, q);
            int u1 = (int)BigInteger.ModPow((h * w) ,1, q);
            int u2 = (int)BigInteger.ModPow((r * w) , 1,q);
            int v = (int)BigInteger.ModPow(BigInteger.ModPow((int)(BigInteger.ModPow(g, u1,p) * BigInteger.ModPow(y, u2,p)) ,1, p) ,1, q);

            
            if (r == v)
            {
                textBox7.Text = "Подпись верна";
            } else
            {
                textBox7.Text = "Подпись ошибочна";
            }

        }
        private int getMaxPrime(int end)
        {
            for (int i = end; i >= 2; i--)
            {
                bool b = true;
                for (int j = 2; j < i; j++)
                {
                    if (i % j == 0 & i % 1 == 0)
                    {
                        b = false;
                    }
                }
                if (b)
                {
                    return i;
                }
            }
            return 1;
        }
        public int mod(int x, int y)
        {
            if (x >= 0)
            {
                return x % y;
            }
            else
            {
                return y + (x % y);
            }
        }
    }
}
