using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Velryba
{
    public partial class Velryba : Form
    {
        public Velryba()
        {
            InitializeComponent();
        }

        private void mzda1_100_TextChanged(object sender, EventArgs e)
        {
            int mzda;
            if (Int32.TryParse(mzda1_100.Text, out mzda))
            {
                mzda1_80.Text = (mzda * 0.8).ToString();
                mzda1_20.Text = (mzda * 0.2).ToString();
            }

            VypoctiMzdu2();
        }

        private void mzda2_100_TextChanged(object sender, EventArgs e)
        {
            int mzda;
            if (Int32.TryParse(mzda2_100.Text, out mzda))
            {
                mzda2_80.Text = (mzda * 0.8).ToString();
                mzda2_20.Text = (mzda * 0.2).ToString();
            }
        }

        private void mzda3_100_TextChanged(object sender, EventArgs e)
        {
            int mzda;
            if (Int32.TryParse(mzda3_100.Text, out mzda))
            {
                mzda3_80.Text = (mzda * 0.8).ToString();
                mzda3_20.Text = (mzda * 0.2).ToString();
            }
        }

        private void uvazek1_KeyUp(object sender, KeyEventArgs e)
        {
            double uvazek;
            if (Double.TryParse(uvazek1.Text, out uvazek))
            {
                pocetHodin1.Text = (uvazek * 40).ToString();
            }
        }

        private void uvazek2_KeyUp(object sender, KeyEventArgs e)
        {
            VypoctiMzdu2();
        }

        private void pocetHodin1_KeyUp(object sender, KeyEventArgs e)
        {
            double pocetHodin;
            if (double.TryParse(pocetHodin1.Text, out pocetHodin))
            {
                uvazek1.Text = (pocetHodin / 40).ToString();
            }
        }

        private void pocetHodin2_KeyUp(object sender, KeyEventArgs e)
        {
            double pocetHodin;
            if (double.TryParse(pocetHodin2.Text, out pocetHodin))
            {
                uvazek2.Text = (pocetHodin / 40).ToString();
            }

            VypoctiMzdu2();
        }

        private void VypoctiMzdu2()
        {
            double uvazek_2;
            if (Double.TryParse(uvazek2.Text, out uvazek_2))
            {
                pocetHodin2.Text = (uvazek_2 * 40).ToString();

                int mzda1;
                double uvazek_1;
                if (Int32.TryParse(mzda1_100.Text, out mzda1) && Double.TryParse(uvazek1.Text, out uvazek_1))
                {
                    mzda3_100.Text = ((mzda1 * uvazek_2) / uvazek_1).ToString();
                }
            }
        }

    }
}
