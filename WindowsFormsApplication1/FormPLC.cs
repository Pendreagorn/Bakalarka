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
    public partial class FormPLC : Form
    {
        public ClassPLC plc = null;

        private int pocetVstupov = 2;
        private int pocetVystupov = 2;
        private int pocetBitov = 16;

        private int sirka = 45;

        public FormPLC()
        {
            InitializeComponent();
            this.Width = 10 + pocetBitov * sirka + 25;
            this.Height = 20 + sirka * pocetVstupov + 20 + sirka * pocetVystupov + 70;
            plc = new ClassPLC(pocetVstupov, pocetVystupov, pocetBitov);
            ClassButton senzorStart = new ClassButton();
            plc.pridajSenzor(senzorStart);
            plc.run();
        }

        private void FormPLC_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Graphics graphics = e.Graphics;

            for (int i = 0; i < pocetVstupov; i++)
            {
                int vstup = plc.Vstup(i);

                for (int j = 0; j < pocetBitov; j++)
                {
                    int stav = vstup & 1;
                    if (stav == 0)
                    {
                        graphics.FillEllipse(
                            Brushes.White, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + 20, 
                            sirka, sirka
                        );
                        graphics.DrawEllipse(
                            Pens.Black, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + 20, 
                            sirka, sirka
                            );
                        this.DrawString(
                            graphics, 
                            sirka * (pocetBitov - 1) - j * sirka + 10 + 12, 
                            i * sirka + 20 + 12, 
                            j.ToString(), 
                            Color.Black);
                    }
                    else
                    {
                        graphics.FillEllipse(
                            Brushes.Black, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + 20, 
                            sirka, sirka
                            );
                        this.DrawString(
                            graphics, 
                            sirka * (pocetBitov - 1) - j * sirka + 10 + 12, 
                            i * sirka + 20 + 12, 
                            j.ToString(), 
                            Color.White);
                    }
                    vstup = vstup >> 1;
                }
            }

            for (int i = 0; i < pocetVystupov; i++)
            {
                int vystup = plc.Vystup(i);

                for (int j = 0; j < pocetBitov; j++)
                {
                    int stav = vystup & 1;
                    if (stav == 0)
                    {
                        graphics.FillEllipse(
                            Brushes.White, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + (20 + pocetVstupov*sirka + 20), 
                            sirka, sirka
                            );
                        graphics.DrawEllipse(
                            Pens.Black, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + (20 + pocetVstupov * sirka + 20), 
                            sirka, sirka
                            );
                        this.DrawString(
                            graphics, 
                            sirka * (pocetBitov - 1) - j * sirka + 10 + 12, 
                            i * sirka + (20 + pocetVstupov * sirka + 20) + 12, 
                            j.ToString(), 
                            Color.Black
                            );
                    }
                    else
                    {
                        graphics.FillEllipse(
                            Brushes.Black, 
                            sirka * (pocetBitov - 1) - j * sirka + 10, 
                            i * sirka + (20 + pocetVstupov * sirka + 20), 
                            sirka, sirka
                            );
                        this.DrawString(graphics, 
                            sirka * (pocetBitov - 1) - j * sirka + 10 + 12, 
                            i * sirka + (20 + pocetVstupov * sirka + 20) + 12, 
                            j.ToString(), 
                            Color.White
                            );
                    }
                    vystup = vystup >> 1;
                }
            }
        }

        public void DrawString(Graphics formGraphics, int x, int y, String drawString, Color color)
        {
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 16);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
        }

        private void FormPLC_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
    }
}
