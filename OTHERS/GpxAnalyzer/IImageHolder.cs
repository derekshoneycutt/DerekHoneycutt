using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpxAnalyzer
{
    public interface IImageHolder
    {
        System.Drawing.Image GetImage(System.Drawing.Imaging.ImageFormat format);
    }
}
