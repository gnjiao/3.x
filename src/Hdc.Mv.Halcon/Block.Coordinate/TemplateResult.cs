using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace Hdc.Mv.Halcon
{
    public class TemplateResult
    {
     
        public string Name { set; get; }
        public double Column1 { set; get; }
        public double Row1 { set; get; }
        public double Column2 { set; get; }
        public double Row2 { set; get; }
        public double RECT1Row1 { set; get; }
        public double RECT1Column1 { set; get; }
        public double RECT1Row2 { set; get; }
        public double RECT1Column2 { set; get; }
        public double RECT2Row1 { set; get; }
        public double RECT2Column1 { set; get; }
        public double RECT2Row2 { set; get; }
        public double RECT2Column2 { set; get; }
        public HTuple MOD1 { set; get; }
        public HTuple MOD2 { set; get; }

        public TemplateResult()
        {
            
        }
        public TemplateResult(double rect1row1, double rect1col, double rect1row2, double rect1col2, double rect2row1, double rect2col, double rect2row2, double rect2col2,
            HTuple mod1, HTuple mod2,double row1, double col1, double row2, double col2)
        {
            RECT1Row1 = rect1row1;
            RECT1Column1 = rect1col;
            RECT1Row2 = rect1row2;
            RECT1Column2 = rect1col2;
            RECT2Row1= rect2row1;
            RECT2Column1= rect2col;
            RECT2Row2= rect2row2;
            RECT2Column2= rect2col2;
            MOD1 = mod1;
            MOD2 = mod2;
            Column1 = col1;
            Row1 = row1;
            Column2 = col2;
            Row2 = row2;
        }

    }
}
