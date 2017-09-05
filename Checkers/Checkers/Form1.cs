using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Checkers
{
    public partial class Form1 : Form
    {
        int count = 0;
        bool moveExtra = false;
        PictureBox selectionBox = null;

        List<PictureBox> blacks = new List<PictureBox>();
        List<PictureBox> whites = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            listAdd();
        }

        private void listAdd()
        {
            blacks.Add(black1);
            blacks.Add(black2);
            blacks.Add(black3);
            blacks.Add(black4);
            blacks.Add(black5);
            blacks.Add(black6);
            blacks.Add(black7);
            blacks.Add(black8);
            blacks.Add(black9);
            blacks.Add(black10);
            blacks.Add(black11);
            blacks.Add(black12);

            whites.Add(white1);
            whites.Add(white2);
            whites.Add(white3);
            whites.Add(white4);
            whites.Add(white5);
            whites.Add(white6);
            whites.Add(white7);
            whites.Add(white8);
            whites.Add(white9);
            whites.Add(white10);
            whites.Add(white11);
            whites.Add(white12);
        }

        private void SelectedDama(object obyekt)
        {
            if (!moveExtra)
            {
                try
                {
                    selectionBox.BackColor = Color.SaddleBrown;
                }
                catch { }
                PictureBox boxMark = (PictureBox)obyekt;
                selectionBox = boxMark;
                selectionBox.BackColor = Color.Green;
            }   
        }

        private void damaClick(object sender, MouseEventArgs e)
        {
            Move((PictureBox)sender);
        }

        private void Move(PictureBox obj)
        {
            if (selectionBox != null) 
            {
                string color = selectionBox.Name.ToString().Substring(0, 5);

                if (Validacion(selectionBox, obj, color))
                {
                    Point loc = selectionBox.Location;
                    selectionBox.Location = obj.Location;
                    int cor = loc.Y - obj.Location.Y;

                    if (!moveinExtras(color) | Math.Abs(avance))
                    {
                        Queen(color);
                        count++;
                        selectionBox.BackColor = Color.SaddleBrown;
                        selectionBox = null;
                        moveExtra = false;
                    }
                    else
                    {
                        moveExtra = true;
                    }
                }
            }
        }

        private void Queen(string color)
        {
            if (color == "black" && selectionBox.Location.Y == 400)
            {
                selectionBox.BackgroundImage = Properties.Resources.black;
                selectionBox.Tag = "queen";
            }
            else if (color == "white" && selectionBox.Location.Y == 50)
            {
                selectionBox.BackgroundImage = Properties.Resources.white;
                selectionBox.Tag = "queen";
            }
        }

        private bool moveinExtras(string color)
        {
            List<PictureBox> opposite = color == "white" ? blacks : whites;
            List<Point> positions = new List<Point>();
            int SigPosition = color == "white" ? -100 : 100;

            positions.Add(new Point(selectionBox.Location.X + 100, selectionBox.Location.Y + SigPosition));
            positions.Add(new Point(selectionBox.Location.X - 100, selectionBox.Location.Y + SigPosition));
            if (selectionBox.Tag=="queen")
            {
                positions.Add(new Point(selectionBox.Location.X + 100, selectionBox.Location.Y - SigPosition));
                positions.Add(new Point(selectionBox.Location.X - 100, selectionBox.Location.Y - SigPosition));
            }
            bool result = false;
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].X >= 50 && positions[i].X <= 400 && positions[i].Y >= 50 && positions[i].Y <= 400) 
                {
                    if (!occupied(positions[i],whites)&& !occupied(positions[i], blacks))
                    {
                        Point pointMedia = new Point(Ortalama(positions[i].X, selectionBox.Location.X), Ortalama(positions[i].Y, selectionBox.Location.Y));
                        if (occupied(pointMedia,opposite))
                        {
                            result = true;
                        }

                    }
                }
            }
            return result;
        }

        private bool occupied(Point point,List<PictureBox> bunch)
        {
            for (int i = 0; i < bunch.Count; i++)
            {
                if (point==bunch[i].Location)
                {
                    return true;
                }
            }
            return false;
        }

        private int Ortalama(int n1, int n2)
        {
            int result = (n1 + n2);
            result = result / 2;
            return Math.Abs(result);
        } 

        public bool Validacion(PictureBox _origen, PictureBox _naming, string color)
        {
            Point Origen = _origen.Location;
            Point Naming = _naming.Location;
            int avance = Origen.Y - Naming.Y;
            avance = color == "white" ? avance : (avance * -1);
            avance = selectionBox.Tag == "queen" ? Math.Abs(avance) : avance;

            if (avance == 50)
            {
                return true;
            }
            else if (avance == 100) 
            {
                Point Media = new Point(Ortalama(Origen.X, Naming.X), Ortalama(Origen.Y, Naming.Y));
                List<PictureBox> opposite = color == "white" ? blacks : whites;

                for (int i = 0; i < opposite.Count; i++)
                {
                    if (opposite[i].Location==Media)
                    {
                        opposite[i].Location = new Point(0, 0);
                        opposite[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private void blackClick(object sender, MouseEventArgs e)
        {
            if (count % 2 == 1)
            {
                SelectedDama(sender);
            }
            else
            {
                MessageBox.Show("The next move of white checker");
            }
        }

        private void whiteClick(object sender, MouseEventArgs e)
        {
            if (count % 2 == 0)
            {
                SelectedDama(sender);
            }
            else
            {
                MessageBox.Show("The next move of black checker");
            }
        }
    }
}
