using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class VyrobnaLinka : Form
    {
        // i pre pohybujuce sa objekty
        int motor1i = 0;
        bool motor1run = false;
        int motor2i = 0;
        bool motor2run = false;
        int kvader2i = 0; // pohyb po ypsilone v zasobniku
        int podavac1i = 0;
        int podavac1run = 0; // 0 stoji 1 doprava 2 dolava
        bool volneMiesto = false; // pri padani kvadrov
        ClassKvader aktualnyKvadervPodavaci = null;

        List<ClassKvader> kvadre = new List<ClassKvader>(); 

        public VyrobnaLinka()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FormPLC formPLC = new FormPLC();
            formPLC.Show();
            ClassKvader kvader = new ClassKvader();
            kvadre.Add(kvader);
            aktualnyKvadervPodavaci = kvader;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            motor1run = true;
            motor2run = true;
            podavac1run = 1;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            motor1run = false;
            motor2run = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // pohyb podavaca
            if (podavac1run == 1)
            {
                podavac1i = podavac1i + 2;
                if (aktualnyKvadervPodavaci != null)
                {
                    aktualnyKvadervPodavaci.polohax += 2;
                }
            }
            if (podavac1run == 2)
            {
                podavac1i = podavac1i - 2;
            }
            if (podavac1run == 1 && podavac1i > 200)
            {
                podavac1run = 2;
            }
            if (podavac1run == 2 && podavac1i < 0)
            {
                podavac1run = 0;
                podavac1i = 0;
                volneMiesto = true;
            }

            // otACANIE pasu 1
            if (motor1run == true)
            {
                motor1i = motor1i + 2;
            }
            // otacanie pasu 2
            if (motor2run == true)
            {
                motor2i = motor2i + 2;
            }

            foreach (ClassKvader kvader in kvadre)
            {
                // otACANIE pasu 1
                if (motor1run == true && kvader.polohax >= 200 && kvader.polohax <= 650)
                {
                    kvader.polohax = kvader.polohax + 2; // posunutie kvadru na pase 1
                }
                // otacanie pasu 2
                if (motor2run == true && kvader.polohax > 650 && kvader.polohax <= 1200)
                {
                    kvader.polohax = kvader.polohax + 2; // posunute kvadru na pase 2
                }
            }

            // padanie kvadrov v zasobniku
            if (volneMiesto)
            {
                kvader2i = kvader2i + 2;
            }
            if (kvader2i > 70)
            {
                kvader2i = 0;
                volneMiesto = false;
                ClassKvader kvader = new ClassKvader();
                kvadre.Add(kvader);
                aktualnyKvadervPodavaci = kvader;
            }
            Invalidate();
        }

        float s = 1f;
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;
           
            nakresliPas(graphics, 750*s, 250 * s, 450 * s, motor1i);
            nakresliPas(graphics, 1250 * s, 250 * s, 450 * s, motor2i);

            nakresliNastroj(graphics, 950 * s, 0 * s);
            nakresliNastroj(graphics, 1450 * s, 0 * s);

            nakresliZasobnik(graphics, 525 * s, 50 * s);

            nakresliPodavac(graphics, 100 * s, 200 * s);

            foreach (ClassKvader kvader in kvadre)
            {
                nakresliKvader(graphics, kvader, 550 * s, 200 * s);
            }
        }

        public void nakresliKvader(Graphics graphics, ClassKvader kvader, float x, float y)
        {
            graphics.FillRectangle(Brushes.Black, x + kvader.polohax*s, y, 150*s, 50*s);
        }

        public void nakresliPas(Graphics graphics, float x, float y, float sirka, int a)
        {
            graphics.DrawLine(Pens.Black, x, y, sirka+x, y);

            graphics.DrawArc(Pens.Black, x-25 * s, y, 50*s, 50 * s, 90, 180);
            graphics.DrawEllipse(Pens.Black, (float)(x-22.5 * s), (float)(y+2.5 * s), 45 * s, 45 * s);
            graphics.DrawEllipse(Pens.Black, (float)(x+sirka-22.5 * s), (float)(y+2.5 * s), 45 * s, 45 * s);
            graphics.DrawArc(Pens.Black, x+sirka-25 * s, y, 50 * s, 50 * s, 270, 180);

            graphics.DrawLine(Pens.Black, x, y+50 * s, sirka+x, y+50 * s);

            a = a % 360;
            float x0 = x;
            float y0 = y+22 * s;
            float bx = x+20 * s;
            float by = y+22 * s;
            float cx = x-20 * s;
            float cy = y+22 * s;

            float b1x = x0 + (float)Math.Round((bx - x0) * Math.Cos(a * Math.PI / 180) - (by - y0) * Math.Sin(a * Math.PI / 180));
            float b1y = y0 + (float)Math.Round((bx - x0) * Math.Sin(a * Math.PI / 180) + (by - y0) * Math.Cos(a * Math.PI / 180));
            float c1x = x0 + (float)Math.Round((cx - x0) * Math.Cos(a * Math.PI / 180) - (cy - y0) * Math.Sin(a * Math.PI / 180));
            float c1y = y0 + (float)Math.Round((cx - x0) * Math.Sin(a * Math.PI / 180) + (cy - y0) * Math.Cos(a * Math.PI / 180));

            graphics.DrawLine(Pens.Black, b1x, b1y, c1x, c1y);
            graphics.DrawLine(Pens.Black, b1x+sirka, b1y, c1x+sirka, c1y);
        }

        public void nakresliNastroj(Graphics graphics, float x, float y)
        {
            graphics.FillRectangle(Brushes.Black, x, y, 50 * s, 150 * s);
        }

        public void nakresliZasobnik(Graphics graphics, float x, float y)
        {
            float sirka = 200 * s;
            float vyska = 200 * s;
            graphics.DrawLine(Pens.Black, x, y, x, y + vyska);
            graphics.DrawLine(Pens.Black, x, y+vyska, x+sirka, y+vyska);
            graphics.DrawLine(Pens.Black, x+sirka, y+vyska, x+sirka, y);

            graphics.FillRectangle(Brushes.Black, x + 25*s, y + kvader2i * s - 75 * s, 150 * s, 50 * s);
            graphics.FillRectangle(Brushes.Black, x + 25*s, y + kvader2i * s, 150 * s, 50 * s);
            graphics.FillRectangle(Brushes.Black, x + 25*s, y + kvader2i * s + 75 * s, 150 * s, 50 * s);
        }

        public void nakresliPodavac(Graphics graphics, float x, float y)
        {
            graphics.FillRectangle(Brushes.Black, x + podavac1i*s, y, 400 * s, 50 * s);
        }

        private void VyrobnaLinka_Resize(object sender, EventArgs e)
        {
            s = this.Width / 1800f;
            btnStart.Left = this.Width / 2 - 100;
            btnStop.Left = this.Width / 2 + 100;
            Invalidate();
        }
    }
}
