using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormMaker
{
    class Question
    {
        public RotatedRect _RBox { get; }
        public bool _bIsCorrect { get; }
        public Question(RotatedRect RBox, bool bIsCorrect = false)
        {
            this._RBox = RBox;
            this._bIsCorrect = bIsCorrect;
        }

    }
}
