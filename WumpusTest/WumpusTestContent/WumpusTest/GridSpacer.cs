using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace WumpusGridSpacer
{
    class GridSpacer
    {
        public int startx;
        public int endx;
        public int starty;
        public int endy;
        public int rows;
        public int columns;

        public GridSpacer(int sx, int ex, int sy, int ey, int r, int c)
        {
            startx = sx;
            endx = ex;
            starty = sy;
            endy = ey;
            rows = r;
            columns = c;
        }


        public String getLocation(int desx, int desy)
        {
            double resx = ((endx - startx) / columns) * desx;
            double resy = ((endy - starty) / rows) * desy;
            String result = "(" + resx + ", " + resy + ")";
            return result;
        }


    }
}
