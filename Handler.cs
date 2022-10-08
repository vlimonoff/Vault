using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vault
{
    internal class Handler
    {
        public Boolean state = false;   // Состояние (0 - горизонтально, 1 - вертикально)
        public PictureBox picture = new PictureBox();
        public Point position;

        public Handler()
        {
            picture.Image = Resource1.handle;
            picture.Show();
            picture.SizeMode = PictureBoxSizeMode.Zoom;
            picture.TabIndex = 0;
            picture.TabStop = false;
            picture.Cursor = Cursors.Hand;
            picture.BackColor = Color.Transparent;
            state = false;
        }

        public void Reverse()
        {
            state = !state;
            Image flipImage = picture.Image;
            flipImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            picture.Image = flipImage;
            picture.Refresh();
        }
    }
}
