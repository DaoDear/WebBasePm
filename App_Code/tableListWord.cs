using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebBasePM
{
    public class tableListWord : tableList
    {

        private string[] colWidth = null;      // should it be int? if yes, not forget to change below modules
        private int headBackground = 0xDADADA;      // should it be int?
        private int[] colBackground = null;
        private int border = 4;     //default

        public enum widthType { autofit, percent, points };
        public widthType colWidthType = widthType.percent;

        public tableListWord()
        {
            rows = new ArrayList();
        }

        public tableListWord(string n)
        {
            rows = new ArrayList();
            name = n;
        }

        public tableListWord(tableList t)
        {
            this.name = t.getName();
            this.rows = t.getRows();
            this.header = t.getHeader();
            this.merge = t.getMerge();
            //this.circleIndex = t.circleIndex;
        }

        public tableListWord(tableList t, int[] indexField)
        {
            this.name = t.getName();
            this.header = t.getHeader();

            header = new string[indexField.Length];     // exception null or 0 element.
            string[] htmp = t.getHeader();

            // clone header
            for (int i = 0; i < indexField.Length; i++)
            {
                if (indexField[i] < htmp.Length)
                {
                    header[i] = htmp[indexField[i]];
                }
                else
                {
                    header[i] = "";
                }
            }

            // clone rows
            for (int i = 0; i < t.getRowNumber(); i++)
            {
                object[] tmps1 = t.getRow(i);
                object[] tmps2 = new object[indexField.Length]; // exception null or 0 element.
                for (int j = 0; j < indexField.Length; j++)
                {
                    if (indexField[j] < htmp.Length)
                    {
                        tmps2[j] = tmps1[indexField[j]];
                    }
                    else
                    {
                        tmps2[j] = "";
                    }
                }
                addRow(tmps2);
            }

            //this.circleIndex = t.circleIndex;
        }

        public void addColWidth(string[] cw)
        {
            colWidth = cw;
        }

        public string[] getColWidth()
        {
            return colWidth;
        }

        public string getColWidth(int index)
        {
            if (colWidth == null || index >= colWidth.Length || colWidth[index] == null)
                return null;
            return colWidth[index];
        }

        public void addColBackground(int[] cb)
        {
            colBackground = cb;
        }

        public int[] getColBackground()
        {
            return colBackground;
        }

        public int getColBackground(int index)
        {
            if (colBackground == null || index >= colBackground.Length)
            {
                return 0xFFFFFF;
            }
            return colBackground[index];
        }

        public void addHeadBackground(int hb)
        {
            headBackground = hb;
        }

        public int getHeadBackground()
        {
            return headBackground;
        }

        public void setBorder(int b)
        {
            border = b;
        }

        public int getBorder()
        {
            return border;
        }

    }
}
