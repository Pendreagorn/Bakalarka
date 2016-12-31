using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace WindowsFormsApplication1
{
    public class ClassPLC
    {
        private int[] Vstupy;
        private int[] Vystupy;
        private int velkost;
        private List<ClassSenzor> senzory = new List<ClassSenzor>();

        public ClassPLC(int pocetVstupov, int pocetVystupov, int velkost)
        {

            Vstupy = new int[pocetVstupov];
            Vystupy = new int[pocetVystupov];
            this.velkost = velkost;

            Vstupy[0] = 2046482637;
            Vstupy[1] = 2046482637;
            Vystupy[0] = 2046482437;
            Vystupy[1] = 2046482437;
        }

        public int Vstup(int cisloVstupu, int poradie, int pocet)
        {
            int hodnota = this.Vstupy[cisloVstupu];
            hodnota = hodnota << poradie;
            hodnota = hodnota >> sizeof(int) - pocet;
            return hodnota;
        }

        public int Vystup(int cisloVystupu, int poradie, int pocet)
        {
            int hodnota = this.Vystupy[cisloVystupu];
            hodnota = hodnota << poradie;
            hodnota = hodnota >> sizeof(int) - pocet;
            return hodnota;
        }

        public int Vstup(int cisloVstupu)
        {
            int hodnota = this.Vstupy[cisloVstupu];
            return hodnota;
        }

        public int Vystup(int cisloVystupu)
        {
            int hodnota = this.Vystupy[cisloVystupu];
            return hodnota;
        }

        public void nastavVstup(int cisloVstupu, int hodnota)
        {
            this.Vstupy[cisloVstupu] = hodnota;
        }

        public void nastavVystup(int cisloVystupu, int hodnota)
        {
            this.Vystupy[cisloVystupu] = hodnota;
        }

        public void run()
        {
            Timer myTimer = new Timer();
            myTimer.Elapsed += new ElapsedEventHandler(DisplayTimeEvent);
            myTimer.Interval = 1000; // 1000 ms is one second
            myTimer.Start();
        }

        private int pocitadlo = 1;

        public void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {    
            if (pocitadlo == 1)
            {
                this.nastavVstup(0, 1);
                this.nastavVstup(1, 1);
                this.nastavVystup(0, 1);
                this.nastavVystup(1, 1);
            }
            if (pocitadlo == 2)
            {
                this.nastavVstup(0, 2);
                this.nastavVstup(1, 2);
                this.nastavVystup(0, 2);
                this.nastavVystup(1, 2);
            }
            if (pocitadlo == 3)
            {
                this.nastavVstup(0, 4);
                this.nastavVstup(1, 4);
                this.nastavVystup(0, 4);
                this.nastavVystup(1, 4);
            }
            if (pocitadlo == 4)
            {
                this.nastavVstup(0, 8);
                this.nastavVstup(1, 8);
                this.nastavVystup(0, 8);
                this.nastavVystup(1, 8);
            }
            if (pocitadlo == 5)
            {
                this.nastavVstup(0, 16);
                this.nastavVstup(1, 16);
                this.nastavVystup(0, 16);
                this.nastavVystup(1, 16);
            }
            pocitadlo++;

            if (pocitadlo == 6)
                pocitadlo = 1;
        }

        public void pridajSenzor(ClassSenzor senzor)
        {
            this.senzory.Add(senzor);
        }
    }
}
